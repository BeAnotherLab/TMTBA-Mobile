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
		PlayerPrefs.SetString ("Video", "file:///storage/emulated/0/Download/FLUPP_JOSE_edition_1080p.mkv");
		//PlayerPrefs.SetString ("Video", "file:///storage/emulated/0/Planchado360_injected.mp4");
		//PlayerPrefs.SetString ("Video2", "FLUPP_JOSE_edition_1080p.mkv");
		PlayerPrefs.SetInt("Assistente", 1);
		//PlayerPrefs.SetInt ("Desktop", 1);
		Application.LoadLevel ("PlayerAssistente");
	}
}
