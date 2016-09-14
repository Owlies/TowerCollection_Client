	using UnityEngine;
using System.Collections;

public class GameOverPopUpController : ViewController {

	float m_scale = 0.3f;
	float m_expandSpeed = 5;
	RectTransform m_rect;

	// Use this for initialization
	void Start () {
		m_rect = GetComponent<RectTransform>();
		m_rect.localScale = Vector3.one * m_scale;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_scale < 1)
		{
			m_scale += Time.deltaTime * m_expandSpeed;
			m_rect.localScale = Vector3.one * m_scale;
		}
	}

	public override void DeActivate()
	{
		Debug.Log("Game restart!");
		// TODO: Add Restart Game Here...
	}
}
