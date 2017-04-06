using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Script to destroy food on collision with prey creature
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class FoodBehviour : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Prey") {
			Destroy (this.gameObject);
		}
	}
}
