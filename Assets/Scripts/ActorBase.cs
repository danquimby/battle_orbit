using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor {
    void Destroy();
}

public class ActorBase : MonoBehaviour, IActor
{
    void Start()
    {
        OnStart();
    }
    void Awake()
    {
        OnAwake();
    }
    public void Release()
    {
        Stop();
        gameObject.SetActive(false);
    }
    public void Destroy() // todo пока не заню нужен ли 
    {
        Stop();
        Console.info("Уничтожение обьекта");
        Destroy(this);
    }
    public virtual void Hit() { }
    protected virtual void OnStart() { }
    protected virtual void OnAwake() { }
    public virtual void Stop() { }

}
