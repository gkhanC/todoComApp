using Newtonsoft.Json;
using todoCOM.Command.FlagCommand;
using todoCOM.Entities;
using todoCOM.Repository;
using todoCOM.Settings;

namespace todoCOM.Utils;

public static class SaveLoadTool
{
    public static bool SaveCategories(Categories categories)
    {
        var flag = false;
        var path = ToDoComSettings.repositoryPath + "\\" + "categories.json";

        if (FindOrCreateFile(path))
        {
            string jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText(path, jsonString);
            flag = true;
        }

        return flag;
    }

    public static Categories LoadCategories(Categories categories)
    {
        var result = new Categories();

        var path = ToDoComSettings.repositoryPath + "\\" + "categories.json";

        if (FindOrCreateFile(path))
        {
            string jsonString = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<Categories>(jsonString);

            if (data != null)
                result = data;
        }

        return result;
    }

    public static void DeleteCategory(string categoryName)
    {
        var path = ToDoComSettings.repositoryPath + "\\" + $"{categoryName}.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static bool SaveStorage(Storage storage)
    {
        var flag = false;
        var path = ToDoComSettings.repositoryPath + "\\" + $"{storage.categoryName}.json";

        if (FindOrCreateFile(path))
        {
            string jsonString = JsonConvert.SerializeObject(storage, Formatting.Indented);
            File.WriteAllText(path, jsonString);
            flag = true;
        }

        return flag;
    }

    public static Storage LoadStorage(Storage storage)
    {
        var result = new Storage();
        var path = ToDoComSettings.repositoryPath + "\\" + $"{storage.categoryName}.json";

        if (FindOrCreateFile(path))
        {
            string jsonString = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<Storage>(jsonString);

            if (data != null)
                result = data;
        }

        return result;
    }

    public static bool SaveRepositoryData(RepositoryData rData)
    {
        var flag = false;

        if (FindOrCreateDirectory(ToDoComSettings.repositoryPath))
        {
            string filePath = ToDoComSettings.repositoryPath + "\\" + "RepositoryData.json";

            if (FindOrCreateFile(filePath))
            {
                string jsonString = JsonConvert.SerializeObject(rData, Formatting.Indented);
                File.WriteAllText(filePath, jsonString);
                flag = true;
            }
        }

        return flag;
    }

    public static RepositoryData LoadRepositoryData(RepositoryData rdata)
    {
        string filePath = ToDoComSettings.repositoryPath + "\\" + "RepositoryData.json";

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<RepositoryData>(jsonString);

            if (data != null)
                rdata = data;
        }
        else
        {
            SaveRepositoryData(rdata);
        }

        return rdata;
    }


    public static bool FindOrCreateDirectory(string directoryPath)
    {
        bool result = false;
        result = Directory.Exists(directoryPath);

        if (!result)
        {
            Directory.CreateDirectory(directoryPath);
            result = true;
        }

        return result;
    }

    public static bool FindOrCreateFile(string filePath)
    {
        bool result = false;
        result = File.Exists(filePath);

        if (!result)
        {
            File.Create(filePath).Close();
            result = true;
        }

        return result;
    }
}