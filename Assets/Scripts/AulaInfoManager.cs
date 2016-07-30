using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class AulaInfoManager : MonoBehaviour {

	/** Direccion base donde recuperar el servicio */
	public static string BASE_URL = "http://gestiondocente.info.unlp.edu.ar/reservas/api/consulta/xaula/0";

	/** Horario de inicio de las actividades */
	public static int COURSES_STARTING_HOUR = 8;

	/** Horario de finalizacion de las actividades */
	public static int COURSES_ENDING_HOUR = 22;

	/** Incremento por minutos */
	public static int STEP_MINUTES = 30;

	/** Info de Cuatrimestres */
	public static int QUARTER_HALF = 6;
	public static int QUARTER_FIRST = 2;
	public static int QUARTER_SECOND = 3;

	/* Cuatrimestres a solicitar */
	public static int[] QUARTERS = { 2, 3 };

	/* Dias de las semana a solicitar */
	public static int[] DAYS_OF_WEEK = { 0, 1, 2, 3, 4, 5, 6 };

	/** Dias de la semana */
	public static Dictionary<int, string> dias = new Dictionary<int, string>();

	/** Lista de aulas habilitadas (aulaID -> Nombre) */
	public static Dictionary<int, string> aulas = new Dictionary<int, string>();

	/** Nomina de aulas con sus schedules (quarter -> aulaID -> Schedule)  */
	public static Dictionary<int, Dictionary<int, AulaSchedule>> scheduleByAula = new Dictionary<int, Dictionary<int, AulaSchedule>>();

	/** Agenda semanal por aula.  ( quarter -> [aulaID -> [DiaSemana -> [Horario, Datos]]]] )  */
	public static Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<AulaTime, AulaData>>>> weeklySchedule = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<AulaTime, AulaData>>>>();

	/** Numero de busquedas a realizar sobre el sistema de aulas.  Al llegar a cero, significa que toda la información ya fue leida */
	protected int pendingTasks = QUARTERS.Length * aulas.Count;

	/** Se completó la actividad de busqueda y parseo de los datos? */
	public bool fetchComplete = false;

	/** Status actual de pre-carga */
	public string currentStatus = null;



	public void Start() {
		currentStatus = "Cargando información de las aulas...";
		loadAllSchedules ();
	}

	void OnGUI() {
		if (currentStatus != null)
			GUI.Label(new Rect(10, 10, Screen.width - 10, 22), currentStatus);
	}

	static AulaInfoManager() {
		// NOMINA DE AULAS.  SEGUN DEFINICION DEL SISTEMA DE AULAS
		aulas.Add (AulasConstants.ID_AULA_1, "AULA 1 - PL/1");
		aulas.Add (AulasConstants.ID_AULA_2, "AULA 2 - APL");
		aulas.Add (AulasConstants.ID_AULA_3, "AULA 3 - COBOL");
		aulas.Add (AulasConstants.ID_AULA_4, "AULA 4 - LISP");
		aulas.Add (AulasConstants.ID_AULA_5, "AULA 5 - FORTRAN");
		aulas.Add (AulasConstants.ID_AULA_1_2, "AULA 1-2 - LOGO");
		aulas.Add (AulasConstants.ID_AULA_1_3, "AULA 1-3 - BASIC");
		aulas.Add (AulasConstants.ID_AULA_1_4, "AULA 1-4 - ALGOL");
		aulas.Add (AulasConstants.ID_AULA_9, "AULA 9");
		aulas.Add (AulasConstants.ID_AULA_6, "AULA 6");
		aulas.Add (AulasConstants.ID_AULA_7, "AULA 7");
		aulas.Add (AulasConstants.ID_AULA_10A, "AULA 10 A");
		aulas.Add (AulasConstants.ID_AULA_10B, "AULA 10 B");
		aulas.Add (AulasConstants.ID_AULA_11, "AULA 11");

		// Dias de la semana
		dias.Add (0, "Lunes");
		dias.Add (1, "Martes");
		dias.Add (2, "Miércoles");
		dias.Add (3, "Jueves");
		dias.Add (4, "Viernes");
		dias.Add (5, "Sábado");
		dias.Add (6, "Domingo");

		foreach (int q in QUARTERS) {
			// Inicializar schedule por aula por cada cuatrimestre
			scheduleByAula.Add (q, new Dictionary<int, AulaSchedule> ());
			// Inicializar agenda semanal por aula por cada cuatrimestre
			weeklySchedule.Add(q, new Dictionary<int, Dictionary<int, Dictionary<AulaTime, AulaData>>>());

		}

		
	}

	/** Accede a la informacion de aulas a fin de cargar las estructuras */
	public void loadAllSchedules() {
		Debug.Log ("Cargando información de las aulas...");
		// Precargar todos los schedules del primer y segundo cuatrimestre
		foreach (int q in QUARTERS) {
			foreach (int aulaID in aulas.Keys) {
				StartCoroutine (addAulaShedule (aulaID, q));
			}
		}
		StartCoroutine (loadFinish());
	}

	/** Actividades posteriores a la carga */
	public IEnumerator loadFinish() {
		do {
			yield return new WaitForSeconds (1);
		} while (pendingTasks > 0);
		Debug.Log ("Finalizado!");
		fetchComplete = true;
		currentStatus = null;
		printCompleteSchedule();
	}


	/** Dada una URL recuperar la agenda de clases */
	public IEnumerator addAulaShedule(int aulaID, int quarter) {
		// Recuperar info
		string url = BASE_URL + "/" + aulaID + "/" + quarter;
		WWW www = new WWW (url);
		yield return www;

		try {
			// Parse de info recuperada
			AulaSchedule aSchedule = JsonUtility.FromJson<AulaSchedule> ("{ \"values\": " + www.text + "}");
			aSchedule.aulaID = aulaID;
			aSchedule.aulaName = aulas [aulaID];
			aSchedule.quarter = quarter;

			// Guardar en estructura general de schedules
	//		Debug.Log(aSchedule.ToString());
			scheduleByAula[quarter].Add (aulaID, aSchedule);
			// Guardar en agenda semanal de actividades
			fillWeeklySchedule (aulaID, quarter, false);	
			pendingTasks--;
		} 
		catch (Exception e) {
			currentStatus = "Sin conexión a sistema de gestión de aulas. Modo offline unicamente.";
			throw e;
		}
	}


	/** Reorganiza la informacion en forma de agenda semanala */
	protected static void fillWeeklySchedule (int aulaID, int quarter, bool allTimesOfDays) {

		// Obtener las lineas de la tabla (8:00, 8:30, 9:00, 9:30... etc);
		List<AulaTime> aulaTimes = getTimesOfDay ();

		// Iterar por los dias de la semana de Lun a Sab
		Dictionary<int, Dictionary<AulaTime, AulaData>> days = new Dictionary<int, Dictionary<AulaTime, AulaData>> ();
		foreach (int day in DAYS_OF_WEEK) {
			// Por cada horario crear la celda y agregarlo al día
			Dictionary<AulaTime, AulaData> aCell = new Dictionary<AulaTime, AulaData> ();
			// Si se detecto el inicio de una clase, entonces "rellenar" o repetir celdas hasta la finalizacion de la misma
			int repeatCells = 0;
			AulaData currentAulaData = null;
			foreach (AulaTime aTime in aulaTimes) {
//				Debug.Log ("day: " + dias[day] + " - hhmm:" + aTime.h + ":" + aTime.m);	

				// Qudan celdas a rellenar?
				if (repeatCells > 0) {
					repeatCells--;
					aCell.Add (aTime, currentAulaData);
					continue;
				}
					
				// Si ya no quedan celdas a rellenar, entonces buscar la clase (puede haber o no clases para el horario actual)
				AulaData aulaData = findClase(aulaID, quarter, day, aTime.h, aTime.m);
				if (aulaData != null) {
					repeatCells = (allTimesOfDays ? getRepeatCellCount (aulaTimes, aulaData) : 0);
					currentAulaData = aulaData;
				}
				aCell.Add (aTime, aulaData);
			}
			days.Add (day, aCell);
		}
		// Incorporar la actividad del dia a la grilla
		weeklySchedule[quarter].Add (aulaID, days);
	}

	/** Busca la actividad para el aula, cuatrimestre, dia y hora indicados */
	public static AulaData findClase(int aulaID, int quarter, int day, string h, string m) {
		AulaData retValue = null;
//		Debug.Log (aulaID + " " + quarter + " " + day + " " + h + " " + m);
		if (scheduleByAula[quarter] == null || scheduleByAula[quarter] [aulaID] == null || scheduleByAula[quarter] [aulaID].values == null)
			return retValue;
		foreach (AulaData aData in scheduleByAula[quarter][aulaID].values) {
			if (aData.dia == day && aData.horaInicio.h == h && aData.horaInicio.m == m) {
//				Debug.Log ("Encontrada: " + aulas[aulaID] + ". Titulo:" + aData.titulo + ". " + aulaID + " " + quarter + " " + dias[day] + " " + h + " " + m);
				// Aprovechamos y le seteamos aulaID y aulaName
				aData.aulaID = aulaID;
				aData.aulaName = aulas [aulaID];
				retValue = aData;
				break;
			}
		}
		return retValue;
	}
		

	/** Retorna la nomina de horarios por dia */
	protected static List<AulaTime> getTimesOfDay() {
		List<AulaTime> retValue = new List<AulaTime> ();
		for (int h = COURSES_STARTING_HOUR; h <= COURSES_ENDING_HOUR; h++) {
			for (int m = 0; m <= STEP_MINUTES; m += STEP_MINUTES) {
//				Debug.Log ("" + h + ":" + m);
				AulaTime aTime = new AulaTime ();
				aTime.h = (h<10 ? "0" + h : ""+h);
				aTime.m = (m==0 ? "00" : ""+m);
				retValue.Add (aTime);
			}
		}	
		return retValue;
	}


	/** Retorna el numero de casilleros en el horario en la grilla de horarios entrel el inicio y el final de la clase */
	protected static int getRepeatCellCount(List<AulaTime> timesOfDay, AulaData criteria) {
		int startPos = -1;
		int endPos = -1;
		int currPos = -1;
		foreach (AulaTime aTime in timesOfDay) {
			currPos++;
			if (startPos > -1 && endPos > -1)
				break;
			if (aTime.h == criteria.horaInicio.h && aTime.m == criteria.horaInicio.m)
				startPos = currPos;
			if (aTime.h == criteria.horaFin.h && aTime.m == criteria.horaFin.m)
				endPos = currPos;
		}
//		Debug.Log ("" + criteria.horaFin.h + ":" + criteria.horaFin.m + " -- " + endPos + " " + startPos + " = " + (endPos - startPos));
		return endPos-startPos-1;
	}


	/** Imprime la agenda diaria.  Si fillBlanks es true, igualmente imprime en pantalla cuando no hay información */
	public static void printDailySchedule(int aulaID, int quarter, int day, bool fillBlanks) {
		//Dictionary<int, Dictionary<AulaTime, AulaData>>
		foreach (AulaTime time in weeklySchedule[quarter][aulaID][day].Keys) {
			if (!fillBlanks && weeklySchedule[quarter] [aulaID] [day] [time] == null)
				continue;
			Debug.Log (aulas[aulaID] + ". " + quarter + "° cuat. " + dias[day] + ". [" + time.h + ":" + time.m + "] - " + (weeklySchedule [quarter] [aulaID] [day] [time] == null ? "" : weeklySchedule [quarter] [aulaID] [day] [time].ToString() ) );
		}
	}


	/** Imprime la agenda semanal.  Si fillBlanks es true, igualmente imprime en pantalla cuando no hay información */
	public static void printWeeklySchedule(int aulaID, int quarter, bool fillBlanks) {
		//Dictionary<int, Dictionary<AulaTime, AulaData>>
		foreach (int day in weeklySchedule[quarter][aulaID].Keys) {
			if (!fillBlanks && weeklySchedule [quarter] [aulaID] [day] == null)
				continue;
			foreach (AulaTime time in weeklySchedule[quarter][aulaID][day].Keys) {
				if (!fillBlanks && weeklySchedule[quarter] [aulaID] [day] [time] == null)
					continue;
				Debug.Log (aulas[aulaID] + ". " + quarter + "° cuat. " + dias[day] + ". [" + time.h + ":" + time.m + "] - " + (weeklySchedule [quarter] [aulaID] [day] [time] == null ? "" : weeklySchedule [quarter] [aulaID] [day] [time].ToString() ) );
			}
		}
	}


	/** Imprime el cronograma completo */
	public static void printCompleteSchedule() {
		foreach (int q in QUARTERS) {
			foreach (int aulaID in aulas.Keys) {
				printWeeklySchedule (aulaID, q, false);
			}
		}
	}


	/** Retorna el numero de semestre actual segun la fecha del día */
	public static int getCurrentQuarter() {
		if (System.DateTime.Today.Month <= QUARTER_HALF)
			return QUARTER_FIRST;
		return QUARTER_SECOND;
	}
}

