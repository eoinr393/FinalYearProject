﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Scritp for showing and controlling the UI for 
/// when a user selects a creature on the evolution scene
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class SelectionGUI : MonoBehaviour {

	public float groupWidth = 150;
	public float groupHeight = 200;

	private float gWidth = 150;
	private float gHeight = 200;

	bool selected = false;
	GameObject creature;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//sends a raycast from the users mouse click down, if it hits a creature, show that creatures statistics
		if (Input.GetMouseButtonDown(0)) {

			RaycastHit hit;

			selected = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);

			GameObject Eval = GameObject.Find ("Evaluator").GetComponent<Optimizer>().Unit;

			if (selected) {
				if (hit.transform.gameObject.tag == Eval.tag) {

					creature = hit.transform.gameObject;

					gWidth = groupWidth;
					gHeight = groupHeight;
				}
				//else do deselect
			} else {
				gWidth = 0;
				gHeight = 0;
			}

			if (creature == null)
				selected = false;
		}
	}

	void OnGUI(){

		var groupX = Screen.width - groupWidth - 10;
		var groupY = 20;

		if (selected){
			GUI.BeginGroup (new Rect (groupX, groupY, groupWidth, groupHeight));
			GUI.Box (new Rect (0, 0, gWidth, gHeight), "Creature Stats");

			GUI.Label (new Rect (10, 30, 100, 30), "Food Eaten: " + creature.GetComponent<CreatureController> ().foodEaten);
			GUI.Label (new Rect (10, 70, 100, 30), "Enemies Hit: " + creature.GetComponent<CreatureController> ().predatorHit);
			GUI.Label (new Rect (10, 110, 100, 30), "Walls Hit: " + creature.GetComponent<CreatureController> ().WallHits);
			GUI.Label (new Rect (10, 150, 100, 30), "Stamina: " + creature.GetComponent<CreatureController> ().curstamina + "/" + creature.GetComponent<CreatureController> ().stamina.ToString("0.0"));


			GUI.EndGroup (); 
		}

	}
}
