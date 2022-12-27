using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using todoCOM.Entities;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM.Repository
{
    public class TaskRepository
    {
        public int TotalTaskCount() => Tasks.Count;
        public int TotalCategoryCount() => CategoryNames.Count;

        public List<string> CategoryNames { get; set; } = new List<string> { "_" };
        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();
        private string _selectedCategory = "_";

        public string selectedCategory()
        {
            if (string.IsNullOrWhiteSpace(_selectedCategory))
                return "";

            return _selectedCategory;
        }

        public bool AddTask(ref TodoTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return false;

            task.Id = TotalTaskCount().ToString().ToLower(CultureInfo.CurrentCulture);
            task.isCompleted = task.isCompleted;

            if (string.IsNullOrWhiteSpace(task.Tag))
            {
                task.Tag = "do";
            }
            else
            {
                task.Tag = task.Tag.ToLower(CultureInfo.CurrentCulture);
            }

            if (string.IsNullOrWhiteSpace(task.Category))
            {
                task.Category = _selectedCategory.ToLower(CultureInfo.CurrentCulture);
            }

            if (!CategoryNames.Contains(task.Category))
                CategoryNames.Add(task.Category);


            task.CreateDate = DateTime.Now.ToShortDateString();

            Tasks.Add(task);
            return true;
        }

        public bool SelectTask(string id, ref TodoTask task)
        {
            var todo = Tasks.Find((x) => x.Id == id);
            if (todo != null)
            {
                task = new TodoTask();
                task = todo;
                return true;
            }

            return false;
        }

        public bool EditTask(ref TodoTask task)
        {
            var todoTask = task;
            var todo = Tasks.Find((x) => x.Id == todoTask.Id);
            if (todo != null)
            {
                var index = Tasks.IndexOf(todo);
                Tasks[index] = todoTask;
                return true;
            }

            return false;
        }

        public bool EditCategory(TodoTask task)
        {
            var i = CategoryNames.IndexOf(_selectedCategory);
            if (i > -1)
            {
                CategoryNames[i] = task.Category;
                var selectedCategory = _selectedCategory;
                Console.WriteLine("kategory");
                Console.WriteLine($" yeni {task.Category}");
                Console.WriteLine($" selected {_selectedCategory}");
                var todoTask = task;

                foreach (var t in Tasks)
                {
                    if (t.Category == selectedCategory)
                    {
                        t.Category = todoTask.Category;
                        Console.WriteLine($"{i} {t.Category}");
                    }
                }

                return true;
            }

            return false;
        }

        public void ShowAll(ConsoleColorSettings colorSettings)
        {
            var allTasksViewer = new AllTaskViewer(this, colorSettings);
            allTasksViewer.Show();
        }

        public void ShowHub(ConsoleColorSettings colorSettings,
            HubViewer.HubDirective directive = HubViewer.HubDirective.All)
        {
            var hubViewer = new HubViewer(this, colorSettings, directive);
            hubViewer.Show();
        }

        public TodoTask? CompleteTask(string id)
        {
            var todo = Tasks.Find((x) => x.Id == id);
            if (todo == null)
            {
                var todoTask = new TodoTask();
                todoTask.Id = "_#null";
                return todoTask;
            }

            var index = Tasks.IndexOf(todo);
            todo.isCompleted = true;
            Tasks[index] = todo;
            return todo;
        }

        public void ShowCategory(ConsoleColorSettings colorSettings)
        {
            var msg = "Selected Category: " + _selectedCategory;
            var list = "";

            for (int i = 0; i < CategoryNames.Count(); i++)
            {
                list += $"\nid: {i} category name: {CategoryNames[i]}";
            }

            var messageViewer = new MessageViewer(msg, colorSettings, list);
            messageViewer.Show();
        }

        public string SelectCategory(int id)
        {
            if (id < CategoryNames.Count)
            {
                _selectedCategory = CategoryNames[id];
            }

            return _selectedCategory;
        }

        public string SelectCategory(string category)
        {
            var s = category.ToLower(CultureInfo.CurrentCulture);
            if (CategoryNames.Contains(category))
            {
                var index = CategoryNames.IndexOf(s);
                _selectedCategory = CategoryNames[index];
            }
            else
            {
                CategoryNames.Add(s);
                _selectedCategory = s;
            }

            return _selectedCategory;
        }

        public string SelectCategoryAndNotify(int id, ConsoleColorSettings colorSettings)
        {
            if (id >= CategoryNames.Count)
            {
                var msg = $"Category is not found with category id: \"{id}\". Selected Category: {_selectedCategory} ";
                var messageViewer = new MessageViewer(msg, colorSettings);
                messageViewer.Show();
            }
            else
            {
                _selectedCategory = CategoryNames[id];
            }

            return _selectedCategory;
        }
    }
}