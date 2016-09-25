using UnityEngine;
using System.Collections;

public class DefaultEventHandler {
    public static void Handle(Event evt)
    {
        Debug.Log("event Handled in handler!");
        //evt.HandleCallBacks(evt);
    }

	public static void FindMatch(Event evt)
	{
		NetworkManager.Instance.SendNetworkRequest("Find match",
			(success) =>
			{
				//Debug.Log("success!");
				GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
			},
			(fail) =>
			{
				Debug.LogWarning("failed!");
			});
	}
}
