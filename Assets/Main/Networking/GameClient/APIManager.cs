using UnityEngine;
using System;
using Owlies.Core;
using Sproto;
using SprotoType;

public class APIManager : HandleBehaviour {
	public double sendFrequency = 10.0f;
	private double sendCoolDown = 0.0f;
	private String serverIpAddress = "192.168.1.96";
	private int port = 8888;
    private int session = 0;

    Person getStubPerson() {
        AddressBook address = new AddressBook();
        address.person = new System.Collections.Generic.List<Person>();

        Person person = new Person();
        person.name = "Amy";
        person.id = 10120;

        person.phone = new System.Collections.Generic.List<Person.PhoneNumber>();
        Person.PhoneNumber num1 = new Person.PhoneNumber();
        num1.number = "123456789";
        num1.type = 1;
        person.phone.Add(num1);

        return person;
    }

    // Use this for initialization
	void Start() {

        NetworkManager.Instance.Connect(serverIpAddress, 8888);
        NetworkManager.Instance.StartNetThread();
        

        // byte[] person_data = person.encode();

        // int totalSize = person_data.Length + 2;
        // this.dataToSend = new byte[totalSize];

        // char byte1 = (char)(totalSize >> 8);
        // char byte2 = (char)(totalSize);

        // this.dataToSend[0] = Convert.ToByte(byte1);
        // this.dataToSend[1] = Convert.ToByte(byte2);
        // System.Buffer.BlockCopy(person_data, 0, this.dataToSend, 2, person_data.Length);

        //ConnectionManager.Instance.serialize(person, eMessageRequestType.ChangeEvent);
        //this.dataToSend = new byte[ConnectionManager.Instance.sendBufferSize];
        //System.Buffer.BlockCopy(ConnectionManager.Instance.sendBuffer, 0, this.dataToSend, 0, ConnectionManager.Instance.sendBufferSize);
        //this.dataToSend[ConnectionManager.Instance.sendBufferSize] = (byte)('\n');

        NetworkManager.Instance.Send(session++, getStubPerson(), eMessageRequestType.ChangeEvent);

    }

	//Update is called once per frame
	override protected void HandleUpdate () {
        // fake return response
        NetworkManager.Instance.Send(session++, getStubPerson(), eMessageRequestType.ChangeEvent);
    }

	public void ProcessData(byte[] data, int size)
	{
		Person person = (Person)ConnectionManager.Instance.deserialize(data, size);
		Debug.Log("Received Person Name: " + person.name);
	}
}
