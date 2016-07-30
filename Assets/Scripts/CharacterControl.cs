using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour {

	public GameObject fpsController;
	protected int speed = 0;
	protected int speedImpulse = 2;
	protected int speedDecay = 1;
	protected int rotation = 0;
	protected int rotationImpulse = 25;
	protected int rotationDecay = 5;
	protected int pan = 0;
	protected int panImpulse = 25;
	protected int panDecay = 5;
	protected float axisFactor = 10.0f;

	GUIStyle style;

	// Use this for initialization
	void Start () {
		style = new GUIStyle ("button");
		style.fontSize = 40;
	}
	
	// Update is called once per frame
	void Update () {
#if MOBILE_INPUT			
		if (speed > 0)
			speed -= speedDecay;
		if (speed < 0)
			speed += speedDecay;

		if (rotation > 0)
			rotation -= rotationDecay;
		if (rotation < 0)
			rotation += rotationDecay;
			
		if (pan > 0)
			pan -= panDecay;
		if (pan < 0)
			pan += panDecay;
		
		CrossPlatformInputManager.SetAxis("Vertical", speed / axisFactor);
		CrossPlatformInputManager.SetAxis("Rotate", rotation / axisFactor);
		CrossPlatformInputManager.SetAxis("Pan", pan / axisFactor);
#endif
	}

	void OnGUI() {
#if MOBILE_INPUT		
		if (fpsController == null || !ButtonScript.getModoLibreState())
			return;
		
		int bSizeW = Screen.width / 5;
		int bSizeH = Screen.height / 5;

		// Forward
		if (GUI.RepeatButton (new Rect (Screen.width - 2 * bSizeW, Screen.height - 2 * bSizeH, bSizeW, bSizeH), "\u21D1", style)) {
			speed = speedImpulse;
		}
	
		// Backward
		if (GUI.RepeatButton (new Rect (Screen.width - 2 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), "\u21D3", style)) {
			speed = -speedImpulse;
		}

		// Left
		if (GUI.RepeatButton (new Rect (Screen.width - 3 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), "\u21B6", style)) {
			rotation = -rotationImpulse;	
		}

		// Right
		if (GUI.RepeatButton (new Rect (Screen.width - 1 * bSizeW, Screen.height - 1 * bSizeH, bSizeW, bSizeH), "\u21B7", style)) {
			rotation = rotationImpulse;
		}

		// Pan Up
		if (GUI.RepeatButton (new Rect (Screen.width - 1 * bSizeW, Screen.height - 2 * bSizeH, bSizeW, bSizeH), "\u2197", style)) {
			pan = -panImpulse;
		}

		// Pan Down
		if (GUI.RepeatButton (new Rect (Screen.width - 3 * bSizeW, Screen.height - 2 * bSizeH, bSizeW, bSizeH), "\u2199", style)) {
			pan = panImpulse;
		}
#endif
	}	
}
