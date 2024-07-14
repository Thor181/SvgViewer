using SvgViewer.V2.Models.Cache;
using SvgViewer.V2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SvgViewer.V2.Services
{
    public class CacheService
    {
        private static readonly string _dataDirectoryPath = Path.Combine(Constants.Paths.RootDirectory, "Cache", "Data");
        private static readonly string _configDirectoryPath = Path.Combine(Constants.Paths.RootDirectory, "Cache", "Config");
        private static readonly string _mapFullPath = Path.Combine(_configDirectoryPath, "Map.json");

        private readonly CacheMap _cacheMap;

        public CacheService()
        {
            if (!Directory.Exists(_dataDirectoryPath))
                Directory.CreateDirectory(_dataDirectoryPath);

            if (!Directory.Exists(_configDirectoryPath))
                Directory.CreateDirectory(_configDirectoryPath);

            _cacheMap = LoadCacheMap();
        }

        public void Cache(byte[] data, string name)
        {
            var newName = Guid.NewGuid().ToString("n") + Path.GetExtension(name);

            var hashString = GetHashString(name);

            if (string.IsNullOrEmpty(hashString))
                return;

            if (_cacheMap.ContainsKey(hashString))
                _cacheMap[hashString] = newName;
            else
                _cacheMap.Add(hashString, newName);

            SaveCacheMap(); 

            File.WriteAllBytes(_dataDirectoryPath + newName, data);
        }

        public byte[]? LoadFromCache(string name)
        {
            var hashString = GetHashString(name);

            if (string.IsNullOrEmpty(hashString))
                return null;

            if (_cacheMap.TryGetValue(hashString, out string? value) && File.Exists(value))
                return File.ReadAllBytes(Path.Combine(_dataDirectoryPath, value));

            return null;
        }

        public bool TryLoadFromCache(string name, out byte[] data)
        {
            data = LoadFromCache(name) ?? Array.Empty<byte>();

            return data.Length > 0;
        }

        private string GetHashString(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var hash = MD5.HashData(Encoding.UTF8.GetBytes(name));
            var hashString = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return hashString;
        }

        private CacheMap LoadCacheMap()
        {
            if (!File.Exists(_mapFullPath))
                return new CacheMap();

            var cacheMapJson = File.ReadAllText(_mapFullPath);

            var cacheMap = JsonSerializer.Deserialize<CacheMap>(cacheMapJson);

            if (cacheMap == null)
                return new CacheMap();

            return cacheMap;
        }

        private void SaveCacheMap()
        {
            var cacheMapJson = JsonSerializer.Serialize(_cacheMap);

            File.WriteAllText(_mapFullPath, cacheMapJson);
        }
    }
}
