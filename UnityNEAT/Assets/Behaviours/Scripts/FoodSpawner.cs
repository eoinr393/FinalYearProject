using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Script to Spawn Food in an area
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class FoodSpawner : MonoBehaviour {

	public int minFoodAmount = 15;
	public GameObject foodPrefab;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		int foodCount = GameObject.FindGameObjectsWithTag ("Food").Length;

		if (foodCount < minFoodAmount) {

			float randX = Random.Range (-40, 40);
			float randZ = Random.Range (-40, 40);

			Instantiate (foodPrefab, new Vector3 (randX, 0.5f, randZ), Quaternion.identity);
		}
	}
}
