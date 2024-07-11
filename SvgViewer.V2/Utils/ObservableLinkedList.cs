using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgViewer.V2.Utils
{
    public class ObservableLinkedList<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        private readonly LinkedList<T> _linkedList = [];

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count => _linkedList.Count;

        public void AddLast(T value)
        {
            _linkedList.AddLast(value);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddFirst(T value)
        {
            _linkedList.AddFirst(value);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveLast()
        {
            _linkedList.RemoveLast();
            
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
