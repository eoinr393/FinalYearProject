using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class BodyController : UnitController {

	public float Speed = 5f;
	public float TurnSpeed = 180f;
	public int FoodEaten = 0;
	public float SensorRange = 30;
	public int time = 0;

	bool IsRunning = true;

	private float initalDist;

	Vector3 goalPos = new Vector3 (100, 10, 1000);

	IBlackBox box;

	// Use this for initialization
	void Start () {
		initalDist = (goalPos - this.transform.position).magnitude;
	}


	// Update is called once per frame
	void FixedUpdate () {
	
		if (IsRunning) {

			ISignalArray inputArr = box.InputSignalArray;

			inputArr [0] = this.GetComponent<HingeJoint>().connectedBody.transform.eulerAngles.x;
			inputArr [1] = this.GetComponent<HingeJoint>().connectedBody.transform.eulerAngles.y;
			inputArr [2] = this.GetComponent<HingeJoint>().connectedBody.transform.eulerAngles.z;

			inputArr [3] = transform.eulerAngles.x;
			inputArr [4] = transform.eulerAngles.y;
			inputArr [5] = transform.eulerAngles.z;

			box.Activate();

			ISignalArray outputArr = box.OutputSignalArray;

			//var += (float)outputArr[0] * 2 - 1;
			//var gas = (float)outputArr[1] * 2 - 1;
			/*var steer = (float)outputArr[0] * 2;
			var gas = 2.0f;

			var moveDist = gas * Speed * Time.deltaTime;
			var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

			transform.Rotate(new Vector3(0, turnAngle, 0));
			transform.Translate(Vector3.forward * moveDist);
			*/

			var steerX = (float)outputArr[0] * 2 * TurnSpeed ;
			var steerZ = (float)outputArr [1] * 2 * TurnSpeed ;

			print ("steerX" + steerX);
			print ("steerZ" + steerZ);


			//this.GetComponent<HingeJoint>().transform.Rotate (new Vector3 (steerX, steerZ, 0));
			this.GetComponent<HingeJoint>().connectedBody.transform.Rotate(new Vector3 (steerX, steerZ, 0) * Time.deltaTime);

			//this.GetComponent<HingeJoint>().connectedBody.AddForce(new Vector3 (steerX, steerZ, 0));

			/*
			foreach (Transform child in transform) {
				if(child.tag == "Leg"){
					print ("Rotating");
					child.transform.Rotate (new Vector3 (steerX, steerZ, 0));
				}
			}*/

			time++;
		}
	}

	public override void Activate(IBlackBox box)
	{
		this.box = box;
		this.IsRunning = true;
	}

	public override void Stop(){
		this.IsRunning = false;
	}

	public override float GetFitness(){
		
		float curDist = (goalPos - this.transform.position).magnitude;

		print ("helllooooooo " + curDist);

		float fit = Mathf.Clamp (initalDist - curDist, 0, 200);

		return fit;
	}
		
}
