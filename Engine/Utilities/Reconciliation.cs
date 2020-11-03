using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IReconciler<T>
    {
        int Compare(T x, T y);
        T Add(T y);
        T Update(T x, T y);
        void Delete(ref T[] data, int i);
    }
    
    public static class Reconciler
    {
        /// <summary>
        /// Reconcile an array with another array
        /// </summary>
        /// <typeparam name="T">The type of the arrays.</typeparam>
        /// <param name="self">The array to serve as the base</param>
        /// <param name="others">The array to reconcile with</param>
        /// <param name="reconciler">The IReconciler with the ability to perform the callbacks for reconciliation</param>
        /// <returns></returns>
        public static void Reconcile<T>(ref T[] self, T[] others, IReconciler<T> reconciler)
        {
            int index1 = 0;
            int index2 = 0;
            var added = new List<T>();
            if (others.Length == 0) // Delete All
            {
                self = new T[0];
                return;
            }
            if (self.Length == 0) // Add All
            {
                foreach (var other in others)
                {
                    added.Add(reconciler.Add(other));
                }
            }
            else
            {
                while (index1 < self.Length && index2 < others.Length)
                {
                    int compare = reconciler.Compare(self[index1], others[index2]);
                    if(compare == 0)
                    {
                        self[index1] = reconciler.Update(self[index1], others[index2]);
                        index1++;
                        index2++;
                    }
                    else if (compare < 0)
                    {
                        reconciler.Delete(ref self, index1);
                        index1++;
                    }
                    else if (compare > 0)
                    {
                        //Add
                        added.Add(reconciler.Add(others[index2]));
                        index2++;
                    }
                }
            }

            int addedCount = added.Count;
            Array.Resize(ref self, self.Length + addedCount);
            for (int i = 0; i < addedCount; i++)
            {
                self[index1 + i] = added[i];
            }
        }
    }
}