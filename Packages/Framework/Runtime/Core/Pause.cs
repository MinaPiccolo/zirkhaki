using System.Collections.Generic;

namespace Revy.Framework
{
    public sealed partial class MFramework
    {
        private static class Pause
        {
            #region Fields

            private static readonly List<IPauseable> _objects = new List<IPauseable>();

            #endregion Fields

            #region Properties

            public static bool IsPaused { get; private set; }

            #endregion Properties

            #region Public Interface

            public static void AddToPauseableList(IPauseable pauseableObject)
            {
                if (!_objects.Contains(pauseableObject))
                {
                    _objects.Add(pauseableObject);
                }
            }

            public static void Start()
            {
                if (!IsPlayBegun || IsPaused || _objects.Count <= 0) return;

                int objectsCount = _objects.Count;
                for (int i = 0; i < objectsCount; ++i)
                {
                    _objects[i].OnPause();
                }
            }

            public static void Stop()
            {
                if (!IsPlayBegun || !IsPaused || _objects.Count <= 0) return;

                int objectsCount = _objects.Count;
                for (int i = 0; i < objectsCount; ++i)
                {
                    _objects[i].OnResume();
                }
            }

            public static void Reset()
            {
                _objects.Clear();
                IsPaused = false;
            }

            public static void Remove(IPauseable obj)
            {
                _objects.Remove(obj);
            }

            #endregion Public Interface

        }
    }
}