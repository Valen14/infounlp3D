using UnityEngine;
using System.Collections;

public class QualityControlGUI : MonoBehaviour {

	public Camera animCamera;
	public Camera roamCamera;

	protected static float MAX_FAR_CLIPPING = 200f;
	protected static float MIN_FAR_CLIPPING = 10f;

	protected float farClipping = MAX_FAR_CLIPPING;

	// Use this for initialization
	void Start () {
		/*
		#if MOBILE_INPUT	
		farClipping = 60;
		#endif
		*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Label (new Rect (10, Screen.height - 30, 50, 25), "Dist:");
		farClipping = GUI.HorizontalSlider (new Rect (50, Screen.height - 25, 120, 25), farClipping, MIN_FAR_CLIPPING, MAX_FAR_CLIPPING);
		animCamera.farClipPlane = farClipping;
		roamCamera.farClipPlane = farClipping;
	}
}
