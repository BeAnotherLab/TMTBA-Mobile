using UnityEngine;
using System.Collections;

public class LoadDalva : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void Load () {
		PlayerPrefs.SetString ("Video", "file:///storage/emulated/0/Download/edition_DALVA_1080p.mkv");
		//PlayerPrefs.SetString ("Video", "file:///storage/emulated/0/Phobia360_injected.mp4");
		//PlayerPrefs.SetString ("Video2", "edition_DALVA_1080p.mkv");
		PlayerPrefs.SetInt("Assistente", 1);
		//PlayerPrefs.SetInt ("Desktop", 1);
		Application.LoadLevel ("PlayerAssistente");
	}
}
