using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.UI;

public class GameScreenController : ViewController {

	public Scrollbar scrollBar;
	public RectTransform screenContainer;

	public Button battleButton;
	private ScrollRect scrollRect;

	public int screenCount = 3;
	public bool moving = false;
	public bool hasMove = false;
	public int screenStartIndex = 0;

	public bool mouseValid = true;

	List<float> screenPositions = new List<float>();
	float screenDistance = 0;

	public int currentScreenIndex = 1;

	// Use this for initialization
	void Start () {
		screenDistance = 0.25f/(screenCount-1);
		for(int i = 0;i<screenCount;i++)
		{
			screenPositions.Add(i * 1.0f / (screenCount-1));
		}

		scrollBar.value = screenPositions[currentScreenIndex];
		if(screenContainer != null)
		{
			scrollRect = screenContainer.GetComponent<ScrollRect>();
		}
		battleButton.onClick.AddListener(()=>{
			SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
		});
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0) && !moving)
		{
			if(scrollBar.value - screenPositions[currentScreenIndex] > screenDistance
				&& currentScreenIndex != (screenCount-1))	// moving right
			{
				moving = true;
				currentScreenIndex++;
			}
			else if(scrollBar.value - screenPositions[currentScreenIndex] < -screenDistance 
				&& currentScreenIndex != 0) // moving left
			{
				moving = true;
				currentScreenIndex--;
			}
		}
		if(moving)
		{
			hasMove = true;
			mouseValid = false;
			float deltaPos = (screenPositions[currentScreenIndex] - scrollBar.value) * 10;
			if(Mathf.Abs(deltaPos) < 0.01f)
			{
				moving = false;
				hasMove = false;
				scrollBar.value = screenPositions[currentScreenIndex];
				scrollRect.enabled = false;
			}
			else
			{
				scrollBar.value += Time.deltaTime*deltaPos;
			}
		}
			
		if(Input.GetMouseButtonUp(0) && !hasMove)
		{
			moving = true;
		}

		// re enable mouse when release
		if(!Input.GetMouseButton(0) && !moving)
		{
			mouseValid = true;
			scrollRect.enabled = true;
		}

	}

}
