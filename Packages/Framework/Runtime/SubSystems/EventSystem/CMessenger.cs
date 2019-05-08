/**
 * Added by mahdi fada
 * Add Time: 8 / 16 / 2017
 * Description: Message
 **/


/*
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * 
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 * 
 * Features:
 	* Prevents a MissingReferenceException because of a reference to a destroyed message handler.
 	* Option to log all messages
 	* Extensive error detection, preventing silent bugs
 * 
 * Usage examples:
 	1. Messenger.AddListener<GameObject>("prop collected", PropCollected);
 	   Messenger.Broadcast<GameObject>("prop collected", prop);
 	2. Messenger.AddListener<float>("speed changed", SpeedChanged);
 	   Messenger.Broadcast<float>("speed changed", 0.5f);
 * 
 * Messenger cleans up its evenTable automatically upon loading of a new level.
 * 
 * Don't forget that the messages that should survive the cleanup, should be marked with Messenger.MarkAsPermanent(string)
 * 
 */

//#define LOG_ALL_MESSAGES
//#define LOG_ADD_LISTENER
//#define LOG_BROADCAST_MESSAGE
//#define REQUIRE_LISTENER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Revy.Framework
{
    public delegate void Callback();

    public delegate void Callback<T>(T arg1);

    public delegate void Callback<T, U>(T arg1, U arg2);

    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

    public class CMessenger<TEventType>
    {
        #region Internal variables

        private const int BIG_NUMBER = int.MaxValue;
        //Disable the unused variable warning
#pragma warning disable 0414
        //Ensures that the MessengerHelper will be created automatically upon start of the game.
        //private MessengerHelper messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();
#pragma warning restore 0414

        public Dictionary<TEventType, Delegate> eventTable = new Dictionary<TEventType, Delegate>();

        //Message handlers that should never be removed, regardless of calling Cleanup
        public List<TEventType> permanentMessages = new List<TEventType>();

        #endregion

        #region Helper methods

        //Marks a certain message as permanent.
        public void MarkAsPermanent(TEventType eventType)
        {
#if LOG_ALL_MESSAGES
            Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
            permanentMessages.Add(eventType);
        }


        public void Cleanup()
        {
#if LOG_ALL_MESSAGES
            Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<TEventType> messagesToRemove = new List<TEventType>();

            foreach (KeyValuePair<TEventType, Delegate> pair in eventTable)
            {
                bool wasFound = false;

                foreach (TEventType message in permanentMessages)
                {
                    //if (pair.Key == message)
                    if (pair.Key.Equals(message))
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (TEventType message in messagesToRemove)
            {
                eventTable.Remove(message);
            }
        }

        public void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<TEventType, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        #endregion

        #region Message logging and exception throwing

        private void OnListenerAdding(TEventType eventType, Delegate listenerBeingAdded)
        {
            if (!eventTable.ContainsKey(eventType)) return;

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(
                    $"Attempting to add listener with inconsistent signature for event instance {eventType}. Current listeners have instance {d.GetType().Name} and listener being added has instance {listenerBeingAdded.GetType().Name}");
            }
        }

        private void OnListenerRemoving(TEventType eventType, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format(
                        "Attempting to remove listener with for event instance \"{0}\" but current listener is null.",
                        eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format(
                        "Attempting to remove listener with inconsistent signature for event instance {0}. Current listeners have instance {1} and listener being removed has instance {2}",
                        eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(string.Format(
                    "Attempting to remove listener for instance \"{0}\" but Messenger doesn't know about this event instance.",
                    eventType));
            }
        }

        private void OnListenerRemoved(TEventType eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        private void OnBroadcasting(TEventType eventType)
        {
#if REQUIRE_LISTENER
            if (!eventTable.ContainsKey(eventType))
            {
                throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            }
#endif
        }

        private BroadcastException CreateBroadcastSignatureException(TEventType eventType)
        {
            return new BroadcastException(string.Format(
                "Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.",
                eventType));
        }

        private class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        private class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }

        #endregion

        #region AddListener

        //No parameters
        public void AddListener(TEventType eventType, Callback handler)
        {
            OnListenerAdding(eventType, handler);

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, handler);
            }
            else if (!eventTable[eventType].GetInvocationList().Contains(handler))
            {
                eventTable[eventType] = (Callback)eventTable[eventType] + handler;
            }
        }

        //Single parameter
        public void AddListener<T>(TEventType eventType, Callback<T> handler)
        {
            OnListenerAdding(eventType, handler);

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, handler);
            }
            else if (!eventTable[eventType].GetInvocationList().Contains(handler))
            {
                eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
            }
        }

        //Two parameters
        public void AddListener<T, U>(TEventType eventType, Callback<T, U> handler)
        {
            OnListenerAdding(eventType, handler);

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, handler);
            }
            else if (!eventTable[eventType].GetInvocationList().Contains(handler))
            {
                eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
            }
        }

        //Three parameters
        public void AddListener<T, U, V>(TEventType eventType, Callback<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, handler);
            }
            else if (!eventTable[eventType].GetInvocationList().Contains(handler))
            {
                eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
            }
        }

        #endregion

        #region RemoveListener

        //No parameters
        public void RemoveListener(TEventType eventType, Callback handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
        public void RemoveListener<T>(TEventType eventType, Callback<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        public void RemoveListener<T, U>(TEventType eventType, Callback<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        public void RemoveListener<T, U, V>(TEventType eventType, Callback<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        public void RemoveAllListener(object instance)
        {
            var eventTableTmp = new Dictionary<TEventType, Delegate>(eventTable);
            foreach (Delegate listeners in eventTable.Values)
            {
                foreach (Delegate aMethod in listeners.GetInvocationList())
                {
                    if (aMethod.Target != null && aMethod.Target == instance)
                    {
                        Delegate listenerTmp = eventTableTmp.Values.First(value => value == listeners);
                        listenerTmp = Delegate.Remove(listenerTmp, aMethod);
                        if (listenerTmp == null)
                        {
                            var keyValueItem = eventTableTmp.First(item => item.Value == listeners);
                            eventTableTmp.Remove(keyValueItem.Key);
                        }
                    }
                }
            }
            eventTable = eventTableTmp;
        }
        #endregion

        #region Broadcast

        //No parameters listene
        public  void Broadcast(TEventType eventType, EEventFilter filter = EEventFilter.None)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            //	Debug.Log("MESSENGER\t" + System.DateTime.Now.ToShortTimeString() + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);
            if (!eventTable.TryGetValue(eventType, out Delegate d)) return;

            var callback = d as Callback;

            if (callback == null)
            {
                throw CreateBroadcastSignatureException(eventType);
            }

            var sortDictionary = new Dictionary<int, int>();
            var index = 0;
            var invocationList = callback.GetInvocationList();
            foreach (var item in invocationList)
            {
                var it = item as Callback;

                if (!_validate(it) || _isFilitered(it, filter))
                {
                    index++;
                    continue;
                }

                var executionAttribute =
                    it.Method.GetCustomAttribute(typeof(CExecutionOrderAttribute)) as CExecutionOrderAttribute;
                sortDictionary.Add(index, executionAttribute?.Order ?? BIG_NUMBER);
                index++;
            }

            var sortedByOrder = sortDictionary.ToList();
            sortedByOrder.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
            foreach (var order in sortedByOrder)
            {
                var value = invocationList[order.Key] as Callback;
                if (value == null) continue;
                _logBroadCast(value, eventType);
                value();
            }
        }

        //Single parameter
        public  void Broadcast<T>(TEventType eventType, T arg1, EEventFilter filter = EEventFilter.None)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            //	Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);
            if (!eventTable.TryGetValue(eventType, out Delegate d)) return;

            var callback = d as Callback<T>;

            if (callback == null)
            {
                throw CreateBroadcastSignatureException(eventType);
            }

            var sortDictionary = new Dictionary<int, int>();
            var index = 0;
            var invocationList = callback.GetInvocationList();
            foreach (var item in invocationList)
            {
                var it = item as Callback<T>;

                if (!_validate(it) || _isFilitered(it, filter))
                {
                    index++;
                    continue;
                }

                var executionAttribute =
                    it.Method.GetCustomAttribute(typeof(CExecutionOrderAttribute)) as CExecutionOrderAttribute;
                sortDictionary.Add(index, executionAttribute?.Order ?? BIG_NUMBER);
                index++;
            }

            var sortedByOrder = sortDictionary.ToList();
            sortedByOrder.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
            foreach (var order in sortedByOrder)
            {
                var value = invocationList[order.Key] as Callback<T>;
                if (value == null) continue;
                _logBroadCast(value, eventType);
                value(arg1);
            }
        }

        //Two parameters
        public  void Broadcast<T, U>(TEventType eventType, T arg1, U arg2, EEventFilter filter = EEventFilter.None)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
//		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif

            OnBroadcasting(eventType);
            if (!eventTable.TryGetValue(eventType, out Delegate d)) return;

            var callback = d as Callback<T, U>;

            if (callback == null)
            {
                throw CreateBroadcastSignatureException(eventType);
            }

            var sortDictionary = new Dictionary<int, int>();
            var index = 0;
            var invocationList = callback.GetInvocationList();
            foreach (var item in invocationList)
            {
                var it = item as Callback<T, U>;

                if (!_validate(it) || _isFilitered(it, filter))
                {
                    index++;
                    continue;
                }

                var executionAttribute =
                    it.Method.GetCustomAttribute(typeof(CExecutionOrderAttribute)) as CExecutionOrderAttribute;
                sortDictionary.Add(index, executionAttribute?.Order ?? BIG_NUMBER);
                index++;
            }

            var sortedByOrder = sortDictionary.ToList();
            sortedByOrder.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
            foreach (var order in sortedByOrder)
            {
                var value = invocationList[order.Key] as Callback<T, U>;
                if (value == null) continue;
                _logBroadCast(value, eventType);
                value(arg1, arg2);
            }
        }

        //Three parameters
        public  void Broadcast<T, U, V>(TEventType eventType, T arg1, U arg2, V arg3,
            EEventFilter filter = EEventFilter.None)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
            //Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);
            if (!eventTable.TryGetValue(eventType, out Delegate d)) return;

            var callback = d as Callback<T, U, V>;
            ;

            if (callback == null)
            {
                throw CreateBroadcastSignatureException(eventType);
            }

            var sortDictionary = new Dictionary<int, int>();
            var index = 0;
            var invocationList = callback.GetInvocationList();
            foreach (var item in invocationList)
            {
                var it = item as Callback<T, U, V>;
                if (!_validate(it) || _isFilitered(it, filter))
                {
                    index++;
                    continue;
                }

                var executionAttribute =
                    it.Method.GetCustomAttribute(typeof(CExecutionOrderAttribute)) as CExecutionOrderAttribute;
                sortDictionary.Add(index, executionAttribute?.Order ?? BIG_NUMBER);
                index++;
            }

            var sortedByOrder = sortDictionary.ToList();
            sortedByOrder.Sort((firstPair, nextPair) => firstPair.Value.CompareTo(nextPair.Value));
            foreach (var order in sortedByOrder)
            {
                var value = invocationList[order.Key] as Callback<T, U, V>;
                if (value == null) continue;
                _logBroadCast(value, eventType);
                value(arg1, arg2, arg3);
            }
        }

        #endregion

        #region Helpers

        private static void _logBroadCast(Delegate del, TEventType eventType)
        {
            //if (del == null || del.Method.DeclaringType == null) return;
            //var field = del.Method.DeclaringType.GetField("LOG_TAG",
            //    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            //var logTag = (string)field?.GetValue(del.Target) ?? string.Empty;
            //logTag = logTag == string.Empty ? del.Method.DeclaringType.Name : logTag;

            //CLog.Log($"({eventType}) event raised.", category: logTag);
        }

        private static bool _isFilitered(Delegate it, EEventFilter filter)
        {
            if (it == null) return true;
            var attribute = it.Method.GetCustomAttribute(typeof(CEventFilterAttribute)) as CEventFilterAttribute;
            var attFilter = attribute?.Filter ?? EEventFilter.None;
            return !attFilter.HasFlag(filter);
        }

        private bool _validate(Delegate d)
        {
            return d != null;
            //if (d?.Target == null)
            //{
            //    return false;
            //}

            //return !(d.Target is UnityEngine.Object) || !((UnityEngine.Object)d.Target).Equals(null);
        }

        #endregion
    }

    //This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
    public sealed class MessengerHelper : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        //Clean up eventTable every time a new level loads.
        public void OnLevelWasLoaded<T>(int unused, CMessengerAsync<T> messenger)
        {
            messenger.Cleanup();
        }
    }
}