using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] protected GameEvent gameEvent;
    [SerializeField] protected UnityEvent unityEvent;
    
    void Awake() => gameEvent.Register(this);
    void OnDestroy() => gameEvent.Deregister(this);
    public virtual void RaiseEvent() => unityEvent.Invoke();
}
