using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand
{
    public class AddCommand : CommandBaseFlag, ITodoCommand
    {
        public string optionString { get; private set; } = "--add";
        public string descriptionString { get; private set; } = "Adds new task.";
        public string aliasString { get; private set; } = "-add";
        public string result { get; private set; } = "";


        private OptionFlags _flag = OptionFlags.None;

        public AddCommand()
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
                _flag = OptionFlags.Add;
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

            var s = string.Empty;

            for (var i = optionIndex + 1; i < args.Length; i++)
            {
                if (args[i].StartsWith('-'))
                    break;

                s += args[i].Replace("\"", "") + " ";
            }

            s.TrimEnd();
            _command.Invoke(args[optionIndex] + " " + $"\"{s}\"");
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
}