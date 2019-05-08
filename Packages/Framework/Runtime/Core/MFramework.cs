using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

#pragma warning disable 168
namespace Revy.Framework
{
    /// <inheritdoc />
    /// <summary>
    /// MFramework  is the base management class for entire framework system. 
    /// It is lone mono behavior object in entire framework system.
    /// All initialization and ticking behavior of FBase object is controlled here. 
    /// </summary>
    public sealed partial class MFramework : MonoBehaviour
    {
        #region Fields

        private static MFramework _instance;
        private static FSFrameworkConfig _config;
#if LOG
        private static float _startTimeStamp;
#endif

#pragma warning disable 414
        private static readonly string LOG_TAG = "<color=green><b>[Framework]</b></color>";
#pragma warning restore 414

        private static bool _isFrameworkStarted;
        private static readonly WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();

        #endregion

        #region Properties

        public const string ConfigAssetName = "FrameworkConfig";
        public static bool IsFrameworkPaused => Pause.IsPaused;
        public static IGameManager GameManager { get; private set; }
        public static bool IsPlayBegun = false;

        #endregion

        #region Events

        /// <summary>
        /// This event will raise when all BeginPlay() callbacks of all FComponents and FClasses invoked.
        /// This event is called after All BeginPlay() and before first Tick().
        /// </summary>
        public static event Action OnPlayBegun;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Startup()
        {
#if LOG
            _startTimeStamp = Time.realtimeSinceStartup;
#endif
            LoadFrameworkConfig();
            if (_config == null)
            {
                CLog.Error("Framework config not found.", category: LOG_TAG);
                return;
            }

            if (_config.DontStartFramework)
            {
                CLog.Log("Framework is not allowed to start.", category: LOG_TAG);
                return;
            }

            Initialization.Reset();
            Injection.Reset();
            Tick.Reset();
            LateTick.Reset();
            FixedTick.Reset();
            GameSystem.Reset();
            Subsystem.Reset();
            Pause.Reset();

            MFramework[] existingFramework = FindObjectsOfType<MFramework>();
            foreach (var t in existingFramework) DestroyImmediate(t);

            Persistent.CreatePersistentGameObject();
            Persistent.PersistentGameObject.AddComponent<MFramework>();
        }

        private void Start()
        {
            if (IsBuildTypeSpecified())
            {
                StartFramework();
            }
            else
                CLog.Warning($" Framework does not start because build setting does have problem.", category: LOG_TAG);
        }

        private void Update()
        {
            if (!IsPlayBegun) return;
            Tick.DoTick();
            Initialization.Initialize();
        }

        private void LateUpdate()
        {
            if (!IsPlayBegun) return;
            LateTick.DoLateTick();
            Initialization.Initialize();
        }

        private void FixedUpdate()
        {
            if (!IsPlayBegun) return;
            FixedTick.DoFixedTick();
            Initialization.Initialize();
        }

        #endregion

        #region Public Interface

        public static void Register(object obj)
        {
            bool isEnabled = true;
            Component component = null;
            if (obj is MonoBehaviour monoBehavior)
            {
                component = monoBehavior;
                isEnabled = monoBehavior.enabled;
            }

            if (obj is ISubsystem subsystem) Subsystem.Add(subsystem);
            if (obj is IGameSystem gameSystem) GameSystem.Add(gameSystem);
            if (component && obj is IPersistable persistable) Persistent.MakePersist(persistable.ObjectToPersist);
            if (!isEnabled) return;
            if (obj is IDIRegister service) Injection.Register(service);
            if (obj is IInjectable injectable)
            {
                if (_isFrameworkStarted)
                {
                    Injection.InitialInject(injectable);
                    Injection.AddToInjectList(injectable);
                }
            }

            if (obj is ITerminalIndex terminalObj) Terminal.IndexObject(terminalObj);
            if (obj is IInitializable initializable)
            {
                Initialization.AddToInitializeList(initializable);
                if (_isFrameworkStarted)
                    initializable.Initialize();
                else
                    Initialization.AddToDelayedInitializeList(initializable);
            }

            if (obj is IPauseable pauseable) Pause.AddToPauseableList(pauseable);
            if (obj is ITick tickObj)
            {
                tickObj.IsActive = true;
                _instance.StartCoroutine(_DoNextFrame(() =>
                {
                    if (tickObj.IsActive) Tick.Add(tickObj);
                }));
            }

            if (obj is ILateTick lateTickObj)
            {
                lateTickObj.IsActive = true;
                _instance.StartCoroutine(_DoNextFrame(() =>
                {
                    if (lateTickObj.IsActive) LateTick.Add(lateTickObj);
                }));
            }

            if (obj is IFixedTick fixedTickObj)
            {
                fixedTickObj.IsActive = true;
                _instance.StartCoroutine(_DoNextFrame(() =>
                {
                    if (fixedTickObj.IsActive) FixedTick.Add(fixedTickObj);
                }));
            }

            if (obj is IEndOfFrameTick endOfFrameTickObj)
            {
                endOfFrameTickObj.IsActive = true;
                _instance.StartCoroutine(_DoNextFrame(() =>
                {
                    if (endOfFrameTickObj.IsActive) EndOfFrameTick.Add(endOfFrameTickObj);
                }));
            }
        }

        public static void UnRegister(System.Object obj)
        {
            if (obj == null)
            {
                CLog.Warning($" Can not unregister null object.", category: LOG_TAG);
                return;
            }

            if (obj is ITick tickObj) Tick.Remove(tickObj);
            if (obj is ILateTick lateTickObj) LateTick.Remove(lateTickObj);
            if (obj is IFixedTick fixTickObj) FixedTick.Remove(fixTickObj);
            if (obj is IEndOfFrameTick endOfFrameTickObj) EndOfFrameTick.Remove(endOfFrameTickObj);
            if (obj is ISubsystem subsystem) Subsystem.Remove(subsystem);
            if (obj is IGameSystem gameSystem) GameSystem.Remove(gameSystem);
            if (obj is IPauseable pauseable) Pause.Remove(pauseable);
        }

        public static async void StartFramework()
        {
            if (_isFrameworkStarted) return;

            _isFrameworkStarted = true;
            float oldTime = Time.realtimeSinceStartup;
            await Subsystem.Setup();
            await GameSystem.Setup();
            await _InitializeInSceneObjects();
            IsPlayBegun = true;
            OnPlayBegun?.Invoke();
            _StartInfiniteEndOfFrameLoop();

            CLog.Log($"Total loading takes {Time.realtimeSinceStartup - oldTime} seconds.", category: LOG_TAG);
        }

        //Initialize in scene initializable objects that live before framework start.
        private static async Task _InitializeInSceneObjects()
        {
            Initialization.InitializeDelayedObjects();
            await Initialization.InitializeAsync();
        }

        public static void SetPause(bool value = true)
        {
            if (value)
                Pause.Start();
            else
                Pause.Stop();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Load configuration from Resources folder.
        /// This method should load framework config synchronously.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void LoadFrameworkConfig()
        {
            _config = Resources.Load<FSFrameworkConfig>(ConfigAssetName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsBuildTypeSpecified()
        {
            bool isBuildSpecifies = false;
            bool isSpecifiedCorrectly = true;
            bool isBuildSettingsMatch = true;

#if DEVELOPMENT || DEVELOPMENT_RELEASE || PUBLIC_RELEASE
            isBuildSpecifies = true;
#endif

#if (DEVELOPMENT || DEVELOPMENT_RELEASE) && PUBLIC_RELEASE
            isSpecifiedCorrectly = false;
            Debug.LogError($"{LOG_TAG} Both <b>development build</b> and <b>release build</b> are specified as preprocessor directives. Make sure just one type of build is specified in preprocessor directives.");
#endif

#if DEVELOPMENT && !DEVELOPMENT_BUILD && !UNITY_EDITOR //DEVELOPMENT_BUILD is not exist in Editor. To prevent incorrect behavior, UNITY_EDITOR added.
            isBuildSettingsMatch = false;
            Debug.LogError($"{LOG_TAG} <b>Build type</b> in <b>Framework configuration</b> is specified as <b>Development</b> but unity build setting is not. Make sure these settings match.");
#endif

#if (DEVELOPMENT_RELEASE || PUBLIC_RELEASE) && DEVELOPMENT_BUILD
            isBuildSettingsMatch = false;
            Debug.LogError($"{LOG_TAG} <b>Build type</b> in <b>Framework configuration</b> is specified as <b>release</b> but unity build setting is development. Make sure these settings match.");
#endif
            if (!isBuildSpecifies)
                Debug.LogError($"Build type does not specified in Framework configuration.");

            return isBuildSpecifies && isSpecifiedCorrectly && isBuildSettingsMatch;
        }

        private static async void _StartInfiniteEndOfFrameLoop()
        {
            while (true)
            {
                await _endOfFrame;
                if (!IsPlayBegun) continue;
                Initialization.Initialize();
                EndOfFrameTick.DoEndOfFrameTick();
            }
        }

        private static IEnumerator _DoNextFrame(Action action)
        {
            yield return null;
            action();
        }

        #endregion Helpers
    }
}