using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Object = System.Object;

namespace Revy.Framework
{
    public sealed partial class MFramework
    {
        private static  class Initialization
        {
            private struct MetaData
            {
                public MetaData(IInitializable obj, int order)
                {
                    Initilizable = obj;
                    ExecutionOrder = order;
                }

                public readonly IInitializable Initilizable;
                public readonly int ExecutionOrder;
            }

            #region Field

            /// <summary>
            /// Contains list of objects that pending to initialize.
            /// </summary>
            private static List<IInitializable> _pendingObjects = new List<IInitializable>();

            /// <summary>
            /// This list contains objects that are instantiated before framework is starting.
            /// </summary>
            private static List<IInitializable> _delayedInitObjects;

            #endregion Field

            #region Class Interface

            public static void Initialize(IReadOnlyList<object> specificObjects = null)
            {
                List<IInitializable> objectsToInit;
                List<IInitializable> notSpecificObjects = null;
                if (specificObjects != null && specificObjects.Count > 0)
                {
                    objectsToInit = new List<IInitializable>();
                    for (int i = 0; i < specificObjects.Count; i++)
                    {
                        Object obj = specificObjects[i];
                        if (obj == null || !(obj is IInitializable iObj)) continue;
                        objectsToInit.Add(iObj);
                        _pendingObjects.Remove(iObj);
                    }
                    _Sort(ref objectsToInit);
                    notSpecificObjects = new List<IInitializable>(_pendingObjects);
                }
                else
                {
                    if (_pendingObjects.Count <= 0) return;
                    objectsToInit = new List<IInitializable>(_pendingObjects);
                }

                _InvokeBeginPlay(objectsToInit);

                if (notSpecificObjects != null && notSpecificObjects.Count > 0)
                {
                    _pendingObjects.AddRange(notSpecificObjects);
                    _Sort(ref _pendingObjects);
                }
            }

            public static async Task InitializeAsync(IReadOnlyList<object> specificObjects = null)
            {
                List<IInitializable> objectsToInit;
                List<IInitializable> notSpecificObjects = null;
                if (specificObjects != null && specificObjects.Count > 0)
                {
                    objectsToInit = new List<IInitializable>();
                    for (int i = 0; i < specificObjects.Count; i++)
                    {
                        Object obj = specificObjects[i];
                        if (obj == null || !(obj is IInitializable iObj)) continue;
                        objectsToInit.Add(iObj);
                        _pendingObjects.Remove(iObj);
                    }
                    _Sort(ref objectsToInit);
                    notSpecificObjects = new List<IInitializable>(_pendingObjects);
                }
                else
                {
                    if (_pendingObjects.Count <= 0) return;
                    objectsToInit = new List<IInitializable>(_pendingObjects);
                }

                await _InvokeBeginPlayAwaitable(objectsToInit);

                if (notSpecificObjects != null && notSpecificObjects.Count > 0)
                {
                    _pendingObjects.AddRange(notSpecificObjects);
                    _Sort(ref _pendingObjects);
                }
            }

            /// <summary>
            /// Delayed objects are objects that invocation of their Initialize method does not happed because
            /// they are instantiated before framework startup(Objects existing in scene).
            /// </summary>
            public static void InitializeDelayedObjects()
            {
                if (_delayedInitObjects == null) return;
                Injection.Inject(_delayedInitObjects);
                int objectsCount = _delayedInitObjects.Count;
                for (int i = 0; i < objectsCount; i++)
                {
                    IInitializable obj = _delayedInitObjects[i];
                    obj?.Initialize();
                }

                _delayedInitObjects.Clear();
            }

            /// <summary>
            /// Add an object to pending initialization objects.
            /// </summary>
            /// <param name="obj"></param>
            public static void AddToInitializeList(IInitializable obj)
            {
                if (obj == null)
                {
                    CLog.Warning($"{LOG_TAG} Can not register null object.");
                    return;
                }

                if (_pendingObjects.Contains(obj)) return;

                _pendingObjects.Add(obj);
                _Sort(ref _pendingObjects);
            }

            public static void AddToDelayedInitializeList(IInitializable obj)
            {
                if (_delayedInitObjects == null) _delayedInitObjects = new List<IInitializable>();
                if (_delayedInitObjects.Contains(obj)) return;

                _delayedInitObjects.Add(obj);
                _Sort(ref _delayedInitObjects);
            }

            public static void Reset()
            {
                _pendingObjects.Clear();
                _delayedInitObjects?.Clear();
            }

            #endregion Class Interface

            #region Helpers

            private static void _InvokeBeginPlay(List<IInitializable> objectsToInit)
            {
                _pendingObjects.Clear();
                Injection.Inject();
                for (int i = 0; i < objectsToInit.Count; ++i)
                {
                    IInitializable obj = objectsToInit[i];
                    if (obj == null || obj.HasInitialized) continue;
                    obj.BeginPlay();
                    obj.HasInitialized = true;
                    Injection.Inject();
                    if (_pendingObjects.Count > 0)
                    {
                        objectsToInit.AddRange(_pendingObjects);
                    }
                }
                _pendingObjects.Clear();
            }

            private static async Task _InvokeBeginPlayAwaitable(List<IInitializable> objectsToInit)
            {
                _pendingObjects.Clear();
                Injection.Inject();
                for (int i = 0; i < objectsToInit.Count; ++i)
                {
                    IInitializable obj = objectsToInit[i];
                    if (obj == null || obj.HasInitialized) continue;
                    await obj.BeginPlay();
                    obj.HasInitialized = true;
                    Injection.Inject();
                    if (_pendingObjects.Count > 0)
                    {
                        objectsToInit.AddRange(_pendingObjects);
                    }
                }
                _pendingObjects.Clear();
            }

            private static void _Sort(ref List<IInitializable> objectsToSort)
            {
                int count = objectsToSort.Count;
                List<MetaData> tmpMetaData = new List<MetaData>();
                for (int i = 0; i < count; i++)
                {
                    IInitializable obj = objectsToSort[i];
                    int order = obj is IExecutionOrder ? ((IExecutionOrder) obj).ExecutionOrder : 0;
                    tmpMetaData.Add(new MetaData(obj, order));
                }

                objectsToSort = tmpMetaData
                    .OrderBy(o => o.ExecutionOrder)
                    .Select(o => o.Initilizable).ToList();
            }

            #endregion
        }
    }
}