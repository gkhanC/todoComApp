using todoCOM.Command.FlagCommand;
using todoCOM.Entities;
using todoCOM.Flags;
using todoCOM.Repository;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM.Command;

public static class CommandRepository
{
    public static void CreateDeleteCategoryCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository, ref OptionFlags flag)
    {
        var deleteCategoryCommand = new DeleteCategoryCommand();
        if (deleteCategoryCommand.Invoke(args))
        {
            var result = deleteCategoryCommand.GetValue();
            var category = deleteCategoryCommand.isNumeric
                ? repository.SelectCategory(Convert.ToInt32(result))
                : result;
            todo.Category = category;
        }

        if (deleteCategoryCommand.GetFlag(out var f))
        {
            flag = f;
            if (flag == OptionFlags.Error)
            {
                var msg_1 =
                    $"Main Command : {deleteCategoryCommand.optionString} or {deleteCategoryCommand.aliasString} (int)categoryId or (string)categoryName -> Deletes the selected category.";

                messageViewer =
                    new MessageViewer(
                        $"{deleteCategoryCommand.optionString}/{deleteCategoryCommand.aliasString} argument is wrong type.",
                        color, msg_1);
                messageViewer.Show();
                repository.ShowCategory(color);
            }
        }
    }


    public static void CreateCleanCommand(string[] args, ConsoleColorSettings color,
        MessageViewer messageViewer, ref OptionFlags flag)
    {
        var cleanCommand = new CleanCommand();
        cleanCommand.Invoke(args);

        if (cleanCommand.GetFlag(out var f))
        {
            if (f == OptionFlags.Error)
            {
                var msg_1 =
                    $"Main Command : {cleanCommand.optionString} or {cleanCommand.aliasString} Cleans all todoCOM data.";

                messageViewer =
                    new MessageViewer(
                        $"{cleanCommand.optionString}/{cleanCommand.aliasString} argument is wrong type.",
                        color, msg_1);
                messageViewer.Show();
            }
            else
            {
                flag = f;
            }
        }
    }


    public static void CreateDeleteCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository, ref OptionFlags flag)
    {
        var deleteCommand = new DeleteCommand();
        if (deleteCommand.Invoke(args))
        {
            var id = deleteCommand.GetValue();
            repository.SelectTask(id, ref todo);
        }

        if (deleteCommand.GetFlag(out var f))
        {
            if (f == OptionFlags.Error)
            {
                var msg_1 =
                    $"Main Command : {deleteCommand.optionString} or {deleteCommand.aliasString} (int)taskId -> Deletes the selected task.";

                messageViewer =
                    new MessageViewer(
                        $"{deleteCommand.optionString}/{deleteCommand.aliasString} argument is wrong type.",
                        color, msg_1);
                messageViewer.Show();
                repository.ShowHub(color);
            }
            else
            {
                flag = f;
            }
        }
    }

    public static void CreateEditCategoryCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref string category, ref TaskRepository repository, ref OptionFlags flag)
    {
        var editCategoryCommand = new EditCategoryCommand();

        if (editCategoryCommand.Invoke(args))
        {
            var val = editCategoryCommand.GetValue();

            editCategoryCommand.GetFlag(out flag);

            if (editCategoryCommand is { isNumeric: true })
            {
                var i = Convert.ToInt32(editCategoryCommand.GetValue());
                todo.Category = repository.SelectCategory(i);
            }

            category = editCategoryCommand.categoryName;
            return;
        }

        editCategoryCommand.GetFlag(out flag);
        if (flag == OptionFlags.Error)
        {
            var msg_1 =
                $"Main Command : {editCategoryCommand.optionString} or {editCategoryCommand.aliasString} (int)categoryId -> Edits selected category's name.";
            var msg_2 =
                $"with Command : --category (string)newName";

            messageViewer =
                new MessageViewer(
                    $"{editCategoryCommand.optionString}/{editCategoryCommand.aliasString} argument is wrong type.",
                    color, msg_1, msg_2);
            messageViewer.Show();
            repository.ShowCategory(color);
        }
    }

    public static void CreateEditCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository, ref OptionFlags flag)
    {
        var editCommand = new EditCommand();
        if (editCommand.Invoke(args))
        {
            var val = editCommand.GetValue();
            editCommand.GetFlag(out flag);

            if (repository.SelectTask(val, ref todo))
            {
                if (!string.IsNullOrWhiteSpace(editCommand.newTitle))
                {
                    todo.Title = editCommand.newTitle;
                }

                return;
            }
        }

        editCommand.GetFlag(out flag);
        if (flag == OptionFlags.Error)
        {
            var msg_1 =
                $"Main Command : {editCommand.optionString} or {editCommand.aliasString} (int)taskId -> Edits task.";
            var msg_2 =
                $"with Command : --add or -add  (string) title -> Edits task's title";
            var msg_3 =
                $"with Command : --complete or -cmp t or f -> The current task is marked as complete or incomplete";

            messageViewer =
                new MessageViewer(
                    $"{editCommand.optionString}/{editCommand.aliasString} argument is wrong type.",
                    color, msg_1, msg_2, msg_3);
            messageViewer.Show();
        }
    }

    public static void CreateTagCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer)
    {
        var tagCommand = new TagCommand();
        if (tagCommand.Invoke(args))
        {
            todo.Tag = tagCommand.GetValue();
        }

        if (tagCommand.GetFlag(out var f))
        {
            if (f == OptionFlags.Error)
            {
                var msg_1 =
                    $"Main Command : {tagCommand.optionString} or {tagCommand.aliasString} (string)Tag -> Edits task's tag.";
                var msg_2 =
                    $"This command can be used after --add or --edit.";
                messageViewer =
                    new MessageViewer(
                        $"{tagCommand.optionString}/{tagCommand.aliasString} argument is wrong type.",
                        color, msg_1, msg_2);
                messageViewer.Show();
            }
        }
    }

    public static void CreateShowAllCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository)
    {
        var showAllCommand = new ShowAllCommand();
        if (showAllCommand.Invoke(args))
        {
            repository.ShowAll(color);
            return;
        }
    }

    public static void CreateAddCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref OptionFlags flag)
    {
        var addCommand = new AddCommand();
        if (addCommand.Invoke(args))
        {
            todo.Title = addCommand.GetValue();
            if (flag != OptionFlags.Edit)
                addCommand.GetFlag(out flag);
        }

        if (addCommand.GetFlag(out var f))
        {
            if (f == OptionFlags.Error)
            {
                var msg_1 =
                    $"Main Command : {addCommand.optionString} or {addCommand.aliasString} (string)Tag -> Adds new todo task..";
                var msg_2 =
                    $"The --add command should include a task body. Example: --add do something.";
                var msg_3 =
                    $"The --add command can also be used in conjunction with the --category, --tag, and --complete commands.";
                messageViewer =
                    new MessageViewer(
                        $"{addCommand.optionString}/{addCommand.aliasString} argument is wrong type.",
                        color, msg_1, msg_2, msg_3);
                messageViewer.Show();
            }
        }
    }

    public static void CreateShowCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository)
    {
        var showCommand = new ShowCommand();
        if (showCommand.Invoke(args))
        {
            var val = showCommand.GetValue();

            if (string.Equals("a", val, StringComparison.CurrentCulture))
            {
                repository.ShowHub(color, HubViewer.HubDirective.All);
            }

            if (string.Equals("uncomp", val, StringComparison.CurrentCulture))
            {
                repository.ShowHub(color, HubViewer.HubDirective.UnCompleted);
            }

            if (string.Equals("comp", val, StringComparison.CurrentCulture))
            {
                repository.ShowHub(color, HubViewer.HubDirective.Completed);
            }

            return;
        }
    }

    public static void CreateCategoryCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository)
    {
        var categoryCommand = new CategoryCommand();
        if (categoryCommand.Invoke(args))
        {
            var val = categoryCommand.GetValue();
            if (string.Equals(val, categoryCommand.categoryKey))
            {
                repository.ShowCategory(color);
                return;
            }

            if (categoryCommand.isNumeric)
            {
                var id = Convert.ToInt32(val);
                todo.Category = repository.SelectCategoryAndNotify(id, color);
            }
            else
            {
                todo.Category = repository.SelectCategory(val);
            }

            if (categoryCommand.isFirst)
            {
                repository.ShowCategory(color);
            }
        }
    }

    public static void CreateCompleteCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
        MessageViewer messageViewer, ref TaskRepository repository)
    {
        var completeCommand = new CompleteCommand();

        if (completeCommand.Invoke(args))
        {
            var val = completeCommand.GetValue();
            if (string.Equals(val, completeCommand.completeKey))
            {
                if (completeCommand.GetFlag(out var f))
                {
                    todo.isCompleted = f == OptionFlags.Complete;
                }
            }
            else
            {
                var res = repository.CompleteTask(val);
                if (res == null || string.Equals(res.Id, res.nullKey))
                {
                    messageViewer = new MessageViewer($"The task is not found with id: {val}", color);
                    messageViewer.Show();
                }
                else
                {
                    messageViewer = new MessageViewer($"The task is completed.", color);
                    messageViewer.Show();
                    var taskViewer = new TaskViewer(res, color);
                    taskViewer.Show();
                }
            }
        }

        //Shows help
        if (completeCommand.GetFlag(out var flag))
        {
            if (flag == OptionFlags.Error)
            {
                var msg_1 =
                    $"Command : {completeCommand.optionString} or {completeCommand.aliasString} (int)taskId -> Completes task.";

                var msg_2 =
                    $"Command : {completeCommand.optionString} or {completeCommand.aliasString} t -> The current task is marked as complete";

                var msg_3 =
                    $"Command : {completeCommand.optionString} or {completeCommand.aliasString} t -> The current task is marked as incomplete";

                messageViewer =
                    new MessageViewer(
                        $"{completeCommand.optionString}/{completeCommand.aliasString} argument is wrong type.",
                        color, msg_1, msg_2, msg_3);
                messageViewer.Show();
            }
        }
    }
}