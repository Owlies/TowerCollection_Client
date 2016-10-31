using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class TwoScrollRect : ScrollRect {

	private bool routeToParent = false;
	ScrollRect parentScroll;

	protected override void Awake ()
	{
		base.Awake ();
		Transform parent = transform.parent;
		while(parent != null) {
			parentScroll = parent.GetComponent<ScrollRect>();
			if(parentScroll != null){
				break;
			}
			parent = parent.parent;
		}
	}


	/// <summary>
	/// Always route initialize potential drag event to parents
	/// </summary>
	public override void OnInitializePotentialDrag (PointerEventData eventData)
	{
		parentScroll.OnInitializePotentialDrag(eventData);
		base.OnInitializePotentialDrag (eventData);

	}

	/// <summary>
	/// Drag event
	/// </summary>
	public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		if(routeToParent)
			parentScroll.OnDrag(eventData);
		else
			base.OnDrag (eventData);
	}

	/// <summary>
	/// Begin drag event
	/// </summary>
	public override void OnBeginDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		if(!horizontal && Math.Abs (eventData.delta.x) > Math.Abs (eventData.delta.y))
			routeToParent = true;
		else if(!vertical && Math.Abs (eventData.delta.x) < Math.Abs (eventData.delta.y))
			routeToParent = true;
		else
			routeToParent = false;

		if(routeToParent)
		{
			parentScroll.OnBeginDrag(eventData);
		}
		else
		{
			base.OnBeginDrag (eventData);
		}
	}

	/// <summary>
	/// End drag event
	/// </summary>
	public override void OnEndDrag (UnityEngine.EventSystems.PointerEventData eventData)
	{
		if(routeToParent)
			parentScroll.OnEndDrag(eventData);
		else
			base.OnEndDrag (eventData);

		routeToParent = false;
	}


}
