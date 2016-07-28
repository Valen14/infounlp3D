using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour {

	public GameObject fpsController;
	protected int speed = 0;
	protected int speedImpulse = 10;
	protected int speedDecay = 1;
	protected int rotation = 0;
	protected int rotationImpulse = 100;
	protected int rotationDecay = 10;

	protected float axisFactor = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (speed > 0)
			speed -= speedDecay;
		if (speed < 0)
			speed += speedDecay;

		if (rotation > 0)
			rotation = rotation - rotationDecay;
		if (rotation < 0)
			rotation = rotation + rotationDecay;
			
		CrossPlatformInputManager.SetAxis("Vertical", speed / axisFactor);
		CrossPlatformInputManager.SetAxis("Rotate", rotation / axisFactor);
	}

	void OnGUI() {
		if (fpsController == null || !ButtonScript.getModoLibreState())
			return;
		
		int bSizeW = Screen.width / 5;
		int bSizeH = Screen.height / 5;

		// Forward
		if (GUI.RepeatButton (new Rect (Screen.width - 2 * bSizeW, Screen.height - 2 * bSizeH, bSizeW, bSizeH), ".·.")) {
			speed = speedImpulse;
		}
	
		// Backward
		if (GUI.RepeatButton (new Rect (Screen.width - 2 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), "·.·")) {
			speed = -speedImpulse;
		}

		// Left
		if (GUI.RepeatButton (new Rect (Screen.width - 3 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), "·:")) {
			rotation = -rotationImpulse;	
		}

		// Right
		if (GUI.RepeatButton (new Rect (Screen.width - 1 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), ":·")) {
			rotation = rotationImpulse;
		}



	}	
}
