using System.Collections.Generic;

namespace Revy.Framework
{
    public static class Tick
    {
        private static readonly TickHandler<ITick> _tickHandler = new TickHandler<ITick>();

        public static void Add(ITick obj)
        {
            _tickHandler.Add(obj);
        }

        public static void Remove(ITick obj)
        {
            _tickHandler.Remove(obj);
        }

        internal static void DoTick()
        {
            ITick[] objectsArray = _tickHandler.ObjectsArray;
            int objectsCount = _tickHandler.ObjectsCount;

            for (int i = 0; i < objectsCount; ++i)
            {
                ITick fObject = objectsArray[i];
                fObject?.Tick();
            }
        }

        internal static void Reset()
        {
            _tickHandler.Reset();
        }
    }
}