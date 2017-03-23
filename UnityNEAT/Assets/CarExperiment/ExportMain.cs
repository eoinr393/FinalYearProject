using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class ExportMain : MonoBehaviour {


	List<string> fileNames = new List<string>();
	List<string> creatureNames = new List<string>();
	List<string> creatureTraits = new List<string> ();

	string selectedCreature = "";
	string traits = "";

	bool showTraits = false;

	// Use this for initialization
	void Start () {
		//get all filenames
		foreach (string s in Directory.GetFiles(Application.persistentDataPath)) {

			string subbed = s.Substring (Application.persistentDataPath.Length + 1);

			fileNames.Add (subbed);
		}

		foreach (string s in fileNames) {
			string name = s.Substring (0, s.IndexOf ("."));

			if (creatureNames.Where (x => x.Equals (name)).Count () < 1) {
				creatureNames.Add (name);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		float buttonX = 20;
		float buttonY = 20;
		float groupWidth = 300;
		float groupHeight = 400;
		float groupX = Screen.width - (groupWidth + 20);
		float groupY = 20;

		foreach (string s in creatureNames) {

			//show traits
			if (GUI.Button (new Rect (buttonX, buttonY, 100, 50), s)) {
				showTraits = true;
				selectedCreature = s;
				traits = getTraits(s);
			}

			buttonY += 50;
		}
		if (showTraits) {
			GUI.BeginGroup (new Rect (groupX, groupY, groupWidth, groupHeight));
			GUI.Box (new Rect (0, 0, groupWidth, groupHeight), "Creature Traits : " + selectedCreature);

			GUI.Label (new Rect (10, 50, 200, 300), traits);

			GUI.Button (new Rect (groupWidth / 2 - 150/2, groupHeight - 50, 150, 40), "Export this Creature");

			GUI.EndGroup ();
		}
	}

	string getTraits(string name){

	
		string filePath = Application.persistentDataPath + "/" + name + ".traits.dat";
		traits = "";

		if (File.Exists (filePath)) {
			
			BinaryFormatter bf = new BinaryFormatter ();
			using (FileStream fs = File.Open (filePath, FileMode.Open)) {
				CreatureData cd = (CreatureData)bf.Deserialize (fs);
				fs.Close ();

				string outputString = "";

				outputString += " Creature Type : " + cd.type;
				outputString += "\n\n Maximum Speed : " + cd.maxSpeed.ToString ("0.0");
				outputString += "\n\n Turn Speed : " + cd.TurnSpeed.ToString ("0.0");
				outputString += "\n\n Stamina : " + cd.stamina.ToString ("0.0");
				outputString += "\n\n Scale of Creature : " + cd.mass.ToString ("0.0");

				Debug.Log (outputString);
				return outputString;
			}
		}

		return " error : missing data";
	}
}
