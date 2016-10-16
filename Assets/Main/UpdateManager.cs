using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void UpdateFun();

public class UpdateManager : MonoBehaviour {

    private static UpdateManager m_instance;
    private UpdateManager() { }

    public static UpdateManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType(typeof(UpdateManager)) as UpdateManager;
            return m_instance;
        }
    }

    List<UpdateFun> updateFuns = new List<UpdateFun>();
    List<UpdateFun> readyToUpdateFuns = new List<UpdateFun>();
    List<UpdateFun> readyToRemoveFuns = new List<UpdateFun>();

    public void RegisterUpdateFun(UpdateFun fun)
    {
        readyToUpdateFuns.Add(fun);
    }

    public void UnRegisterUpdateFun(UpdateFun fun)
    {
        readyToRemoveFuns.Add(fun);
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        foreach (UpdateFun fun in readyToUpdateFuns)
            updateFuns.Add(fun);
        readyToUpdateFuns.Clear();
        foreach (UpdateFun fun in readyToRemoveFuns)
            updateFuns.Remove(fun);
        readyToRemoveFuns.Clear();
        foreach (UpdateFun fun in updateFuns)
            fun();
	}
}
