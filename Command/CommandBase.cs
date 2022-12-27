using System.CommandLine;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace todoCOM.Command;

public abstract class CommandBase
{
    protected RootCommand _command;
    protected Option<string> _option;
    public abstract bool Invoke(string[] args);
    public abstract string GetValue();

    public CommandBase()
    {
    }
}