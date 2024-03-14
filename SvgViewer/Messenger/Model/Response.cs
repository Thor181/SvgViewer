using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.Messenger.Model
{
    public class Response<T>
    {
        public bool Empty { get; init; }
        public List<T> MailList { get; set; }
    }
}
