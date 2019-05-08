using System.Collections.Generic;

namespace Revy.Framework
{
    public static class LateTick
    {
        private static readonly TickHandler<ILateTick> _tickHandler = new TickHandler<ILateTick>();

        public static void Add(ILateTick obj)
        {
            _tickHandler.Add(obj);
        }

        public static void Remove(ILateTick obj)
        {
            _tickHandler.Remove(obj);
        }

        internal static void DoLateTick()
        {
            ILateTick[] objectsArray = _tickHandler.ObjectsArray;
            int objectsCount = _tickHandler.ObjectsCount;

            for (int i = 0; i < objectsCount; ++i)
            {
                ILateTick fObject = objectsArray[i];
                fObject?.LateTick();
            }
        }

        internal static void Reset()
        {
            _tickHandler.Reset();
        }
    }
}