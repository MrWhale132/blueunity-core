using System.Collections.Generic;
using UnityEngine;

namespace Theblueway.Core.DataStructures
{
    public interface IBindable
    {
        public RandomId ObjectId { get; }
    }

    //not thread-safe
    public class SymmetricOneToOneBinder<T1,T2> where T1 : IBindable where T2 : IBindable
    {
        //immutable keys, so dicts wont broke
        public Dictionary<RandomId, T2> __leftToRight;
        public Dictionary<RandomId, T1> __rightToLeft;



        public SymmetricOneToOneBinder()
        {
            __leftToRight = new Dictionary<RandomId, T2>();
            __rightToLeft = new Dictionary<RandomId, T1>();
        }

        public T1 this[T2 key] {
            get => __rightToLeft[key.ObjectId];
            private set => __rightToLeft[key.ObjectId] = value;
        }

        public T2 this[T1 key] {
            get => __leftToRight[key.ObjectId];
            private set => __leftToRight[key.ObjectId] = value;
        }



        public bool IsBound(T1 left)
        {
            return __leftToRight.ContainsKey(left.ObjectId);
        }
        public bool IsBound(T2 right)
        {
            return __rightToLeft.ContainsKey(right.ObjectId);
        }


        public void Bind(T1 left, T2 right)
        {
            if (IsBound(left) || IsBound(right))
            {
                Debug.LogError("OneToOneBinder: Attempt to bind already bound keys");
                return;
            }

            this[left] = right;
            this[right] = left;
        }

        public void Rebind(T1 left, T2 right)
        {
            if (!IsBound(left) && !IsBound(right))
            {
                Debug.LogError("OneToOneBinder: Attempt to rebind keys that are not bound");
                return;
            }

            if (IsBound(left))
            {
                Unbind(left);
            }
            else if (IsBound(right))
            {
                Unbind(right);
            }

            this[left] = right;
            this[right] = left;
        }

        public void Unbind(T1 left)
        {
            if (!IsBound(left))
            {
                Debug.LogError("OneToOneBinder: Attempt to unbind a key that is not bound");
                return  ;
            }

            T2 right = this[left];
            __leftToRight.Remove(left.ObjectId);
            __rightToLeft.Remove(right.ObjectId);
        }

        public void Unbind(T2 right)
        {
            if (!IsBound(right))
            {
                Debug.LogError("OneToOneBinder: Attempt to unbind a key that is not bound");
                return;
            }

            T1 left = this[right];
            __rightToLeft.Remove(right.ObjectId);
            __leftToRight.Remove(left.ObjectId);
        }
    }
}
