using UnityEngine;
using System.Collections;

[System.Serializable]
public class AulaTime {

	/**
	 * Hora:Minuto
	 */

	// Informacion recibida en el JSon
	public string h;
	public string m;

	public AulaTime()
	{
		h = "00";
		m = "00";
	}

	public AulaTime(string hora, string minutos)
	{
		h = hora;
		m = minutos;
	}

	public override string ToString() {
		return h + ":" + m;
	}
}
