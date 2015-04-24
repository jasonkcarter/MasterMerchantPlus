/* From http://stackoverflow.com/questions/25976022/alternative-for-sortedsett-in-portable-class-library
 * By Peter Ritchie
 */

using System.Collections;
using System.Collections.Generic;

namespace MMPlus.Shared.Utility
{
    public class SortedCollection<T> : ICollection<T>
    {
        private readonly List<T> _collection = new List<T>();
        private readonly IComparer<T> _comparer;

        public SortedCollection()
        {
            _comparer = Comparer<T>.Default;
        }

        public SortedCollection(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public void Add(T item)
        {
            if (Count == 0)
            {
                _collection.Add(item);
                return;
            }
            var minimum = 0;
            var maximum = _collection.Count - 1;

            while (minimum <= maximum)
            {
                var midPoint = (minimum + maximum)/2;
                var comparison = _comparer.Compare(_collection[midPoint], item);
                if (comparison == 0)
                {
                    return; // already in the list, do nothing
                }
                if (comparison < 0)
                {
                    minimum = midPoint + 1;
                }
                else
                {
                    maximum = midPoint - 1;
                }
            }
            _collection.Insert(minimum, item);
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}