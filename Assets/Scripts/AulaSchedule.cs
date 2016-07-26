using UnityEngine;
using System.Collections;

[System.Serializable]
public class AulaSchedule {

	/**
	 * Conjunto de clases que se dictan en un aula determinada
	 */

	public int aulaID;
	public int quarter;
	public string aulaName;


	// Informacion recibida en el JSon
	public AulaData[] values;


	public override string ToString() {
		if (values == null)
			return "";
		string retValue = "[Aula:" + aulaName + ". Quarter:" + quarter + ".] ";
		foreach (AulaData aulaData in values) {
			retValue = retValue + aulaData.ToString();
		}
		return retValue;
	}

}
