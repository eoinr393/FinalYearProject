using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
///Script for spawning enemies in a certain are
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class SpawnEnemies : MonoBehaviour {


	public string creatureTag = "Predator";
	public int minEnemyAmount = 10;
	public GameObject enemyPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int enemyCount = GameObject.FindGameObjectsWithTag (creatureTag).Length;


		enemyPrefab.layer = LayerMask.NameToLayer ("Default");
		if (enemyCount < minEnemyAmount) {

			float randX = Random.Range (-40, 40);
			float randZ = Random.Range (-40, 40);
			Instantiate (enemyPrefab, new Vector3 (randX, 5f, randZ), Quaternion.identity);
		}
	}
}
