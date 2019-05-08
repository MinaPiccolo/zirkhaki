using System.Collections.Generic;
using System.Linq;

namespace Revy.Framework
{
    internal class TickHandler<T> where T : IActiveable
    {
        private List<T> _objects = new List<T>();

        /// <summary>
        /// Contains array type of '_registeredObjectsForTick' because array is faster to iterate over List.
        /// </summary>
        internal T[] ObjectsArray;

        internal int ObjectsCount;

        internal void Add(T obj)
        {
            if (obj == null || _objects.Contains(obj)) return;

            obj.IsActive = true;
            _objects.Add(obj);
            _Sort();
            _UpdateObjectsCount();
            ObjectsArray = _objects.ToArray();
        }

        internal void Remove(T obj)
        {
            if (obj == null) return;
            obj.IsActive = false;
            _objects.Remove(obj);
            _UpdateObjectsCount();
            ObjectsArray = _objects.ToArray();
        }


        internal void Reset()
        {
            _objects.Clear();
            ObjectsArray = null;
        }

        private void _UpdateObjectsCount()
        {
            if (_objects != null)
                ObjectsCount = _objects.Count;
        }

        private void _Sort()
        {
            int count = _objects.Count;
            List<MetaData<T>> orderedObjects = new List<MetaData<T>>();
            for (int i = 0; i < count; i++)
            {
                T obj = _objects[i];
                int order = obj is IExecutionOrder ? ((IExecutionOrder) obj).ExecutionOrder : 0;
                orderedObjects.Add(new MetaData<T>(obj, order));
            }

            _objects = orderedObjects
                .OrderBy(o => o.ExecutionOrder)
                .Select(o => o.TickObject).ToList();
        }

        private struct MetaData<R>
        {
            public MetaData(R obj, int order)
            {
                TickObject = obj;
                ExecutionOrder = order;
            }

            public readonly R TickObject;
            public readonly int ExecutionOrder;
        }
    }
}