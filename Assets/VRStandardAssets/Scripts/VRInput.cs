using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;

namespace VRStandardAssets.Utils
{
    // This class encapsulates all the input required for most VR games.
    // It has events that can be subscribed to by classes that need specific input.
    // This class must exist in every scene and so can be attached to the main
    // camera for ease.
    public class VRInput : MonoBehaviour
    {
        //Swipe directions
        public enum SwipeDirection
        {
            NONE,
            UP,
            DOWN,
            LEFT,
            RIGHT
        };


        public event Action<SwipeDirection> OnSwipe;                // Called every frame passing in the swipe, including if there is no swipe.
        public event Action OnClick;                                // Called when Fire1 is released and it's not a double click.
        public event Action OnDown;                                 // Called when Fire1 is pressed.
        public event Action OnUp;                                   // Called when Fire1 is released.
        public event Action OnDoubleClick;                          // Called when a double click is detected.
        public event Action OnCancel;                               // Called when Cancel is pressed.

		public MediaPlayerCtrl media;
		public GameObject CameraLeft;
		public GameObject CameraRight;
		public GameObject Head;
		public GameObject Sphere;
		public bool playing = true;

		bool clickedOnce = false;
		public string singleTapMove;
		public string doubleTapMove;
		float count = 0.0f;

		public Text textPlay;
		public Text textReceived;
		public Text textSent;
		public Text textRecentered;
		public Text textButton;
		int recentered = 0;

		public Text textFPS;
		float deltaTime = 0.0f;
		private Quaternion rotacaoOriginal = Quaternion.identity;
		private Vector3 posicaoOriginal = Vector3.zero;

		OscOut oscOut;
		OscIn oscIn;
		const string play = "/test";
		const string recenter = "/test2";
		const string path = "/test3";
		const string pause = "/test4";

        [SerializeField] private float m_DoubleClickTime = 0.2f;    //The max time allowed between double clicks
        [SerializeField] private float m_SwipeWidth = 0.995f;         //The width of a swipe

        private Vector2 m_MouseDownPosition;                        // The screen position of the mouse when Fire1 is pressed.
        private Vector2 m_MouseUpPosition;                          // The screen position of the mouse when Fire1 is released.
        private float m_LastMouseUpTime;                            // The time when Fire1 was last released.
        private float m_LastHorizontalValue;                        // The previous value of the horizontal axis used to detect keyboard swipes.
        private float m_LastVerticalValue;                          // The previous value of the vertical axis used to detect keyboard swipes.

        public float DoubleClickTime{ get { return m_DoubleClickTime; } }

		private void Start() {
			//click = GetComponent(typeof(Play)) as Play;
			//doubleclick = GetComponent (typeof(Stop)) as Stop;

			media.enabled = true;

			CameraLeft = GameObject.Find ("Main Camera Left");
			CameraRight = GameObject.Find ("Main Camera Right");


			if (PlayerPrefs.GetInt ("Assistente") == 1) {
				OnDoubleClick += VRInput_OnDoubleClick;
			} else {
				OnClick += VRInput_OnClick;
			}
			OnSwipe += HandleSwipe;

			initOSC();

			if (PlayerPrefs.GetInt("Assistente") == 1) {
				String file;
				file = PlayerPrefs.GetString ("Video");
				oscOut.Send( path, file );
				textSent.text = "Sent: " + file;
			}
		}

		private void initOSC (){
			oscOut = gameObject.AddComponent<OscOut>();
			oscIn = gameObject.AddComponent<OscIn>();

			oscOut.Open( 7000, "255.255.255.255" );
			oscIn.Open( 7000 );
			oscIn.Map( play, OnMessageReceived );
			oscIn.Map (pause, OnPauseReceived);
			oscIn.Map (path, OnPathReceived);
			oscIn.Map (recenter, OnRecenterReceived);

			if (PlayerPrefs.GetInt ("Assistente") == 1) {
				oscIn.Unmap (OnPathReceived);
				oscIn.Unmap (OnRecenterReceived);
			}

		}

		void OnMessageReceived (OscMessage message) {
			float value;
			if (message.TryGet (0, out value)) {
				textReceived.text = "Received: " + value.ToString();
				if (value == 1.0f) {
					media.Play ();
					playing = true;
				}
				if (value == 0.0f) {
					media.Stop ();
					playing = false;
				}
			}
		}

		void OnRecenterReceived (OscMessage message2) {
			Recenter ();
			recentered++;
			textRecentered.text = "Recentered: " + recentered.ToString();
		}

		void Recenter () {
			//Sphere.transform.rotation = Head.transform.rotation * new Quaternion (0.0f, -170.0f, -180.0f, 0.0f);
			Sphere.transform.rotation = Quaternion.Euler(99.0f, -Head.transform.localEulerAngles.y, -180.0f);
			rotacaoOriginal = Quaternion.Euler(99.0f, -Head.transform.localEulerAngles.y, -180.0f);
			//posicaoOriginal = Sphere.transform.position;
		}

		void OnPathReceived (OscMessage message3) {
			PlayerPrefs.DeleteKey ("Video");
			String value;
			if (message3.TryGet (0, out value)) {
				PlayerPrefs.SetString ("Video", value);
				textReceived.text = "Received: " + value;
			}
			Application.LoadLevel ("PlayerUsuario");
		}

		void OnPauseReceived (OscMessage message4) {
			float value;
			if (message4.TryGet (0, out value)) {
				textReceived.text = "Received: " + value.ToString();
				if (value == 1.0f) {
					media.Play ();
					playing = true;
					textButton.text = "Pause";
				}
				if (value == 0.0f) {
					media.Pause ();
					playing = false;
					textButton.text = "Play";
				}
			}
		}

		public void Pausar() {
			if (playing) {
				oscOut.Send( pause, 0.0f );
				media.Pause ();
				playing = false;
				textSent.text = "Sent: 0";
			}
			else if (!playing) {
				oscOut.Send( pause, 1.0f );
				media.Play ();
				playing = true;
				textSent.text = "Sent: 1";
			}
		}

		public void Calibrar() {
			Recenter ();
			recentered++;
			textRecentered.text = "Recentered: " + recentered.ToString();
			oscOut.Send( recenter, 1.0f );
		}

		void HandleSwipe(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
			case VRInput.SwipeDirection.NONE:
				break;
			case VRInput.SwipeDirection.UP:
				//Subir ();
				break;
			case VRInput.SwipeDirection.DOWN:
				//Descer ();
				break;
			case VRInput.SwipeDirection.LEFT:
				Afastar ();
				break;
			case VRInput.SwipeDirection.RIGHT:
				Aproximar ();
				break;
			}
		}

		void Afastar () {
			CameraLeft.transform.localPosition = CameraLeft.transform.localPosition + new Vector3(-5.0f, 0.0f, 0.0f);
			CameraRight.transform.localPosition = CameraRight.transform.localPosition + new Vector3(5.0f, 0.0f, 0.0f);
		}

		void Aproximar () {
			CameraLeft.transform.localPosition = CameraLeft.transform.localPosition + new Vector3(5.0f, 0.0f, 0.0f);
			CameraRight.transform.localPosition = CameraRight.transform.localPosition + new Vector3(-5.0f, 0.0f, 0.0f);
		}

		void Subir() {
			CameraLeft.transform.position = CameraLeft.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
			CameraRight.transform.position = CameraRight.transform.position + new Vector3(0.0f, -5.0f, 0.0f);
		}

		void Descer() {
			CameraLeft.transform.position = CameraLeft.transform.position + new Vector3(0.0f, -5.0f, 0.0f);
			CameraRight.transform.position = CameraRight.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
		}

		void VRInput_OnClick ()
		{
			Recenter ();
			recentered++;
			textRecentered.text = "Recentered: " + recentered.ToString();
			oscOut.Send( recenter, 1.0f );
		}

		void VRInput_OnDoubleClick ()
		{
			if (playing) {
				oscOut.Send( play, 0.0f );
				media.Stop ();
				playing = false;
				textSent.text = "Sent: 0";
			}
			else if (!playing) {
				oscOut.Send( play, 1.0f );
				media.Play ();
				playing = true;
				textSent.text = "Sent: 1";
				//Recenter ();
				//recentered++;
				textRecentered.text = "Recentered: " + recentered.ToString();
			}
		}

        private void Update()
        {
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			textFPS.text = "FPS: " + text;

            CheckInput();
			Sphere.transform.rotation = rotacaoOriginal;
			//Sphere.transform.position = posicaoOriginal;
			textPlay.text = "Playing: " + playing.ToString();

			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (PlayerPrefs.GetInt ("Assistente") == 1) {
					PlayerPrefs.DeleteAll ();
					Application.LoadLevel ("EscolhaDeVideo");
				} else {
					PlayerPrefs.DeleteAll ();
					Application.Quit ();
				}
			}
        }
			
        private void CheckInput()
        {
            // Set the default swipe to be none.
            SwipeDirection swipe = SwipeDirection.NONE;

            if (Input.GetButtonDown("Fire1"))
            {
                // When Fire1 is pressed record the position of the mouse.
                m_MouseDownPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
                // If anything has subscribed to OnDown call it.
                if (OnDown != null)
                    OnDown();
            }

            // This if statement is to gather information about the mouse when the button is up.
			if (Input.GetButtonUp ("Fire1")) {
				// When Fire1 is released record the position of the mouse.
				m_MouseUpPosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

				// Detect the direction between the mouse positions when Fire1 is pressed and released.
				swipe = DetectSwipe ();
			}

            // If there was no swipe this frame from the mouse, check for a keyboard swipe.
            if (swipe == SwipeDirection.NONE)
                swipe = DetectKeyboardEmulatedSwipe();

            // If there are any subscribers to OnSwipe call it passing in the detected swipe.
            if (OnSwipe != null)
                OnSwipe(swipe);

            // This if statement is to trigger events based on the information gathered before.

			if(Input.GetButtonUp ("Fire1") && Math.Abs(((m_MouseUpPosition - m_MouseDownPosition).normalized).x) < 0.99f && Math.Abs(((m_MouseUpPosition - m_MouseDownPosition).normalized).y) < 0.99f)
            {
				
                // If anything has subscribed to OnUp call it.
                if (OnUp != null)
                    OnUp();

				StartCoroutine (ClickEvent ());

				/*
                // If the time between the last release of Fire1 and now is less
                // than the allowed double click time then it's a double click.
				if (Time.time - m_LastMouseUpTime < m_DoubleClickTime)
                {
                    // If anything has subscribed to OnDoubleClick call it.
                    if (OnDoubleClick != null)
                        OnDoubleClick();
                }
				else
                {
                    // If it's not a double click, it's a single click.
                    // If anything has subscribed to OnClick call it.
                    if (OnClick != null)
                        OnClick();
                }

                // Record the time when Fire1 is released.
                m_LastMouseUpTime = Time.time;
				*/
            }


            // If the Cancel button is pressed and there are subscribers to OnCancel call it.
            if (Input.GetButtonDown("Cancel"))
            {
                if (OnCancel != null)
                    OnCancel();
            }
        }

		public IEnumerator ClickEvent()
		{
			if (!clickedOnce && count < m_DoubleClickTime) {
				clickedOnce = true;
			} else {
				clickedOnce = false;
				yield break;  //If the button is pressed twice, don't allow the second function call to fully execute.
			}
			yield return new WaitForEndOfFrame();

			while(count < m_DoubleClickTime)
			{
				if(!clickedOnce)
				{
					if (OnDoubleClick != null)
						OnDoubleClick();
					count = 0f;
					clickedOnce = false;
					yield break;
				}
				count += Time.deltaTime;// increment counter by change in time between frames
				yield return null; // wait for the next frame
			}
			if (OnClick != null)
				OnClick();
			count = 0f;
			clickedOnce = false;
		}

        private SwipeDirection DetectSwipe ()
        {
            // Get the direction from the mouse position when Fire1 is pressed to when it is released.
            Vector2 swipeData = (m_MouseUpPosition - m_MouseDownPosition).normalized;

            // If the direction of the swipe has a small width it is vertical.
            bool swipeIsVertical = Mathf.Abs (swipeData.x) < m_SwipeWidth;

            // If the direction of the swipe has a small height it is horizontal.
            bool swipeIsHorizontal = Mathf.Abs(swipeData.y) < m_SwipeWidth;

            // If the swipe has a positive y component and is vertical the swipe is up.
            if (swipeData.y > 0f && swipeIsVertical)
                return SwipeDirection.UP;

            // If the swipe has a negative y component and is vertical the swipe is down.
            if (swipeData.y < 0f && swipeIsVertical)
                return SwipeDirection.DOWN;

            // If the swipe has a positive x component and is horizontal the swipe is right.
            if (swipeData.x > 0f && swipeIsHorizontal)
                return SwipeDirection.RIGHT;

            // If the swipe has a negative x component and is vertical the swipe is left.
            if (swipeData.x < 0f && swipeIsHorizontal)
                return SwipeDirection.LEFT;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NONE;
        }
			
        private SwipeDirection DetectKeyboardEmulatedSwipe ()
        {
            // Store the values for Horizontal and Vertical axes.
            float horizontal = Input.GetAxis ("Horizontal");
            float vertical = Input.GetAxis ("Vertical");

            // Store whether there was horizontal or vertical input before.
            bool noHorizontalInputPreviously = Mathf.Abs (m_LastHorizontalValue) < float.Epsilon;
            bool noVerticalInputPreviously = Mathf.Abs(m_LastVerticalValue) < float.Epsilon;

            // The last horizontal values are now the current ones.
            m_LastHorizontalValue = horizontal;
            m_LastVerticalValue = vertical;

            // If there is positive vertical input now and previously there wasn't the swipe is up.
            if (vertical > 0f && noVerticalInputPreviously)
                return SwipeDirection.UP;

            // If there is negative vertical input now and previously there wasn't the swipe is down.
            if (vertical < 0f && noVerticalInputPreviously)
                return SwipeDirection.DOWN;

            // If there is positive horizontal input now and previously there wasn't the swipe is right.
            if (horizontal > 0f && noHorizontalInputPreviously)
                return SwipeDirection.RIGHT;

            // If there is negative horizontal input now and previously there wasn't the swipe is left.
            if (horizontal < 0f && noHorizontalInputPreviously)
                return SwipeDirection.LEFT;

            // If the swipe meets none of these requirements there is no swipe.
            return SwipeDirection.NONE;
        }
        
        private void OnDestroy()
        {
            // Ensure that all events are unsubscribed when this is destroyed.
            OnSwipe = null;
            OnClick = null;
            OnDoubleClick = null;
            OnDown = null;
            OnUp = null;
			PlayerPrefs.DeleteAll ();
        }
    }
}