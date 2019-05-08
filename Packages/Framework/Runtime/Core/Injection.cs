using System.Collections.Generic;

namespace Revy.Framework
{
    public sealed partial class MFramework
    {
        private static class Injection
        {
            #region Fields

            private static readonly List<IInjectable> _objects = new List<IInjectable>();

            #endregion Fields

            #region Public Interface

            /// <summary>
            /// Register an object into the Service locater.
            /// </summary>
            /// <param name="obj"></param>
            public static void Register(IDIRegister obj)
            {
                if (obj != null && obj.ServiceType != null)
                    CServiceLocator.Register(obj.ServiceType, obj);
            }

            /// <summary>
            /// Add an object into the pending injection list.
            /// </summary>
            /// <param name="obj"></param>
            public static void AddToInjectList(IInjectable obj)
            {
                if (!_objects.Contains(obj))
                    _objects.Add(obj);
            }

            public static void Inject(IReadOnlyList<object> specificObjects = null)
            {
                List<IInjectable> objectsToInject = null;
                if (specificObjects != null && specificObjects.Count > 0)
                {
                    objectsToInject = new List<IInjectable>();
                    for (int i = 0; i < specificObjects.Count; i++)
                    {
                        object obj = specificObjects[i];
                        if (obj == null || !(obj is IInjectable iObj)) continue;
                        objectsToInject.Add(iObj);
                    }
                }
                else
                {
                    if (_objects.Count <= 0) return;
                    objectsToInject = new List<IInjectable>(_objects);
                }

                int objectsCount = objectsToInject.Count;
                for (int i = 0; i < objectsCount; ++i)
                {
                    IInjectable obj = objectsToInject[i];
                    if (obj == null) continue;
                    CDependencyInjection.Inject(obj);
                    _objects.Remove(obj);
                }
            }

            /// <summary>
            /// This injection occur at object creation.
            /// At this point it is possible that some objects does not created yet
            /// </summary>
            /// <param name="obj"></param>
            public static void InitialInject(IInjectable obj)
            {
                if (obj != null)
                    CDependencyInjection.Inject(obj);
            }

            public static void Reset()
            {
                _objects.Clear();
            }
            #endregion Public Interface
        }
    }
}