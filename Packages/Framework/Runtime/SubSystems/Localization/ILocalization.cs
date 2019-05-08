/*
 * Description:
 * Author: Mohammad Hasan Bigdeli
 * Creation Date: 1 / 27 / 2018
 */

using System;

namespace Revy.Framework.Localization
{
    public interface ILocalization
    {
        #region Properties
        ELanguage CurrentLanguage { get; }
        Action OnLanguageChange { get; set; }
        #endregion

        #region Methods
        string GetText(string key);

        FSTextSetting GetTextSetting();

        void ChangeLanguage(ELanguage language);
        #endregion
    }
}