using todoCOM.Entities;
using todoCOM.Utils;

namespace todoCOM.View;

public class DeleteCategoryNotifyViewer : TaskViewer
{
    private TodoTask _task;
    private ConsoleColorSettings colorSettings;

    public override void Show()
    {
        var separator = new string('_', 110);

        Console.ForegroundColor = colorSettings.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = colorSettings.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);

        if (String.IsNullOrWhiteSpace(_task.Category))
        {
            Console.ForegroundColor = colorSettings.FailedNotifyColor.ForegroundColor;
            Console.BackgroundColor = colorSettings.FailedNotifyColor.BackgroundColor;
            Console.WriteLine("The Category's data is lost.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = colorSettings.SuccessNotifyColor.ForegroundColor;
            Console.BackgroundColor = colorSettings.SuccessNotifyColor.BackgroundColor;
            Console.WriteLine($"The Category \"{_task.Category}\" deleted.");
            Console.ResetColor();
        }

        Console.ForegroundColor = colorSettings.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = colorSettings.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);
        //base.Show();
    }

    public DeleteCategoryNotifyViewer(TodoTask task, ConsoleColorSettings colorSettings) : base(task, colorSettings)
    {
        _task = task;
        this.colorSettings = colorSettings;
    }
}