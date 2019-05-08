/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 10 / 10 / 2017
 * Description: The terminal system that used to invoke methods, manipulating fields and properties.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
#if PUBLIC_RELEASE
    [CDisableAutoInstantiationAttribute]
#endif
    public class FTerminal : FComponent, IInitializable
    {
        /// <summary>
        /// This class will hold required data to index commands in a dictionary.
        /// </summary>
        private class CTerminalCommandData
        {
            #region Properties

            /// <summary>
            /// This is information of the object member that has CField, CProperty or CMethod attribute.
            /// </summary>
            public MemberInfo Info { get; private set; }

            /// <summary>
            /// This is a reference to the member info owner (Object).
            /// </summary>
            public object Obj { get; private set; }

            #endregion

            #region Constructor

            public CTerminalCommandData(object obj, MemberInfo memberInfo)
            {
                Info = memberInfo;
                Obj = obj;
            }

            #endregion
        }

        #region Fields

        private const BindingFlags BINDING_FLAG = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                                  BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        private UnityEngine.UI.InputField _input;
        private UnityEngine.UI.Text _output;
        private WaitForSeconds waitForSeconds = new WaitForSeconds(5);

        private static Dictionary<string, CTerminalCommandData> indexedCommands =
            new Dictionary<string, CTerminalCommandData>();

        //#pragma warning disable 414
        //        private CTerminalPrimitiveCommands _terminalPrimitiveCommands;
        //#pragma warning restore 414

        private static readonly string LOG_PREFIX = $"<b><color=fuchsia>[Terminal]</color></b>";

        #endregion

        #region Methods

        public bool HasInitialized { get; set; }

        public void Initialize()
        {
            _input = GetComponent<UnityEngine.UI.InputField>();
            _output = transform.parent.Find("Output").GetComponentInChildren<UnityEngine.UI.Text>();

            _input.onEndEdit.AddListener(new UnityEngine.Events.UnityAction<string>(OnInputEndEdit));

            gameObject.SetActive(false);
            SetActiveOutput(false);
        }

        public Task BeginPlay()
        {
            return Task.CompletedTask;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_input)
            {
                _input.text = string.Empty;
                _input.ActivateInputField();
            }

        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _input.DeactivateInputField();
        }

        #endregion

        #region Helpers

        private void OnInputEndEdit(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                _input.gameObject.SetActive(false);
                return;
            }

            string[] inputSegments = input.Split(' ');

            if (inputSegments == null || inputSegments.Length == 0)
            {
                _input.gameObject.SetActive(false);
                return;
            }

            if (!IsCommandExsit(inputSegments[0]))
            {
                _input.gameObject.SetActive(false);
                return;
            }

            CTerminalCommandData commandData = indexedCommands[inputSegments[0].ToLower()];

            if (inputSegments.Length == 1)
            {
                if (commandData.Info is FieldInfo)
                    GetField(commandData.Obj, (FieldInfo)commandData.Info);
                else if (commandData.Info is PropertyInfo)
                    GetProperty(commandData.Obj, (PropertyInfo)commandData.Info);
                else if (commandData.Info is MethodInfo)
                    InvokeMethod(commandData.Obj, (MethodInfo)commandData.Info);
            }
            else if (inputSegments.Length == 2)
            {
                if (commandData.Info is FieldInfo)
                    SetField(commandData.Obj, (FieldInfo)commandData.Info, inputSegments[1]);
                else if (commandData.Info is PropertyInfo)
                    SetProperty(commandData.Obj, (PropertyInfo)commandData.Info, inputSegments[1]);
                else if (commandData.Info is MethodInfo)
                    InvokeMethod(commandData.Obj, (MethodInfo)commandData.Info, inputSegments[1]);
            }
            else
            {
                if (commandData.Info is FieldInfo || commandData.Info is PropertyInfo)
                {
                    _input.gameObject.SetActive(false);

                    Debug.LogError($"{LOG_PREFIX} \"{commandData.Info.Name}\" command has to many argument.");
                    DisplayOutput(LogType.Error,
                        $"{LOG_PREFIX} \"{commandData.Info.Name}\" command has to many argument.");
                }

                List<object> parameters = new List<object>();

                for (int i = 1; i < inputSegments.Length; ++i)
                    parameters.Add(inputSegments[i]);

                InvokeMethod(commandData.Obj, (MethodInfo)commandData.Info, parameters.ToArray());
            }

            _input.gameObject.SetActive(false);
        }

        private void SetActiveOutput(bool value)
        {
            _output.transform.parent.gameObject.SetActive(value);
        }

        private bool IsCommandExsit(string command)
        {
            if (!indexedCommands.Keys.Contains(command.ToLower()))
            {
                Debug.LogWarningFormat(
                    $"{LOG_PREFIX} \"{command}\" is not recognized as an internal or external command");
                DisplayOutput(LogType.Warning,
                    $"{LOG_PREFIX} \"{command}\" is not recognized as an internal or external command");
                return false;
            }

            return true;
        }

        private void GetField(object obj, FieldInfo fieldInfo)
        {
            Debug.Log($"{LOG_PREFIX} Value of <b>{fieldInfo.Name}</b> is <b>{fieldInfo.GetValue(obj)}</b>.");
            DisplayOutput(LogType.Log,
                $"{LOG_PREFIX} Value of <b>{fieldInfo.Name}</b> is <b>{fieldInfo.GetValue(obj)}</b>.");
        }

        private void SetField(object obj, FieldInfo fieldInfo, object value)
        {
            try
            {
                fieldInfo.SetValue(obj, Convert.ChangeType(value, fieldInfo.FieldType));

                Debug.Log($"{LOG_PREFIX} Value of <b>{fieldInfo.Name}</b> set to <b>{value}</b>");
                DisplayOutput(LogType.Log, $"{LOG_PREFIX} Value of <b>{fieldInfo}</b> set to <b>{value}</b>");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void GetProperty(object obj, PropertyInfo propertyInfo)
        {
            Debug.Log($"{LOG_PREFIX} Value of <b>{propertyInfo.Name}</b> is <b>{propertyInfo.GetValue(obj)}</b>.");
            DisplayOutput(LogType.Log,
                $"{LOG_PREFIX} Value of <b>{propertyInfo.Name}</b> is <b>{propertyInfo.GetValue(obj)}</b>.");
        }

        private void SetProperty(object obj, PropertyInfo propertyInfo, object value)
        {
            try
            {
                propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType));

                Debug.Log($"{LOG_PREFIX} Value of <b>{propertyInfo.Name}</b> is <b>{propertyInfo.GetValue(obj)}</b>.");
                DisplayOutput(LogType.Log,
                    $"{LOG_PREFIX} Value of <b>{propertyInfo.Name}</b> is <b>{propertyInfo.GetValue(obj)}</b>.");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void InvokeMethod(object obj, MethodInfo methodInfo, params object[] parameters)
        {
            object invokeResult = null;

            if (parameters.Length == 0)
            {
                invokeResult = methodInfo.Invoke(obj, parameters);

                if (methodInfo.ReturnParameter.ParameterType != typeof(void))
                {
                    Debug.LogFormat(
                        $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked and it has return <b>{invokeResult}</b>");
                    DisplayOutput(LogType.Log,
                        $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked and it has return <b>{invokeResult}</b>");
                }
                else
                {
                    Debug.Log($"{LOG_PREFIX}The <b>{methodInfo.Name}</b> has invoked.");
                    DisplayOutput(LogType.Log, $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked.");
                }

                return;
            }

            ParameterInfo[] parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Length < parameters.Length)
            {
                Debug.LogError(
                    $"{LOG_PREFIX} Argument's count that passed to invoking \"{methodInfo.Name}\" method is more than required.");
                DisplayOutput(LogType.Error,
                    $"{LOG_PREFIX} Argument's count that passed to invoking \"{methodInfo.Name}\" method is more than required.");
                return;
            }

            if (parameterInfos.Length > parameters.Length)
            {
                Debug.LogError(
                    $"{LOG_PREFIX} Argument's count that passed to invoking \"{methodInfo.Name}\" method is less than required.");
                DisplayOutput(LogType.Error,
                    $"{LOG_PREFIX} Argument's count that passed to invoking \"{methodInfo.Name}\" method is less than required.");
                return;
            }

            for (int i = 0; i < parameters.Length; ++i)
            {
                parameters[i] = Convert.ChangeType(parameters[i], parameterInfos[i].ParameterType);
            }

            invokeResult = methodInfo.Invoke(obj, parameters);

            if (methodInfo.ReturnParameter.ParameterType != typeof(void))
            {
                Debug.Log(
                    $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked and it has return <b>{invokeResult}</b>");
                DisplayOutput(LogType.Log,
                    $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked and it has return <b>{invokeResult}</b>");
            }
            else
            {
                Debug.LogFormat($"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked.");
                DisplayOutput(LogType.Log, $"{LOG_PREFIX} The <b>{methodInfo.Name}</b> has invoked.");
            }
        }

        private void DisplayOutput(LogType logType, string condition)
        {
            switch (logType)
            {
                case LogType.Error:
                    _output.color = Color.red;
                    break;
                case LogType.Warning:
                    _output.color = Color.yellow;
                    break;
                case LogType.Log:
                    _output.color = Color.green;
                    break;
                case LogType.Exception:
                    _output.color = Color.red;
                    break;
            }

            _output.text = condition;

            FCoroutineAgent.Instance.StopCoroutine(ToggleOutputPanel());
            FCoroutineAgent.Instance.StartCoroutine(ToggleOutputPanel());
        }

        private System.Collections.IEnumerator ToggleOutputPanel()
        {
            SetActiveOutput(true);

            yield return waitForSeconds;

            SetActiveOutput(false);
        }

        #endregion

        #region Static Methods

        public static void IndexObject(ITerminalIndex obj)
        {
            IndexFields(obj);
            IndexProperties(obj);
            IndexMethods(obj);
        }

        #endregion

        #region Static Helpers

        private static void IndexFields(ITerminalIndex obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BINDING_FLAG);

            foreach (FieldInfo it in fieldInfos)
            {
                try
                {
                    if (it.IsDefined(typeof(CFieldAttribute), false))
                    {
                        CustomAttributeData customAttributeData = it.GetCustomAttributesData()
                            .Where(row => row.AttributeType == typeof(CFieldAttribute)).Single();
                        int nameArgumanPosition = customAttributeData.Constructor.GetParameters()
                            .Where(row => row.Name == "name").Single().Position;
                        string commandName = customAttributeData.ConstructorArguments.ToArray()[nameArgumanPosition]
                            .Value.ToString().ToLower();

                        if (indexedCommands.Keys.Contains(commandName)) return;

                        indexedCommands.Add(commandName, new CTerminalCommandData(obj, it));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"{LOG_PREFIX} {ex}");
                }
            }
        }

        private static void IndexProperties(ITerminalIndex obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] propertiesInfos = type.GetProperties(BINDING_FLAG);

            foreach (PropertyInfo it in propertiesInfos)
            {
                if (it.IsDefined(typeof(CPropertyAttribute), false))
                {
                    try
                    {
                        CustomAttributeData customAttributeData = it.GetCustomAttributesData()
                            .Where(row => row.AttributeType == typeof(CPropertyAttribute)).Single();
                        int nameArgumanPosition = customAttributeData.Constructor.GetParameters()
                            .Where(row => row.Name == "name").Single().Position;
                        string commandName = customAttributeData.ConstructorArguments.ToArray()[nameArgumanPosition]
                            .Value.ToString().ToLower();
                        if (indexedCommands.Keys.Contains(commandName)) return;
                        indexedCommands.Add(commandName.ToLower(), new CTerminalCommandData(obj, it));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"{LOG_PREFIX} {ex}");
                    }
                }
            }
        }

        private static void IndexMethods(ITerminalIndex obj)
        {
            Type type = obj.GetType();
            MethodInfo[] methodInfos = type.GetMethods(BINDING_FLAG);

            foreach (MethodInfo it in methodInfos)
            {
                if (it.IsDefined(typeof(CMethodAttribute), false))
                {
                    try
                    {
                        ParameterInfo[] parameterInfos = it.GetParameters();

                        if (parameterInfos.Where(row =>
                                    !row.ParameterType.IsPrimitive && row.ParameterType != typeof(System.String))
                                .ToArray()
                                .Length != 0)
                        {
                            Debug.LogWarning(
                                $"{LOG_PREFIX} Can't index \"{it.Name}\" method because it has reference type in it's parameters list.\n Note: Methods can have string parameter.");
                            continue;
                        }

                        CustomAttributeData customAttributeData = it.GetCustomAttributesData()
                            .Where(row => row.AttributeType == typeof(CMethodAttribute)).Single();
                        int nameArgumanPosition = customAttributeData.Constructor.GetParameters()
                            .Where(row => row.Name == "name").Single().Position;
                        string commandName = customAttributeData.ConstructorArguments.ToArray()[nameArgumanPosition]
                            .Value.ToString().ToLower();
                        if (indexedCommands.Keys.Contains(commandName)) return;
                        indexedCommands.Add(commandName.ToLower(), new CTerminalCommandData(obj, it));
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"{LOG_PREFIX} {ex}");
                    }
                }
            }
        }

        #endregion
    }
}