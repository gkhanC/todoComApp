using System.CommandLine.Invocation;
using System.CommandLine;

namespace todoCOM.Command;

public class CommandHandler : ICommandHandler
{
    public List<Option<string>> options = new List<Option<string>>();

    public void AddOption(Option<string> opt) => options.Add(opt);

    public int Invoke(InvocationContext context)
    {
        foreach (var opt in options)
        {
            var result = context.ParseResult.GetValueForOption(opt);
            if (result == null) throw new ArgumentNullException(nameof(result));
            Console.WriteLine("Result: " +  result);
        }

        return 0;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        foreach (var opt in options)
        {
            var result = context.ParseResult.GetValueForOption(opt);
            if (result == null) throw new ArgumentNullException(nameof(result));
            Console.WriteLine("Result: " +  result);
        }


        return 0;
    }
}