using UnityEngine;
using System.Collections;

public class Communication : MonoBehaviour {

	OscOut oscOut;
	OscIn oscIn;
	public MediaPlayerCtrl media;

	const string address = "/test";
	/*
	// Use this for initialization
	void Start () {
		oscOut = gameObject.AddComponent<OscOut>();
		oscIn = gameObject.AddComponent<OscIn>();

		oscOut.Open( 7000, "255.255.255.255" );
		oscIn.Open( 7000 );

		oscIn.Map( address, OnMessageReceived );
	}
	
	// Update is called once per frame
	void Update () {
		int value = 1;
		oscOut.Send( address, value );
	}

	void OnMessageReceived(OscMessage message) {
		int value;
		if( !message.TryGet( 0, out value ) ) return;

		if (media.GetCurrentState() == media.MEDIAPLAYER_STATE.PAUSED || media.GetCurrentState() == media.MEDIAPLAYER_STATE.STOPPED)
			media.Play ();
	}
	*/
}
