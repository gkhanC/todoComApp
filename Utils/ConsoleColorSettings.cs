using System;
using Newtonsoft.Json;
using System.IO;

namespace todoCOM.Utils
{
    [Serializable]
    public class ConsoleColorSettings
    {
        [JsonProperty(nameof(SuccessNotifyColor))]
        public ConsoleColorData SuccessNotifyColor { get; set; } = new ConsoleColorData(ConsoleColor.Green);

        [JsonProperty(nameof(FailedNotifyColor))]
        public ConsoleColorData FailedNotifyColor { get; set; } = new ConsoleColorData(ConsoleColor.Red);

        [JsonProperty(nameof(WarningNotifyColor))]
        public ConsoleColorData WarningNotifyColor { get; set; } = new ConsoleColorData(ConsoleColor.Yellow);

        [JsonProperty(nameof(SeparatorColor))]
        public ConsoleColorData SeparatorColor { get; set; } = new ConsoleColorData(ConsoleColor.DarkMagenta);

        [JsonProperty(nameof(IdColor))]
        public ConsoleColorData IdColor { get; set; } = new ConsoleColorData(ConsoleColor.Yellow);

        [JsonProperty(nameof(TagColor))]
        public ConsoleColorData TagColor { get; set; } = new ConsoleColorData(ConsoleColor.DarkYellow);

        [JsonProperty(nameof(TitleColor))]
        public ConsoleColorData TitleColor { get; set; } = new ConsoleColorData(ConsoleColor.White);

        [JsonProperty(nameof(DoneTitleColor))]
        public ConsoleColorData DoneTitleColor { get; set; } = new ConsoleColorData(ConsoleColor.Magenta);

        [JsonProperty(nameof(CategoryColor))]
        public ConsoleColorData CategoryColor { get; set; } = new ConsoleColorData(ConsoleColor.DarkBlue);

        [JsonProperty(nameof(CreateDateColor))]
        public ConsoleColorData CreateDateColor { get; set; } = new ConsoleColorData(ConsoleColor.Blue);

        [JsonProperty(nameof(DueDateColor))]
        public ConsoleColorData DueDateColor { get; set; } = new ConsoleColorData(ConsoleColor.DarkGreen);

        [JsonProperty(nameof(DefaultColor))]
        public ConsoleColorData DefaultColor { get; set; } = new ConsoleColorData(ConsoleColor.White);

        [JsonProperty(nameof(SettingFileName))]
        public string SettingFileName { get; set; } = "consoleColorSetting.json";

        [JsonProperty(nameof(SettingFilePath))]
        public string SettingFilePath { get; set; } = Directory.GetCurrentDirectory() + "\\Settings";

        private void CreateToSave()
        {
            if (!Directory.Exists(SettingFilePath))
            {
                Directory.CreateDirectory(SettingFilePath);
            }

            if (!File.Exists(SettingFilePath + "\\" + SettingFileName))
            {
                File.Create(SettingFilePath + "\\" + SettingFileName).Close();
            }
        }

        public void Save()
        {
            CreateToSave();
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(SettingFilePath + "\\" + SettingFileName, jsonString);
        }

        public void Load()
        {
            string filePath = SettingFilePath + "\\" + SettingFileName;

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                try
                {
                    ConsoleColorSettings settings = JsonConvert.DeserializeObject<ConsoleColorSettings>(jsonString);

                    SuccessNotifyColor = settings.SuccessNotifyColor;
                    FailedNotifyColor = settings.FailedNotifyColor;
                    WarningNotifyColor = settings.WarningNotifyColor;
                    SeparatorColor = settings.SeparatorColor;
                    IdColor = settings.IdColor;
                    TagColor = settings.TagColor;
                    TitleColor = settings.TitleColor;
                    DoneTitleColor = settings.DoneTitleColor;
                    CategoryColor = settings.CategoryColor;
                    CreateDateColor = settings.CreateDateColor;
                    DueDateColor = settings.DueDateColor;
                    DefaultColor = settings.DefaultColor;
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                CreateToSave();
                Save();
            }
        }

        public ConsoleColorSettings()
        {
        }

        public class ConsoleColorData
        {
            [JsonProperty("foregroundColor")] public ConsoleColor ForegroundColor { get; set; }
            [JsonProperty("backgroundColor")] public ConsoleColor BackgroundColor { get; set; }

            public ConsoleColorData(ConsoleColor foreground = ConsoleColor.White,
                ConsoleColor background = ConsoleColor.Black)
            {
                ForegroundColor = foreground;
                BackgroundColor = background;
            }
        }
    }
}