using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

	public int minFoodAmount = 15;
	public GameObject foodPrefab;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		int foodCount = GameObject.FindGameObjectsWithTag ("Food").Length;

		if (foodCount < minFoodAmount) {

			float randX = Random.Range (-40, 40);
			float randZ = Random.Range (-40, 40);

			Instantiate (foodPrefab, new Vector3 (randX, 0.5f, randZ), Quaternion.identity);
		}
	}
}
