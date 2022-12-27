using System.CommandLine;

namespace todoCOM.Command;

public class ShowAllCommand : CommandBase
{
    private string optionString { get; set; } = "--show-all";
    private string descriptionString { get; set; } = "Shows all tasks.";
    private string aliasString { get; set; } = "-shwa";
    private string result { get; set; } = "";

    private bool isInvoked { get; set; } = false;

    public ShowAllCommand()
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
            if (i > -1)
            {
                if (_command != null && res)
                    _command.Invoke(args[i] + " " + "show-all");
            }
        }

        return res;
    }

    public override string GetValue()
    {
        return result;
    }
}