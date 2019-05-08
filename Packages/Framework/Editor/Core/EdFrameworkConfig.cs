/**
 * Author: Mohammad Hasan Bigdeli
 * Edited by: ideen molavi nejad, ideenmolavi@gmail.com
 * Creation Date: 9 / 19 / 2017
 * Description: Editor tool for FrameworkConfig to set GameManager class and GameManagerConfig
 */
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Revy.Framework.Editor
{
    public class FrameworkConfigMenuItem
    {
        private static readonly string FrameworkRootFolderName = "Revy-Framework";
        private static readonly string RelativeResourcesFolderAddresss = $"Assets/{FrameworkRootFolderName}/Resources";
        private static readonly string RelativeConfigFileAddress = $"{RelativeResourcesFolderAddresss}/{MFramework.ConfigAssetName}";
        private static readonly string AbsoluteConfigFileAddress = $"{Application.dataPath}/{FrameworkRootFolderName}/Resources/{MFramework.ConfigAssetName}.asset";
        private static readonly string AbsoluteResourcesFolderAddress = $"{Application.dataPath}/{FrameworkRootFolderName}/Resources";

        [MenuItem(itemName: "Revy/Framework/Configuration")]
        public static void Window()
        {
            if (!File.Exists(AbsoluteConfigFileAddress))
            {
                if (!AssetDatabase.IsValidFolder(RelativeResourcesFolderAddresss))
                {
                    Directory.CreateDirectory(AbsoluteResourcesFolderAddress);
                }
                AssetDatabase.Refresh();
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FSFrameworkConfig>(), $"{RelativeConfigFileAddress}.asset");
                AssetDatabase.SaveAssets();
            }

            var config = Resources.Load<FSFrameworkConfig>(MFramework.ConfigAssetName);
            if (config == null)
            {
                CLog.Warning("Framework config not found.");
                return;
            }
            Selection.activeObject = config;
        }
    }

    [CustomEditor(typeof(FSFrameworkConfig))]
    public class EdFrameworkConfig : UnityEditor.Editor
    {
        private System.Collections.Generic.List<System.Type> _gameManagers;
        private static int _selectedGameManagerIndex;

        private const string GAME_MANAGER_TYPE_NAME_PARAMETER_NAME = "_gameManagerTypeName";
        private const string GAME_MANAGER_CONFIG_BUNDLE_NAME = "_gameManagerConfigBundleName";
        private const string GAME_MANAGER_CONFIG_ASSET_NAME = "_gameManagerConfigAssetName";
        private const string DONT_START_FRAMEWORK = "_dontStartFramework";
        private const string SUBSYSTEMS_ENABLE_STATE_ARRAY = "_subsystemsEnableState";
        private const string SUBSYSTEMS_NAME_ARRAY = "_subsystemsName";
        private const string SUBSYSTEMS_FULL_NAME_ARRAY = "_subsystemsFullName";
        private const string GAMESYSTEMS_ENABLE_STATE_ARRAY = "_gameSystemsEnableState";
        private const string GAMESYSTEMS_NAME_ARRAY = "_gameSystemsName";
        private const string GAMESYSTEMS_FULL_NAME_ARRAY = "_gameSystemsFullName";
        private const string BUILD_TYPE_FIELD_NAME = "_buildType";
        private const string LOG_FIELD_NAME = "_log";
        private const string GAME_UNQUE_SYMBOL_FIELD_NAME = "_currentGameUniqueSymbol";
        private const string BACKEND_SYMBOL_FIELD_NAME = "_backendSymbol";
        private const string LOG_WARNING_FIELD_NAME = "_logWarning";
        private const string LOG_ERROR_FIELD_NAME = "_logError";
        private const string LOG_EXCEPTION_FIELD_NAME = "_logException";

        private static SerializedProperty _gameManagerConfigBundleName;
        private static SerializedProperty _gameManagerConfigAssetName;
        private static SerializedProperty _buildType;
        private static SerializedProperty _log;
        private static SerializedProperty _logWarning;
        private static SerializedProperty _logError;
        private static SerializedProperty _logException;
        private static SerializedProperty _currentGameUniqueSymbol;
        private static SerializedProperty _backendSymbol;
        private static SerializedProperty _subsystemsEnableState;
        private static SerializedProperty _subsystemsName;
        private static SerializedProperty _subsystemsFullName;
        private static SerializedProperty _gameSystemsEnableState;
        private static SerializedProperty _gameSystemsName;
        private static SerializedProperty _gameSystemsFullName;

        private static string _replacePattern;
        private static int _oldbuildTypeIndex = -1;
        private const string LOG_SYMBOLE = "LOG";
        private const string LOG_WARNING_SYMBOLE = "LOG_WARNING";
        private const string LOG_ERROR_SYMBOLE = "LOG_ERROR";
        private const string LOG_EXCEPTION_SYMBOLE = "LOG_EXCEPTION";
        private const string ASSET_BUNDLE_SIMULATE_SYMBOL = "SIMULATE_ASSET_BUNDLE";

        public void OnEnable()
        {
            FSFrameworkConfig.Instance?.CheckForNewSubsystems();
            FSFrameworkConfig.Instance?.CheckForNewGameSystems();
            _gameManagerConfigAssetName = serializedObject.FindProperty(GAME_MANAGER_CONFIG_ASSET_NAME);
            _buildType = serializedObject.FindProperty(BUILD_TYPE_FIELD_NAME);
            _log = serializedObject.FindProperty(LOG_FIELD_NAME);
            _logWarning = serializedObject.FindProperty(LOG_WARNING_FIELD_NAME);
            _logError = serializedObject.FindProperty(LOG_ERROR_FIELD_NAME);
            _logException = serializedObject.FindProperty(LOG_EXCEPTION_FIELD_NAME);
            _currentGameUniqueSymbol = serializedObject.FindProperty(GAME_UNQUE_SYMBOL_FIELD_NAME);
            _backendSymbol = serializedObject.FindProperty(BACKEND_SYMBOL_FIELD_NAME);
            _subsystemsEnableState = serializedObject.FindProperty(SUBSYSTEMS_ENABLE_STATE_ARRAY);
            _subsystemsName = serializedObject.FindProperty(SUBSYSTEMS_NAME_ARRAY);
            _subsystemsFullName = serializedObject.FindProperty(SUBSYSTEMS_FULL_NAME_ARRAY);
            _gameSystemsEnableState = serializedObject.FindProperty(GAMESYSTEMS_ENABLE_STATE_ARRAY);
            _gameSystemsName = serializedObject.FindProperty(GAMESYSTEMS_NAME_ARRAY);
            _gameSystemsFullName = serializedObject.FindProperty(GAMESYSTEMS_FULL_NAME_ARRAY);
            _gameManagers = CUtilities.GetAllImplementingTypes(typeof(IGameManager)).ToList();
            _gameManagers.Insert(0, null);

            string serializedTypeName = serializedObject.FindProperty(GAME_MANAGER_TYPE_NAME_PARAMETER_NAME).stringValue;

            Type selectedType = Type.GetType(serializedTypeName);

            if (selectedType == null)
                _selectedGameManagerIndex = 0;
            else
                _selectedGameManagerIndex = _gameManagers.IndexOf(selectedType);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();

            GenerateUI();

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateUI()
        {
            EditorGUILayout.InspectorTitlebar(true, serializedObject.targetObject);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            DrawStartFramework();
            DrawBuildTypeConfig();
            EditorGUILayout.EndVertical();
            DrawGameManagerConfig();
            DrawSubsystemsConfig();
            DrawGameSystemsConfig();
            DrawLogType();
        }

        private void DrawStartFramework()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(DONT_START_FRAMEWORK));
        }

        private static void DrawBuildTypeConfig()
        {
            if (_buildType == null) return;

            if (EditorApplication.isCompiling)
                GUI.enabled = false;
            EditorGUILayout.PropertyField(_buildType);
            GUI.enabled = true;

            var selectedBuildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            var newBuildTypeIndex = _buildType.enumValueIndex;

            if (newBuildTypeIndex != _oldbuildTypeIndex)
            {
                switch (newBuildTypeIndex)
                {
                    case 0:
                        UpdateScriptingDefineSymbolsForBuildType(FSFrameworkConfig.EBuildType.DEVELOPMENT.ToString(), selectedBuildTarget);
                        break;
                    case 1:
                        UpdateScriptingDefineSymbolsForBuildType(FSFrameworkConfig.EBuildType.DEVELOPMENT_RELEASE.ToString().ToUpper(), selectedBuildTarget);
                        break;
                    case 2:
                        UpdateScriptingDefineSymbolsForBuildType(FSFrameworkConfig.EBuildType.PUBLIC_RELEASE.ToString().ToUpper(), selectedBuildTarget);
                        break;
                }
            }

            _oldbuildTypeIndex = newBuildTypeIndex;
        }

        private static void DrawLogType()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Log Level");
            var selectedBuildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            _log.boolValue = EditorGUILayout.Toggle("Log", _log.boolValue);
            _logWarning.boolValue = EditorGUILayout.Toggle("Warning", _logWarning.boolValue);
            _logError.boolValue = EditorGUILayout.Toggle("Error", _logError.boolValue);
            _logException.boolValue = EditorGUILayout.Toggle("Exception", _logException.boolValue);

            if (EditorApplication.isCompiling)
                GUI.enabled = false;
            if (GUILayout.Button("Apply"))
            {
                UpdateScriptingDefineSymbolsForLogType(_log.boolValue, _logWarning.boolValue, _logError.boolValue, _logException.boolValue, selectedBuildTarget);
            }
            GUI.enabled = true;

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
        }

        private static void DrawCurrentGameSymbol()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            _currentGameUniqueSymbol.stringValue = EditorGUILayout.TextField("Game Symbol", _currentGameUniqueSymbol.stringValue);
            if (EditorApplication.isCompiling)
                GUI.enabled = false;

            if (GUILayout.Button("Apply"))
            {
                _currentGameUniqueSymbol.stringValue = _currentGameUniqueSymbol.stringValue.Trim(';', ' ');

                if (!string.IsNullOrEmpty(_currentGameUniqueSymbol.stringValue))
                {
                    SetScriptSymbole(_currentGameUniqueSymbol.stringValue);
                }
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawBackendSymboleDefinition()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            _backendSymbol.stringValue = EditorGUILayout.TextField("Backend Symbol", _backendSymbol.stringValue);
            if (EditorApplication.isCompiling)
                GUI.enabled = false;

            if (GUILayout.Button("Apply"))
            {
                _backendSymbol.stringValue = _backendSymbol.stringValue.Trim(';', ' ');

                if (!string.IsNullOrEmpty(_backendSymbol.stringValue))
                {
                    SetScriptSymbole(_backendSymbol.stringValue);
                }
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        private static void UpdateScriptingDefineSymbolsForBuildType(string newDefineSymbole, BuildTargetGroup targetGroup)
        {
            var curretnDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            curretnDefines = curretnDefines.Replace(";", " ");
            // \b means whole word.
            //Remove already existing build type and add new one.
            _replacePattern = $@"\b{FSFrameworkConfig.EBuildType.DEVELOPMENT.ToString()}\b";
            curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");

            _replacePattern = $@"\b{FSFrameworkConfig.EBuildType.DEVELOPMENT_RELEASE.ToString()}\b";
            curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");

            _replacePattern = $@"\b{FSFrameworkConfig.EBuildType.PUBLIC_RELEASE.ToString()}\b";
            curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, $"{curretnDefines} {newDefineSymbole}");
        }

        private static void UpdateScriptingDefineSymbolsForLogType(bool isLog, bool isWarning, bool isError, bool isException, BuildTargetGroup targetGroup)
        {

            string curretnDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            curretnDefines = curretnDefines.Replace(";", " ");

            if (isLog)
            {
                _replacePattern = $@"\b{LOG_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
                curretnDefines = $"{curretnDefines} {LOG_SYMBOLE}";
            }
            else
            {
                _replacePattern = $@"\b{LOG_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            }

            if (isWarning)
            {
                _replacePattern = $@"\b{LOG_WARNING_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
                curretnDefines = $"{curretnDefines} {LOG_WARNING_SYMBOLE}";
            }
            else
            {
                _replacePattern = $@"\b{LOG_WARNING_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            }

            if (isError)
            {
                _replacePattern = $@"\b{LOG_ERROR_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
                curretnDefines = $"{curretnDefines} {LOG_ERROR_SYMBOLE}";
            }
            else
            {
                _replacePattern = $@"\b{LOG_ERROR_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            }

            if (isException)
            {
                _replacePattern = $@"\b{LOG_EXCEPTION_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
                curretnDefines = $"{curretnDefines} {LOG_EXCEPTION_SYMBOLE}";
            }
            else
            {
                _replacePattern = $@"\b{LOG_EXCEPTION_SYMBOLE}\b";
                curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            }


            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, curretnDefines);
        }

        private void DrawGameManagerConfig()
        {
            if (_gameManagers == null) return;
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Game Manager");
            _selectedGameManagerIndex = EditorGUILayout.Popup(_selectedGameManagerIndex, _gameManagers.Select(x => x == null ? "None" : x.Name).ToArray());
            EditorGUILayout.EndHorizontal();

            DrawCurrentGameSymbol();
            DrawBackendSymboleDefinition();

            EditorGUILayout.Space();
            serializedObject.FindProperty(GAME_MANAGER_TYPE_NAME_PARAMETER_NAME).stringValue = _selectedGameManagerIndex == 0 ? "" : _gameManagers[_selectedGameManagerIndex].AssemblyQualifiedName;

            EditorGUILayout.EndVertical();
        }

        private static void DrawSubsystemsConfig()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            if (_subsystemsEnableState == null || _subsystemsEnableState.arraySize == 0 ||
                _subsystemsName == null || _subsystemsName.arraySize == 0 ||
                _subsystemsFullName == null || _subsystemsFullName.arraySize == 0)
            {
                EditorGUILayout.LabelField("Subsystems Activation State");
                EditorGUILayout.LabelField("NO SUBSYSTEM FOUND!");
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.LabelField("Subsystems Activation State");
            int arraySize = _subsystemsEnableState.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                _subsystemsEnableState.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(_subsystemsName.GetArrayElementAtIndex(i).stringValue, _subsystemsEnableState.GetArrayElementAtIndex(i).boolValue);
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawGameSystemsConfig()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            if (_gameSystemsEnableState == null || _gameSystemsEnableState.arraySize == 0 ||
                _gameSystemsName == null || _gameSystemsName.arraySize == 0 ||
                _gameSystemsFullName == null || _gameSystemsFullName.arraySize == 0)
            {
                EditorGUILayout.LabelField("Game Systems Activation State");
                EditorGUILayout.LabelField("NO GAME SYSTEM FOUND!");
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.LabelField("Game Systems Activation State");
            int arraySize = _gameSystemsEnableState.arraySize;
            for (int i = 0; i < arraySize; i++)
            {
                _gameSystemsEnableState.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(_gameSystemsName.GetArrayElementAtIndex(i).stringValue, _gameSystemsEnableState.GetArrayElementAtIndex(i).boolValue);
            }

            EditorGUILayout.EndVertical();
        }

        private static void SetScriptSymbole(string newSymbol)
        {
            var selectedBuildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            var curretnDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTarget);
            var _replacePattern = $@"\b{newSymbol}\b";
            curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            curretnDefines = $"{curretnDefines} {newSymbol}";
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTarget, curretnDefines);
        }

        private static void RemoveScriptSymbole(string existingSymbole)
        {
            var selectedBuildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            var curretnDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(selectedBuildTarget);
            var _replacePattern = $@"\b{existingSymbole}\b";
            curretnDefines = Regex.Replace(curretnDefines, _replacePattern, "");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTarget, curretnDefines);
        }
    }
}