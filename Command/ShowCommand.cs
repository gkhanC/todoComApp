using System.CommandLine;
using System.Globalization;

namespace todoCOM.Command;

public class ShowCommand : CommandBase
{
    private string optionString { get; set; } = "--show";
    private string descriptionString { get; set; } = "Shows task.";
    private string aliasString { get; set; } = "-shw";
    private string result { get; set; } = "";

    private bool isInvoked { get; set; } = false;

    public ShowCommand()
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
        var res = args.Contains(optionString) || args.Contains(aliasString);

        if (res)
        {
            var input = args.ToList();
            var i = input.IndexOf(optionString);
            i = i < 0 ? input.IndexOf(aliasString) : i;
            if (i > -1)
            {
                var e = "a";
                if (i + 1 < input.Count)
                {
                    var msg = args[i + 1].ToLower(CultureInfo.CurrentCulture);
                    if (msg.Contains("uncomp", StringComparison.CurrentCulture))
                    {
                        e = "uncomp";
                    }
                    else if (msg.Contains("comp", StringComparison.CurrentCulture))
                    {
                        e = "comp";
                    }
                }

                if (_command != null && res)
                    _command.Invoke(args[i] + " " + e);
            }
        }

        return res;
    }

    public override string GetValue()
    {
        return result;
    }
}