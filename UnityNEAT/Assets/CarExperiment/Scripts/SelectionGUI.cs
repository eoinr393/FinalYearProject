using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionGUI : MonoBehaviour {

	public float groupWidth = 150;
	public float groupHeight = 200;

	bool selected = false;
	GameObject creature;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)) {

			RaycastHit hit;

			selected = Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit);

			GameObject Eval = GameObject.Find ("Evaluator").GetComponent<Optimizer>().Unit;

			if (selected) {
				if (hit.transform.gameObject.tag == Eval.tag) {

					creature = hit.transform.gameObject;
				}
				//else do deselect
			}

			if (creature == null)
				selected = false;
		}


		
	}

	void OnGUI(){

		var groupX = Screen.width - groupWidth - 10;
		var groupY = 20;

		if (selected) {
			
			GUI.BeginGroup (new Rect (groupX, groupY, groupWidth, groupHeight));
			GUI.Box (new Rect (0, 0, groupWidth, groupHeight), "Creature Stats");

			GUI.Label (new Rect (10, 30, 100, 30), "Food Eaten: " + creature.GetComponent<CarController>().foodEaten);
			GUI.Label (new Rect (10, 70, 100, 30), "Enemies Hit: " + creature.GetComponent<CarController>().predatorHit);
			GUI.Label (new Rect (10, 110, 100, 30), "Walls Hit: " + creature.GetComponent<CarController>().WallHits);
			GUI.Label (new Rect (10, 110, 100, 30), "Stamina: " + creature.GetComponent<CarController>().curstamina + "/" + creature.GetComponent<CarController>().stamina);


			GUI.EndGroup (); 
		}
	}
}
