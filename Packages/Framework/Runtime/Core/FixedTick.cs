namespace Revy.Framework
{
    public static class FixedTick
    {
        private static readonly TickHandler<IFixedTick> _tickHandler = new TickHandler<IFixedTick>();

        public static void Add(IFixedTick obj)
        {
            _tickHandler.Add(obj);
        }

        public static void Remove(IFixedTick obj)
        {
            _tickHandler.Remove(obj);
        }

        internal static void DoFixedTick()
        {
            IFixedTick[] objectsArray = _tickHandler.ObjectsArray;
            int objectsCount = _tickHandler.ObjectsCount;

            for (int i = 0; i < objectsCount; ++i)
            {
                IFixedTick fObject = objectsArray[i];
                fObject?.FixedTick();
            }
        }

        internal static void Reset()
        {
            _tickHandler.Reset();
        }
    }
}