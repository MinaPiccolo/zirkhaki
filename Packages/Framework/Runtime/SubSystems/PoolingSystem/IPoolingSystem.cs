/*
* Description:
* Author: Hesam Sehat, sehathesam@gmail.com
* Creation Data: , 27 Dec 2018
*/

using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
    public interface IPoolingSystem
    {       
        /// <summary>
        /// Create a pool of given item.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="item"></param>
        /// <param name="reserveAmount">Initial reserve amount</param>
        /// <param name="parent"></param>
        /// <param name="isExtendable">If set to true pool will extend when there is no item available in pool</param>
        /// <param name="extendAmount">Amount of extend when there is no item available in pool</param>
        /// <returns></returns>
        bool SetItem(string itemName, GameObject sourceObject, int reserveAmount, Transform parent = null, bool isExtendable = false, int extendAmount = 5);

        /// <summary>
        /// Return an item to the pool again.
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="sourceObject"></param>
        void ReturnItem(string itemName, GameObject sourceObject);

        /// <summary>
        /// Return an available item.
        /// If there is no item available it will try to extend pool.
        /// </summary>
        /// <param name="itemName">Name of already pooled item</param>
        /// <returns></returns>
        GameObject GetItem(string itemName);
        
        /// <summary>
        /// Remove and destroy all items inside the pool.
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Destroy an item and all its instances and remove it from pool.
        /// </summary>
        /// <param name="itemName"></param>
        void RemoveItem(string itemName);
    }
}