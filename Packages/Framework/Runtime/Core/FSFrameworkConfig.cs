using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Revy.Framework
{
#pragma warning disable 414
    public class FSFrameworkConfig : FScriptableObject
    {
        #region Fields

        [SerializeField] private bool _dontStartFramework = false;

        [SerializeField] private string _gameManagerTypeName = typeof(FDefaultGameManager).Name;

        [SerializeField] private string _gameManagerConfigBundleName = "";

        [SerializeField] private string _gameManagerConfigAssetName = "";

        [SerializeField] private EBuildType _buildType = EBuildType.DEVELOPMENT;

        [SerializeField] private bool _log = true;

        [SerializeField] private bool _logWarning = true;

        [SerializeField] private bool _logError = true;

        [SerializeField] private bool _logException = true;

        [SerializeField] private string _currentGameUniqueSymbol = "";

        [SerializeField] private bool _assetBundleSimulation = false;

        [SerializeField] private string _backendSymbol = "";

        [SerializeField] private bool[] _subsystemsEnableState = new bool[0];

        [SerializeField] private string[] _subsystemsName = new string[0];

        [SerializeField] private string[] _subsystemsFullName = new string[0];

        [SerializeField] private bool[] _gameSystemsEnableState = new bool[0];

        [SerializeField] private string[] _gameSystemsName = new string[0];

        [SerializeField] private string[] _gameSystemsFullName = new string[0];
        #endregion

        #region Properties

#if UNITY_EDITOR
        public static FSFrameworkConfig Instance { get; private set; }
#endif

        public System.Type GameManagerType => Type.GetType(_gameManagerTypeName);

        public bool DontStartFramework => _dontStartFramework;

        public string GameManagerConfigBundleName => _gameManagerConfigBundleName;

        public string GameManagerConfigAssetName => _gameManagerConfigAssetName;

        public EBuildType BuildType => _buildType;

        #endregion

        #region Mono Callbacks

        private void Awake()
        {
            _InitSubsystemsConfig();
            _InitGameSystemsConfig();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            Instance = this;
#endif
        }

        #endregion

        #region Public Methods

        public bool IsSubsystemEnable(Type subsystemType)
        {
            string typeName = subsystemType.AssemblyQualifiedName;
            int length = _subsystemsFullName.Length;
            for (int i = 0; i < length; i++)
            {
                string name = _subsystemsFullName[i];
                if (name == typeName)
                    return _subsystemsEnableState[i];
            }
            return false;
        }

        public bool IsGameSystemEnable(Type gameSystemType)
        {
            string typeName = gameSystemType.AssemblyQualifiedName;
            int length = _gameSystemsFullName.Length;
            for (int i = 0; i < length; i++)
            {
                string name = _gameSystemsFullName[i];
                if (name == typeName)
                    return _gameSystemsEnableState[i];
            }
            return false;
        }
#if UNITY_EDITOR
        public void CheckForNewSubsystems()
        {
            Type[] currentSubsystemsArray = CUtilities.GetAllImplementingTypes(typeof(ISubsystem));
            List<Type> currentSubsystems;
            if (currentSubsystemsArray == null)
                return;
            else
                currentSubsystems = new List<Type>(currentSubsystemsArray);

            var fullNames = new List<string>(_subsystemsFullName);
            var names = new List<string>(_subsystemsName);
            var enableState = new List<bool>(_subsystemsEnableState);
            foreach (var subsystem in currentSubsystems)
            {
                if (!fullNames.Contains(subsystem.AssemblyQualifiedName))
                {
                    fullNames.Add(subsystem.AssemblyQualifiedName);
                    names.Add(subsystem.Name);
                    enableState.Add(true);
                }
            }

            var tmpFullNames = new List<string>(fullNames);
            for (int i = 0; i < tmpFullNames.Count; i++)
            {
                string storedFullNames = tmpFullNames[i];
                if (!currentSubsystems.Exists((t) => { return t.AssemblyQualifiedName == storedFullNames; }))
                {
                    fullNames.RemoveAt(i);
                    names.RemoveAt(i);
                    enableState.RemoveAt(i);
                }
            }
            _subsystemsFullName = fullNames.ToArray();
            _subsystemsName = names.ToArray();
            _subsystemsEnableState = enableState.ToArray();
        }

        public void CheckForNewGameSystems()
        {
            Type[] currentGameSystemsArray = CUtilities.GetAllImplementingTypes(typeof(IGameSystem));
            List<Type> currentGameSystems;
            if (currentGameSystemsArray == null)
                return;
            else
                currentGameSystems = new List<Type>(currentGameSystemsArray);

            var fullNames = new List<string>(_gameSystemsFullName);
            var names = new List<string>(_gameSystemsName);
            var enableState = new List<bool>(_gameSystemsEnableState);
            foreach (var gameSystem in currentGameSystems)
            {
                if (fullNames.Contains(gameSystem.AssemblyQualifiedName)) continue;
                if (gameSystem.GetInterfaces().Contains(typeof(IGameManager))) continue;

                fullNames.Add(gameSystem.AssemblyQualifiedName);
                names.Add(gameSystem.Name);
                enableState.Add(true);
            }

            var tmpFullNames = new List<string>(fullNames);
            for (int i = 0; i < tmpFullNames.Count; i++)
            {
                string storedFullNames = tmpFullNames[i];
                if (!currentGameSystems.Exists((t) => { return t.AssemblyQualifiedName == storedFullNames; }))
                {
                    fullNames.RemoveAt(i);
                    names.RemoveAt(i);
                    enableState.RemoveAt(i);
                }
            }
            _gameSystemsFullName = fullNames.ToArray();
            _gameSystemsName = names.ToArray();
            _gameSystemsEnableState = enableState.ToArray();
        }
#endif

        #endregion Public Methods

        #region Helpers

        private void _InitSubsystemsConfig()
        {
            var allSubsystems = CUtilities.GetAllImplementingTypes(typeof(ISubsystem));
            if (allSubsystems == null || _subsystemsFullName.Length > 0 || _subsystemsName.Length > 0 || _subsystemsEnableState.Length > 0)
                return;

            int length = allSubsystems.Length;
            _subsystemsEnableState = new bool[length];
            _subsystemsName = new string[length];
            _subsystemsFullName = new string[length];
            for (int i = 0; i < length; i++)
            {
                Type subsystem = allSubsystems[i];
                _subsystemsFullName[i] = subsystem.AssemblyQualifiedName;
                _subsystemsName[i] = subsystem.Name;
                _subsystemsEnableState[i] = true;
            }
        }

        private void _InitGameSystemsConfig()
        {
            var allGameSystems = new List<Type>(CUtilities.GetAllImplementingTypes(typeof(IGameSystem)));
            if (allGameSystems.Count == 0 || _gameSystemsFullName.Length > 0 || _gameSystemsName.Length > 0 || _gameSystemsEnableState.Length > 0)
                return;

            allGameSystems.RemoveAll((t) => t.GetInterfaces().Contains(typeof(IGameManager)));
            int length = allGameSystems.Count;

            _gameSystemsEnableState = new bool[length];
            _gameSystemsName = new string[length];
            _gameSystemsFullName = new string[length];

            for (int i = 0; i < length; i++)
            {
                Type gameSystem = allGameSystems[i];
                _gameSystemsFullName[i] = gameSystem.AssemblyQualifiedName;
                _gameSystemsName[i] = gameSystem.Name;
                _gameSystemsEnableState[i] = true;
            }
        }

        #endregion

        #region Nested Types

        public enum EBuildType { DEVELOPMENT, DEVELOPMENT_RELEASE, PUBLIC_RELEASE }

        #endregion
    }
}