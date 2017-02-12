using UnityEngine;
using GameSocket;
using System;
using Owlies.Core;
using Sproto;
using SprotoType;

public class APIManager : HandleBehaviour {
	public double sendFrequency = 3.0f;
	private double sendCoolDown = 0.0f;
	public bool NewClient = false;
    private SocketClient client;
	public TCPClient tClient;
	NetworkRequest pendingRequest;
	private String serverIpAddress = "127.0.0.1";
	private int port = 8888;

    byte[] dataToSend;

	void tryCreateConnection() {

		if(NewClient)
		{
			if(tClient.connectState == ConnectionState.NotConnected)
			{
				Debug.Log("connecting...");
				tClient.StartConnect();
			}
		}
		else
		{
			if (this.client == null) {
				this.client = new SocketClient();
			}
			if (!this.client.isConnected()) {
				this.client.CreateConnection(serverIpAddress, port);
			}
		}
	}

    // Use this for initialization
	void Start() {
		this.tryCreateConnection();
		AddressBook address = new AddressBook ();
		address.person = new System.Collections.Generic.List<Person> ();

		Person person = new Person ();
		person.name = "Amy";
		person.id = 10120;

		person.phone = new System.Collections.Generic.List<Person.PhoneNumber> ();
		Person.PhoneNumber num1 = new Person.PhoneNumber ();
		num1.number = "123456789";
		num1.type = 1;
		person.phone.Add (num1);

		// byte[] person_data = person.encode();

		// int totalSize = person_data.Length + 2;
		// this.dataToSend = new byte[totalSize];
		
		// char byte1 = (char)(totalSize >> 8);
		// char byte2 = (char)(totalSize);
		
		// this.dataToSend[0] = Convert.ToByte(byte1);
		// this.dataToSend[1] = Convert.ToByte(byte2);
		// System.Buffer.BlockCopy(person_data, 0, this.dataToSend, 2, person_data.Length);

		ConnectionManager.Instance.serialize(person, eMessageRequestType.ChangeEvent);
		this.dataToSend = new byte[ConnectionManager.Instance.sendBufferSize];
		System.Buffer.BlockCopy(ConnectionManager.Instance.sendBuffer, 0, this.dataToSend, 0, ConnectionManager.Instance.sendBufferSize);
	}

	//Update is called once per frame
	override protected void HandleUpdate () {
		// fake return response
		if(pendingRequest != null && (UnityEngine.Random.Range(0,1.0f) > 0.95f))
		{
			GetResponse(this.client.receivedData, this.client.receivedDataSize);
			pendingRequest = null;
		}

		if (this.sendCoolDown > 0) {
			this.sendCoolDown -= Time.deltaTime;
			return;
		}
		this.tryCreateConnection();
		if(NewClient)
		{
			tClient.SendMessageToServer(this.dataToSend, this.dataToSend.Length);
		}
        else
		{
			this.client.SendMessageToServer(this.dataToSend);
			if (this.client.receivedDataSize > 0) {
				//GetResponse(this.client.receivedData, this.client.receivedDataSize);
				Person person = (Person)ConnectionManager.Instance.deserialize(this.client.receivedData, this.client.receivedDataSize);
				Debug.Log("Received Person Name: " + person.name);
				this.client.receivedDataSize = 0;
			}
		}
		Debug.Log("Data sent!");
		this.sendCoolDown = this.sendFrequency;
    }

	public void ProcessData(byte[] data, int size)
	{
		Person person = (Person)ConnectionManager.Instance.deserialize(data, size);
		Debug.Log("Received Person Name: " + person.name);
	}

	public void SendRequest(NetworkRequest request)
	{
		pendingRequest = request;
		this.client.SendMessageToServer(request.mRequestData);
	}

	void GetResponse(byte[] response, int packageSize)
	{
		if (pendingRequest != null) {
			pendingRequest.SetResponse(response, true);
		}
	}
}
