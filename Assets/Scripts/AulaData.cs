using UnityEngine;
using System.Collections;


[System.Serializable]
public class AulaData  {

	/**
	 * Informacion de una clase en particular
	 */

	public int aulaID;
	public string aulaName;

	// Informacion recibida en el JSon
	public string titulo;
	public string docente;
	public string tipo;
	public string semestre;
	public int dia;
	public AulaTime horaInicio; 
	public AulaTime horaFin;


	public override string ToString() {
		return titulo + "; " +
				docente + "; " +
				tipo + "; " +
				semestre + "; " +
			    AulaInfoManager.dias[dia] + "; " +
				"De " + horaInicio.ToString() + " a " +	horaFin.ToString() + "; ";
	}

	public string ToShortString() {
		return	"[" + horaInicio.ToString() + " a " +	horaFin.ToString() + "]- " + 
				titulo + "; " +
				docente + "; " +
				tipo + "; ";
	}
}
