using UnityEngine;
using System.Collections;

public class ViewController : HandleBehaviour {

    protected void SendEvent(EVT_TYPE t)
    {
        GameManager.Instance.SendEvent(t);
    }

    protected void SendEvent(Event evt)
    {
        GameManager.Instance.SendEvent(evt);
    }

    virtual protected void Init(Event evt)
    {

    }

	virtual public void DeActivate()
	{
		
	}
}
