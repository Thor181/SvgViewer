using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using SharpVectors.Converters;
using SvgViewer.Command;
using SvgViewer.Messenger;

namespace SvgViewer.Editor
{
    public class EditorViewModel : BaseViewModel
    {
        private Uri data;
        public Uri Data { get => data; set => SetNotifyProperty(ref data, value); }

        private string code;
        public string Code { get => code; set => SetNotifyProperty(ref code, value); }

        private FlowDocument flowDocument;
        public FlowDocument FlowDocument { get => flowDocument; set => SetNotifyProperty(ref flowDocument, value); }

        public EditorViewModel()
        {
            HandleMessenger();
        }

        private void HandleMessenger()
        {
            var response = Messenger<string>.Default.Claim(nameof(EditorViewModel));
            
            if (!response.Empty)
            {
                var raw = response.MailList.SingleOrDefault();
                 
                var text = File.ReadAllText(raw).Split('>').Select(x => x + '>');

                var flowDocument = new FlowDocument();

                var p = new List<Paragraph>();

                foreach (var line in text)
                {
                    var newP = new Paragraph(new Run(line));
                }

                flowDocument.Blocks.AddRange(p);

                FlowDocument = flowDocument;    


                Data = new Uri(raw);
            }
        }
    }
}
