using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehviour : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Prey") {
			Destroy (this.gameObject);
		}
	}
}
