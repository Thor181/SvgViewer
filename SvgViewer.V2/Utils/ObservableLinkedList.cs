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

        public T? First
        {
            get
            {
                var node = _linkedList.First;

                if (node == null)
                    return default;

                var value = node.Value;

                return value;
            }
        }

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

        public void Move(T value, Placement placement)
        {
            if (value == null) 
                throw new ArgumentNullException(nameof(value));

            if (!_linkedList.Contains(value))
                throw new InvalidOperationException("The collection does not contain the passed value");

            _linkedList.Remove(value);

            AddFirst(value);
        }

        public void RemoveLast()
        {
            _linkedList.RemoveLast();
            
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Remove(T value)
        {
            _linkedList.Remove(value);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            _linkedList.Clear();

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
