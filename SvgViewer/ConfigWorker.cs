using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer
{
    public class ConfigWorker
    {
        private readonly string _documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly string _configFilePath;
        private Settings Settings;

        public List<string> LastDirectories { get => Settings.LastDirectories; }
        public int MaxCountLastFiles
        {
            get => Settings.MaxCountLastFiles; 
            set
            {
                Settings.MaxCountLastFiles = value;
                Save();
            }
        }
        public List<string> LastFiles { get => Settings.LastFiles; }

        private string ConfigDirectoryPath
        {
            get
            {
                var path = Path.Combine(_documentsPath, "SvgViewer");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public ConfigWorker()
        {
            _configFilePath = Path.Combine(ConfigDirectoryPath, "config.json");

            if (!ConfigExists())
                CreateConfig();

            Settings = Load();
        }

        private bool ConfigExists()
        {
            return File.Exists(_configFilePath);
        }

        private Settings CreateConfig()
        {
            Settings = new Settings();
            Save();
            return Load();
        }

        public void AddLastDirectory(string path)
        {
            Settings.LastDirectories.Add(path);
            Save();
        }

        public void AddLastFiles(string path)
        {
            if (Settings.LastFiles.Count == MaxCountLastFiles)
                Settings.LastFiles.RemoveAt(MaxCountLastFiles - 1);

            Settings.LastFiles.Add(path);
            Save();
        }

        public List<string> GetLastDirectoris()
        {
            return Settings.LastDirectories;
        }

        #region Save/Load
        public void Save()
        {
            var json = Settings.Serialize();
            File.WriteAllText(_configFilePath, json);
        }

        public Settings Load()
        {
            var text = File.ReadAllText(_configFilePath);
            var settings = new Settings();
            settings.Deserialize(text);
            return settings;
        }
        #endregion  
    }
}

