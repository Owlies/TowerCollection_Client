using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour {

	public Text text;
	public Button button;

	public bool selected = false;
	Color unselectedColor = Color.white;
	Color selectedColor = Color.gray;
	ColorBlock colors;
	Image image;

	void Start()
	{
		image = GetComponent<Image>();
	}

	public void SetText(string info)
	{
		if(text != null)
		{
		   text.text = info;
		}
	}

	public void SetSwitchable(bool enabled)
	{
		if(button != null)
		{
			if(enabled)
			{
				button.onClick.AddListener( Switchable );
			}
			else
			{
				button.onClick.RemoveListener( Switchable );
			}
		}
	}

	public void AddOnClick(UnityEngine.Events.UnityAction action)
	{
		if(button!=null)
		{
			button.onClick.AddListener(action);
		}
	}

	public void Deselect()
	{
		if(image != null && selected)
		{
			image.color = unselectedColor;
			selected = false;
		}
	}

	public void Select()
	{
		if(image != null && !selected)
		{
			image.color = selectedColor;
			selected = true;
		}
	}

	void Switchable()
	{
		if(image == null)
			return;
		selected = !selected;
		if(selected)
		{
			image.color = selectedColor;
		}
		else
		{
			image.color = unselectedColor;
		}
	}

}
