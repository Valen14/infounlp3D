using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AulasConstants  {

	public static int ID_AULA_1 = 12;
	public static int ID_AULA_2 = 13;
	public static int ID_AULA_3 = 14;
	public static int ID_AULA_4 = 15;
	public static int ID_AULA_5 = 16;
	public static int ID_AULA_1_2 = 18;
	public static int ID_AULA_1_3 = 19;
	public static int ID_AULA_1_4 = 20;
	public static int ID_AULA_9 = 23;
	public static int ID_AULA_6 = 24;
	public static int ID_AULA_7 = 25;
	public static int ID_AULA_10A = 27;
	public static int ID_AULA_10B = 28;
	public static int ID_AULA_11 = 29;

	private static float alturaMediaPB = 1.5f;
	private static float alturaMediaP1 = 7.8f;
	public static Vector3 POS_AULA_1 = new Vector3(-137, alturaMediaPB, 75);
	public static Vector3 POS_AULA_2 = new Vector3(-129, alturaMediaPB, 70);
	public static Vector3 POS_AULA_3 = new Vector3(-120, alturaMediaPB, 65);
	public static Vector3 POS_AULA_4 = new Vector3(-110, alturaMediaPB, 60);
	public static Vector3 POS_AULA_5 = new Vector3(-93, alturaMediaPB, 55);
	public static Vector3 POS_AULA_1_2 = new Vector3(-137, alturaMediaP1, 70);
	public static Vector3 POS_AULA_1_3 = new Vector3(-125, alturaMediaP1, 63);
	public static Vector3 POS_AULA_1_4 = new Vector3(-110, alturaMediaP1, 55);
	public static Vector3 POS_AULA_9 = new Vector3(-55, alturaMediaPB, 30);
	public static Vector3 POS_AULA_6 = new Vector3(-75, alturaMediaPB, 40);
	public static Vector3 POS_AULA_7 = new Vector3(-68, alturaMediaPB, 37);
	public static Vector3 POS_AULA_10A = new Vector3(-48, alturaMediaPB, 43);
	public static Vector3 POS_AULA_10B = new Vector3(-44, alturaMediaPB, 50);
	public static Vector3 POS_AULA_11 = new Vector3(-34, alturaMediaPB, 68);

	public static Dictionary<int, Vector3> aulasPosition = new Dictionary<int, Vector3>();

	static AulasConstants() {
		aulasPosition.Add(ID_AULA_1, POS_AULA_1);
		aulasPosition.Add(ID_AULA_2, POS_AULA_2);
		aulasPosition.Add(ID_AULA_3, POS_AULA_3);
		aulasPosition.Add(ID_AULA_4, POS_AULA_4);
		aulasPosition.Add(ID_AULA_5, POS_AULA_5);
		aulasPosition.Add(ID_AULA_1_2, POS_AULA_1_2);
		aulasPosition.Add(ID_AULA_1_3, POS_AULA_1_3);
		aulasPosition.Add(ID_AULA_1_4, POS_AULA_1_4);
		aulasPosition.Add(ID_AULA_9, POS_AULA_9);
		aulasPosition.Add(ID_AULA_6, POS_AULA_6);
		aulasPosition.Add(ID_AULA_7, POS_AULA_7);
		aulasPosition.Add(ID_AULA_10A, POS_AULA_10A);		
		aulasPosition.Add(ID_AULA_10B, POS_AULA_10B);
		aulasPosition.Add(ID_AULA_11, POS_AULA_11);
	}


}
