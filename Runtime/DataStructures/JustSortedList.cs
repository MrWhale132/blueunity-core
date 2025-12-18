using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets._Project.Scripts.UtilScripts.DataStructures
{
    public class JustSortedList<T> : IEnumerable<T>
    {
        public List<T> _items = new();
        public IComparer<T> _comparer;
        public bool _autoSort;

        //for deser
        public JustSortedList()
        {
            
        }

        public JustSortedList(IComparer<T> comparer, bool autoSort = true)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _autoSort = autoSort;
        }

        public void Add(T item)
        {
            if (_autoSort)
            {
                int index = _items.BinarySearch(item, _comparer);
                if (index < 0) index = ~index;
                _items.Insert(index, item);
            }
            else
            {
                _items.Add(item);
            }
        }

        public void Sort()
        {
            _items.Sort(_comparer);
        }

        public void Remove(T item)
        {
            _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public int Count => _items.Count;

        public T this[int index] => _items[index];

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
