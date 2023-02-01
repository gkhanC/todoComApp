using System;
using System.Security.Cryptography.X509Certificates;
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

            Console.WindowWidth = 120;
            Console.BufferWidth = 120;
            
            var messageViewer = new MessageViewer("", color);
            Console.Clear();
            
            
            if (args.Length == 0)
            {
                var msg = $"TodoCom Initialized. \nTodoCom working on \"{_REPOSİTORY.selectedCategory}\" category. ";
                messageViewer.SetMessage(msg);
                messageViewer.Show();
                return;
            }

            string categoryName = "";

            CommandRepository.CreateCleanCommand(args, color, messageViewer, ref _FLAG);

            if (_FLAG == OptionFlags.Clean)
            {
                messageViewer.SetMessage("Are you sure you want to delete all data? -> [y]Yes or [n]No");
                messageViewer.Show();
                var result = Console.ReadKey(true);
                var isCleared = false;

                if (result.KeyChar.ToString().ToLower() == "y")
                {
                    isCleared = _REPOSİTORY.Clean();

                    if (isCleared)
                    {
                        messageViewer.SetMessage("todoCom data cleared.");
                        messageViewer.Show();
                        return;
                    }
                }
                else if (result.KeyChar.ToString().ToLower() == "n")
                {
                    messageViewer.SetMessage("TodoCom data cleaning has been cancelled.");
                    messageViewer.Show();
                    return;
                }

                messageViewer.SetMessage("Unable to clear todoCom data.");
                messageViewer.Show();
                return;
            }

            CommandRepository.CreateDeleteCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY, ref _FLAG);

            if (_FLAG == OptionFlags.Delete)
            {
                if (_REPOSİTORY.DeleteTask(todo))
                {
                    var deleteNotify = new DeleteNotifyViewer(todo, color);
                    deleteNotify.Show();
                }
                else
                {
                    Console.WriteLine("The task could not be deleted.");
                }

                _REPOSİTORY.ShowHub(color, HubViewer.HubDirective.All);
                return;
            }

            CommandRepository.CreateDeleteCategoryCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY,
                ref _FLAG);

            if (_FLAG == OptionFlags.DeleteCategory)
            {
                if (_REPOSİTORY.DeleteCategory(todo.Category))
                {
                    var deleteNotify = new DeleteCategoryNotifyViewer(todo, color);
                    deleteNotify.Show();
                }
                else
                {
                    Console.WriteLine($"The category could not be deleted.");
                }

                _REPOSİTORY.ShowCategory(color);
                return;
            }

            CommandRepository.CreateEditCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY, ref _FLAG);
           

            if (_FLAG != OptionFlags.Edit)
                CommandRepository.CreateEditCategoryCommand(args, ref todo, color, messageViewer, ref categoryName,
                    ref _REPOSİTORY, ref _FLAG);

            if (_FLAG == OptionFlags.EditCategory)
            {
                todo.Category = categoryName;

                if (_REPOSİTORY.EditCategory(todo))
                {
                    _REPOSİTORY.ShowHub(color, HubViewer.HubDirective.All);
                }

                return;
            }

            if (_FLAG != OptionFlags.EditCategory)
            {
                CommandRepository.CreateCategoryCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY);
            }

            CommandRepository.CreateCompleteCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY);
            CommandRepository.CreateTagCommand(args, ref todo, color, messageViewer);
            CommandRepository.CreateAddCommand(args, ref todo, color, messageViewer, ref _FLAG);

            if (_FLAG == OptionFlags.Edit)
            {
               
                if (_REPOSİTORY.EditTask(ref todo))
                {
                    var notify = new EditTaskNotifyViewer(todo, color);
                    notify.Show();
                    _FLAG = OptionFlags.None;
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

            CommandRepository.CreateShowCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY);
            CommandRepository.CreateShowAllCommand(args, ref todo, color, messageViewer, ref _REPOSİTORY);
        }
    }
}