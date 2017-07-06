using UnityEngine;
using System.Collections;

public class LoadJose : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Load () {
		//PlayerPrefs.SetString ("VideoUsuario", "file:///storage/emulated/0/FLUPP_JOSE_edition_4K.mkv");
		//PlayerPrefs.SetString ("Video", "file:///storage/sdcard0/FLUPP_JOSE_edition_4K.mp4");

		PlayerPrefs.SetString ("Video", "Jose");

		PlayerPrefs.SetInt("Assistente", 1);
		Application.LoadLevel ("PlayerAssistente");
	}
}
