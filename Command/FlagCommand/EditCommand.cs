using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using todoCOM.Command.Abstract;
using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand
{
    public class EditCommand : CommandBaseFlag, ITodoCommand
    {
        public string optionString { get; private set; } = "--edit";
        public string descriptionString { get; private set; } = "Edits new task.";
        public string aliasString { get; private set; } = "-edt";
        public string result { get; private set; } = "";

        public bool isNumeric { get; private set; } = false;
        public bool isFirst { get; private set; } = false;

        private OptionFlags _flag = OptionFlags.None;
        public string newTitle;

        public EditCommand()
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
                _flag = OptionFlags.Edit;
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
                _flag = OptionFlags.Error;
                if (args.Length > optionIndex + 1 && isFirst)
                {
                    var msg = args[optionIndex + 1].ToLower(CultureInfo.CurrentCulture).Replace(" ", "")
                        .Replace("\"", "");

                    if (args.Length > optionIndex + 2)
                        newTitle = args[optionIndex + 2];

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
}