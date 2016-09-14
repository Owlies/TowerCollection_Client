using UnityEngine;
using System.Collections;

public class CrystalUI : MonoBehaviour {

	private int myValue = 1;
	private float curScale = 1;
	private float expandSpeed;
	public  float expandTime = 1;

	bool collected = false;
	static Vector3 destPoint = new Vector3(47, 47, 403);

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {

		if(!collected)
		{
			curScale += expandSpeed * Time.deltaTime;
			if(curScale >= myValue)	// crystal droped
			{
				curScale = myValue;
				CollectCrystal();
			}
			transform.localScale = Vector3.one * curScale * 2;
		}
		else
		{
			Vector3 dir = destPoint - transform.position;
			Vector3 moveDis = dir * Time.deltaTime * 10;
			if(transform.localScale.x > 5)
			{
				transform.localScale -= transform.localScale * moveDis.magnitude/dir.magnitude;
			}

			transform.position += moveDis;
			if(dir.magnitude < 5)
			{
				DestroyCrystal();
			}
		}
	}

	public void SetCrystalValue(int value)
	{
		myValue = value;
		curScale = 0.5f;
		expandSpeed = (myValue - curScale)/expandTime;
	}

	public void CollectCrystal()
	{
		collected = true;
	}

	public void DestroyCrystal()
	{
        GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_CHANGE_EC, myValue);
		Destroy(gameObject);
	}
}
