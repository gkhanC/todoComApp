using todoCOM.Entities;
using todoCOM.Utils;

namespace todoCOM.View;

public class MessageViewer : Viewer
{
    private string _message = "";
    private ConsoleColorSettings _color;
    private string[] _bottomInfo;

    public override void Show()
    {
        if (!string.IsNullOrWhiteSpace(_message))
        {
            var separator = new string('_', 110);

            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            Console.WriteLine(separator);

            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            var header = $"TodoCOM Shows Message: ";
            Console.Write($"{header,-81}");
            Console.WriteLine("");
            Console.ForegroundColor = _color.SuccessNotifyColor.ForegroundColor;
            Console.BackgroundColor = _color.SuccessNotifyColor.BackgroundColor;
            Console.WriteLine($"{_message,-110}");
            Console.ResetColor();

            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            Console.WriteLine(separator);

            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            foreach (var info in _bottomInfo)
            {
                Console.WriteLine(info);
            }

            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            Console.WriteLine(separator);

            Console.ResetColor();
        }
    }

    public void SetMessage(string msg)
    {
        _message = msg;
    }

    public MessageViewer(string message, ConsoleColorSettings color, params string[] bottomInfo) : base()
    {
        _message = message;
        this._color = color;
        _bottomInfo = bottomInfo;
    }
}