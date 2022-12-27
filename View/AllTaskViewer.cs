using System.Security.Cryptography.X509Certificates;
using todoCOM.Entities;
using todoCOM.Repository;
using todoCOM.Utils;
using System.Linq;
using System.Xml.XPath;

namespace todoCOM.View
{
    public class AllTaskViewer : Viewer
    {
        private TaskRepository _repository;
        private ConsoleColorSettings _color;

        public override void Show()
        {
            var separator = new string('_', 113);

            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            Console.WriteLine(separator);

            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            var header = "TodoCOM Shows All Tasks";
            Console.Write($"{header,-81}");

            Console.ForegroundColor = _color.DefaultColor.ForegroundColor;
            Console.BackgroundColor = _color.DefaultColor.BackgroundColor;
            var totalTask = "Total number of tasks:" + _repository.TotalTaskCount().ToString();
            Console.Write($"{totalTask,-28}");
            Console.WriteLine("");

            ShowHeader(_color);

            Console.ForegroundColor = _color.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = _color.SeparatorColor.BackgroundColor;
            Console.WriteLine(separator);

            var categories = CreateCategorySeparators();

            foreach (var selectedCategory in categories)
            {
                if (selectedCategory.taskListUnCompleted.Count > 0)
                {
                    foreach (var selected in selectedCategory.taskListUnCompleted)
                    {
                        var taskViewer = new TaskViewer(selected, _color);
                        taskViewer.Show();
                    }
                }

                if (selectedCategory.taskListCompleted.Count > 0)
                {
                    foreach (var selected in selectedCategory.taskListCompleted)
                    {
                        var taskViewer = new TaskViewer(selected, _color);
                        taskViewer.Show();
                    }
                }
            }
        }

        public void ShowHeader(ConsoleColorSettings getColor)
        {
            var eTask = new TodoTask();

            Console.ForegroundColor = getColor.SeparatorColor.ForegroundColor;
            Console.BackgroundColor = getColor.SeparatorColor.BackgroundColor;

            Console.Write($"{nameof(eTask.Id),-3}" + "  ");

            Console.Write("Done" + " ");

            Console.Write($"<{nameof(eTask.Tag) + ">:",-6}" + "  ");

            Console.Write($"{nameof(eTask.Title),-60}" + "  ");

            Console.Write($"{nameof(eTask.Category),-10}" + "  ");

            Console.Write($"{nameof(eTask.CreateDate),8}" + "  ");

            Console.Write($"{nameof(eTask.DueDate),8}" + "\n");

            Console.ResetColor();
        }


        private CategorySeparator[] CreateCategorySeparators()
        {
            var separators = new List<CategorySeparator>();

            foreach (var name in _repository.CategoryNames)
            {
                var selected = _repository.Tasks.Where((x) => x.Category == name);
                var cat = new CategorySeparator();
                cat.categoryName = name ?? "";
                foreach (var task in selected)
                {
                    cat.AddTask(task);
                }

                separators.Add(cat);
            }

            var category_separators = separators.OrderBy((x) => x.categoryName);
            return category_separators.ToArray();
        }

        public AllTaskViewer(TaskRepository repo, ConsoleColorSettings colorSetting)
        {
            _color = colorSetting;
            _repository = repo;
        }
    }
}