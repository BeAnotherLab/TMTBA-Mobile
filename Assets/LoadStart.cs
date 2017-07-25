using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class LoadStart : MonoBehaviour {
	public Button start;
	public Dropdown videoDropDown;

	// Use this for initialization
	void Start () {
		Vector3 pos = start.transform.position;
		pos.x = (Screen.width / 2);
		pos.y = (Screen.height * 1/11);
		start.transform.position = pos;
		Vector2 size = new Vector2(Screen.width/5.5f, Screen.height/20);
		start.image.rectTransform.sizeDelta = size;

		pos = videoDropDown.transform.position;
		pos.x = (Screen.width / 2);
		pos.y = (Screen.height * 1/1.2f);
		videoDropDown.transform.position = pos;
		videoDropDown.image.rectTransform.sizeDelta = size;
			
	
		try {
			DirectoryInfo dir = new DirectoryInfo(GetAndroidPath());
			List<FileInfo> infoList = new List<FileInfo>();

			FileInfo[] info = dir.GetFiles("*.mp4");
			infoList.AddRange(info);
			info = dir.GetFiles("*.mkv");
			infoList.AddRange(info);

			infoList.ToArray ();

			foreach (FileInfo f in infoList) {
				videoDropDown.options.Add (new Dropdown.OptionData {text=f.ToString()});
			}

			videoDropDown.captionText.text = "Select a video: " + videoDropDown.options [videoDropDown.value].text;
		}
		catch (ArgumentException e) {
			videoDropDown.captionText.text = "No videos were found at " + GetAndroidPath() + ".";
		}
	}

	public string GetAndroidPath() {
		string path = "";

		try {
			AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
			path = jc.CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getAbsolutePath");
			PlayerPrefs.SetString ("Path", path);
			return path;
		}
		catch (Exception e) {
			videoDropDown.captionText.text = "Couldn't get path to Android's directory.";
			return null;
		}
	}

	// Update is called once per frame
	void Update () {
		int index = videoDropDown.value;
		List<Dropdown.OptionData> options = videoDropDown.options;
	
		if (options[index].text == "Select a video" || videoDropDown.captionText.text.Equals( "No videos were found at " + GetAndroidPath() + ".") || options[index].text == "Couldn't get path to Android's directory.") {
			start.interactable = false;
			start.enabled = false;
		} else {
			start.interactable = true;
			start.enabled = true;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void Clicked() {
		int index = videoDropDown.value;
		List<Dropdown.OptionData> options = videoDropDown.options;

		PlayerPrefs.SetString ("Video", options[index].text);
		PlayerPrefs.SetInt("Manager", 1);
		Application.LoadLevel ("ManagerPlayer");
	}
}