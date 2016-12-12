
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
public class Webcam : MonoBehaviour
{
	//public MediaPlayerCtrl m_srcVideo;
    public MeshRenderer[] UseWebcamTexture;
    private WebCamTexture webcamTexture;
	public System.Diagnostics.Process process;
	[SerializeField]
	float fps;

	float deltaTime = 0.0f;

	System.IO.BinaryWriter writer = null;

	void Update()
	{
        
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		//Renderer renderer = GetComponent<Renderer> ();
		//renderer.transform.localScale = new Vector3 (1, -1, 1);
		//WebCamTexture background = (WebCamTexture) renderer.material.mainTexture;
		//background.GetPixels32 ();
		Color32[] rawdata = webcamTexture.GetPixels32 (); //webcamTexture.GetPixels32 ();
		byte[] mydata = new byte[rawdata.Length * 4];

		for (int i = 0; i < rawdata.Length; i++) {
			mydata [i * 4 + 0] = rawdata [i].b;
			mydata [i * 4 + 1] = rawdata [i].g;
			mydata [i * 4 + 2] = rawdata [i].r;
			mydata [i * 4 + 3] = 255; //rawdata [i].a;
		}
		writer.Write (mydata);
		//process.StandardInput.Write (rawdata);
		//process.StandardInput.Close ();
		//process.StandardInput.Flush();
		//Debug.Log("h= " + webcamTexture.height.ToString() + " w= " + webcamTexture.width.ToString());
		//process.StandardInput.BaseStream.Write(rawdata,0,rawdata.Length*4);
	}

    void Start()
    {
        webcamTexture = new WebCamTexture();
		int h = webcamTexture.height;
		int w = webcamTexture.width;
		var startinfo = new System.Diagnostics.ProcessStartInfo ();
		//startinfo.FileName = "C:/Users/leiter/ADM/PPGEE/TMBA/ffmpeg-20161027-bf14393-win64-static/bin/ffmpeg.exe";
		//startinfo.Arguments = "-f rawvideo -pix_fmt bgra -s 1366x768 -i - -threads 0 -presetfast -y -f mpegts udp://localhost:1234";
		//startinfo.FileName = "C:\\Users\\leiter\\ADM\\PPGEE\\TMBA\\ffmpegbin\\ffmpeg.exe";
		//startinfo.Arguments = "-f rawvideo -pix_fmt bgra -s 1366x768 -i pipe:0 -threads 0 -presetfast -y -f mpegts udp://localhost:1234";
		//startinfo.RedirectStandardInput = true;
		//startinfo.UseShellExecute = false;
		//startinfo.CreateNoWindow = true;
		process = new System.Diagnostics.Process ();
		process.StartInfo.FileName = "C:\\Users\\leiter\\ADM\\PPGEE\\TMBA\\ffmpegbin\\ffmpeg.exe";
		//BK working line: process.StartInfo.Arguments = "-f rawvideo -pix_fmt bgra -s " + "640" + "x" + "480" + " -i pipe:0 -threads 0 -preset fast -y -f mpegts udp://localhost:1234 -f sdl" ;
		//process.StartInfo.Arguments = "-re -f rawvideo  -pix_fmt bgra -s  " + "640" + "x" + "480" + " -i pipe:0 -threads 0 -framerate 60 -vcodec h264 -bufsize 1 -tune zerolatency -preset fast -y -f mpegts udp://localhost:1234";
		process.StartInfo.Arguments = "-re -f rawvideo  -pix_fmt bgra -s  " + 640 + "x" + 480 + " -i  pipe:0 -threads 0  -vcodec h264 -bufsize 0.01 -tune zerolatency  -preset fast -y -f mpegts udp://localhost:1234";
		
		//process.StartInfo.Arguments = "-re -f rawvideo  -pix_fmt bgra -s  " + 640 + "x" + 480 + " -i  pipe:0 -threads 0  -vcodec h264 -bufsize 0.01  -preset fast -y -f mpegts udp://localhost:1234";

		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.RedirectStandardError = false;

		process.Start ();

		//string error = process.StandardError.ReadLine ();
		//while (error != null) {
		//	Debug.Log (error);
		//	error = process.StandardError.ReadLine ();
		//}

		writer = new System.IO.BinaryWriter (process.StandardInput.BaseStream);
		

		//webcamTexture.requestedFPS = 30;
	
		//GUILayout.TextField(webcamTexture.videoVerticallyMirrored,);
		foreach(MeshRenderer r in UseWebcamTexture)
		{
			r.material.mainTexture = webcamTexture;
			//Color32[] rawdata = webcamTexture.GetPixels32 ();
			//process.StandardInput.Write(rawdata);
		}

        GetComponent<Renderer>().material.mainTexture = webcamTexture;

		GetComponent<Renderer>().transform.localScale = new Vector3(1,-1,1);
        webcamTexture.Play();
   }

    void OnGUI()
    {
		if (webcamTexture.isPlaying)
			
        {
            if (GUILayout.Button("Pause"))
            {
                webcamTexture.Pause();
            }
            if (GUILayout.Button("Stop"))
            {
                webcamTexture.Stop();
            }
        }
        else
        {
            if (GUILayout.Button("Play"))
            {
                webcamTexture.Play();
            }
        }

		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 100, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		string text2 = webcamTexture.height.ToString();
		GUI.Label(rect, text2, style);

    }
	
//	    public void GetFFMPEG(string args) 
//		{
//            if (Application.platform == RuntimePlatform.WindowsEditor)
//            {
//
//            }
//            else if (Application.platform == RuntimePlatform.Android)
//            {
//                AndroidJavaObject jo = new AndroidJavaObject("com.github.hiteshsondhi88.sampleffmpeg");
//            
//                jo.Call(args);
//                return jo;
//            }
//        }
//    

}