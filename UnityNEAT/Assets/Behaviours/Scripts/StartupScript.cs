using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// This Script is used for the initialization of the 
/// evolution section of this project.
/// 
/// It sets the type and number of creatures to be spawned,
/// as well as attatces the necassary game components to 
/// each game object
/// 
/// --Eoin Raeside 04/2017
/// </summary>

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

	//set which creatures are about to be spawned and in what numbers
	void setSpawning(){
		SpawnEnemies sp = GetComponent<SpawnEnemies> ();
		FoodSpawner fs = GetComponent<FoodSpawner> ();

		//if evolving creature is prey
		if (SelectionMenu.herb) {
			sp.enemyPrefab = predpref;
			sp.enemyPrefab.GetComponent<Predator> ().enabled = true;
			sp.creatureTag = "Predator";
			sp.minEnemyAmount = 25;
			fs.minFoodAmount = 45;

		} else {
			sp.enemyPrefab = preypref;
			sp.enemyPrefab.GetComponent<Prey> ().enabled = true;
			sp.creatureTag = "Prey";
			sp.minEnemyAmount = 30;
			fs.minFoodAmount = 25;
		}
		sp.enemyPrefab.GetComponent<CreatureController> ().enabled = false;

		sp.enabled = true;
	}

	//initialize the optimizer that is to be used in the scene
	void setUpOptimizer(){
		//if creature to be evolved is of type prey
		if (SelectionMenu.herb) {
			creature = preypref;
			creature.GetComponent<Prey> ().enabled = false;
			creature.GetComponent<CreatureController> ().predstr = "Predator";
			creature.GetComponent<CreatureController> ().foodstr = "Food";
			creature.GetComponent<CreatureController> ().setRays (preySight, preyFov);

		} else {
			creature = predpref;
			creature.GetComponent<Predator> ().enabled = false;
			creature.GetComponent<CreatureController> ().predstr = "Food";
			creature.GetComponent<CreatureController> ().foodstr = "Prey";
			creature.GetComponent<CreatureController> ().setRays (predSight, predFov);
		}

		//layer
		creature.layer = LayerMask.NameToLayer("Car");

		//change the attributes of the creature
		creature.GetComponent<CreatureController> ().enabled = true;
		creature.GetComponent<CreatureController> ().setTraits (SelectionMenu.speed * 1.5f, SelectionMenu.speed, SelectionMenu.turnSpeed * 36, SelectionMenu.stamina);

		creature.transform.localScale = new Vector3 (SelectionMenu.size, SelectionMenu.size, SelectionMenu.size);
		creature.GetComponent<Rigidbody> ().mass = SelectionMenu.size;

		//set the inputs for the neural network
		int inputs = creature.GetComponent<CreatureController> ().searchObjectCount * creature.GetComponent<CreatureController> ().rayCount + 1;// 1 is for stamina 

		//attatch the optimizer to the Evaluator gameobject
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
