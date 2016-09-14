using UnityEngine;
using System.Collections;

public class StatusUI : MonoBehaviour {

	public GameObject HPBarUI;
	public GameObject CurrentHPUI;

	public float CurHP = 1;
	public float RealHP = 1;
	public float ChangeHPSpeed = 1;

	// Use this for initialization
	void Start () {
		SetHPBar();
	}
	
	// Update is called once per frame
	void Update () {
		if(RealHP < CurHP)
		{
			CurHP -= Time.deltaTime * ChangeHPSpeed ;
			CurHP = CurHP < RealHP ? RealHP : CurHP;
		}
		else if(RealHP > CurHP)
		{
			CurHP += Time.deltaTime * ChangeHPSpeed ;
			CurHP = CurHP > RealHP ? RealHP : CurHP;
		}
		SetHPBar();
	}

	// Make sure HP is no less than 0
    public void SetHP(float val)
    {
        RealHP = val > 0 ? val : 0;
    }

	public void SetHPBar()
	{
		Vector3 scale = CurrentHPUI.transform.localScale;
		scale.x = CurHP;
		CurrentHPUI.transform.localScale = scale;
		Vector3 pos = CurrentHPUI.transform.localPosition;
		pos.x = CurHP*1.2f - 1.2f;
		CurrentHPUI.transform.localPosition = pos;

		CurrentHPUI.GetComponent<SpriteRenderer>().color = new Color(1-CurHP, CurHP, 0) * 2;
		HPBarUI.SetActive(CurHP > 0 && RealHP < 1);
		CurrentHPUI.SetActive(RealHP < 1);
	}
}
