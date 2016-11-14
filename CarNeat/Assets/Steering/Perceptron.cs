using UnityEngine;
using System.Collections;

public class Perceptron : MonoBehaviour {

	float[] weights;
	float c;


	// Use this for initialization
	void Start () {
	
	}

	public Perceptron(int n){
		float constant = 0.001f;
		weights = new float[n];
		c = constant;

		for (int i = 0; i < n; i++) {
			weights [i] = Random.Range (-1.0f,1.0f);
		}
	}

	public Vector3 FeedForward(Vector3[] forces){
		Vector3 sum = new Vector3();

		for (int i = 0; i < weights.Length; i++) {
			sum += forces[i] * weights[i];
		}

		return sum;
	}

	public void Train(Vector3[] inputs, Vector3 error){

		for(int i = 0; i < inputs.Length; i++) {
			weights[i] += c * error.x * inputs[i].x;
			weights [i] += c * error.z * inputs [i].z;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
