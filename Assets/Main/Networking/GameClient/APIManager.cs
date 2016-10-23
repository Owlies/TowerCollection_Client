using UnityEngine;
using GameSocket;
using ProtoBufDataTemplate;
using ProtoBuf.Meta;
using System;

public class APIManager : HandleBehaviour {
	public double sendFrequency = 3.0f;
	private double sendCoolDown = 0.0f;
    private SocketClient client;
	NetworkRequest pendingRequest;

    byte[] dataToSend;

    // Use this for initialization
    void Start () {
		this.client = new SocketClient();
		this.client.CreateConnection("127.0.0.1", 8888);
		Item testItem = new Item("huayu", 999, ItemType.Hat);
		byte[] data = ProtoBufLoaderTemplate.serializeProtoObject<Item>(testItem);
		int size = data.Length;
		
		string protobufType = "Item";
		
		int totalSize = data.Length + 4 + protobufType.Length;
		this.dataToSend = new byte[totalSize];
		
		
		char byte3 = (char)(protobufType.Length/256);
		char byte4 = (char)(protobufType.Length%256);
		this.dataToSend[2] = Convert.ToByte(byte3);
		this.dataToSend[3] = Convert.ToByte(byte4);
		
		char[] charArr = protobufType.ToCharArray();
		for (int i = 0; i < charArr.Length; i++) {
			byte current = Convert.ToByte(charArr[i]);
			this.dataToSend[i + 4] = current;
		}
		
		char byte1 = (char)(totalSize/256);
		char byte2 = (char)(totalSize%256);
		
		this.dataToSend[0] = Convert.ToByte(byte1);
		this.dataToSend[1] = Convert.ToByte(byte2);
		
		System.Buffer.BlockCopy(data, 0, this.dataToSend, 4 + protobufType.Length, data.Length);
    }
	
	// Update is called once per frame
	override protected void HandleUpdate () {
		// fake return response
		if(pendingRequest != null && (UnityEngine.Random.Range(0,1.0f) > 0.95f))
		{
			GetResponse(ProtoBufLoaderTemplate.serializeProtoObject<Item>(new Item("Huayu?", 999, ItemType.Hat)));
			pendingRequest = null;
		}

		if (this.sendCoolDown > 0) {
			this.sendCoolDown -= Time.deltaTime;
			return;
		}
        Debug.Log("Data sent!");
        this.client.SendMessageToServer(this.dataToSend);
		this.sendCoolDown = this.sendFrequency;

        if (this.client.receivedDataSize > 0) {
			GetResponse(this.client.receivedData);
            Item receivedItem = ProtoBufLoaderTemplate.deserializeProtoObject<Item>(this.client.receivedData);
            Debug.Log("Received Item Name: " + receivedItem.name);
            this.client.receivedDataSize = 0;
        }
        

    }

	public void SendRequest(NetworkRequest request)
	{
		pendingRequest = request;
		this.client.SendMessageToServer(request.mRequestData);
	}

	void GetResponse(byte[] response)
	{
		pendingRequest.SetResponse(response, true);
	}
}
