using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SvgViewer
{
    public class DirectoriesWorker
    {
        private static string PathsFile
        {
            get
            {
                return $"{Path.GetTempPath()}\\SvgViewerLastDirectories.txt";
            }
        }


        public static void WriteToFile(string path)
        {
            if (path == null)
                return;

            var lastDirs = ReadFromFile();

            if (lastDirs?.Count() > 10)
            {
                if (IsPath(path))
                {
                    lastDirs.ToList().Insert(0, path);
                    File.Delete(PathsFile);
                    File.AppendAllLines(PathsFile, lastDirs.Take(10));
                    return;
                }
            }

            if (IsPath(path))
                File.AppendAllText(PathsFile, path + "\n");
        }

        public static IEnumerable<string> ReadFromFile()
        {
            if (!File.Exists(PathsFile))
                return null;

            var lastDirs = File.ReadAllText(PathsFile)
                .Split("\n")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Reverse();

            return lastDirs;
        }

        private static bool IsPath(string path)
        {
            return path.Contains("\\");
        }
    }
}
