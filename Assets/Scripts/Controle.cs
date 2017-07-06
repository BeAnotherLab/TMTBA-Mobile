using UnityEngine;
using System.Collections;

public class Controle : MonoBehaviour {
	public GameObject BJose;
	public GameObject BDalva;

	// Use this for initialization
	void Start () {
		/*
		float w = Screen.width, h = Screen.height;
		//BJose.transform.localScale = new Vector3 (w/4000, h/500, 0.0f);
		BJose.transform.localScale = new Vector3 (w/4000, h/1000, 0.0f);
		//BJose.transform.position += new Vector3 (290.0f, -300.0f, 0.0f);
		BJose.transform.position = new Vector3 (w/4, h/1.25f, 0.0f);

		//BDalva.transform.localScale = new Vector3 (w/4000, h/500, 0.0f);
		BDalva.transform.localScale = new Vector3 (w/4000, h/1000, 0.0f);
		//BDalva.transform.position += new Vector3 (-290.0f, -300.0f, 0.0f);
		BDalva.transform.position = new Vector3 (w/1.3f, h/1.25f, 0.0f);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
