using System.Globalization;
using todoCOM.Entities;
using todoCOM.Utils;
using todoCOM.View;

namespace todoCOM.Repository
{
    public class TaskRepository
    {
        public int totalTaskCount { get; set; } = 0;
        public int categoryCount => Categories.categoryNames.Count;

        public string selectedCategory { get; set; } = "main";


        public int getTotalTaskCount() => Tasks.Count;
        public int getTotalCategoryCount() => Categories.categoryNames.Count;

        public Categories Categories { get; set; } = new Categories();
        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();

        public TaskRepository()
        {
            LoadCategories();
            LoadRepositoryData();
        }

        public string getSelectedCategory()
        {
            if (string.IsNullOrWhiteSpace(selectedCategory))
                return "main";

            return selectedCategory;
        }

        public void LoadCategories()
        {
            var categories = SaveLoadTool.LoadCategories(new Categories());

            if (categories != null)
                Categories = categories;
        }

        public void SaveCategories()
        {
            SaveLoadTool.SaveCategories(Categories);
        }

        public void LoadRepositoryData()
        {
            var rData = new RepositoryData();
            var loadedData = SaveLoadTool.LoadRepositoryData(rData);
            totalTaskCount = loadedData.totalTaskCount;
            selectedCategory = loadedData.selectedCategory;
        }

        public void LoadTasks()
        {
            var storage = new Storage();
            storage.categoryName = selectedCategory.ToLower(CultureInfo.CurrentCulture);
            storage = SaveLoadTool.LoadStorage(storage);
            Tasks = storage.tasks;
        }

        public void LoadTasks(string category)
        {
            var storage = new Storage();
            storage.categoryName = category;
            selectedCategory = category;
            storage = SaveLoadTool.LoadStorage(storage);
            Tasks = storage.tasks;
        }

        public void SaveTasks()
        {
            var storage = new Storage();
            storage.categoryName = selectedCategory;
            storage.tasks = Tasks;
            SaveLoadTool.SaveStorage(storage);
            SaveLoadTool.SaveCategories(Categories);
        }

        public void SaveRepository()
        {
            var repo = new RepositoryData();
            repo.selectedCategory = selectedCategory;
            repo.totalTaskCount = getTotalTaskCount();
            repo.categoryCount = categoryCount;

            SaveLoadTool.SaveRepositoryData(repo);
        }

        public bool AddTask(ref TodoTask task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                return false;

            if (string.IsNullOrWhiteSpace(task.Category))
            {
                task.Category = selectedCategory.ToLower(CultureInfo.CurrentCulture);
            }
            else
            {
                selectedCategory = task.Category.ToLower(CultureInfo.CurrentCulture);
            }

            LoadTasks(task.Category);


            task.Id = (getTotalTaskCount() + 1).ToString().ToLower(CultureInfo.CurrentCulture);

            if (string.IsNullOrWhiteSpace(task.Tag))
            {
                task.Tag = "do";
            }
            else
            {
                task.Tag = task.Tag.ToLower(CultureInfo.CurrentCulture);
            }


            if (!Categories.categoryNames.Contains(task.Category))
                Categories.categoryNames.Add(task.Category);


            task.CreateDate = DateTime.Now.ToShortDateString();

            if (task.isCompleted)
                task.DueDate = DateTime.Now.ToShortDateString();


            Tasks.Add(task);

            SaveTasks();

            SaveCategories();
            SaveRepository();


            return true;
        }

        public bool DeleteTask(TodoTask todo)
        {
            var flag = false;
            LoadTasks();

            foreach (var selected in Tasks)
            {
                if (selected != null)
                {
                    if (selected.Id == todo.Id)
                    {
                        Tasks.Remove(selected);
                        SetsAllId();
                        flag = true;
                        break;
                    }
                }
            }

            if (flag)
            {
                SaveTasks();
                SaveRepository();
            }

            return flag;
        }

        public bool DeleteCategory(string categoryName)
        {
            LoadCategories();

            if (!Categories.categoryNames.Contains(categoryName))
                return false;

            Categories.categoryNames.Remove(categoryName);
            SaveLoadTool.DeleteCategory(categoryName);
            SaveCategories();

            selectedCategory = "main";
            SaveRepository();

            return true;
        }

        public bool DeleteCategory(int categoryId)
        {
            LoadCategories();

            if (categoryId >= Categories.categoryNames.Count())
                return false;

            var categoryName = Categories.categoryNames[categoryId];

            if (!categoryName.Contains(categoryName))
                return false;

            Categories.categoryNames.Remove(categoryName);

            SaveLoadTool.DeleteCategory(categoryName);
            SaveCategories();

            selectedCategory = "main";
            SaveRepository();

            return true;
        }

        public bool Clean()
        {
            LoadCategories();

            foreach (var VARIABLE in Categories.categoryNames)
            {
                SaveLoadTool.DeleteCategory(VARIABLE);
            }

            selectedCategory = "main";
            Categories.categoryNames = new List<string>();
            Categories.categoryNames.Add(selectedCategory);
            Tasks = new List<TodoTask>();

            SaveCategories();
            SaveRepository();

            return Tasks.Count == 0 && Categories.categoryNames.Count == 0;
        }

        public bool SelectTask(string id, ref TodoTask task)
        {
            LoadTasks();

            var todo = Tasks.Find((x) => x.Id == id);
            if (todo != null)
            {
                task = new TodoTask();
                task = todo;
                selectedCategory = todo.Category;
                return true;
            }

            SaveTasks();
            SaveRepository();

            return false;
        }

        public bool EditTask(ref TodoTask task)
        {
            LoadTasks();

            var todoTask = task;
            var todo = Tasks.Find((x) => x.Id == todoTask.Id);
            if (todo != null)
            {
                var index = Tasks.IndexOf(todo);
                Tasks[index] = todoTask;
                SaveTasks();
                SaveRepository();
                return true;
            }


            return false;
        }

        public bool InsertTask(ref TodoTask task)
        {
            LoadTasks();

            var todoTask = task;
            var index = Convert.ToInt32(todoTask.Id);

            Tasks.Insert(index > 0 ? index - 1 : index, todoTask);

            SetsAllId();
            SaveTasks();
            SaveRepository();

            return true;

        }

        private void SetsAllId()
        {
            for (var i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].Id = (i + 1).ToString();
            }
        }

        public bool EditCategory(TodoTask task)
        {
            LoadCategories();

            var i = Categories.categoryNames.IndexOf(this.selectedCategory);
            if (i <= -1) return false;

            Categories.categoryNames[i] = task.Category;
            var sCategory = this.selectedCategory;
            LoadTasks(sCategory);

            var todoTask = task;

            foreach (var t in Tasks)
            {
                if (t.Category == sCategory)
                {
                    t.Category = todoTask.Category;
                }
            }

            this.selectedCategory = todoTask.Category;
            SaveCategories();
            SaveTasks();
            SaveRepository();

            return true;
        }

        public void ShowAll(ConsoleColorSettings colorSettings)
        {
            LoadCategories();

            var taskL = new List<TodoTask>();
            foreach (var VARIABLE in Categories.categoryNames)
            {
                LoadTasks(VARIABLE);
                taskL.AddRange(Tasks);
            }

            Tasks = taskL;

            var allTasksViewer = new AllTaskViewer(this, colorSettings);
            allTasksViewer.Show();
        }

        public void ShowHub(ConsoleColorSettings colorSettings,
            HubViewer.HubDirective directive = HubViewer.HubDirective.All)
        {
            LoadTasks();

            var hubViewer = new HubViewer(this, colorSettings, directive);
            hubViewer.Show();
        }

        public TodoTask? CompleteTask(string id)
        {
            LoadTasks();

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

            SaveTasks();
            SaveRepository();

            return todo;
        }

        public void ShowCategory(ConsoleColorSettings colorSettings)
        {
            LoadCategories();

            var msg = "Selected Category: " + selectedCategory;
            var list = "";

            for (int i = 0; i < Categories.categoryNames.Count(); i++)
            {
                list += $"\nid: {i} category name: {Categories.categoryNames[i]}";
            }

            var messageViewer = new MessageViewer(msg, colorSettings, list);
            messageViewer.Show();
        }

        public string SelectCategory(int id)
        {
            LoadCategories();

            if (id < Categories.categoryNames.Count)
            {
                selectedCategory = Categories.categoryNames[id];
            }

            SaveRepository();

            return selectedCategory;
        }

        public string SelectCategory(string category)
        {
            LoadCategories();

            var s = category.ToLower(CultureInfo.CurrentCulture);
            if (Categories.categoryNames.Contains(category))
            {
                var index = Categories.categoryNames.IndexOf(s);
                selectedCategory = Categories.categoryNames[index];
            }
            else
            {
                Categories.categoryNames.Add(s);
                selectedCategory = s;
            }

            SaveRepository();

            return selectedCategory;
        }

        public string SelectCategoryAndNotify(int id, ConsoleColorSettings colorSettings)
        {
            LoadCategories();

            if (id >= Categories.categoryNames.Count)
            {
                var msg = $"Category is not found with category id: \"{id}\". Selected Category: {selectedCategory} ";
                var messageViewer = new MessageViewer(msg, colorSettings);
                messageViewer.Show();
            }
            else
            {
                selectedCategory = Categories.categoryNames[id];
            }

            SaveRepository();
            return selectedCategory;
        }
    }
}