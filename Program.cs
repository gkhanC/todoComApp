using System;
using todoCOM.Entities;
using todoCOM.Command;
using todoCOM.Command.FlagCommand;
using todoCOM.Flags;
using todoCOM.Repository;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM
{
    public class Program
    {
        private static TaskRepository _REPOSİTORY = new TaskRepository();
        private static OptionFlags _FLAG;

        static void Main(string[] args)
        {
            var todo = new TodoTask();
            var color = new ConsoleColorSettings();
            color.Load();

            var t = new TodoTask();
            t.Title = "Bu bir denemtaskdir";
            t.isCompleted = true;
            _REPOSİTORY.AddTask(ref t);

            var t1 = new TodoTask();
            t1.Title = "Bu bir denemtaskdir2";
            t1.isCompleted = false;
            _REPOSİTORY.AddTask(ref t1);

            var t2 = new TodoTask();
            t2.Title = "Bu bir denemtaskdir2";
            t2.Category = "önemli";
            _REPOSİTORY.AddTask(ref t2);

            var t3 = new TodoTask();
            t3.Title = "Bu bir denemtaskdir2";
            t3.Category = "deneme";
            t3.isCompleted = true;
            _REPOSİTORY.AddTask(ref t3);
            var messageViewer = new MessageViewer("", color);
            Console.Clear();

            CreateEditCommand(args, ref todo, color, messageViewer);


            bool isCategorySetter = false;
            string categoryName = "";
            CreateEditCategoryCommand(args, ref todo, color, messageViewer, ref isCategorySetter, ref categoryName);

            if (_FLAG != OptionFlags.EditCategory)
            {
                CreateCategoryCommand(args, ref todo, color, messageViewer);
            }

            CreateCompleteCommand(args, ref todo, color, messageViewer);
            CreateTagCommand(args, ref todo, color, messageViewer);
            CreateAddCommand(args, ref todo, color, messageViewer);

            if (_FLAG == OptionFlags.Edit)
            {
                if (_REPOSİTORY.EditTask(ref todo))
                {
                    var notify = new EditTaskNotifyViewer(todo, color);
                    notify.Show();
                    _FLAG = OptionFlags.None;
                }
            }

            if (_FLAG == OptionFlags.EditCategory)
            {
                todo.Category = categoryName;


                if (_REPOSİTORY.EditCategory(todo))
                {
                    Console.WriteLine("kat:" + todo.Category);
                    Console.WriteLine("Kategory değiştirldi");
                }
                else
                {
                    Console.WriteLine("Kategory değiştirilemedi");
                    return;
                }
            }

            if (_FLAG == OptionFlags.Add)
            {
                var isAdded = _REPOSİTORY.AddTask(ref todo);
                if (isAdded)
                {
                    var notify = new AddNotifyViewer(todo, color);
                    notify.Show();
                }
            }

            CreateShowCommand(args, ref todo, color, messageViewer);
            CreateShowAllCommand(args, ref todo, color, messageViewer);
        }

        public static void CreateEditCategoryCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer, ref bool isCategorySetter, ref string category)
        {
            var editCategoryCommand = new EditCategoryCommand();

            if (editCategoryCommand.Invoke(args))
            {
                var val = editCategoryCommand.GetValue();

                editCategoryCommand.GetFlag(out _FLAG);

                if (editCategoryCommand is { isNumeric: true, isCategorySetter: true })
                {
                    var i = Convert.ToInt32(editCategoryCommand.GetValue());
                    todo.Category = _REPOSİTORY.SelectCategory(i);
                }

                isCategorySetter = editCategoryCommand.isCategorySetter;
                category = editCategoryCommand.categoryName;
            }
            else
            {
                editCategoryCommand.GetFlag(out _FLAG);
                if (_FLAG == OptionFlags.Error)
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
                    _REPOSİTORY.ShowCategory(color);
                }
            }
        }

        public static void CreateEditCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var editCommand = new EditCommand();
            if (editCommand.Invoke(args))
            {
                var val = editCommand.GetValue();
                editCommand.GetFlag(out _FLAG);
                if (!_REPOSİTORY.SelectTask(val, ref todo))
                {
                    var msg_1 =
                        $"Main Command : {editCommand.optionString} or {editCommand.aliasString} (int)taskId -> Edits task.";
                    var msg_2 =
                        $"with Command : --add or -add  (string) title -> Edits task's title";
                    var msg_3 =
                        $"with Command : --complete or -cmp t or f -> The current task is marked as complete or incomplete";

                    messageViewer =
                        new MessageViewer(
                            $"The task that Task's id: {val} is not found with {editCommand.optionString}/{editCommand.aliasString} argument.",
                            color, msg_1, msg_2, msg_3);
                    messageViewer.Show();
                }
            }
            else
            {
                editCommand.GetFlag(out _FLAG);
                if (_FLAG == OptionFlags.Error)
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
        }

        public static void CreateTagCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var tagCommand = new TagCommand();
            if (tagCommand.Invoke(args))
            {
                todo.Tag = tagCommand.GetValue();
            }
        }

        public static void CreateShowAllCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var showAllCommand = new ShowAllCommand();
            if (showAllCommand.Invoke(args))
            {
                _REPOSİTORY.ShowAll(color);
                return;
            }
        }

        public static void CreateAddCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var addCommand = new AddCommand();
            if (addCommand.Invoke(args))
            {
                todo.Title = addCommand.GetValue();
                if (_FLAG != OptionFlags.Edit)
                    addCommand.GetFlag(out _FLAG);
            }
        }

        public static void CreateShowCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var showCommand = new ShowCommand();
            if (showCommand.Invoke(args))
            {
                var val = showCommand.GetValue();

                if (string.Equals("a", val, StringComparison.CurrentCulture))
                {
                    _REPOSİTORY.ShowHub(color, HubViewer.HubDirective.All);
                }

                if (string.Equals("uncomp", val, StringComparison.CurrentCulture))
                {
                    _REPOSİTORY.ShowHub(color, HubViewer.HubDirective.UnCompleted);
                }

                if (string.Equals("comp", val, StringComparison.CurrentCulture))
                {
                    _REPOSİTORY.ShowHub(color, HubViewer.HubDirective.Completed);
                }

                return;
            }
        }

        public static void CreateCategoryCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var categoryCommand = new CategoryCommand();
            if (categoryCommand.Invoke(args))
            {
                var val = categoryCommand.GetValue();
                if (string.Equals(val, categoryCommand.categoryKey))
                {
                    _REPOSİTORY.ShowCategory(color);
                    return;
                }

                if (categoryCommand.isNumeric)
                {
                    var id = Convert.ToInt32(val);
                    todo.Category = _REPOSİTORY.SelectCategoryAndNotify(id, color);
                }
                else
                {
                    todo.Category = _REPOSİTORY.SelectCategory(val);
                }

                if (categoryCommand.isFirst)
                {
                    _REPOSİTORY.ShowCategory(color);
                }
            }
        }

        public static void CreateCompleteCommand(string[] args, ref TodoTask todo, ConsoleColorSettings color,
            MessageViewer messageViewer)
        {
            var completeCommand = new CompleteCommand();
            if (completeCommand.Invoke(args))
            {
                var val = completeCommand.GetValue();
                if (string.Equals(val, completeCommand.completeKey))
                {
                    if (completeCommand.GetFlag(out var f))
                    {
                        if (f == OptionFlags.Error)
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

                        todo.isCompleted = f == OptionFlags.Complete;
                    }
                }
                else
                {
                    var res = _REPOSİTORY.CompleteTask(val);
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
        }
    }
}