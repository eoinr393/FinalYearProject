using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class CarController : UnitController {

    bool IsRunning;
    public float SensorRange = 10;
	private Vector3 initPos;
	private Vector3 goToPos;
	private float maxDist = 0;
    IBlackBox box;

	public float period = 2.0f;
	public float angle = 5.0f;
	private float t;
	private float limbCount = 0;

	private GameObject body = null;

	// Use this for initialization
	void Start () {
		initPos = this.gameObject.transform.GetChild(0).transform.position;
		print ("Creature position = " + initPos);

		goToPos = new Vector3 (transform.position.x + 100, transform.position.y, transform.position.z);

		//set body
		foreach (Transform child in transform) {
			if (child.tag == "Body") {
				body = child.gameObject;
			}
		}
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (IsRunning)
        {
			//check distance
			float curDist = Vector3.Distance (this.gameObject.transform.position, initPos);
			if(maxDist < curDist)
					maxDist = curDist;

			///////////////////////////
			//set neural network inputs
			///////////////////////////
			ISignalArray inputArr = box.InputSignalArray;

			int arrayInc = 0;
			foreach (HingeJoint hinge in body.GetComponents<HingeJoint>()) {
				inputArr [arrayInc] = hinge.connectedBody.transform.eulerAngles.z;
				arrayInc++;
			}

			limbCount = arrayInc;

			inputArr [arrayInc] = body.transform.eulerAngles.z;
			inputArr [arrayInc + 1] = body.transform.eulerAngles.y;
			inputArr [arrayInc + 2] = body.transform.eulerAngles.x;

			box.Activate();

			ISignalArray outputArr = box.OutputSignalArray;

			/////////////////////
			//do the leg rotating
			/////////////////////
			t = t + Time.deltaTime;

			int compInc = 0;

			foreach(HingeJoint comp in body.GetComponents<HingeJoint>()){
				float hAngle = Mathf.Clamp((float)outputArr [compInc],-200.0f,200.0f) * 20;
				float hPeriod = Mathf.Clamp((float)outputArr [compInc + 1], 0.0f, 10.0f) * 2;

				hPeriod = (float)System.Math.Round (hPeriod, 3);
				float phase = (float)Mathf.Sin(t / hPeriod);

				float rotamt = hAngle * phase;

				print ("Adding " + rotamt + " of Torque from angle=" + hAngle + " period=" + hPeriod + " t: " + t);
				Rigidbody leg = comp.connectedBody;
			
				leg.AddTorque((new Vector3 (0,0,rotamt)));

				compInc += 2;
			}

			//var steerZ =(float)outputArr[0] * TurnSpeed;

			//float torque = steerZ * 1000;
			//print ("steerX" + steerX);
			//print ("steerZ" + steerZ);

			//this.GetComponent<HingeJoint>().transform.Rotate (new Vector3 (steerX, steerZ, 0));
			//this.GetComponent<HingeJoint>().connectedBody.transform.Rotate(new Vector3 (0, 0, steerZ) * Time.deltaTime);

			//this.gameObject.transform.GetChild (1).Rotate (new Vector3 (0, 0, steerZ) * Time.deltaTime, Space.Self);
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 0, steerZ));

			/*foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					print ("found Leg");
					//child.GetComponent<Rigidbody> ().AddTorque (0, torque, 0, ForceMode.Force);
				}
			}*/

        }
    }

    public override void Stop()
    {
        this.IsRunning = false;
    }

    public override void Activate(IBlackBox box)
    {
        this.box = box;
        this.IsRunning = true;
    }

    public override float GetFitness()
    {
        /*if (Lap == 1 && CurrentPiece == 0)
        {
            return 0;
        }
        int piece = CurrentPiece;
        if (CurrentPiece == 0)
        {
            piece = 17;
        }
        float fit = Lap * piece - WallHits * 0.2f;
        print(string.Format("Piece: {0}, Lap: {1}, Fitness: {2}", piece, Lap, fit));
        if (fit > 0)
        {
            return fit;
        }*/
		print ("Checking fitness..");
		try{

			if(Vector3.Distance(goToPos, transform.position) > Vector3.Distance(initPos, goToPos))
				return 0;


			Vector3 targetVec = new Vector3(goToPos.x,0, goToPos.z) - new Vector3(initPos.x,0,initPos.z);
			float fit = (float)Vector3.Distance(initPos, transform.position) - (float)(Vector3.Angle(targetVec, transform.right) * 0.1);

			print("Fitness:: Distance: " + (float)Vector3.Distance(initPos, transform.position) + " || Angle Difference: " + (float)Vector3.Angle(targetVec, transform.right));

			if(fit < 1){
				return 0;
			}

			return fit;
		}
		catch{
			print ("failed to get distance");
			return 0;
		}

    }

    



    //void OnGUI()
    //{
    //    GUI.Button(new Rect(10, 200, 100, 100), "Forward: " + MovingForward + "\nPiece: " + CurrentPiece + "\nLast: " + LastPiece + "\nLap: " + Lap);
    //}
    
}
