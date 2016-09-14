using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrystalBarController : ViewController {

	public int realCrystalValue = 0;
	public float curCrystalValue = 0;
	const int maxValue = 100;

	public RectTransform crystalCount;

	void Awake()
	{
		Init();
	}

	void Init()
	{
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		FillCrystalBar();
	}

	void FillCrystalBar()
	{
		if(curCrystalValue < realCrystalValue)
		{
			curCrystalValue += (realCrystalValue-curCrystalValue) * 0.1f;
		}
		else if(curCrystalValue > realCrystalValue)
		{
			curCrystalValue = realCrystalValue;
		}
		crystalCount.sizeDelta = new Vector2(curCrystalValue * 4.3f, crystalCount.sizeDelta.y);
	}

	public void AddCrystal(int count)
	{
		//Debug.Log("add crystal : " + count);
		realCrystalValue += count;
		realCrystalValue = realCrystalValue > maxValue ? maxValue : realCrystalValue;
		realCrystalValue = realCrystalValue < 0 ? 0 : realCrystalValue;
	}

	public void CloseAll()
	{
	}
}