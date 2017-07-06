using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoadStart : MonoBehaviour {
	public InputField video;
	public Button start;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (video.text == "") {
			start.interactable = false;
		} else {
			start.interactable = true;
		}
	}

	public void Clicked() {
		PlayerPrefs.SetString ("Video", video.text);
		PlayerPrefs.SetInt("Manager", 1);
		Application.LoadLevel ("ManagerPlayer");
	}
}
