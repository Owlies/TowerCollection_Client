using UnityEngine;
using System.Collections;


public class Unit : MonoBehaviour {

    public int ID;
    private static int GlobalID = 0;

    public UnitState baseState;
    public UnitState currentState;
    public UnitFactor stateFactor;
    public Buffs buffs;
    public ActionQueue AI;

    public StatusUI status;

    public void Init()
    {
        GlobalID ++;
        ID = GlobalID;
        buffs = new Buffs();
		AddStatusUI();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void AddStatusUI()
	{

		GameObject statusUI = Instantiate(Resources.Load<GameObject>("Model/Prefabs/StatusUI"));
		statusUI.name = "StatusUI";
		statusUI.transform.SetParent(transform, false);
		statusUI.transform.localScale = Vector3.one * 10;
		status = statusUI.GetComponent<StatusUI>();
	}

	protected void AdjustStatusUIPos()
	{

		// Adjust position
		float maxPoint = 0;
		MeshRenderer[] meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
		for(int i = 0;i<meshes.Length;i++)
		{
			Bounds bound = meshes[0].bounds;
			maxPoint = bound.max.y > maxPoint ? bound.max.y : maxPoint;
		}
		status.transform.localPosition += new Vector3(0, maxPoint - 10, 0);
	}

    virtual public void Remove()
    {
        GameObject.Destroy(gameObject);
    }
}
