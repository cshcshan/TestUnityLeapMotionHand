using UnityEngine;
using System.Collections;
using Leap;

public class LeapMotionTest : MonoBehaviour {
	public delegate void TriggerDelegate(Collider otherObject);

	public GameObject mouse;
	public GameObject leftMouse;
	public GameObject rightMouse;
	
	Controller leapController;

	void Awake() {
		Mouse m = mouse.GetComponent<Mouse> ();
		if (m == null) {
			m = mouse.AddComponent<Mouse>();
			m.TriggerEvent += TriggerObject;
		}
	}

	// Use this for initialization
	void Start () {
		if (leapController == null) {
			Debug.Log("START");

			leapController = new Controller();
			StartCoroutine(DetectHand());
		} else {
			Debug.Log("無法偵測到Leap Motion裝置");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void TriggerObject(Collider other) {
		Debug.Log ("TriggerObject: " + other.name);
	}

	IEnumerator DetectHand() {
		// THUMB、INDEX、MIDDLE、RING、PINKY
		// Leap Motion Interaction Box: {width: 235mm, height: 235mm(start: 82.5mm); depth: 147mm}
		// Unity Position Range: {x: -8 ~ 8, y: -5 ~ 5}

		int LEAP_WIDTH = 235;
		int LEAP_HEIGHT = 235;
		int LEAP_DEPTH = 147;

		int CAMERA_WIDTH = 16;
		int CAMERA_HEIGHT = 10;

		float MAXX = (float)(CAMERA_WIDTH - leftMouse.transform.localScale.x) * 0.5f;
		float MAXY = (float)(CAMERA_HEIGHT - leftMouse.transform.localScale.y) * 0.5f;

		Frame frame;
		Vector3 fingerPosition;

		while (true) {
			if (leapController != null) {
				frame = leapController.Frame ();
				foreach (Hand hand in frame.Hands) {
					foreach (Finger finger in hand.Fingers) {
						switch (finger.Type) {
							case Finger.FingerType.TYPE_INDEX:
							fingerPosition = new Vector3((float)(finger.TipPosition.x / (LEAP_WIDTH * 0.5f)) * CAMERA_WIDTH, 
							                             (float)((finger.TipPosition.y - 82.5f) / LEAP_HEIGHT) * CAMERA_HEIGHT - CAMERA_HEIGHT * 0.5f, 
							                             (float)(finger.TipPosition.z / (LEAP_DEPTH * 0.5f)) * 10.0f);
							if (fingerPosition.x > MAXX) fingerPosition.x = MAXX;
							if (fingerPosition.x < -MAXX) fingerPosition.x = -MAXX;
							if (fingerPosition.y > MAXY) fingerPosition.y = MAXY;
							if (fingerPosition.y < -MAXY) fingerPosition.y = -MAXY;
							fingerPosition.z = 0;
							if (hand.IsLeft) {
								leftMouse.transform.position = fingerPosition;
							} else if (hand.IsRight) {
								//rightMouse.transform.localPosition = fingerPosition;
							}
							mouse.transform.position = fingerPosition;
							break;
						default:
							break;
						}
					}
				}
			}	
			yield return new WaitForSeconds(0.2f);
		}
	}
}
