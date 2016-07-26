using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AulaInfoShow : MonoBehaviour {

	public GameObject fpsController;
	public GameObject aulaInfoPanel;
	public GameObject aulaInfoText;
	public GameObject aulaTituloText;
	public GameObject antClaseButton;
	public GameObject sigClaseButton;
	public GameObject antDiaButton;
	public GameObject sigDiaButton;
	public float umbralDistancia = 5f;

	protected static int POLL_INTERVAL_FRAMES = 30;
	protected int frameCount = 0;
	protected static int today = (int) System.DateTime.Now.DayOfWeek - 1;
	protected static string sinClase = "No hay clases en el horario actual.";

	protected int day;
	protected int quarter;
	protected int idMinimoAnterior = -1;
	protected int claseUIIndex = -1;
	protected int claseAnteriorIndex = -1;
	protected int claseSiguienteIndex = -1;
	protected List<string> clasesDia = new List<string>();


	// Use this for initialization
	void Start () {
		day = today;
		quarter = AulaInfoManager.getCurrentQuarter();
		}
	
	// Update is called once per frame
	void Update () {

		frameCount++;
		if (frameCount < POLL_INTERVAL_FRAMES)
			return;
		
		frameCount=0;

		// verifico proximidad por cada aula
		Vector3 posActual = fpsController.transform.position;
		float minimo = 9999999f;
		float actual;
		int idMinimo = -1;
		foreach (int aulaID in AulasConstants.aulasPosition.Keys) {
			actual = Vector3.Distance(posActual, AulasConstants.aulasPosition[aulaID]);
			if ((actual < umbralDistancia) && (actual < minimo))
			{
				minimo = actual;
				idMinimo = aulaID;
			}
		}

		if( (idMinimo > -1) && (ButtonScript.getModoLibreState()) )
		{
			clasesDia.Clear ();

			bool hayCambioDeAula = ((idMinimoAnterior == -1) || (idMinimoAnterior != idMinimo));
			if (hayCambioDeAula)
				claseUIIndex = -1;

			// SCHEDULE DEL DIA
			int aulaID = idMinimo;
			int minutosMin; int minutosMax, minutosActual;
			bool fillBlanks = false;
			int claseActualIndex = -1;
			string claseUIString = "";

			// AULA MAS CERCANA
			//Debug.Log ("Estoy mas cerca de: " + AulaInfoManager.aulas[aulaID]);

			// PREPARO DATOS
			// TITULO (DATOS DE FECHA Y AULA, INDEPENDIENTE DE LA CLASE A MOSTRAR)
			UnityEngine.UI.Text contenedorTitulo = aulaTituloText.GetComponent<UnityEngine.UI.Text>();
			contenedorTitulo.text = (AulaInfoManager.aulas[aulaID] + " - " + AulaInfoManager.dias[day] + "\n\n"); // GUI

			// PARA CADA CLASE DEL AULA ACTUAL
			foreach (AulaTime time in AulaInfoManager.weeklySchedule[quarter][aulaID][day].Keys) {

				if (!fillBlanks && AulaInfoManager.weeklySchedule[quarter] [aulaID] [day] [time] == null)
					continue;
				clasesDia.Add(AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time] == null ? "" : AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time].ToShortString());

				// CALCULO DE CLASE ACTUAL
				minutosMin = int.Parse((AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time]).horaInicio.h)*60;
				minutosMin = minutosMin + int.Parse((AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time]).horaInicio.m);
				minutosMax = int.Parse((AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time]).horaFin.h)*60;
				minutosMax = minutosMax + int.Parse((AulaInfoManager.weeklySchedule [quarter] [aulaID] [day] [time]).horaFin.m);
				minutosActual = int.Parse(System.DateTime.Now.Hour.ToString())*60;
				minutosActual = minutosActual + int.Parse(System.DateTime.Now.Minute.ToString());
				// SI ESTOY PROCESANDO LA CLASE ACTUAL
				//Debug.Log ("MIN-MAX: " + minutosMin + "-" + minutosMax);
				//Debug.Log ("ACTUAL: " + minutosActual);
				if ((minutosActual > minutosMin) && (minutosActual < minutosMax)) 
				{
					claseActualIndex = clasesDia.Count - 1;
					if (hayCambioDeAula)
						claseUIIndex = claseActualIndex;
				} 
				// SINO, ACTUALIZO ANTERIOR O SIGUIENTE
				else 
				{
					if ((minutosMax < minutosActual))
						claseAnteriorIndex = clasesDia.Count - 1;
					if ((minutosMin > minutosActual))
						claseSiguienteIndex = clasesDia.Count - 1;
				}
			}

			// SE ESTA ENTRANDO EN UNA NUEVA AULA Y NO EXISTE CLASE ACTUAL
			if (claseUIIndex == -1) 
			{
				claseUIString = sinClase;
				//Debug.Log ("ant: " + claseAnteriorIndex);
				//Debug.Log ("sig: " + claseSiguienteIndex);
			}
			// HAY UNA CLASE PARA MOSTRAR
			else
			{
				// ACTUALIZO MENSAJE
				claseUIString = clasesDia[claseUIIndex];
				// SI ES LA ACTUAL
				if ( (claseUIIndex == claseActualIndex) && (day == today) )
					claseUIString = claseUIString + " <<< AHORA >>> ";
			}

			// ACTUALIZO ID MINIMO DE AULA ANTERIOR (EN ESTE MOMENTO LA ACTUAL)
			idMinimoAnterior = idMinimo;

			// MUESTRO DATOS
			UnityEngine.UI.Text contenedorInfo = aulaInfoText.GetComponent<UnityEngine.UI.Text>();
			if ( (claseUIIndex == claseActualIndex) && (day == today) )
				contenedorInfo.color = Color.blue;
			else
				contenedorInfo.color = Color.black;
			contenedorInfo.text = (claseUIString);
			aulaInfoPanel.SetActive(true);
		}
		else
		{
			aulaInfoPanel.SetActive(false);
			idMinimoAnterior = -1;
			claseUIIndex = -1;
			claseAnteriorIndex = -1;
			claseSiguienteIndex = -1;
		}
	}

	// CLICK ANTERIOR BUTTON
	public void clickAntClaseButton() {
		// SI ESTAMOS MOSTRANDO EL CARTEL DE SIN CLASES, VOY A LA QUE SERIA LA ANTERIOR
		if (claseUIIndex == -1) 
		{
			claseUIIndex = claseAnteriorIndex;
		} 
		else 
		{
			if (claseUIIndex > 0)
				claseUIIndex--;
		}
	}

	// CLICK SIGUIENTE BUTTON
	public void clickSigClaseButton() {
		// SI ESTAMOS MOSTRANDO EL CARTEL DE SIN CLASES, VOY A LA QUE SERIA LA SIGUIENTE
		if (claseUIIndex == -1) 
		{
			claseUIIndex = claseSiguienteIndex;
		} 
		else 
		{
			if (claseUIIndex < clasesDia.Count-1)
				claseUIIndex++;
		}	
	}


	// CLICK ANTERIOR BUTTON
	public void clickAntDiaButton() {
		if (day > 0)
		{
			day--;
			idMinimoAnterior = -1;
			claseUIIndex = -1;
			claseAnteriorIndex = -1;
			claseSiguienteIndex = -1;
		}
	}

	// CLICK SIGUIENTE BUTTON
	public void clickSigDiaButton() {
		if (day < 6) // (day < 7) 
		{
			day++;
			idMinimoAnterior = -1;
			claseUIIndex = -1;
			claseAnteriorIndex = -1;
			claseSiguienteIndex = -1;
		}
	}

}
