using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionKey
{
    OnPlayerHit,
}

public class EventManager : SceneSingletonMonoBehaviour<EventManager>
{

        private readonly Dictionary<ActionKey, MulticastDelegate> _events = new();
        
        public void AddListener(ActionKey actionKey, MulticastDelegate listener)
        {
            if (_events.TryGetValue(actionKey, out var existing))
            {
                if (existing.GetType() != listener.GetType())
                {
                    throw new InvalidOperationException($"이벤트 [{actionKey}]는 {existing.GetType()} 형식만 받을 수 있음. 현재: {listener.GetType()}");
                }
                _events[actionKey] = (MulticastDelegate)MulticastDelegate.Combine(existing, listener);
            }
            else
            {
                _events[actionKey] = listener;
            }
        }
        
        public void RemoveListener(ActionKey actionKey, MulticastDelegate listener)
        {
            if (_events.TryGetValue(actionKey, out var existing))
            {
                if (existing.GetType() != listener.GetType())
                {
                    throw new InvalidOperationException($"이벤트 [{actionKey}] 구독 해제 시 형식 불일치. 기대: {existing.GetType()} / 현재: {listener.GetType()}");
                }
                var updated = MulticastDelegate.Remove(existing, listener);
                if (updated == null)
                {
                    _events.Remove(actionKey);
                }
                else
                {
                    _events[actionKey] = (MulticastDelegate)updated;
                }
            }
        }
        
        public void Invoke(ActionKey actionKey, params object[] args)
        {
            if (_events.TryGetValue(actionKey, out var del))
            {
                del.DynamicInvoke(args);
            }
        }
}
