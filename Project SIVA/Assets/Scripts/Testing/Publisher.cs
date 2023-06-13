 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Publisher : MonoBehaviour
{
    private List<IObserver> _observers = new();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers(GameEvents gameEvent)
    {
        _observers.ForEach(_observer =>
       {
           _observer.OnNotify(gameEvent);
       });
    }
}
