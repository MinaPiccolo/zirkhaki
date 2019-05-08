/**
 * َAuthor: Ideen Molavi Nejad
 * Email: ideenmolavi@gmail.com
 **/

using System;

namespace Revy.Framework
{
    public class EventSystem : ISubsystem, IEventSystem, IDIRegister
    {
        private readonly CMessenger<string> _messenger;
        private readonly CMessengerAsync<string> _messengerAsync;

        Type IDIRegister.ServiceType => typeof(IEventSystem);

        #region Constructor & Destructor

        public EventSystem()
        {
            MFramework.Register(this);
            _messenger = new CMessenger<string>();
            _messengerAsync = new CMessengerAsync<string>();
        }

        ~EventSystem()
        {
            MFramework.UnRegister(this);
        }

        #endregion Constructor & Destructor

        void IEventSystem.ListenToEvent(string eventName, Callback callback)
        {
            _messenger.AddListener(eventName, callback);
        }

        void IEventSystem.ListenToEvent<T1>(string eventName, Callback<T1> callback)
        {
            _messenger.AddListener(eventName, callback);
        }

        void IEventSystem.ListenToEvent<T1, T2>(string eventName, Callback<T1, T2> callback)
        {
            _messenger.AddListener(eventName, callback);
        }

        void IEventSystem.ListenToEvent<T1, T2, T3>(string eventName, Callback<T1, T2, T3> callback)
        {
            _messenger.AddListener(eventName, callback);
        }

        void IEventSystem.RemoveEventListener<T1>(string eventName, Callback<T1> callback)
        {
            _messenger.RemoveListener(eventName, callback);
        }

        void IEventSystem.RemoveEventListener<T1, T2>(string eventName, Callback<T1, T2> callback)
        {
            _messenger.RemoveListener(eventName, callback);
        }

        void IEventSystem.RemoveEventListener<T1, T2, T3>(string eventName, Callback<T1, T2, T3> callback)
        {
            _messenger.RemoveListener(eventName, callback);
        }

        void IEventSystem.RemoveEventListener(string eventName, Callback callback)
        {
            _messenger.RemoveListener(eventName, callback);
        }

        void IEventSystem.ListenToEvent(string eventName, CallbackAsync callBack)
        {
            _messengerAsync.AddListener(eventName, callBack);
        }

        public void ListenToEvent<T1>(string eventName, CallbackAsync<T1> callBack)
        {
            _messengerAsync.AddListener(eventName, callBack);
        }

        public void ListenToEvent<T1, T2>(string eventName, CallbackAsync<T1, T2> callBack)
        {
            _messengerAsync.AddListener(eventName, callBack);
        }

        public void ListenToEvent<T1, T2, T3>(string eventName, CallbackAsync<T1, T2, T3> callBack)
        {
            _messengerAsync.AddListener(eventName, callBack);
        }

        public void RemoveEventListener(string eventName, CallbackAsync callBack)
        {
            _messengerAsync.RemoveListener(eventName, callBack);
        }

        public void RemoveEventListener<T1>(string eventName, CallbackAsync<T1> callBack)
        {
            _messengerAsync.RemoveListener(eventName, callBack);
        }

        public void RemoveEventListener<T1, T2>(string eventName, CallbackAsync<T1, T2> callBack)
        {
            _messengerAsync.RemoveListener(eventName, callBack);
        }

        public void RemoveEventListener<T1, T2, T3>(string eventName, CallbackAsync<T1, T2, T3> callBack)
        {
            _messengerAsync.RemoveListener(eventName, callBack);
        }

        void IEventSystem.BroadcastEvent(string eventName, EEventFilter filter, bool invokeSync, bool invokeAsync)
        {
            if (invokeSync)
                _messenger.Broadcast(eventName, filter);

            if (invokeAsync)
                _messengerAsync.Broadcast(eventName, filter);
        }

        void IEventSystem.BroadcastEvent<T1>(string eventName, T1 inputValue, EEventFilter filter, bool invokeSync,
            bool invokeAsync)
        {
            if (invokeSync)
                _messenger.Broadcast(eventName, inputValue, filter);

            if (invokeAsync)
                _messengerAsync.Broadcast(eventName, inputValue, filter);
        }

        void IEventSystem.BroadcastEvent<T1, T2>(string eventName, T1 inputValue1, T2 inputValue2, EEventFilter filter,
            bool invokeSync, bool invokeAsync)
        {
            if (invokeSync)
                _messenger.Broadcast(eventName, inputValue1, inputValue2, filter);

            if (invokeAsync)
                _messengerAsync.Broadcast(eventName, inputValue1, inputValue2, filter);
        }

        void IEventSystem.BroadcastEvent<T1, T2, T3>(string eventName, T1 inputValue1, T2 inputValue2, T3 inputValue3,
            EEventFilter filter, bool invokeSync, bool invokeAsync)
        {
            if (invokeSync)
                _messenger.Broadcast(eventName, inputValue1, inputValue2, inputValue3, filter);

            if (invokeAsync)
                _messengerAsync.Broadcast(eventName, inputValue1, inputValue2, inputValue3, filter);
        }

        void IEventSystem.RemoveAllListeners(object instance)
        {
            _messenger.RemoveAllListener(instance);
            _messengerAsync.RemoveAllListener(instance);
        }
    }
}