using todoCOM.Entities;
using todoCOM.Command;
using todoCOM.Flags;
using todoCOM.Repository;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM
{
    public class Program
    {
        private static TaskRepository _REPOSITORY = new TaskRepository();
        private static OptionFlags _FLAG;

        static void Main(string[] args)
        {


            if (args.Length > 0 && (args[0] == "-h" || args[0] == "--help"))
            {
                DisplayHelp();
                return;
            }

            var todo = new TodoTask();
            var color = new ConsoleColorSettings();
            color.Load();

            Console.WindowWidth = 120;
            Console.BufferWidth = 120;

            var messageViewer = new MessageViewer("", color);
            Console.Clear();


            if (args.Length == 0)
            {
                var msg = $"TodoCom Initialized. \nTodoCom working on \"{_REPOSITORY.selectedCategory}\" category. ";
                messageViewer.SetMessage(msg);
                messageViewer.Show();
                return;
            }

            string categoryName = "";

            if (args[0] == "-h" || args[0] == "-help")
            {
                System.Console.WriteLine("Oldu");
                return;
            }

            CommandRepository.CreateCleanCommand(args, color, messageViewer, ref _FLAG);

            if (_FLAG == OptionFlags.Clean)
            {
                messageViewer.SetMessage("Are you sure you want to delete all data? -> [y]Yes or [n]No");
                messageViewer.Show();
                var result = Console.ReadKey(true);
                var isCleared = false;

                if (result.KeyChar.ToString().ToLower() == "y")
                {
                    isCleared = _REPOSITORY.Clean();

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

            CommandRepository.CreateDeleteCommand(args, ref todo, color, messageViewer, ref _REPOSITORY, ref _FLAG);

            if (_FLAG == OptionFlags.Delete)
            {
                if (_REPOSITORY.DeleteTask(todo))
                {
                    var deleteNotify = new DeleteNotifyViewer(todo, color);
                    deleteNotify.Show();
                }
                else
                {
                    Console.WriteLine("The task could not be deleted.");
                }

                _REPOSITORY.ShowHub(color, HubViewer.HubDirective.All);
                return;
            }

            CommandRepository.CreateDeleteCategoryCommand(args, ref todo, color, messageViewer, ref _REPOSITORY,
                ref _FLAG);

            if (_FLAG == OptionFlags.DeleteCategory)
            {
                if (_REPOSITORY.DeleteCategory(todo.Category))
                {
                    var deleteNotify = new DeleteCategoryNotifyViewer(todo, color);
                    deleteNotify.Show();
                }
                else
                {
                    Console.WriteLine($"The category could not be deleted.");
                }

                _REPOSITORY.ShowCategory(color);
                return;
            }

            CommandRepository.CreateInsertCommand(args, ref todo, color, messageViewer, ref _REPOSITORY, ref _FLAG);

            if (_FLAG == OptionFlags.Insert)
            {
                if (_REPOSITORY.InsertTask(ref todo))
                {
                    var notify = new InsertTaskNotifyViewer(todo, color);
                    notify.Show();
                    _FLAG = OptionFlags.None;
                }
            }


            CommandRepository.CreateEditCommand(args, ref todo, color, messageViewer, ref _REPOSITORY, ref _FLAG);


            if (_FLAG != OptionFlags.Edit)
                CommandRepository.CreateEditCategoryCommand(args, ref todo, color, messageViewer, ref categoryName,
                    ref _REPOSITORY, ref _FLAG);

            if (_FLAG == OptionFlags.EditCategory)
            {
                todo.Category = categoryName;

                if (_REPOSITORY.EditCategory(todo))
                {
                    _REPOSITORY.ShowHub(color, HubViewer.HubDirective.All);
                }

                return;
            }

            if (_FLAG != OptionFlags.EditCategory)
            {
                CommandRepository.CreateCategoryCommand(args, ref todo, color, messageViewer, ref _REPOSITORY);
            }

            CommandRepository.CreateCompleteCommand(args, ref todo, color, messageViewer, ref _REPOSITORY);
            CommandRepository.CreateTagCommand(args, ref todo, color, messageViewer);
            CommandRepository.CreateAddCommand(args, ref todo, color, messageViewer, ref _FLAG);

            if (_FLAG == OptionFlags.Edit)
            {

                if (_REPOSITORY.EditTask(ref todo))
                {
                    var notify = new EditTaskNotifyViewer(todo, color);
                    notify.Show();
                    _FLAG = OptionFlags.None;
                }
            }

            if (_FLAG == OptionFlags.Add)
            {
                var isAdded = _REPOSITORY.AddTask(ref todo);
                if (isAdded)
                {
                    var notify = new AddNotifyViewer(todo, color);
                    notify.Show();
                }
            }

            CommandRepository.CreateShowCommand(args, ref todo, color, messageViewer, ref _REPOSITORY);
            CommandRepository.CreateShowAllCommand(args, ref todo, color, messageViewer, ref _REPOSITORY);
        }

        private static void DisplayHelp()
        {
            System.Console.WriteLine(helpText);
        }

        static string helpText = @"
TodoCom - ToDo Task Yönetim Uygulaması

Kullanım:
  todoc [komut] [seçenekler]

Komutlar:
  --add, -add                  Yeni bir görev ekler.
  --clean, -clean              Tüm kategorileri ve görevleri temizler.
  --complete, -com             Bir görevi tamamlandı olarak işaretler.
  --delete, -del               Bir görevi siler.
  --delete-category, -delc     Bir kategoriyi ve içindeki görevleri siler.
  --edit, -edt                 Bir görevi düzenler.
  --edit-category, -edtc       Bir kategoriyi düzenler.
  --show, -shw                 Seçili kategorideki görevleri görüntüler.
  --show-all, -shwa            Tüm görevleri görüntüler.
  --tag, -tag                  Bir göreve etiket ekler.
  --insert, -ins               Belirtilen indekse yeni görev ekler.

Seçenekler:
  --category <kategori>        İlgili kategoriyi belirler.
  --id <id>                    İlgili görevin ID'sini belirler.
  --comp                       Tamamlanmış görevleri görüntüler.
  --uncomp                     Tamamlanmamış görevleri görüntüler.
";
    }
}