using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int i = 0;
        while (i < Input.touchCount)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
				HandleClickPoint(Input.GetTouch(i).position);
            }
            ++i;
        }

        if (Input.GetMouseButtonDown(0))
        {
			HandleClickPoint(Input.mousePosition);
        }  
	}

	private void HandleClickPoint(Vector3 point)
	{
    //    Ray ray = Camera.main.ScreenPointToRay(point);
    //    RaycastHit raycastInfo;
    //    if (Physics.Raycast(ray, out raycastInfo, GameConstant.MAX_DISTANCE, 1 << LayerMask.NameToLayer("Tower")))
    //    {
    //        Tower towerA = raycastInfo.collider.gameObject.GetComponentInParent<Tower>();
    //        if(towerA != null)
    //        {
    //            towerA.Ultimate();
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Gameobject : " + raycastInfo.collider.gameObject.name + " doesn't have Tower!");
    //        }
    //    }

    //    if (Physics.Raycast(ray, out raycastInfo, GameConstant.MAX_DISTANCE, 1 << LayerMask.NameToLayer("UI")))
    //    {
    //        CrystalUI crystal = raycastInfo.collider.gameObject.GetComponentInParent<CrystalUI>();
    //        if(crystal != null)
    //        {
    //            crystal.CollectCrystal();
    //        }
    //    }
    }
}
