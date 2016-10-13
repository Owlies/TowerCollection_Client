using UnityEngine;
using ProtoBufDataTemplate;
using System.Collections;

public class DefaultEventHandler {
    public static void Handle(Event evt)
    {
        Debug.Log("event Handled in handler!");
        //evt.HandleCallBacks(evt);
    }

	public static void FindMatch(Event evt)
	{
		Item item = new Item();
		item.name = "FindMatch";
		NetworkManager.Instance.SendNetworkRequest<Item>(item,
			(data)=>{
				Item receivedItem = ProtoBufLoaderTemplate.deserializeProtoObject<Item>(data);
				Debug.Log(receivedItem.name);
				GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
			},
			(data)=>{
				Debug.LogWarning("find failed!");
			}
		);
		//NetworkManager.Instance.SendNetworkRequest("Find match",
		//	(success) =>
		//	{
		//		//Debug.Log("success!");
		//		GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
		//	},
		//	(fail) =>
		//	{
		//		Debug.LogWarning("failed!");
		//	});
	}
}
