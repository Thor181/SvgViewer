using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SvgViewer
{
    public class Settings
    {
        public List<string> LastDirectories { get; set; } = new List<string>();
        public List<string> LastFiles { get; set; } = new List<string>();
        public int MaxCountLastFiles { get; set; } = 20;

        public string Serialize()
        {
            return JsonSerializer.Serialize(this, typeof(Settings));
        }

        public Settings Deserialize(string json)
        {
            var settings = JsonSerializer.Deserialize<Settings>(json);

            LastDirectories = settings.LastDirectories;
            LastFiles = settings.LastFiles;
            MaxCountLastFiles = settings.MaxCountLastFiles;

            return this;
        }
    }
}
