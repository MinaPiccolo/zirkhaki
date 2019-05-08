/*
* Description:
* Author: Mohammad Hasan Bigdeli
* Edited by ideen molavi
* Creation Data: May 4, 2018
*/
using System;
using System.Diagnostics;
using UnityEngine;
using UDebug = UnityEngine.Debug;
using UObject = UnityEngine.Object;

namespace Revy.Framework
{
    /// <summary>
    /// CLog provide some debug utilities such as Longing system and etc.
    /// Each utility needs specific symbols to work.
    /// <Author>Mohammad Hasan Bigdeli</Author>
    /// <CreationDate>April/24/2018</CreationDate>
    /// </summary>
    public static class CLog
    {
        #region Fields
        private const string LOG = "LOG";
        private const string LOG_WARNING = "LOG_WARNING";
        private const string LOG_ERROR = "LOG_ERROR";
        private const string LOG_EXCEPTION = "LOG_ERROR";
        #endregion

        #region Methods
        [Conditional(LOG)]
        public static void Log(object message, UObject sender = null, string category = "[Default]")
        {
            string senderName = sender != null ? $" | <color=lime>{sender.name}</color> | <color=lime>{sender.GetInstanceID()}</color> " : string.Empty;

            string outputMessage = $"<b><color=blue>{category}</color>{senderName} | {message}</b>";

            UDebug.Log(outputMessage);
        }

        [Conditional(LOG_WARNING)]
        public static void Warning(object message, UObject sender = null, string category = "[Default]")
        {
            string senderName = sender != null ? $" | <color=yellow>{sender.name}</color> | <color=yellow>{sender.GetInstanceID()}</color> " : string.Empty;

            string outputMessage = $"<b><color=yellow>{category}</color>{senderName} | {message}</b>";

            UDebug.LogWarning(outputMessage);
        }

        [Conditional(LOG_ERROR)]
        public static void Error(object message, UObject sender = null, string category = "[Default]")
        {
            string senderName = sender != null ? $" | <color=red>{sender.name}</color> | <color=red>{sender.GetInstanceID()}</color> " : string.Empty;

            string outputMessage = $"<b><color=red>{category}</color>{senderName} | {message}</b>";

            UDebug.LogError(outputMessage);
        }

        [Conditional(LOG_EXCEPTION)]
        public static void Exception(object message, UObject sender = null, string category = "[Default]")
        {
            string senderName = sender != null ? $" | <color=red>{sender.name}</color> | <color=red>{sender.GetInstanceID()}</color> " : string.Empty;

            string outputMessage = $"<b><color=red> Exception | {category}</color>{senderName} | <color=red>{DateTime.Now}</color> | {message}</b>";

            UDebug.LogError(outputMessage);
        }
        #endregion
    }
}