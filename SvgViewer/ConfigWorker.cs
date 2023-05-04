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

        public ConfigWorker()
        {
            _configFilePath = Path.Combine(_documentsPath, "SvgViewer", "config.json");


            if (!ConfigExists())
                CreateConfig();

            Settings = new Settings().Deserialize(_configFilePath);
        }

        private bool ConfigExists()
        {
            return File.Exists(_configFilePath);
        }

        private Settings CreateConfig(Settings settings)
        {
            var setting = settings.Deserialize(_configFilePath);
            return setting;
        }

        public void Save()
        {

        }

        public void Load()
        {

        }

    }
}

