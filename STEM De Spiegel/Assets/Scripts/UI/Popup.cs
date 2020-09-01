using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{
    public UnityEvent yesEvent;
    public UnityEvent noEvent;
    public UnityEvent cancelEvent;

    public void yesTrigger()
    {
        yesEvent.Invoke();
        Destroy(gameObject);
    }

    public void noTrigger()
    {
        noEvent.Invoke();
    }

    public void cancelTrigger()
    {
        cancelEvent.Invoke();
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
