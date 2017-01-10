using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class evento : MonoBehaviour {

	public GameObject uiWrapper;
	public Text sendLabel;
	public Text receiveLabel;

	OscOut oscOut;
	OscIn oscIn;

	const string address = "/test";


	void Start()
	{
		// Create objects for sending and receiving
		oscOut = gameObject.AddComponent<OscOut>();
		oscIn = gameObject.AddComponent<OscIn>(); 

		// Prepare for broadcasting messages to devices on the local network to be received by applications listening on port 7000.
		oscOut.Open( 7000, "255.255.255.255" );

		// For the hardcode haters, you can get the broadcast address like this System.Net.IPAddress.Broadcast.ToString().

		// Prepare for receiving unicasted and broadcasted messages from this and other devices on port 7000.
		oscIn.Open( 7000 );

		// Forward recived messages with address to method
		oscIn.Map( address, OnMessageReceived );

		// Show UI
		//uiWrapper.SetActive( true );
	}

	void Update()
	{
		// Send a random value
		/*
			float value = Random.value;
			oscOut.Send( address, value );

			// Update label
			if( oscOut.isOpen ) sendLabel.text = value.ToString();
			*/
	}

	void OnMessageReceived( OscMessage message )
	{
		// Get the value
		float value;
		if( !message.TryGet( 0, out value ) ) return;

		// Update label
		receiveLabel.text = value.ToString();
	}

	public void botao () {
		uiWrapper.SetActive( true );
		float value = Random.value;
		oscOut.Send( address, value );
		if( oscOut.isOpen ) sendLabel.text = value.ToString();
	}
}
