﻿using SvgViewer.V2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var lastDirectories = LoadLastDirectories();

            if (lastDirectories == null || lastDirectories.Count == 0)
                return Array.Empty<string>();

            return lastDirectories.Select(x => x.Path).ToArray();
        }

        public void Save(string path)
        {
            var lastDirectories = LoadLastDirectories() 
                ?? [];

            if (ContainsWithIgnoreCase(lastDirectories, path))
                return;

            lastDirectories.Add(new LastEntity(path));

            var array = lastDirectories.Select(x => x.Path).ToArray();
            var json = JsonSerializer.Serialize(array, _jsonSerializerOptions);

            File.WriteAllText(_path, json);
        }

        private List<LastEntity>? LoadLastDirectories()
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
