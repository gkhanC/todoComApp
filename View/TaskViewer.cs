using todoCOM.Entities;
using todoCOM.Settings;
using todoCOM.Utils;

namespace todoCOM.View;

public class TaskViewer : Viewer
{
    private TodoTask _task;
    private ConsoleColorSettings _color;

    public override void Show()
    {
        Console.ForegroundColor = _color.IdColor.ForegroundColor;
        Console.BackgroundColor = _color.IdColor.BackgroundColor;
        Console.Write($"{_task.Id,-3}" + "  ");

        Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
        Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
        Console.Write("[");

        if (_task.isCompleted)
        {
            Console.ForegroundColor = _color.SuccessNotifyColor.ForegroundColor;
            Console.BackgroundColor = _color.SuccessNotifyColor.BackgroundColor;
        }
        else
        {
            Console.ForegroundColor = _color.FailedNotifyColor.ForegroundColor;
            Console.BackgroundColor = _color.FailedNotifyColor.BackgroundColor;
        }

        var comp = _task.isCompleted ? "X" : "_";
        Console.Write($"{comp}");

        Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
        Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
        Console.Write("]" + "  ");

        Console.ForegroundColor = _color.TagColor.ForegroundColor;
        Console.BackgroundColor = _color.TagColor.BackgroundColor;
        Console.Write($"<{_task.Tag + ">:",-6}" + "  ");

        Console.ResetColor();

        var lineIndex = 0;
        var title = _task.Title.Split(" ");
        var result = new string[1] { "" };

        Console.ForegroundColor = !_task.isCompleted
            ? _color.TitleColor.ForegroundColor
            : _color.DoneTitleColor.ForegroundColor;
        Console.BackgroundColor = !_task.isCompleted
            ? _color.TitleColor.BackgroundColor
            : _color.DoneTitleColor.BackgroundColor;

        foreach (var VARIABLE in title)
        {
            if (result[lineIndex].Length + (VARIABLE.Length + 1) > 60)
            {
                lineIndex++;
                Array.Resize(ref result, result.Length + 1);
                result[lineIndex] += VARIABLE + " ";
                continue;
            }

            result[lineIndex] += VARIABLE + " ";
        }

        Console.Write($"{result[0],-60}" + "  ");


        Console.ForegroundColor = _color.CategoryColor.ForegroundColor;
        Console.BackgroundColor = _color.CategoryColor.BackgroundColor;
        Console.Write($"{_task.Category,-10}" + "  ");


        Console.ForegroundColor = _color.CreateDateColor.ForegroundColor;
        Console.BackgroundColor = _color.CreateDateColor.BackgroundColor;
        Console.Write($"{_task.CreateDate,8}" + "  ");

        Console.ForegroundColor = _color.DueDateColor.ForegroundColor;
        Console.BackgroundColor = _color.DueDateColor.BackgroundColor;

        Console.Write($"{_task.DueDate,8}" + "\n");

        Console.ResetColor();

        Console.ForegroundColor = !_task.isCompleted
            ? _color.TitleColor.ForegroundColor
            : _color.DoneTitleColor.ForegroundColor;
        Console.BackgroundColor = !_task.isCompleted
            ? _color.TitleColor.BackgroundColor
            : _color.DoneTitleColor.BackgroundColor;

        var space = new string(' ', 19);
        for (int i = 1; i < result.Length; i++)
        {
            Console.WriteLine(space + $"{result[i],-60}");
        }

        if (ToDoComSettings.useTaskSeparator)
        {
            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            var separator = new string('-', 108);
            Console.WriteLine(separator);
        }

        Console.WriteLine("");

        Console.ResetColor();
    }

    public TaskViewer(TodoTask task, ConsoleColorSettings colorSettings)
    {
        _task = task;
        this._color = colorSettings;
    }
}