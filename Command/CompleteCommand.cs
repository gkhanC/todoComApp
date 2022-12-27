using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Command.FlagCommand;
using todoCOM.Flags;

namespace todoCOM.Command;

/// <summary>
/// The Complete command changes the isComplete information of the task.
/// </summary>
public class CompleteCommand : CommandBaseFlag , ITodoCommand
{
    public string optionString { get; private set; } = "--complete";

    public string descriptionString { get; private set; } =
        " The Complete command changes the isComplete information of the task.";

    public string aliasString { get; private set; } = "-cmp";
    public string result { get; private set; } = "";
    public string completeKey { get; } = "_#comp";

    private OptionFlags _flag = OptionFlags.None;

    public CompleteCommand()
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

        _flag = OptionFlags.None;

        if (!containsOption) return false;

        var optionIndex = Array.IndexOf(args, optionString);
        optionIndex = optionIndex < 0 ? Array.IndexOf(args, aliasString) : optionIndex;

        if (optionIndex <= -1) return false;

        var msg = completeKey;

        if (optionIndex > 0)
        {
            if (args.Length > optionIndex + 1)
            {
                var directive = args[optionIndex + 1];

                directive.ToLower(CultureInfo.CurrentCulture);

                if (directive.StartsWith('-'))
                    return false;


                var replace = directive.Replace("\"", "").Replace(" ", "");
                if (replace.Contains("false", StringComparison.CurrentCulture) ||
                    replace.Contains("f", StringComparison.CurrentCulture))
                {
                    _flag = OptionFlags.UnComplete;
                }
                else if (replace.Contains("true", StringComparison.CurrentCulture) ||
                         replace.Contains("t", StringComparison.CurrentCulture))
                {
                    _flag = OptionFlags.Complete;
                }
            }
            else
            {
                _flag = OptionFlags.UnComplete;
            }
        }
        else
        {
            if (args.Length > optionIndex + 1)
            {
                if (Regex.IsMatch(args[optionIndex + 1], @"^\d+$"))
                {
                    msg = Regex.Match(args[optionIndex + 1], @"^\d+$").Value;
                    _flag = OptionFlags.Complete;
                }
                else
                {
                    _flag = OptionFlags.Error;
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
