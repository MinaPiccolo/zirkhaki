using System.Collections.Generic;

namespace Revy.Framework
{
    public static class EndOfFrameTick
    {
        private static readonly TickHandler<IEndOfFrameTick> _tickHandler = new TickHandler<IEndOfFrameTick>();

        public static void Add(IEndOfFrameTick obj)
        {
            _tickHandler.Add(obj);
        }

        public static void Remove(IEndOfFrameTick obj)
        {
            _tickHandler.Remove(obj);
        }

        internal static void DoEndOfFrameTick()
        {
            IEndOfFrameTick[] objectsArray = _tickHandler.ObjectsArray;
            int objectsCount = _tickHandler.ObjectsCount;

            for (int i = 0; i < objectsCount; ++i)
            {
                IEndOfFrameTick fObject = objectsArray[i];
                fObject?.EndOfFrameTick();
            }
        }

        internal static void Reset()
        {
            _tickHandler.Reset();
        }
    }
}