using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeBarController : ViewController {

	private Text m_time;
	private Animator m_animator;
	private bool m_startPop;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		m_startPop = false;
		for(int i = 0;i<transform.childCount;i++)
		{
			if(transform.GetChild(i).name == "Time")
			{
				m_time = transform.GetChild(i).GetComponent<Text>();
				m_animator = m_time.GetComponent<Animator>();
			}
		}
	}

	public void SetTime(int seconds)
	{
		m_time.text = seconds/60 + ":" + (seconds%60).ToString("00");
		m_time.color = seconds > 10 ? Color.white : Color.red;
		if(seconds < 11 && !m_startPop)
		{
			m_animator.SetTrigger("TimePop");
			m_startPop = false;
		}
		if(seconds <= 0)
		{
			m_animator.SetTrigger("TimeIdle");
		}
	}
}
