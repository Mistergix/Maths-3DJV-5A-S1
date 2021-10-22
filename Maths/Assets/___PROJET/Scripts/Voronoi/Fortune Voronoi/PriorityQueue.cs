using System;
using System.Collections.Generic;

namespace ESGI.Voronoi.Fortune_Voronoi
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> data;

        public PriorityQueue()
        {
            this.data = new List<T>();
        }
	
        public int Count => Data.Count;

        public List<T> Data => data;

        public bool IsConsistent()
        {
            if (Data.Count == 0) return true;
            int li = Data.Count - 1; // dernier index
            for (int pi = 0; pi < Data.Count; ++pi) // index parents
            {
                int lci = 2 * pi + 1; // index enfant gauche
                int rci = 2 * pi + 2; // index enfant droit
                if (lci <= li && Data[pi].CompareTo(Data[lci]) > 0) return false;
                if (rci <= li && Data[pi].CompareTo(Data[rci]) > 0) return false;
            }
            return true; 
        }

        public void Enqueue(T item)
        {
            Data.Add(item);
            int ci = Data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if (Data[ci].CompareTo(Data[pi]) >= 0)
                    break;
                T tmp = Data[ci]; Data[ci] = Data[pi]; Data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            if (Data.Count==0) return default(T);
            int li = Data.Count - 1;
            T frontItem = Data[0];
            Data[0] = Data[li];
            Data.RemoveAt(li);

            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li) break;
                int rc = ci + 1;
                if (rc <= li && Data[rc].CompareTo(Data[ci]) < 0)
                    ci = rc;
                if (Data[pi].CompareTo(Data[ci]) <= 0) break;
                T tmp = Data[pi]; Data[pi] = Data[ci]; Data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public bool Empty()
        {
            return Data.Count <= 0;
        }
    }
}