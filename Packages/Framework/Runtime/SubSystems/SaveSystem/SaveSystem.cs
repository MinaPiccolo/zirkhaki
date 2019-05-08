using System;
using System.IO;
using System.Text;
/**
* Author: Mahdi Fada.
* CreationTime: 11 / 5 / 2018
* Edited by: Ideen Molavi. ideenmolavi@gmail.com
* Description: Save and Load non-unity object on file
**/

using UnityEngine;
/*
============== Android with split application binary enabled

Application.dataPath:                       /storage/emulated/0/Android/obb/im.getsocial.testapp.unity/main.1.im.getsocial.testapp.unity.obb
Application.persistentDataPath:             /storage/emulated/0/Android/data/im.getsocial.testapp.unity/files
Application.streamingAssetsPath: jar:file:///storage/emulated/0/Android/obb/im.getsocial.testapp.unity/main.1.im.getsocial.testapp.unity.obb!/assets


============== Android with split application binary Disabled

Application.dataPath:                                                  /data/app/im.getsocial.testapp.unity-2/base.apk
Application.persistentDataPath:             /storage/emulated/0/Android/data/im.getsocial.testapp.unity/files
Application.streamingAssetsPath:                            jar:file:///data/app/im.getsocial.testapp.unity-2/base.apk!/assets


============== iOS

Application.dataPath:                /private/var/mobile/Containers/Bundle/Application/71C38EEB-1C3E-4AB1-918A-3BF379CBE5C1/unity.app/Data
Application.persistentDataPath:      /var/mobile/Containers/Data/Application/055811B9-D125-41B1-A078-F898B06F8C58/Documents
Application.streamingAssetsPath: /private/var/mobile/Containers/Bundle/Application/71C38EEB-1C3E-4AB1-918A-3BF379CBE5C1/unity.app/Data/Raw
*/
namespace Revy.Framework
{
    public class SaveSystem : ISubsystem, ISaveSystem
    {
        #region Fields
        private string _dataPath;
        #endregion Fields

        #region Properties
        
        public Type ServiceType => typeof(ISaveSystem);
        
        #endregion Properties

        #region Constructor & Destructor

        public SaveSystem()
        {
            MFramework.Register(this);
#if UNITY_EDITOR
            if (!Directory.Exists("SaveCach"))
            {
                Directory.CreateDirectory("SaveCach");
            }

            _dataPath = "SaveCach/";
#else
            _dataPath = $"{Application.persistentDataPath}/";
#endif
        }

        ~SaveSystem()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        #region Class Interface

        void ISaveSystem.Save<T>(T item, string fileName)
        {
            string textData = JsonUtility.ToJson(item);
#if UNITY_EDITOR
            _WriteTextInFile(textData, fileName);
#else
            try
            {
                byte[] binaryData = Encoding.UTF8.GetBytes(textData);
                if (binaryData != null)
                {
                    _WriteBytesInFile(binaryData, fileName);
                }
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "Save System");
            }
#endif
        }

        T ISaveSystem.Load<T>(string fileName)
        {
            string textData = string.Empty;
            T result = default(T);
#if UNITY_EDITOR
            textData = _ReadTextFromFile(fileName);
#else
            try
            {
                byte[] binaryData = _ReadBytesFromFile(fileName);
                if (binaryData != null)
                {
                    textData = Encoding.UTF8.GetString(binaryData);
                }
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "Save System");
            }
#endif
            if (!string.IsNullOrEmpty(textData))
            {
                result = JsonUtility.FromJson<T>(textData);
            }
            return result;
        }

        #endregion

        #region Helpers
        private void _WriteBytesInFile(byte[] dataBuffer, string fileName)
        {
            string path = $"{_dataPath}{fileName}.bin";
            try
            {
                File.WriteAllBytes(path, dataBuffer);
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "File System");
            }
        }

        private byte[] _ReadBytesFromFile(string fileName)
        {
            string path = $"{_dataPath}{fileName}.bin";
            byte[] result = null;
            try
            {
                result = File.ReadAllBytes(path);
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
            {
                CLog.Warning($"File({path}) not found.", category: "File System");
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "File System");
            }
            return result;
        }

        private void _WriteTextInFile(string dataString, string fileName)
        {
            string path = $"{_dataPath}{fileName}.json";
            try
            {
                File.WriteAllText(path, dataString);
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "File System");
            }
        }

        private string _ReadTextFromFile(string fileName)
        {
            string path = $"{_dataPath}{fileName}.json";
            string result = string.Empty;
            try
            {
                result = File.ReadAllText(path);
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
            {
                CLog.Warning($"File({path}) not found.", category: "File System");
            }
            catch (Exception e)
            {
                CLog.Exception(e, category: "File System");
            }
            return result;
        }
        #endregion
    }
}
