/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Time: 8 / 29 / 2017
 * Description: This class will show fps on screen with IGUI
 */

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
    public class FFPSCounter : FComponent, IInitializable, ITick
    {
        #region Fields
        private UnityEngine.UI.Text _fpsText;

        private const float _fpsMeasurePeriod = 0.5f;
        private int _FpsAccumulator = 0;
        private float _FpsNextPeriod = 0;
        private int _CurrentFps;
        private const string GREEN = "00ff00";
        private const string RED = "FF0000";
        #endregion

        #region Properties

        bool IActiveable.IsActive { get; set; }

        #endregion

        #region Methods

        public bool HasInitialized { get; set; }

        public  void Initialize()
        {
            _fpsText = GetComponent<UnityEngine.UI.Text>();
            gameObject.SetActive(false);
        }

        public  Task BeginPlay()
        {
            _FpsNextPeriod = Time.realtimeSinceStartup + _fpsMeasurePeriod;

            return Task.CompletedTask;
        }

        public void Tick()
        {
            _FpsAccumulator++;
            if (Time.realtimeSinceStartup > _FpsNextPeriod)
            {
                _CurrentFps = (int)(_FpsAccumulator / _fpsMeasurePeriod);
                _FpsAccumulator = 0;
                _FpsNextPeriod += _fpsMeasurePeriod;
            }

            string color = _CurrentFps > 50 ? GREEN : RED;

            _fpsText.text = $"<color=#{color}><b>FPS {_CurrentFps}</b></color>";
        }
        #endregion

        #region Helper
        [CMethod("FPS")]
        private void ToggleFPS()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
        #endregion

        public int CompareTo(IInitializable other)
        {
            throw new System.NotImplementedException();
        }
    }
}
