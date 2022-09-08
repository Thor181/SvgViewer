using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var lastDirs = ReadFromFile();
            if (!lastDirs.Contains(path))
            {
                File.AppendAllText(PathsFile, path + "\n");
            }
        }
        public static IEnumerable<string> ReadFromFile()
        {
            if (!File.Exists(PathsFile))
                return null;

            var lastDirs = File.ReadAllText(PathsFile).Split("\n").Where(x => !(string.IsNullOrEmpty(x)
                                                                             && string.IsNullOrWhiteSpace(x))).Reverse();
            return lastDirs;
        }
    }
}
