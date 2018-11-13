using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour {
	public static TreeManager st;

	public HUDNode startPoint;

	[HideInInspector]
	public int strBase, intBase, dexBase;
	[HideInInspector]
	public int strength, intelligence, dexterity;
	[HideInInspector]
	public int strMult, intMult, dexMult;

	public Text str, intl, dex;

	public GameObject tooltip;

	void Awake(){
		st = this;
	}

	void Start () {
		startPoint.active = true;
	}

	public void updateStats () {
		strength = strBase + strBase * strMult / 100;
		intelligence = intBase + intBase * intMult / 100;
		dexterity = dexBase + dexBase * dexMult / 100;

		str.text = strength.ToString ();
		intl.text = intelligence.ToString ();
		dex.text = dexterity.ToString ();
	}

	public void activateTooltip(bool state, Vector3 position, string txt){
		tooltip.SetActive (state);
		tooltip.transform.position = position;
		tooltip.GetComponentInChildren<Text> ().text = txt;
	}
}
