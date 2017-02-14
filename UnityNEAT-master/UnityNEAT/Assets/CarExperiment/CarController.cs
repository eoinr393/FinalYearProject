using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class CarController : UnitController {

    public float Speed = 5f;
    public float TurnSpeed = 180f;
    public int Lap = 1;
    public int CurrentPiece, LastPiece;
    bool IsRunning;
    public float SensorRange = 10;
    int WallHits; 
	private Vector3 initPos;
	private float maxDist = 0;
    IBlackBox box;

	// Use this for initialization
	void Start () {
		initPos = this.gameObject.transform.GetChild(0).transform.position;
		print ("Creature position = " + initPos);
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        //grab the input axes
        //var steer = Input.GetAxis("Horizontal");
        //var gas = Input.GetAxis("Vertical");

        ////if they're hittin' the gas...
        //if (gas != 0)
        //{
        //    //take the throttle level (with keyboard, generally +1 if up, -1 if down)
        //    //  and multiply by speed and the timestep to get the distance moved this frame
        //    var moveDist = gas * speed * Time.deltaTime;

        //    //now the turn amount, similar drill, just turnSpeed instead of speed
        //    //   we multiply in gas as well, which properly reverses the steering when going 
        //    //   backwards, and scales the turn amount with the speed
        //    var turnAngle = steer * turnSpeed * Time.deltaTime * gas;

        //    //now apply 'em, starting with the turn           
        //    transform.Rotate(0, turnAngle, 0);

        //    //and now move forward by moveVect
        //    transform.Translate(Vector3.forward * moveDist);
        //}

        // Five sensors: Front, left front, left, right front, right

        if (IsRunning)
        {
			//check distance
			float curDist = Vector3.Distance (this.gameObject.transform.position, initPos);
			if(maxDist < curDist)
					maxDist = curDist;

			ISignalArray inputArr = box.InputSignalArray;

			inputArr[0] = this.gameObject.transform.GetChild (1).transform.eulerAngles.x;
			inputArr[2] = this.gameObject.transform.GetChild (1).transform.eulerAngles.y;
			inputArr[3] = this.gameObject.transform.GetChild (1).transform.eulerAngles.z;
			inputArr[4] = this.gameObject.transform.GetChild (0).transform.eulerAngles.x;
			inputArr[5] = this.gameObject.transform.GetChild (0).transform.eulerAngles.y;
			inputArr[6] = this.gameObject.transform.GetChild (0).transform.eulerAngles.z;

			box.Activate();

			ISignalArray outputArr = box.OutputSignalArray;

			var steerZ =(float)outputArr[0] * TurnSpeed;
			//var steerZ = (float)outputArr [1] * 2 * TurnSpeed ;
			float torque = steerZ * 1000;
			//print ("steerX" + steerX);
			//print ("steerZ" + steerZ);

			//this.GetComponent<HingeJoint>().transform.Rotate (new Vector3 (steerX, steerZ, 0));
			//this.GetComponent<HingeJoint>().connectedBody.transform.Rotate(new Vector3 (0, 0, steerZ) * Time.deltaTime);

			//this.gameObject.transform.GetChild (1).Rotate (new Vector3 (0, 0, steerZ) * Time.deltaTime, Space.Self);
			//this.gameObject.transform.GetChild (1).gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 0, steerZ));

			foreach (Transform child in transform) {
				if (child.tag == "Leg") {
					print ("found Leg");
					child.GetComponent<Rigidbody> ().AddTorque (0, torque, 0, ForceMode.Force);
				}
			}

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

			float fit = maxDist/10;

			if(fit < 1){
				return 0;
			}

			return maxDist/ 10;
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
