
using System.Collections.Generic;
using System;

namespace Theblueway.Core.Runtime.DataStructures
{
    /// <summary>
    /// This data structure is a List of Lists for prefab or sceneplaced variants/overrides, 
    /// so that they can contribute to a list of items without overriding the property of the original prefab.
    /// This way a prefab variant / sceneplaced object will still receive the changes made in the prefab they are originating from, 
    /// while they can also add their own items to the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class InheritableList<T>
    {
        public List<ListHolder<T>> _listChain;
        public List<ListHolder<T>> listChain {
            get
            {
                InitIfNeeded();
                return _listChain;
            }
        }


        public void InitIfNeeded()
        {
            if (_listChain == null) _listChain = new();
            if(_listChain.Count == 0) _listChain.Add(new ListHolder<T>() { Items = new List<T>() });
        }

        public IEnumerable<T> CombinedItems {
            get
            {
                foreach (var list in listChain)
                {
                    foreach (var item in list.Items)
                    {
                        yield return item;
                    }
                }
            }
        }

        public List<T> PersonalItems {
            get
            {
                if (listChain == null || listChain.Count == 0) return null;

                return listChain[^1].Items;
            }
        }

        public IEnumerable<T> InheritedItems {
            get
            {
                for (int i = 0; i < listChain.Count - 1; i++)
                {
                    var list = listChain[i];

                    foreach (var item in list.Items)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    [Serializable]
    public class ListHolder<T>
    {
        public List<T> Items;
    }
}
