using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void DeActivate()
	{
		
	}
}
