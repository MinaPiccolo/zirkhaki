using System;
using UnityEngine;

namespace Revy.Framework
{
    public static partial class UtilStatic
    {
        public static class Device
        {
            private static readonly string LOG_TAG = "Device";

            public static bool HasPermission(string permission)
            {
                try
                {
                    AndroidJavaClass unityPlayerJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity =
                        unityPlayerJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaClass ContextCompatJavaClass =
                        new AndroidJavaClass("android.support.v4.content.ContextCompat");
                    object[] args = new object[]
                    {
                        currentActivity,
                        permission
                    };
                    int num = ContextCompatJavaClass.CallStatic<int>("checkSelfPermission", args);
                    return num == 0;
                }
                catch (Exception e)
                {
                    CLog.Exception($"Can not check device permission because of an exception. {e.Message}",
                        category: LOG_TAG);
                    return false;
                }
            }

            public static string GetAndroidId()
            {
                AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject androidJavaObject =
                    @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
                AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
                androidJavaClass2.CallStatic<string>("getString", new object[]
                {
                    androidJavaObject,
                    "android_id"
                });
                return androidJavaClass2.CallStatic<string>("getString", new object[]
                {
                    androidJavaObject,
                    "android_id"
                });
            }
        }
    }
}