using UnityEngine;
using GameSocket;
using ProtoBufDataTemplate;
using ProtoBuf.Meta;
using System;

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
		byte[] data = ProtoBufLoaderTemplate.serializeProtoObject<Item>(testItem);
		int size = this.dataToSend.Length;
		char byte1 = (char)(size/256);
		char byte2 = (char)(size%256);

		this.dataToSend = new byte[data.Length + 2];
		this.dataToSend[0] = Convert.ToByte(byte1);
		this.dataToSend[1] = Convert.ToByte(byte2);
		System.Buffer.BlockCopy(data, 0, this.dataToSend, 2, data.Length);
    }
	
	// Update is called once per frame
	void Update () {
		if (this.sendCoolDown > 0) {
			this.sendCoolDown -= Time.deltaTime;
			return;
		}
        Debug.Log("Data sent!");
        this.client.SendMessageToServer(this.dataToSend);
		this.sendCoolDown = this.sendFrequency;

        if (this.client.receivedDataSize > 0) {
            Item receivedItem = ProtoBufLoaderTemplate.deserializeProtoObject<Item>(this.client.receivedData);
            Debug.Log("Received Item Name: " + receivedItem.name);
            this.client.receivedDataSize = 0;
        }
        
    }
}
