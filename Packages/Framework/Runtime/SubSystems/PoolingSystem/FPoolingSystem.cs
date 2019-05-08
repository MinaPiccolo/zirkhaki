/*
* Description:
* Author: Hesam Sehat, sehathesam@gmail.com
* Creation Data: , 27 Dec 2018
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Revy.Framework
{
    public class FPoolingSystem : ISubsystem, IPoolingSystem
    {
        #region  Field

        //--------------Private field---------------
        private const string LOG_TAG = "[FPooling System]";
        private readonly Dictionary<string, ItemPool> _pooledItems = new Dictionary<string, ItemPool>();
        private ItemPool _cacheItem;

        #endregion

        #region Properties

        public Type ServiceType => typeof(IPoolingSystem);
        
        #endregion

        #region Constructor & Destructor

        public FPoolingSystem()
        {
            MFramework.Register(this);
        }

        ~FPoolingSystem()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        #region Class Interface

        void IPoolingSystem.ReturnItem(string itemName, GameObject objectToReturn)
        {
            if (!_pooledItems.ContainsKey(itemName)) return;

            var items = _pooledItems[itemName].Items;
            int itemCount = items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                Item item = items[i];
                if (item.Value == objectToReturn)
                {
                    item.IsAvailable = true;
                    _pooledItems[itemName].Items[i] = item;
                }
            }
        }

        bool IPoolingSystem.SetItem(string itemName, GameObject item, int reserveAmount, Transform parent, bool isExtendable, int extendAmount)
        {
            return _SetPoolItem(itemName, item, reserveAmount, parent, isExtendable, extendAmount);
        }

        GameObject IPoolingSystem.GetItem(string itemName)
        {
            return _GetPooledItem(itemName);
        }

        void IPoolingSystem.Clear()
        {
            foreach (var pooledItem in _pooledItems)
            {
                foreach (var item in pooledItem.Value.Items)
                {
                    UnityEngine.Object.DestroyImmediate(item.Value);
                }
            }
            _pooledItems.Clear();
        }

        void IPoolingSystem.RemoveItem(string itemName)
        {
            if (!_pooledItems.ContainsKey(itemName)) return;

            foreach (var item in _pooledItems[itemName].Items)
            {
                UnityEngine.Object.DestroyImmediate(item.Value);
            }
            _pooledItems.Remove(itemName);
        }

        #endregion

        #region Helpers     

        private GameObject _GetPooledItem(string itemName)
        {
            if (!isValid()) return null;

            ItemPool pooledItem = _pooledItems[itemName];
            for (int i = 0; i < pooledItem.Items.Count; i++)
            {
                var item = pooledItem.Items[i];
                if (item.IsAvailable)
                {
                    item.IsAvailable = false;
                    pooledItem.Items[i] = item;
                    return item.Value;
                }
            }

            if (pooledItem.IsExtendable)
            {
                CLog.Log($"Extending pool for {itemName}.");
                Transform parent = pooledItem.Parent;
                for (int i = 0; i < pooledItem.ExtendAmount; i++)
                {
                    var newObject =  Object.Instantiate(pooledItem.SourceItem);
                    if (parent != null) newObject.transform.SetParent(parent);
                    pooledItem.Items.Add(new Item(newObject));
                }
                return _GetPooledItem(itemName);
            }
            else
            {
                CLog.Warning($"{itemName} pool item is reached it's pool limit and is not extend able.");
            }

            return null;

            bool isValid()
            {
                if (_pooledItems.ContainsKey(itemName) == false)
                {
                    CLog.Warning($"{itemName} does not exist in pooling system please set object first", category: LOG_TAG);
                    return false;
                }

                return true;
            }
        }

        private bool _SetPoolItem(string itemName, GameObject sourceObject, int reserveAmount, Transform parent = null, bool extendable = false, int extendAmount = 5)
        {
            if (!isValid()) return false;

            var newItems = new List<Item>();
            sourceObject.SetActive(false);
            for (int i = 0; i < reserveAmount; i++)
            {
                var newObject = Object.Instantiate(sourceObject);
                if (parent != null) newObject.transform.SetParent(parent);
                newItems.Add(new Item(newObject));
            }
            _pooledItems.Add(itemName, new ItemPool(newItems, sourceObject, parent, reserveAmount, extendable, extendAmount));

            return true;

            bool isValid()
            {
                if (sourceObject == null)
                {
                    CLog.Warning("Item is null.", category: LOG_TAG);
                    return false;
                }

                if (string.IsNullOrEmpty(itemName))
                {
                    CLog.Warning("Item name is empty or null.", category: LOG_TAG);
                    return false;
                }

                if (_pooledItems.ContainsKey(itemName))
                {
                    CLog.Warning("Pool item already exist.", category: LOG_TAG);
                    return false;
                }

                if (reserveAmount <= 0) reserveAmount = 5;
                if (extendAmount <= 0) extendAmount = 5;
                return true;
            }
        }

        #endregion

        #region Nested Types

        private struct ItemPool
        {
            public ItemPool(List<Item> items, GameObject sourceItem, Transform parent, int reserve, bool isExtendable = false, int extendAmount = 5)
            {
                SourceItem = sourceItem;
                Items = items;
                Parent = parent;
                ReserveAmount = reserve;
                IsExtendable = isExtendable;
                ExtendAmount = extendAmount;
            }

            public readonly List<Item> Items;
            public readonly GameObject SourceItem;
            public readonly Transform Parent;
            public readonly int ReserveAmount;
            public readonly bool IsExtendable;
            public readonly int ExtendAmount;
        }

        private struct Item
        {
            public Item(GameObject value)
            {
                Value = value;
                _isAvailable = true;
                value.SetActive(false);
            }
            public readonly GameObject Value;
            public bool IsAvailable
            {
                get
                {
                    return _isAvailable;
                }
                set
                {
                    _isAvailable = value;
                    Value.SetActive(!value);
                }
            }
            private bool _isAvailable;
        }

        #endregion
    }
}