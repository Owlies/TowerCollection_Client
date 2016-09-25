using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviour {

	private static NetworkManager m_instance;
	private NetworkManager() { }

	[Range(0,1)]
	public float mSimulateSuccessRate = 0.5f;

	[Range(0,1)]
	public float mSimulateRespondTime = 0.5f;

	public static NetworkManager Instance
	{
		get
		{
			if (m_instance == null)
				m_instance = GameObject.FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
			return m_instance;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendNetworkRequest(string request, Action<object> success, Action<object> fail)
	{
		//Debug.Log("sending request : " + request);
		StartCoroutine( SimulateNetworkRequest(request, success, fail) );
	}

	IEnumerator SimulateNetworkRequest(string request, Action<object> success, Action<object> fail)
	{
		if(Random.Range(0,1.0f) < mSimulateSuccessRate)
		{
			yield return new WaitForSeconds(mSimulateRespondTime);
			//Debug.LogWarning("Success Request : " + request);
			success("success");
		}
		else
		{
			yield return new WaitForSeconds(mSimulateRespondTime);
			fail("fail");
		}
	}
}
