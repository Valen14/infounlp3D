using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public GameObject UIPanel;
    public GameObject MenuButton;
    public GameObject animCamera;
    public GameObject freeCamera;
	public GameObject fpsController;
    public GameObject dualJoystick;
	protected static bool modoLibre;
		
	void Start() {
		fpsController.GetComponent<CharacterController>().enabled = false;
		modoLibre = false;
	}

	// ESTA EN MODO LIBRE?
	public static bool getModoLibreState() {
		return modoLibre;
	}

	/*
    bool animPlaying = false;
	float seconds = 0;
	float animLength = 0;
    
	void Update() {

		seconds = seconds + Time.deltaTime;

		// Si finalizo una animacion, entonces "elevar" un poco el personaje (las animaciones lo dejan por debajo del piso y colisionan)
		if (animPlaying && seconds >= animLength) {
			Debug.Log("Fin Animacion");
			animPlaying = false;
            // Detener la animacion, elevar pos Y y reactivar CharacterController
            animCamera.transform.GetComponent<Animation> ().Stop();
            //animCamera.transform.position = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + 1, targetObject.transform.position.z);
            //animCamera.transform.GetComponent<CharacterController> ().enabled = true;
        }
    }
    */


    // FREE MODE
    public void freeMode()
    {
        animCamera.GetComponent<Camera>().enabled = false;
        freeCamera.GetComponent<Camera>().enabled = true;
		fpsController.GetComponent<CharacterController>().enabled = true;
        dualJoystick.GetComponent<Canvas>().enabled = true;
        // escondo panel de botones
        UIPanel.SetActive(false);
        MenuButton.SetActive(true);
		modoLibre = true;
    }

    // EXIT APP
    public void exit() {
        Application.Quit();
    }

    // SHOW MENU
    public void clickMenu() {
        MenuButton.SetActive(false);
		dualJoystick.GetComponent<Canvas>().enabled = false;
		UIPanel.SetActive(true);
		modoLibre = false;
    }

    // SHOW INSCRIPCIONES ANIM
    public void goInscripciones() {
		playAnim ("Inscripciones");
    }

    // SHOW ALUMNOS ANIM
    public void goAlumnos() {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("Alumnos"));
        //playAnim ("Alumnos");
    }

    // SHOW BUFFET ANIM
    public void goBuffet()
    {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("Buffet"));
        //playAnim ("Buffet");
    }

	// SHOW BUFFET ANIM
	public void goAereo()
	{
		playAnim("Inscripciones");
		StartCoroutine(esperarSeg("Aereo"));
		//playAnim ("Aereo");
	}
	
	// SHOW PB-BAÑOS-F ANIM
	public void goBanosPBF()
    {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("BanosPBF"));
        //playAnim ("BanosPBF");
    }

    // SHOW PB-BAÑOS-M ANIM
    public void goBanosPBM()
    {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("BanosPBM"));
        //playAnim ("BanosPBM");
    }

    // SHOW P1-BAÑOS-F ANIM
    public void goBanosP1F()
    {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("BanosP1F"));
        //playAnim ("BanosP1F");
    }

    // SHOW P1-BAÑOS-M ANIM
    public void goBanosP1M()
    {
        playAnim("Inscripciones");
        StartCoroutine(esperarSeg("BanosP1M"));
        //playAnim ("BanosP1M");
    }


	public void playAnim(string anim) {

        animCamera.GetComponent<Camera>().enabled = true;
        freeCamera.GetComponent<Camera>().enabled = false;
		fpsController.GetComponent<CharacterController>().enabled = false;
        dualJoystick.GetComponent<Canvas>().enabled = false;

        Debug.Log("Ejecutando animacion: " + anim);
        // Si hay una animacion, omitir movimientos de caracter y deteccion de colisiones
        //animCamera.transform.GetComponent<CharacterController> ().enabled = false;
        UIPanel.SetActive(false);
        // Frenamos cualquier tipo de animacion activa.  Retrocedemos y ejecutamos la animacion recibida como argumento        
        animCamera.transform.GetComponent<Animation>().Stop();
        animCamera.transform.GetComponent<Animation>().Rewind();
        animCamera.transform.GetComponent<Animation>().Play(anim);
        // Tomamos la longitud de la animacion a reproducir, a fin de saber cuando finaliza la misma
        /*
        animLength = animCamera.transform.GetComponent<Animation> ().GetClip (anim).length;
		seconds = 0;
		animPlaying = true;
        */

		MenuButton.SetActive(true);
	}

    IEnumerator esperarSeg(string anim)
    {
        yield return new WaitForSeconds(0.01f);
        playAnim (anim);
    }

}
