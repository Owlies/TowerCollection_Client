using UnityEngine;
using System.Collections;

public class DefaultEventHandler {
    public static void Handle(Event evt)
    {
        Debug.Log("event Handled in handler!");
        //evt.HandleCallBacks(evt);
    }
}
