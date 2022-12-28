using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand;

public class DeleteCommand : CommandBaseFlag, ITodoCommand
{
    public string optionString { get; private set; } = "--delete";
    public string descriptionString { get; private set; } = "delete";
    public string aliasString { get; private set; } = "-del";
    public string result { get; private set; } = "";
    public bool isNumeric { get; private set; } = false;

    private OptionFlags _flag = OptionFlags.None;

    public DeleteCommand()
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
            _flag = OptionFlags.Delete;
            result = x;
        }, _option);
    }

    public override bool Invoke(string[] args)
    {
        var containsOption = args.Contains(optionString) || args.Contains(aliasString);

        _flag = OptionFlags.None;

        if (!containsOption) return false;

        var optionIndex = Array.IndexOf(args, optionString);
        optionIndex = optionIndex < 0 ? Array.IndexOf(args, aliasString) : optionIndex;

        if (optionIndex <= -1) return false;

        if (args.Length - 1 < optionIndex + 1)
        {
            _flag = OptionFlags.Error;
            return false;
        }

        var msg = args[optionIndex + 1].ToLower(CultureInfo.CurrentCulture).Replace(" ", "")
            .Replace("\"", "");

        if (!Regex.IsMatch(msg, @"^\d+$"))
        {
            _flag = OptionFlags.Error;
            return false;
        }

        isNumeric = true;
        if (isNumeric)
        {
            var replace = Regex.Match(msg, @"^\d+$").Value;
            if (!string.IsNullOrEmpty(msg))
            {
                _command.Invoke(args[optionIndex] + " " + replace);
                return true;
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