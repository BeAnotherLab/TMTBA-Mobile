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
		//PlayerPrefs.SetString ("VideoUsuario", "file:///storage/emulated/0/edition_DALVA_4K.mkv");
		//PlayerPrefs.SetString ("Video", "file:///storage/sdcard0/edition_DALVA_4K.mp4");
		//PlayerPrefs.SetString ("Video", "file:///storage/emulated/0/edition_DALVA_4K.mkv");

		PlayerPrefs.SetString ("Video", "Dalva");
		PlayerPrefs.SetInt("Assistente", 1);
		Application.LoadLevel ("PlayerAssistente");
	}
}
