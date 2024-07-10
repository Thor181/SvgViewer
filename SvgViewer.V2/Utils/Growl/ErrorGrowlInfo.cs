using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Utils.Growl
{
    public abstract class ErrorGrowlInfo : GrowlInfo
    {
        public static readonly GrowlInfo Instance;

        static ErrorGrowlInfo()
        {
            Instance = new GrowlInfo()
            {
                Message = "Не удалось скопировать",
                WaitTime = 1
            };
        }
    }
}
