using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand;

public class CleanCommand : CommandBaseFlag, ITodoCommand
{
    public string optionString { get; private set; } = "--clean";
    public string descriptionString { get; private set; } = "delete";
    public string aliasString { get; private set; } = "-clean";
    public string result { get; private set; } = "";
    public bool isNumeric { get; private set; } = false;

    private OptionFlags _flag = OptionFlags.None;

    public CleanCommand()
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
            _flag = OptionFlags.Clean;
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

        if (optionIndex != 0)
        {
            _flag = OptionFlags.Error;
            return false;
        }

        _command.Invoke(args[optionIndex] + " " + "clean");
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