/*
 * The FResourceManagement class allows you to find and access Objects including assets.
 * Author: Ideen Molavi, ideenmolavi@gmail.com
 * Creation date: November 15, 2017
 */

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if SIMULATE_ASSET_BUNDLE && UNITY_EDITOR
using AssetBundles.Manager;
#endif

namespace Revy.Framework.ResourceManagement
{
    public class FResourceManagement : ISubsystem, IResourceManagement
    {
        #region  Fields

        private const string LOG_TAG = "<color=green><b>[Resource Management]</b></color>";

        #endregion

        #region Properties

        public Type ServiceType => typeof(IResourceManagement);
        
        #endregion

        #region Constructor & Destructor

        public FResourceManagement()
        {
            MFramework.Register(this);
        }

        ~FResourceManagement()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        #region Class Interface

        async Task<T> IResourceManagement.LoadAsset<T>(string assetName)
        {
            var oldTime = Time.realtimeSinceStartup;
            var tcs = new TaskCompletionSource<bool>();
            T loadedAsset = null;
            Addressables.LoadAsset<T>(assetName).Completed += (op) =>
             {
                 loadedAsset = op.Result;
                 if (loadedAsset == null)
                 {
                     CLog.Warning($"Loading asset({assetName}) failed.", category: LOG_TAG);
                     tcs.SetResult(false);
                     return;
                 }
                 CLog.Log($"Loading asset({assetName}) takes {Time.realtimeSinceStartup - oldTime} seconds.", category: LOG_TAG);
                 tcs.SetResult(true);
             };
            await tcs.Task;
            return loadedAsset;
        }

        #endregion

        #region Helpers

        #endregion
    }
}