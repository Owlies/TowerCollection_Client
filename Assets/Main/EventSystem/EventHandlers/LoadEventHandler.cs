using UnityEngine;
using System.Collections;

public class LoadEventHandler
{
    public static void LoadFailed(Event evt)
    {
        Debug.Log( evt.evt_obj[0] +" Load failed!");
        //evt.HandleCallBacks(evt);
    }

}
