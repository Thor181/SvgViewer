using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Utils
{
    public abstract class SuccessGrowlInfo : GrowlInfo
    {
        public static readonly GrowlInfo Instance;

        static SuccessGrowlInfo()
        {
            Instance = new GrowlInfo()
            {
                Message = "Скопировано",
                WaitTime = 1
            };
        }
    }
}
