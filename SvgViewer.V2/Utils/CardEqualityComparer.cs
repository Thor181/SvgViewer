using SvgViewer.V2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Utils
{
    public class CardEqualityComparer : IEqualityComparer<Card>
    {
        public static readonly CardEqualityComparer Instance = new CardEqualityComparer();

        public bool Equals(Card? x, Card? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (object.ReferenceEquals(x, y)) 
                return true;

            if (x.FilePath == y.FilePath)
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return obj.GetHashCode();
        }
    }
}
