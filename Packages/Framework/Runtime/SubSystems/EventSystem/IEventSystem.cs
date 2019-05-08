/**
 * Author: ideen molavi
 * CreationTime: 10 / 4 / 2017
 * Description:  interface  Event Manager
 **/


using System;

namespace Revy.Framework
{
    public interface IEventSystem
    {
        void ListenToEvent(string eventName, Callback callBack);

        void ListenToEvent<T1>(string eventName, Callback<T1> callBack);

        void ListenToEvent<T1, T2>(string eventName, Callback<T1, T2> callBack);

        void ListenToEvent<T1, T2, T3>(string eventName, Callback<T1, T2, T3> callBack);

        void BroadcastEvent(string eventName, EEventFilter filter = EEventFilter.None, bool invokeSync = true,
            bool invokeAsync = true);

        void BroadcastEvent<T1>(string eventName, T1 inputValue, EEventFilter filter = EEventFilter.None,
            bool invokeSync = true, bool invokeAsync = true);

        void BroadcastEvent<T1, T2>(string eventName, T1 inputValue1, T2 inputValue2,
            EEventFilter filter = EEventFilter.None, bool invokeSync = true, bool invokeAsync = true);

        void BroadcastEvent<T1, T2, T3>(string eventName, T1 inputValue1, T2 inputValue2, T3 inputValue3,
            EEventFilter filter = EEventFilter.None, bool invokeSync = true, bool invokeAsync = true);

        void RemoveEventListener(string eventName, Callback callBack);
        void RemoveEventListener<T1>(string eventName, Callback<T1> callBack);

        void RemoveEventListener<T1, T2>(string eventName, Callback<T1, T2> callBack);

        void RemoveEventListener<T1, T2, T3>(string eventName, Callback<T1, T2, T3> callBack);

        void ListenToEvent(string eventName, CallbackAsync callBack);

        void ListenToEvent<T1>(string eventName, CallbackAsync<T1> callBack);

        void ListenToEvent<T1, T2>(string eventName, CallbackAsync<T1, T2> callBack);

        void ListenToEvent<T1, T2, T3>(string eventName, CallbackAsync<T1, T2, T3> callBack);

        void RemoveEventListener(string eventName, CallbackAsync callBack);

        void RemoveEventListener<T1>(string eventName, CallbackAsync<T1> callBack);

        void RemoveEventListener<T1, T2>(string eventName, CallbackAsync<T1, T2> callBack);

        void RemoveEventListener<T1, T2, T3>(string eventName, CallbackAsync<T1, T2, T3> callBack);

        /// <summary>
        /// Remove all listeners that related to instance.
        /// </summary>
        /// <param name="instance"></param>
        void RemoveAllListeners(object instance);
    }
}