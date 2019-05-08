/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 10 / 17 / 2017
 * Description: This class contains primitive commands for terminal system.
 */

using UnityEngine;

namespace Revy.Framework
{
    public class CTerminalPrimitiveCommands : FClass
    {
        #region Properties
        [CProperty("TimeScale")]
        private float TimeScale
        {
            get
            {
                return Time.timeScale;
            }
            set
            {
                Time.timeScale = value;
            }
        }

        [CProperty("Version")]
        private string Version
        {
            get { return Application.version; }
        }

        [CProperty("UnityVersion")]
        private string EngineVersion
        {
            get { return Application.unityVersion; }
        }

        [CProperty("Resolution")]
        private string Resolution
        {
            get { return Screen.currentResolution.ToString(); }
        }

        [CProperty("Aspect")]
        private float AspectRatio
        {
            get { return Camera.main.aspect; }
        }

        [CProperty("DPI")]
        private float DPI
        {
            get { return Screen.dpi; }
        }

        [CProperty("FullScreen")]
        private bool FullScreen
        {
            get { return Screen.fullScreen; }
            set { Screen.fullScreen = value; }
        }

        [CProperty("DataPath")]
        private string DataPath
        {
            get { return Application.dataPath; }
        }
        #endregion

        #region Methods
        [CMethod("Pause")]
        private void Pause()
        {
            MFramework.SetPause(true);
        }

        [CMethod("Resume")]
        private void Resume()
        {
            MFramework.SetPause(false);
        }

        [CMethod("Startup")]
        private void StartupFramework()
        {
            MFramework.StartFramework();
        }

        [CMethod("KeepDisplayOn")]
        private void KeepDisplayOn()
        {
            Screen.sleepTimeout = 0;
        }

        [CMethod("Quit")]
        private void Quit()
        {
            Application.Quit();
        }

        [CMethod("TotalMemory")]
        private long TotalMemory()
        {
            return System.GC.GetTotalMemory(false);
        }

        [CMethod("GCCollect")]
        private void GCCollect()
        {
            System.GC.Collect();
        }
        #endregion
    }
}