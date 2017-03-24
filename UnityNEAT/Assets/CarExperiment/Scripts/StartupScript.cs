using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour {

	public GameObject preypref;
	public GameObject predpref;

	public static float preyFov = 15;
	public static float predFov = 5;
	public static float preySight = 40;
	public static float predSight = 45;

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
			creature.GetComponent<CarController> ().predstr = "Predator";
			creature.GetComponent<CarController> ().foodstr = "Food";
			creature.GetComponent<CarController> ().setRays (preySight, preyFov);

		} else {
			creature = predpref;
			creature.GetComponent<PredatorScript> ().enabled = false;
			creature.GetComponent<CarController> ().predstr = "Food";
			creature.GetComponent<CarController> ().foodstr = "Prey";
			creature.GetComponent<CarController> ().setRays (predSight, predFov);
		}

		//layer
		creature.layer = LayerMask.NameToLayer("Car");

		//change the attributes of the creature
		creature.GetComponent<CarController> ().enabled = true;

		creature.GetComponent<CarController> ().setTraits (SelectionMenu.speed * 1.5f, SelectionMenu.speed, SelectionMenu.turnSpeed * 36, SelectionMenu.stamina);

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
