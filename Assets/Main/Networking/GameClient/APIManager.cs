using UnityEngine;
using GameSocket;
using System;
using Owlies.Core;
using Sproto;
using SprotoType;

public class APIManager : HandleBehaviour {
	public double sendFrequency = 3.0f;
	private double sendCoolDown = 0.0f;
    private SocketClient client;
	NetworkRequest pendingRequest;
	private String serverIpAddress = "127.0.0.1";
	private int port = 8888;

    byte[] dataToSend;

	void tryCreateConnection() {
		if (this.client == null) {
			this.client = new SocketClient();
		}
		if (!this.client.isConnected()) {
			this.client.CreateConnection(serverIpAddress, port);
		}
	}

    // Use this for initialization
	void Start() {
		this.tryCreateConnection();
		AddressBook address = new AddressBook ();
		address.person = new System.Collections.Generic.List<Person> ();

		Person person = new Person ();
		person.name = "Alice";
		person.id = 10000;

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
        Debug.Log("Data sent!");
        this.client.SendMessageToServer(this.dataToSend);
		this.sendCoolDown = this.sendFrequency;

        if (this.client.receivedDataSize > 0) {
			//GetResponse(this.client.receivedData, this.client.receivedDataSize);
			Person person = (Person)ConnectionManager.Instance.deserialize(this.client.receivedData, this.client.receivedDataSize);
            Debug.Log("Received Person Name: " + person.name);
            this.client.receivedDataSize = 0;
        }
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
