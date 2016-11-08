using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class BodyController : UnitController {

	public float Speed = 5f;
	public float TurnSpeed = 180f;
	public int FoodEaten = 0;
	public int time = 0;
	bool MovingForward = true;
	public float SensorRange = 30;
	bool IsRunning = true;
	IBlackBox box;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (IsRunning) {
			float frontSensor = 0;
			float leftFrontSensor = 0;
			float leftSensor = 0;
			float rightFrontSensor = 0;
			float rightSensor = 0;
			// Front sensor
			RaycastHit hit;
			if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0, 0, 1).normalized), out hit, SensorRange))
			{
				if (hit.collider.tag.Equals("Food"))
				{
					frontSensor = 1 - hit.distance / SensorRange;
				}
			}

			if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(0.5f, 0, 1).normalized), out hit, SensorRange))
			{
				if (hit.collider.tag.Equals("Food"))
				{
					rightFrontSensor = 1 - hit.distance / SensorRange;
				}
			}

			if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(1, 0, 0).normalized), out hit, SensorRange))
			{
				if (hit.collider.tag.Equals("Food"))
				{
					rightSensor = 1 - hit.distance / SensorRange;
				}
			}

			if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-0.5f, 0, 1).normalized), out hit, SensorRange))
			{
				if (hit.collider.tag.Equals("Food"))
				{
					leftFrontSensor = 1 - hit.distance / SensorRange;
				}
			}

			if (Physics.Raycast(transform.position + transform.forward * 1.1f, transform.TransformDirection(new Vector3(-1, 0, 0).normalized), out hit, SensorRange))
			{
				if (hit.collider.tag.Equals("Food"))
				{
					leftSensor = 1 - hit.distance / SensorRange;
				}
			}

			ISignalArray inputArr = box.InputSignalArray;
			inputArr[0] = frontSensor;
			inputArr[1] = leftFrontSensor;
			inputArr[2] = leftSensor;
			inputArr[3] = rightFrontSensor;
			inputArr[4] = rightSensor;

			box.Activate();

			ISignalArray outputArr = box.OutputSignalArray;

			var steer = (float)outputArr[0] * 2 - 1;
			var gas = (float)outputArr[1] * 2 - 1;

			var moveDist = gas * Speed * Time.deltaTime;
			var turnAngle = steer * TurnSpeed * Time.deltaTime * gas;

			transform.Rotate(new Vector3(0, turnAngle, 0));
			transform.Translate(Vector3.forward * moveDist);

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

		if (FoodEaten == 0) {
			return 0;
		}

		float fit = FoodEaten / time;

		if (fit > 0) {
			return fit;
		}

		return 0;
	}

	void OnTriggerEnter(Collider collider){

		if (collider.tag.Equals ("Food")) {
			FoodEaten++;
		}
	}
}
