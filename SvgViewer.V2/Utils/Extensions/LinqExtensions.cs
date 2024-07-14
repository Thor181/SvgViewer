using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Utils.Extensions
{
    public static class LinqExtensions
    {
        public static bool ContainsAny<T>(this IEnumerable<T> collection, IEnumerable<T> comparableCollection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            foreach (var item in collection)
            {
                if (comparableCollection.Contains(item))
                    return true;
            }

            return false;
        }
    }
}
