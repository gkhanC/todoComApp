using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand;

public class EditCategoryCommand : CommandBaseFlag, ITodoCommand
{
    public string optionString { get; private set; } = "--edit-category";
    public string descriptionString { get; private set; } = "Edits selected category's name.";
    public string aliasString { get; private set; } = "-edtc";
    public string result { get; private set; } = "";

    public bool isNumeric { get; private set; } = false;
    public bool isFirst { get; private set; } = false;
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

        _flag = OptionFlags.None;
        if (containsOption)
        {
            var input = args.ToList();
            var optionIndex = input.IndexOf(optionString);
            optionIndex = optionIndex < 0 ? input.IndexOf(aliasString) : optionIndex;

            isFirst = optionIndex == 0;

            if (optionIndex != 0) return false;
            if (args.Length <= optionIndex + 2 || !isFirst) return false;
            var msg = args[optionIndex + 1].ToLower(CultureInfo.CurrentCulture).Replace(" ", "")
                .Replace("\"", "");


            categoryName = args[optionIndex + 2].ToLower(CultureInfo.CurrentCulture).Replace(" ", "")
                .Replace("\"", "");

            if (!Regex.IsMatch(msg, @"^\d+$")) return false;

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