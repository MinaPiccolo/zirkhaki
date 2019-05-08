using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Revy.Framework.Localization
{
    public class FSLocalizationConfig : FScriptableObject
    {
        #region Fields
        [SerializeField]
        private ELanguage _defaultLanguage = ELanguage.FA;

        [SerializeField]
#pragma warning disable 649
        private string _csvFileName;
#pragma warning restore 649
        [SerializeField]
        private List<FSTextSetting> _setting = new List<FSTextSetting>(Enum.GetNames(typeof(ELanguage)).Length);

        private Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();

        private readonly static string LOG_PREFIX = "<b><color=orange>[Localization]</color></b>";
        #endregion

        #region Properties
        public ELanguage DefaultLanguage { get { return _defaultLanguage; } }

        public Dictionary<string, List<string>> Data { get { return _data; } }

        public List<FSTextSetting> Settings { get { return _setting; } }
        #endregion

        #region Methods
        public void LoadCSVFile()
        {
            UnityEngine.Object obj = Resources.Load(_csvFileName);

            if (obj == null)
            {
                Debug.LogWarning($"{LOG_PREFIX} CSV file can't be find.");
                return;
            }

            string[] rows = obj.ToString().Split('\n');

            for (int i = 1; i < rows.Length; ++i)
            {
                string[] columns = rows[i].Split(',');

                if (string.IsNullOrEmpty(columns[0])) continue;

                if(_data.ContainsKey(columns[0]))
                {
                    Debug.LogWarning($"{LOG_PREFIX} The {columns[0]} key has already added in localization management.");
                    continue;
                }

                List<string> translates = new List<string>();

                for (int j = 1; j < columns.Length; ++j)
                {
                    translates.Add(columns[j]);
                }

                _data.Add(columns[0], translates);
            }
        }
        #endregion
    }
}
