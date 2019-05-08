/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 10 / 9 / 2017
 * Description: 
 */

#if UNITY_EDITOR || UNITY_WINDOWS
#define DEBUG_EDITOR
#elif UNITY_ANDROID
#define DEBUG_ANDROID
#endif

//#define DEBUG_ANDROID

using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
#if PUBLIC_RELEASE
    [CDisableAutoInstantiationAttribute]
#endif
    public class FDebug : FClass, ISubsystem, IDebug, ITick, IInitializable
    {
        #region Fields

        private GameObject _debugCanvas;
        private const string LOG_TAG = "FDebug";
#pragma warning disable 414, 649
        private FTerminal _terminal;
        private FFPSCounter _fpsCounter;
#pragma warning restore 414, 649

        #endregion

        #region Properties

        bool IActiveable.IsActive { get; set; }

        #endregion

        #region Methods

        public Type ServiceType => typeof(IDebug);

        public bool HasInitialized { get; set; }
     
        public void Initialize()
        {
            var debugGo = Resources.Load<GameObject>("Debug");
            if (debugGo == null)
            {
                CLog.Warning("Debug prefab is not found.", category: LOG_TAG);
            }

            _debugCanvas = Persistent.Instantiate(debugGo, PersistentSubCategories.OTHER);
        }

        public Task BeginPlay()
        {
            if (_debugCanvas == null) return Task.CompletedTask;

            _terminal = _debugCanvas.transform.Find("Terminal").gameObject.AddComponent<FTerminal>();

            _fpsCounter = _debugCanvas.transform.Find("FPS").gameObject.AddComponent<FFPSCounter>();

            return Task.CompletedTask;
        }

        public void Tick()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleTerminal();
            }
#elif UNITY_ANDROID && !PUBLIC_RELEASE
            if (Input.touchCount == 3 && Input.GetTouch(2).phase == TouchPhase.Began)
            {
                ToggleTerminal();
            }

#endif
        }

        #endregion

        #region Methods

        public void ToggleTerminal()
        {
            bool state = _terminal.gameObject.activeInHierarchy ? false : true;

            _terminal.gameObject.SetActive(state);
        }

        #endregion

    }
}