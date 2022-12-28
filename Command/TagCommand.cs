using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Command.FlagCommand;
using todoCOM.Flags;

namespace todoCOM.Command;

public class TagCommand : CommandBaseFlag, ITodoCommand
{
    public string optionString { get; private set; } = "--tag";
    public string descriptionString { get; private set; } = "Edits  task's tag.";
    public string aliasString { get; private set; } = "-tag";
    public string result { get; private set; } = "";

    private OptionFlags _flag = OptionFlags.None;

    public TagCommand()
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

        if (!containsOption) return false;

        _flag = OptionFlags.None;
        var optionIndex = Array.IndexOf(args, optionString);
        optionIndex = optionIndex < 0 ? Array.IndexOf(args, aliasString) : optionIndex;

        if (optionIndex <= -1) return false;

        var msg = "_";

        if (optionIndex == 0)
        {
            _flag = OptionFlags.Error;
            return false;
        }

        if (args.Length > optionIndex + 1)
        {
            var directive = args[optionIndex + 1];

            if (directive != null)
            {
                var loverDirective = directive.ToLower(CultureInfo.CurrentCulture);
                var replace = "";

                replace = loverDirective.Length > 3
                    ? loverDirective.Replace("\"", "").Replace(" ", "").Substring(0, 3)
                    : loverDirective.Replace("\"", "").Replace(" ", "");

                if (directive.StartsWith('-'))
                    return false;

                if (Regex.IsMatch(replace, @"^[^\d.,-]*$"))
                {
                    msg = Regex.Match(replace, @"^[^\d.,-]*$").Value;
                }
            }
        }

        _command.Invoke(args[optionIndex] + " " + msg);
        return true;
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