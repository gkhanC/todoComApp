namespace todoCOM.Settings;

public static class ToDoComSettings
{ 
    public static string settingsFileName { get; set; } = "AppSettings.json";
    public static string settingFilePath { get; set; } = Directory.GetCurrentDirectory() + "\\Settings" + $"\\{settingsFileName}";
    public static string repositoryPath { get; set; } = Directory.GetCurrentDirectory() + "\\Repository";

   
    
    
    public static bool useTaskSeparator { get; set; } = false;
}