using UnityEngine;
using GameSocket;
using ProtoBufDataTemplate;
using ProtoBuf.Meta;

public class APIManager : MonoBehaviour {
	public double sendFrequency = 3.0f;
	private double sendCoolDown = 0.0f;
    private SocketClient client;

    byte[] dataToSend;

    // Use this for initialization
    void Start () {
        this.client = new SocketClient();
        this.client.CreateConnection("127.0.0.1", 8888);
        Item testItem = new Item("huayu", 999, ItemType.Hat);
        this.dataToSend = ProtoBufLoaderTemplate.serializeProtoObject<Item>(testItem);
    }
	
	// Update is called once per frame
	void Update () {
		if (this.sendCoolDown > 0) {
			this.sendCoolDown -= Time.deltaTime;
			return;
		}
        Debug.Log("Data sent!");
        this.client.SendMessageToServer(dataToSend);
		this.sendCoolDown = this.sendFrequency;
    }
}
