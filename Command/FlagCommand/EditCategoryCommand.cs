using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand;

public class EditCategoryCommand : CommandBaseFlag, ITodoCommand
{
    public string optionString { get; private set; } = "--edit-category";
    public string optionCategoryString { get; set; } = "--category";
    public string descriptionString { get; private set; } = "Edits selected category's name.";
    public string aliasString { get; private set; } = "-edtc";
    public string aliasCategoryString { get; set; } = "-cat";
    public string result { get; private set; } = "";

    public bool isNumeric { get; private set; } = false;
    public bool isFirst { get; private set; } = false;
    public bool isCategorySetter { get; private set; } = false;
    public string categoryName { get; private set; } = "";

    private OptionFlags _flag = OptionFlags.None;

    public EditCategoryCommand()
    {
        Init();
    }

    private void Init()
    {
        _option = new Option<string>(optionString, descriptionString);
        _option.AddAlias(aliasString);
        _command = new RootCommand(descriptionString);
        _command.AddOption(_option);
        _command.SetHandler((x) =>
        {
            if (string.IsNullOrWhiteSpace(x)) return;
            _flag = OptionFlags.EditCategory;
            result = x;
        }, _option);
    }

    public override bool Invoke(string[] args)
    {
        var containsOption = args.Contains(optionString) || args.Contains(aliasString);
        var containCategoryCommand = args.Contains(optionCategoryString) || args.Contains(aliasCategoryString);
        isCategorySetter = !containCategoryCommand;

        if (containsOption)
        {
            var input = args.ToList();
            int optionIndex = input.IndexOf(optionString);
            optionIndex = optionIndex < 0 ? input.IndexOf(aliasString) : optionIndex;

            isFirst = optionIndex == 0;

            _flag = OptionFlags.Error;

            var range = isCategorySetter ? 2 : 1;

            if (args.Length > optionIndex + range && isFirst)
            {
                var msg = args[optionIndex + 1].ToLower(CultureInfo.CurrentCulture).Replace(" ", "")
                    .Replace("\"", "");

                if (isCategorySetter)
                {
                    categoryName = args[optionIndex + 2].ToLower(CultureInfo.CurrentCulture).Replace(" ", "");
                }

                if (Regex.IsMatch(msg, @"^\d+$"))
                {
                    isNumeric = true;
                    if (isNumeric)
                    {
                        var replace = Regex.Match(msg, @"^\d+$").Value;
                        if (!String.IsNullOrEmpty(msg))
                        {
                            _command.Invoke(args[optionIndex] + " " + replace);
                            return true;
                        }
                    }
                }
                else
                {
                    _flag = OptionFlags.Error;
                }
            }
            else
            {
                _flag = OptionFlags.Error;
            }
        }

        return false;
    }

    public override string GetValue()
    {
        return result;
    }

    public override bool GetFlag(out OptionFlags flag)
    {
        flag = _flag;
        return _flag != OptionFlags.None;
    }
}