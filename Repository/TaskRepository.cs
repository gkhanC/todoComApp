using System.Globalization;
using todoCOM.Entities;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM.Repository
{
    public class TaskRepository
    {
        public int totalTaskCount { get; set; } = 0;
        public int categoryCount { get; set; } = 0;

        public string selectedCategory { get; set; } = "_";


        public int getTotalTaskCount() => totalTaskCount;
        public int getTotalCategoryCount() => CategoryNames.Count;

        public List<string> CategoryNames { get; set; } = new List<string> { "_" };
        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();

        public TaskRepository()
        {
            var rData = new RepositoryData();
            var loadedData = SaveLoadTool.LoadRepositoryData(rData);
            totalTaskCount = loadedData.totalTaskCount;
            categoryCount = loadedData.categoryCount;
            selectedCategory = loadedData.selectedCategory;
        }

        public string getSelectedCategory()
        {
            if (string.IsNullOrWhiteSpace(selectedCategory))
                return "_";

            return selectedCategory;
        }

        public bool AddTask(ref TodoTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return false;

            task.Id = getTotalTaskCount().ToString().ToLower(CultureInfo.CurrentCulture);

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
                task.Category = selectedCategory.ToLower(CultureInfo.CurrentCulture);
            }

            if (!CategoryNames.Contains(task.Category))
                CategoryNames.Add(task.Category);


            task.CreateDate = DateTime.Now.ToShortDateString();

            if (task.isCompleted)
                task.DueDate = DateTime.Now.ToShortDateString();

            Tasks.Add(task);
            return true;
        }

        public bool DeleteTask(TodoTask todo)
        {
            if (Tasks.Contains(todo))
            {
                Tasks.Remove(todo);
                return true;
            }

            return false;
        }

        public bool DeleteCategory(string categoryName)
        {
            if (!CategoryNames.Contains(categoryName))
                return false;

            CategoryNames.Remove(categoryName);

            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].Category == categoryName)
                {
                    Tasks.RemoveAt(i);
                }
            }


            if (selectedCategory == categoryName)
            {
                if (CategoryNames.Count > 0)
                {
                    selectedCategory = CategoryNames[0];
                }
                else
                {
                    selectedCategory = "_";
                }
            }

            return true;
        }

        public bool DeleteCategory(int categoryId)
        {
            if (categoryId >= CategoryNames.Count())
                return false;

            var categoryName = CategoryNames[categoryId];

            if (!categoryName.Contains(categoryName))
                return false;

            CategoryNames.Remove(categoryName);

            for (int i = 0; i < Tasks.Count; i++)
            {
                if (Tasks[i].Category == categoryName)
                {
                    Tasks.RemoveAt(i);
                }
            }

            if (selectedCategory == categoryName)
            {
                if (CategoryNames.Count > 0)
                {
                    selectedCategory = CategoryNames[0];
                }
                else
                {
                    selectedCategory = "_";
                }
            }

            return true;
        }

        public bool Clean()
        {
            CategoryNames = new List<string>();
            Tasks = new List<TodoTask>();

            //TODO: veriler tamamaen silinecek


            return Tasks.Count == 0 && CategoryNames.Count == 0;
        }

        public bool SelectTask(string id, ref TodoTask task)
        {
            var todo = Tasks.Find((x) => x.Id == id);
            if (todo != null)
            {
                task = new TodoTask();
                task = todo;
                selectedCategory = todo.Category;
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
            var i = CategoryNames.IndexOf(this.selectedCategory);
            if (i <= -1) return false;

            CategoryNames[i] = task.Category;
            var selectedCategory = this.selectedCategory;

            var todoTask = task;

            foreach (var t in Tasks)
            {
                if (t.Category == selectedCategory)
                {
                    t.Category = todoTask.Category;
                }
            }

            this.selectedCategory = todoTask.Category;

            return true;
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
                todoTask.Id = todoTask.nullKey;
                return todoTask;
            }

            var index = Tasks.IndexOf(todo);
            todo.isCompleted = true;
            todo.DueDate = DateTime.Now.ToShortDateString();
            Tasks[index] = todo;
            return todo;
        }

        public void ShowCategory(ConsoleColorSettings colorSettings)
        {
            var msg = "Selected Category: " + selectedCategory;
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
                selectedCategory = CategoryNames[id];
            }

            return selectedCategory;
        }

        public string SelectCategory(string category)
        {
            var s = category.ToLower(CultureInfo.CurrentCulture);
            if (CategoryNames.Contains(category))
            {
                var index = CategoryNames.IndexOf(s);
                selectedCategory = CategoryNames[index];
            }
            else
            {
                CategoryNames.Add(s);
                selectedCategory = s;
            }

            return selectedCategory;
        }

        public string SelectCategoryAndNotify(int id, ConsoleColorSettings colorSettings)
        {
            if (id >= CategoryNames.Count)
            {
                var msg = $"Category is not found with category id: \"{id}\". Selected Category: {selectedCategory} ";
                var messageViewer = new MessageViewer(msg, colorSettings);
                messageViewer.Show();
            }
            else
            {
                selectedCategory = CategoryNames[id];
            }

            return selectedCategory;
        }
    }
}