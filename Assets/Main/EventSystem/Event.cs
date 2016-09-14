using UnityEngine;
using System.Collections;

public class Event
{
    public Event(EVT_TYPE t, object obj= null)
    { 
        type = t; 
        evt_obj = new ArrayList(); 
        if (obj != null)
            evt_obj.Add(obj); 
    }
    public Event(EVT_TYPE t, ArrayList list) { type = t; evt_obj = list; }

    public ArrayList evt_obj;
    public EVT_TYPE type;
}
