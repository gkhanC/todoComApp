using todoCOM.Entities;
using todoCOM.Utils;

namespace todoCOM.View;

public class EditTaskNotifyViewer : TaskViewer
{
    private TodoTask _task;
    private ConsoleColorSettings colorSettings;

    public override void Show()
    {
        var separator = new string('_', 110);

        Console.ForegroundColor = colorSettings.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = colorSettings.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);


        Console.ForegroundColor = colorSettings.SuccessNotifyColor.ForegroundColor;
        Console.BackgroundColor = colorSettings.SuccessNotifyColor.BackgroundColor;
        Console.WriteLine("The Task's data is edited.");

        Console.ResetColor();


        Console.ForegroundColor = colorSettings.SeparatorColor.ForegroundColor;
        Console.BackgroundColor = colorSettings.SeparatorColor.BackgroundColor;
        Console.WriteLine(separator);
        base.Show();
    }

    public EditTaskNotifyViewer(TodoTask task, ConsoleColorSettings colorSettings) : base(task, colorSettings)
    {
        _task = task;
        this.colorSettings = colorSettings;
    }
}