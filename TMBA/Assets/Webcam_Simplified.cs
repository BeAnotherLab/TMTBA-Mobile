
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;



public class Webcam_Simplified : UnityEngine.Networking.NetworkBehaviour
{

	private WebCamTexture webcamTexture;
	public RawImage rawimage;
    public Quaternion baseRotation;
    float deltaTime = 0.0f;

    //public class SyncListColorArray : SyncListStruct<Color32> { }
    //public class SyncListByteArray : SyncListStruct<Byte> { }

    //{
    //    SyncListColorArray synclistcolor = new SyncListColorArray();
    //}

    //public SyncListColorArray synclistcolor = new SyncListColorArray();
    //public SyncListByteArray synclistbyte = new SyncListByteArray();

    //[SyncVar]
    //public WebCamTexture webcamtextureClient;

    //[SyncVar]
    public byte[] data;
    [SyncVar]
    public int datasize;
    public bool isserver;
    public bool isNetworkSync = true;
    public int h;
    public int w;

    //[SyncVar]
    public WebCamTexture webcamtextureServer;
    
    public override void OnStartClient()
    {

        
        base.OnStartClient();

        //rawimage.GetComponent<Texture3D>().SetPixels32(ColorByteArrayToColor32Array(data));


        //rawimage.texture = webcamtextureServer;
        //rawimage.material.mainTexture = webcamtextureServer;
        
    }
    void Start () 
	{
        NetworkConnection Log = new NetworkConnection();
        
        isserver = gameObject.transform.parent.GetComponent<NetworkIdentity>().isServer;

        //h = (rawimage.texture).height;
        //w = (rawimage.texture).width;
        h = 100;
        w = 100;
        if (!isserver)
        {
            return;
        }
        else
        {
            baseRotation = transform.rotation;
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
            webcamtextureServer = new WebCamTexture();
            rawimage.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(Screen.width / 100, Screen.height / 100, 1.0f));
            webcamtextureServer.Stop();
            webcamtextureServer.requestedFPS = 60f;
            webcamtextureServer.Play();
            
            //baseRotation = transform.rotation;
            //Application.RequestUserAuthorization(UserAuthorization.WebCam);
            //webcamtextureServer = new WebCamTexture();
            //rawimage.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(Screen.width / 100, Screen.height / 100, 1.0f));
            //webcamtextureServer.Stop();
            //webcamtextureServer.requestedFPS = 60f;
            //rawimage.texture = webcamtextureClient;
            //rawimage.material.mainTexture = webcamtextureClient;
        }


	}

	void Update()
		{
            //transform.rotation = baseRotation * Quaternion.AngleAxis(webcamTexture.videoRotationAngle, Vector3.up);
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        //if (!gameObject.transform.parent.GetComponent<NetworkIdentity>().isServer)
        if (NetworkClient.active & !NetworkServer.active)
        {
            //data = Color32ArrayToByteArray(webcamTexture.GetPixels32());
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            if (data.Length> 0)
            {
                
                NetworkReader reader = new NetworkReader();
                OnDeserialize(reader, true);
                tex.LoadRawTextureData(data);
                tex.Apply();
            }

        }
        //if (!gameObject.transform.parent.GetComponent<NetworkIdentity>().isClient)
        if (NetworkServer.active)
        {
            Texture2D tex = new Texture2D(w, h,TextureFormat.RGB24,false);
            data = tex.GetRawTextureData();
            datasize= data.Length;
            NetworkWriter writer = new NetworkWriter();
            OnSerialize(writer, true);
            //rawimage.GetComponent<Texture3D>().SetPixels32(ColorByteArrayToColor32Array(data));
        }
    }

	void OnGUI()
	    {
        //if (webcamtextureServer == null) { return; }
        //WebCamTexture webcamtexture = ((WebCamTexture)rawimage.texture);

        //    if (webcamtexture.isPlaying)

        //    {
        //        if (GUILayout.Button("Pause"))
        //        {
        //        webcamtexture.Pause();
        //        }
        //        if (GUILayout.Button("Stop"))
        //        {
        //        webcamtexture.Stop();
        //        }
        //    }
        //    else
        //    {
        //        if (GUILayout.Button("Play"))
        //        {
        //        webcamtexture.Play();
        //        }
        //    }

        

        int w = Screen.width, h = Screen.height;
	
			GUIStyle style = new GUIStyle();
	
			Rect rect = new Rect(0, 100, w, h * 2 / 100); 
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 10 / 100;
			style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		string text2 = (1.0f / Time.smoothDeltaTime).ToString();

        text2 = GetText(); //webcamTexture.deviceName + ":" + webcamTexture.videoRotationAngle.ToString();
            GUI.Label(rect, text2, style);
	
	    }
    

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        writer.Write(data, data.Length);
        
        //writer.WriteBytesAndSize(data,data.Length);
        //return base.OnSerialize(writer, initialState);
        return isNetworkSync;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {

        //base.OnDeserialize(reader, initialState);
        data = reader.ReadBytes(datasize);
        //data = reader.ReadBytesAndSize();
        Debug.Log(data.Length.ToString());

    }

    public string GetText()
    {
        
        string text = "";
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
            text = text + Environment.NewLine + devices[i].name;
        //text = transform.parent.gameObject.transform.GetChild(0).GetComponent<NetworkView>().isMine.ToString();
        return datasize.ToString();

    }

    private static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        //SyncListByteArray[] bytes = new SyncListByteArray[colors.Length * 4];
        byte[] bytes = new byte[colors.Length * 4];
        for (int i = 0; i < bytes.Length / 4; i += 4)
        {
            
            bytes[i] = colors[i].r;
            bytes[i + 1] = colors[i].g;
            bytes[i + 2] = colors[i].b;
            bytes[i + 3] = colors[i].a;
        }
        return bytes;
    }

    private static Color32[] ColorByteArrayToColor32Array(byte[] bytes)
    {
        Color32[] colors = new Color32[bytes.Length / 4];
        for (int i = 0; i < bytes.Length / 4; i += 4)
        {
            colors[i].r = bytes[i];
            colors[i].g = bytes[i+1];
            colors[i].b = bytes[i+2];
            colors[i].a = bytes[i+3];
        }
        return colors;
    }


}
//public class Webcam_Simplified : MonoBehaviour
//{
//	
//    public MeshRenderer[] UseWebcamTexture;
//    private WebCamTexture webcamTexture;
//	public System.Diagnostics.Process process;
//	[SerializeField]
//	float fps;
//
//	float deltaTime = 0.0f;
//
//	System.IO.BinaryWriter writer = null;
//
//	void Update()
//	{
//		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
//
//	}
//
//    void Start()
//    {
//		//BackgroundTexture backtexture = gameObject.AddComponent<GUITexture>();
//		//backtexture.pixelInset = new Rect(0,0,Screen.width,Screen.height);
//        webcamTexture = new WebCamTexture();
//		int h = webcamTexture.height;
//		int w = webcamTexture.width;
//		float sh = Screen.width;
//		float sw = Screen.height;
//		Debug.Log("h: " + Screen.height.ToString() + " w: " + Screen.width.ToString());
//		//GetComponent ("Cube").transform.localPosition.x = sh/2;
//		//GetComponent ("Cube").transform.localPosition.y = sy/2;
//		//GetComponent ("Cube").transform.localScale = new Vector3 (Screen.width / 2, Screen.height / 2, 1);
//		//webcamTexture.requestedFPS = 30;
//		//GameObject.Find("Cube").transform.localScale = Vector3.Scale(transform.localScale, new Vector3(Screen.width / w, Screen.height / h, 1.0f));
//	
//
//		//GUILayout.TextField(webcamTexture.videoVerticallyMirrored,);
//		foreach(MeshRenderer r in UseWebcamTexture)
//		{
//			r.material.mainTexture = webcamTexture;
//			//Color32[] rawdata = webcamTexture.GetPixels32 ();
//			//process.StandardInput.Write(rawdata);
//		}
//		//GetComponent<Renderer> ().transform.localScale = new Vector3 (Screen.height, Screen.width, 1);
//        GetComponent<Renderer>().material.mainTexture = webcamTexture;
//
//		GetComponent<Renderer>().transform.localScale = new Vector3(1,-1,1);
//		//GetComponent<Renderer> ().transform.localScale = new Vector3 (Screen.width, Screen.height, 1);
//        webcamTexture.Play();
//   }
//
//    void OnGUI()
//    {
//		if (webcamTexture.isPlaying)
//			
//        {
//            if (GUILayout.Button("Pause"))
//            {
//                webcamTexture.Pause();
//            }
//            if (GUILayout.Button("Stop"))
//            {
//                webcamTexture.Stop();
//            }
//        }
//        else
//        {
//            if (GUILayout.Button("Play"))
//            {
//                webcamTexture.Play();
//            }
//        }
//
//		int w = Screen.width, h = Screen.height;
//
//		GUIStyle style = new GUIStyle();
//
//		Rect rect = new Rect(0, 100, w, h * 2 / 100); 
//		style.alignment = TextAnchor.UpperLeft;
//		style.fontSize = h * 2 / 100;
//		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
//		float msec = deltaTime * 1000.0f;
//		float fps = 1.0f / deltaTime;
//		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
//		string text2 = webcamTexture.height.ToString();
//		GUI.Label(rect, text2, style);
//
//    }
//
//}