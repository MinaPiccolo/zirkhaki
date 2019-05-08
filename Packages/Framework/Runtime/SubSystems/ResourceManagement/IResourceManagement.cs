/*
 * The IResourceManagement interface implementations allows you to find and access Objects including assets.
 * Author: Ideen Molavi, ideenmolavi@gmail.com
 * Creation date: November 15, 2017
 */

using System.Threading.Tasks;
namespace Revy.Framework.ResourceManagement
{
    public interface IResourceManagement
    {
        Task<T> LoadAsset<T>(string assetName) where T : UnityEngine.Object;
    }

    #region Types

    public enum EResourcePreprationResult { Successfull, Faild }

    #endregion
}
