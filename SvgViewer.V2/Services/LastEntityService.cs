using SvgViewer.V2.Models;
using SvgViewer.V2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SvgViewer.V2.Services
{
    public class LastEntityService
    {
        private readonly string _path = string.Empty;

        private static readonly JsonSerializerOptions _jsonSerializerOptions;

        static LastEntityService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public LastEntityService(string configPath)
        {
            _path = configPath;
        }

        public string[] Load()
        {
            var lastEntities = LoadLastEntities();

            if (lastEntities == null || lastEntities.Count == 0)
                return Array.Empty<string>();

            return lastEntities.Select(x => x.Path).ToArray();
        }

        public void Save(string path, bool removeLast)
        {
            var lastEntities = LoadLastEntities()
                ?? [];

            if (ContainsWithIgnoreCase(lastEntities, path))
                return;

            var lastEntitiesEnumerable = lastEntities.Prepend(new LastEntity(path))
                                                     .Select(x => x.Path);

            if (removeLast)
            {
                var lastEntry = lastEntitiesEnumerable.Last();
                lastEntitiesEnumerable = lastEntitiesEnumerable.TakeWhile(x => lastEntry != x);
            }

            var array = lastEntitiesEnumerable.ToArray();

            SaveLastEntitiesInternal(array);
        }

        public void Move(string path, Placement placement)
        {
            var lastEntities = LoadLastEntities();

            if (lastEntities == null || lastEntities.Count == 0)
                return;

            if (!ContainsWithIgnoreCase(lastEntities, path))
                return;

            lastEntities.RemoveAll(x =>  x.Path == path);

            if (placement == Placement.End)
                lastEntities.Add(new LastEntity(path));
            else if (placement == Placement.Begin)
                lastEntities = lastEntities.Prepend(new LastEntity(path)).ToList();

            SaveLastEntities(lastEntities);
        }

        public void Remove(string path)
        {
            var lastEntities = LoadLastEntities();

            if (lastEntities == null || lastEntities.Count == 0)
                return;

            if (!ContainsWithIgnoreCase(lastEntities, path))
                return;

            lastEntities.RemoveAll(x => x.Path == path);

            SaveLastEntities(lastEntities);
        }

        private void SaveLastEntities(IEnumerable<LastEntity> lastEntities)
        {
            var array = lastEntities.Select(x => x.Path).ToArray();

            SaveLastEntitiesInternal(array);
        }

        private void SaveLastEntitiesInternal(string[] paths)
        {
            var json = JsonSerializer.Serialize(paths, _jsonSerializerOptions);

            File.WriteAllText(_path, json);
        }

        private List<LastEntity>? LoadLastEntities()
        {
            var directory = Path.GetDirectoryName(_path)
                ?? throw new InvalidOperationException($"Directory is not evaluated from path: {_path}");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(_path))
                return null;

            var json = File.ReadAllText(_path);
            var lastDirectories = JsonSerializer.Deserialize<string[]>(json, _jsonSerializerOptions);

            return lastDirectories?.Select(x => new LastEntity(x)).ToList();
        }

        private static bool ContainsWithIgnoreCase(List<LastEntity> lastFiles, string path)
        {
            foreach (var item in lastFiles)
            {
                if (string.Equals(item.Path, path, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
