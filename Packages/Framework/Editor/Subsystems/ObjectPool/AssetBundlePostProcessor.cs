/*
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 11 / 28 / 2017
 * Description: This class will used to determine that asset bundle name for an asset has changed.
 */

public class AssetBundlePostProcessor : UnityEditor.AssetPostprocessor
{
    #region Fields
    private static bool _isAssetBundleDirty;
    #endregion

    #region Properties
    public static bool IsAssetBundleDirty
    {
        get
        {
            if (!_isAssetBundleDirty) return _isAssetBundleDirty;

            _isAssetBundleDirty = false;

            return true;
        }
    }
    #endregion

    #region Methods
    public void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    {
        _isAssetBundleDirty = true;
    }
    #endregion
}
