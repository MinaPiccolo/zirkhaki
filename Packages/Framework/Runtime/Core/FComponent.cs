using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
    /// <summary>
    /// FComponent is the base class from which every framework component(Attachable to a game object) script derives.
    /// </summary>
    public abstract class FComponent : MonoBehaviour
    {
        #region Fields

        private bool _hasBeenRegistred;

        #endregion

        #region MonoBehaviour

        protected virtual void Awake()
        {
            MFramework.Register(this);
            _hasBeenRegistred = enabled;
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
            if (!_hasBeenRegistred)
            {
                MFramework.Register(this);
                _hasBeenRegistred = true;
            }
        }

        protected virtual void OnDisable()
        {
            MFramework.UnRegister(this);
            _hasBeenRegistred = false;
        }

        /// <summary>
        /// Note that iOS applications are usually suspended and do not quit. 
        /// You should tick "Exit on Suspend" in Player settings for iOS builds to cause the game to quit and not suspend, 
        /// otherwise you may not see this call. If "Exit on Suspend" is not ticked then you will see calls to OnApplicationPause instead.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            //We do this here because at OnDestroy() it is possible that persistent game object is deactivate
            // and we need Invoke FComponent.Shutown() with StartCoroutin
            MFramework.UnRegister(this);
        }

        /// <summary>
        /// Use this method with care inside child objects.
        /// Always invoke base method.
        /// </summary>
        protected virtual void OnDestroy()
        {
            MFramework.UnRegister(this);
        }

        #endregion

        #region Public Interface

        /// <summary>
        /// Use as parameter for StartCoroutine() method.
        /// e.g StartCoroutine( CoroutineWithCallback(method with IEnumerator return type, finish callback ) )
        /// </summary>
        /// <param gameObjectName="routine"></param>
        /// <param gameObjectName="callback"></param>
        /// <param name="routine"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator CoroutineWithCallback(IEnumerator routine, Action callback)
        {
            yield return StartCoroutine(routine);
            callback();
        }

        /// <summary>
        /// Invoke callback when coroutine finished.
        /// </summary>
        /// <param gameObjectName="routine"></param>
        /// <param gameObjectName="callback"></param>
        /// <param name="yieldInstruct"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator CoroutineWithCallback(YieldInstruction yieldInstruct, Action callback)
        {
            yield return yieldInstruct;
            callback();
        }

        /// <summary>
        /// Wraps task functionality into the coroutine scheme. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public IEnumerator TaskToCoroutineAsync(Task task)
        {
            if (task != null)
                yield return new WaitUntil(() => task.IsCompleted);
        }

        /// <summary>
        /// Wraps task functionality into the coroutine scheme. 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public IEnumerator TaskToCoroutineAsync<T>(Task<T> task)
        {
            yield return new WaitUntil(() => task.IsCompleted);
        }

        /// <summary>
        /// Wraps coroutine functionality into the async/task scheme. 
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public async Task CoroutineToTaskAsync(IEnumerator coroutine)
        {
            var tcs = new TaskCompletionSource<object>();
            StartCoroutine(CoroutineWithCallback(coroutine, () => tcs.TrySetResult(null)));
            await tcs.Task;
        }

        /// <summary>
        /// Wraps coroutine functionality into the async/task scheme. 
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="yieldInstruct"></param>
        /// <returns></returns>
        public async Task CoroutineToTaskAsync(YieldInstruction yieldInstruct)
        {
            TaskCompletionSource<System.Object> tcs = new TaskCompletionSource<object>();
            StartCoroutine(CoroutineWithCallback(yieldInstruct, () => tcs.TrySetResult(null)));
            await tcs.Task;
        }

        #endregion

        #region Protected

        /// <summary>
        /// Halt any async operation.
        /// </summary>
        /// <returns></returns>
        protected async Task HaltProccess()
        {
            await CoroutineToTaskAsync(new WaitUntil(() => false));
        }

        #endregion

    }
}