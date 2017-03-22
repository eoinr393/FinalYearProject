using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour {

	public GameObject preypref;
	public GameObject predpref;

	GameObject creature;
	SelectionMenu selMen;

	// Use this for initialization
	void Start () {
		setUpOptimizer ();
		setSpawning ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setSpawning(){
		SpawnEnemies sp = GetComponent<SpawnEnemies> ();
		FoodSpawner fs = GetComponent<FoodSpawner> ();

		if (SelectionMenu.herb) {
			sp.enemyPrefab = predpref;
			sp.enemyPrefab.GetComponent<PredatorScript> ().enabled = true;
			sp.creatureTag = "Predator";
			sp.minEnemyAmount = 15;
			fs.minFoodAmount = 20;

		} else {
			sp.enemyPrefab = preypref;
			sp.enemyPrefab.GetComponent<PreyScript> ().enabled = true;
			sp.creatureTag = "Prey";
			sp.minEnemyAmount = 15;
			fs.minFoodAmount = 10;
		}
		sp.enemyPrefab.GetComponent<CarController> ().enabled = false;

		sp.enabled = true;
	}

	void setUpOptimizer(){

		if (SelectionMenu.herb) {
			creature = preypref;
			creature.GetComponent<PreyScript> ().enabled = false;
			creature.GetComponent<CarController> ().predstr = "Prey";
			creature.GetComponent<CarController> ().foodstr = "Food";

		} else {
			creature = predpref;
			creature.GetComponent<PredatorScript> ().enabled = false;
			creature.GetComponent<CarController> ().predstr = "Food";
			creature.GetComponent<CarController> ().foodstr = "Prey";
		}

		//layer
		creature.layer = LayerMask.NameToLayer("Car");

		//change the attributes of the creature
		creature.GetComponent<CarController> ().enabled = true;
		creature.GetComponent<CarController> ().TurnSpeed = SelectionMenu.turnSpeed * 36;
		creature.GetComponent<CarController> ().Speed = SelectionMenu.speed;
		creature.GetComponent<CarController> ().maxSpeed = SelectionMenu.speed * 1.5f;
		creature.GetComponent<CarController> ().stamina = SelectionMenu.stamina;

		creature.transform.localScale = new Vector3 (SelectionMenu.size, SelectionMenu.size, SelectionMenu.size);
		creature.GetComponent<Rigidbody> ().mass = SelectionMenu.size;

			
		int inputs = creature.GetComponent<CarController> ().searchObjectCount * creature.GetComponent<CarController> ().rayCount + 1;// 1 is for stamina 


		GameObject eval = GameObject.Find ("Evaluator");
		eval.SetActive (false);
		eval.AddComponent(typeof(Optimizer));
		eval.GetComponent<Optimizer> ().creatureName = SelectionMenu.creatureName;
		eval.GetComponent<Optimizer> ().Unit = creature;
		eval.GetComponent<Optimizer> ().Trials = 1;
		eval.GetComponent<Optimizer> ().TrialDuration = 25;
		eval.GetComponent<Optimizer> ().StoppingFitness = 15;
		eval.SetActive (true);


	}
}
