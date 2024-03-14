using SvgViewer.Messenger.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.Messenger
{
    public class Messenger<T>
    {
        public static Messenger<T> Default { get; private set; } = new Messenger<T>();

        private Dictionary<string, List<T>> Mailbox = new();
        public event EventHandler<T> MessagePushed;

        private Messenger()
        {

        }

        public void PosteRestante(T value, string addressee)
        {
            if (Mailbox.ContainsKey(addressee))
            {
                Mailbox[addressee].Add(value);
                MessagePushed?.Invoke(this, value);
                return;
            }

            var mailList = new List<T>() { value };

            Mailbox[addressee] = mailList;
            MessagePushed?.Invoke(this, value);
        }

        public Response<T> Claim(string addressee)
        {
            if (Mailbox.ContainsKey(addressee))
            {
                var mailList = Mailbox[addressee];

                Mailbox.Remove(addressee);

                return new Response<T>() { Empty = false, MailList = mailList };
            }

            return new Response<T>() { Empty = true };
        }

    }
}
