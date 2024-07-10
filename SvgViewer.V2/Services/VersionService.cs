using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Services
{
    public class VersionService
    {
        public string? Version { get; set; }

        public VersionService()
        {
            var assembly = Assembly.GetEntryAssembly() 
                ?? throw new InvalidOperationException("Entry assembly can't be null");
            
            Version = assembly.GetName().Version?.ToString();
        }
    }
}
