using UnityEngine;
using System.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRTK;
using VRTK.SecondaryControllerGrabActions;
using VRTK.GrabAttachMechanics;
using VRTK.Highlighters;
using ModVR;

public class ModVR_ColorUtil {

    public static void changeColor(GameObject obj, string color)
	{
		Color newColor = new Color(0,0,0,1);
		switch (color){
			case "red":
				newColor = Color.red;
				break;
			case "yello":
				newColor = Color.yellow;			
				break;
			case "blue":
				newColor = Color.blue;			
				break;
			case "green":
				newColor = Color.green;			
				break;
			case "black":
				newColor = Color.black;			
				break;
			case "white":
				newColor = Color.white;			
				break;
		}		
		changeToCustomColor(obj,newColor);
	}
	public static void changeToCustomColor(GameObject obj, Color newColor)
	{
		obj.GetComponent<Renderer>().material.color = newColor;
	}
	public static Color combineTwoColors(Color fg, Color bg)
	{
		Color r = new Color(0,0,0,1);

		// r.a = 1 - (1 - fg.a) * (1 - bg.a);
		if (r.a < 1.0e-6) return r; // Fully transparent -- R,G,B not important
		// r.r = fg.r * fg.a / r.a + bg.r * bg.a * (1 - fg.a) / r.a;
		// r.g = fg.g * fg.a / r.a + bg.g * bg.a * (1 - fg.a) / r.a;
		// r.b = fg.b * fg.a / r.a + bg.b * bg.a * (1 - fg.a) / r.a;
		r.r = (fg.r + bg.r) / 2;
		r.b = (fg.b + bg.b) / 2;
		r.g = (fg.g + bg.g) / 2;
		Debug.Log(fg);
		Debug.Log(bg);
		Debug.Log(r);
		return r;
	}	
}
