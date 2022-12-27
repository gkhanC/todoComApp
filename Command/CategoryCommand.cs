using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;

namespace todoCOM.Command;

public class CategoryCommand : CommandBase, ITodoCommand
{
    public string optionString { get; private set; } = "--category";
    public string descriptionString { get; private set; } = "Select task's category.";
    public string aliasString { get; private set; } = "-cat";
    public string result { get; private set; } = "";

    public bool isNumeric { get; private set; } = false;
    public bool isFirst { get; private set; } = false;


    public string categoryKey { get; } = "_#category";

    public CategoryCommand()
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
            result = x;
        }, _option);
    }

    public override bool Invoke(string[] args)
    {
        var containsOption = args.Contains(optionString) || args.Contains(aliasString);
        if (containsOption)
        {
            var input = args.ToList();
            int optionIndex = input.IndexOf(optionString);
            optionIndex = optionIndex < 0 ? input.IndexOf(aliasString) : optionIndex;
            isFirst = optionIndex == 0;

            var message = ExtractMessage(input, optionIndex);

            if (!String.IsNullOrEmpty(message))
            {
                _command.Invoke(args[optionIndex] + " " + message);
            }
        }

        return containsOption;
    }

    public string ExtractMessage(List<string> input, int optionIndex)
    {
        if (optionIndex > -1 && optionIndex + 1 < input.Count)
        {
            var part = input[optionIndex + 1].ToLower(CultureInfo.CurrentCulture).Replace(" ", "").Replace("\"", "");

            if (Regex.IsMatch(part, @"^\d+$"))
            {
                isNumeric = true;
                return Regex.Match(part, @"^\d+$").Value;
            }
            else
            {
                return part.Length > 10 ? part.Substring(0, 10) : part;
            }
        }
        else
        {
            return categoryKey;
        }
    }

    public override string GetValue()
    {
        return result;
    }
}