using UnityEngine;
using System.Collections;
using Sproto;
using SprotoType;

public class DefaultEventHandler {
    public static void Handle(Event evt)
    {
        Debug.Log("event Handled in handler!");
        //evt.HandleCallBacks(evt);
    }

	public static void FindMatch(Event evt)
	{
		Person person = new Person();
		person.name = "FindMatch";
		//NetworkManager.Instance.SendNetworkRequest<Person>(person,
		//	(data)=>{
		//		Person receivedPerson = new Person(data);
		//		Debug.Log(receivedPerson.name);
		//		GameManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_ENTER_GAME);
		//	},
		//	(data)=>{
		//		Debug.LogWarning("find failed!");
		//	}
		//);
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
