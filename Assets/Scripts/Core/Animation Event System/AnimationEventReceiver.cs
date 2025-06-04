using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AnimationEventSystem
{
    public class AnimationEventReceiver : MonoBehaviour
    {
        protected Dictionary<string, Delegate> observers = new();
        
        public void RegisterEvent<T>(string key, Action<T> handler)
        {
            if (observers.TryGetValue(key, out var existing))
            {
                observers[key] = (Action<T>)existing + handler;
                return;
            }
            
            observers[key] = handler;
        }
        
        public void RaiseEvent<T>(string key, T param)
        {
            if (!observers.TryGetValue(key, out var handler)) return;
         
            (handler as Action<T>)?.Invoke(param);
        }
        
        public void RegisterEvent(string key, Action handler)
        {
            if (observers.TryGetValue(key, out var existing))
            {
                observers[key] = (Action)existing + handler;
                return;
            }
         
            observers[key] = handler;
        }
        
        public void RaiseEvent(string key)
        {
            if (!observers.TryGetValue(key, out var handler)) return;
            
            (handler as Action)?.Invoke();
        }
        
        public void ClearEvent()
        {
            observers.Clear();
        }
    }
}
