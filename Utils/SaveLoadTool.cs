using Newtonsoft.Json;
using todoCOM.Repository;
using todoCOM.Settings;

namespace todoCOM.Utils;

public static class SaveLoadTool
{
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

            if (rdata != null)
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