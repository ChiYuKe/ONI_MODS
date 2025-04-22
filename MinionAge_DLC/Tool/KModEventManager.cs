using System;
using System.Collections.Generic;

namespace KModTool
{
    public class KModEventManager
    {
        // 单例模式
        private static KModEventManager _instance;
        public static KModEventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KModEventManager();
                }
                return _instance;
            }
        }

        // 使用字典存储事件及其订阅的监听器
        private readonly Dictionary<string, HashSet<Action<object>>> eventListeners = new Dictionary<string, HashSet<Action<object>>>();


        // 订阅事件
        public void Subscribe(string eventName, Action<object> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new HashSet<Action<object>>();
            }

            eventListeners[eventName].Add(listener);  // HashSet 会自动防止重复
        }

        // 触发事件
        public void TriggerEvent(string eventName, object data)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                // 创建监听器集合的副本
                var listenersCopy = new List<Action<object>>(eventListeners[eventName]);

                foreach (var listener in listenersCopy)
                {
                    listener(data);  // 执行回调
                }
            }
        }


        // 取消订阅
        public void Unsubscribe(string eventName, Action<object> listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                var listeners = eventListeners[eventName];
                listeners.Remove(listener);  // 直接移除
                if (listeners.Count == 0)
                {
                    eventListeners.Remove(eventName);  // 如果该事件没有监听器，删除事件
                }
            }
        }
    }
}
