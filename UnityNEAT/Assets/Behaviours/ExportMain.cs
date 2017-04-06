using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Xml;

public class ExportMain : MonoBehaviour {


	List<string> fileNames = new List<string>();
	List<string> creatureNames = new List<string>();

	public GameObject prey;
	public GameObject predator;

	string creatureType = "";
	string selectedCreature = "";
	string traits = "";

	bool showTraits = false;
	bool showDelete = false;

	public Texture deleteTexture;

	private CreatureData selectedData;

	// Use this for initialization
	void Start () {
		GetFiles ();
	}


	//get all filenames
	void GetFiles(){

		fileNames.Clear ();
		creatureNames.Clear ();

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

	void OnGUI(){
		float buttonX = 20;
		float buttonY = 50;
		float groupWidth = 300;
		float groupHeight = 400;
		float groupX = Screen.width - (groupWidth + 20);
		float groupY = 20;

		if(GUI.Button (new Rect (10, 10, 75, 30), "Main Menu")){
			SceneManager.LoadScene ("MainMenu");
		}

		foreach (string s in creatureNames) {

			//show traits
			if (GUI.Button (new Rect (buttonX, buttonY, 100, 50), s)) {
				showTraits = true;
				selectedCreature = s;
				traits = getTraits(s);
				previewCreature ();
			}

			buttonY += 50;
		}
		if (showTraits) {
			GUI.BeginGroup (new Rect (groupX, groupY, groupWidth, groupHeight));
			GUI.Box (new Rect (0, 0, groupWidth, groupHeight), "Creature Traits : " + selectedCreature);

			GUI.Label (new Rect (10, 50, 200, 300), traits);

			if(GUI.Button (new Rect (groupWidth - 50, 5, 30, 30), deleteTexture)){
				showDelete = true;
			}
			if (GUI.Button (new Rect (groupWidth / 2 - 150 / 2, groupHeight - 50, 150, 40), "Export this Creature")) {
				//TODO export the creature
				ExportCreature ec = new ExportCreature(selectedCreature,selectedData);
				ec.export ();

			}

			GUI.EndGroup ();
		}
		if (showDelete) {
			GUI.BeginGroup (new Rect (Screen.width / 2 -100, Screen.height / 2 - 100, 200, 200));
			GUI.Box (new Rect (0, 0, 200, 150), "Delete " + selectedCreature + "?");

			if (GUI.Button (new Rect (20, 100, 50, 20), "Yes")) {
				DeleteBehaviour ();
				showDelete = false;
			}

			if (GUI.Button (new Rect (130, 100, 50, 20), "No")) {
				showDelete = false;
			}
			GUI.EndGroup ();
		}
	}

	//TODO use traits in this
	void previewCreature(){
		PreviewExport pe;
		if (creatureType.ToLower () == "prey")
			pe = new PreviewExport (selectedCreature, prey);
		else
			pe = new PreviewExport (selectedCreature, predator);

		pe.LoadCreature ();
	}

	//get name to show
	string getTraits(string name){

		string filePath = Application.persistentDataPath + "/" + name + ".traits.dat";
		traits = "";

		if (File.Exists (filePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			using (FileStream fs = File.Open (filePath, FileMode.Open)) {
				CreatureData cd = (CreatureData)bf.Deserialize (fs);
				selectedData = cd;

				fs.Close ();

				string outputString = "";

				outputString += " Creature Type : " + cd.type;
				creatureType = cd.type;
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

	void DeleteBehaviour(){
		File.Delete (Application.persistentDataPath + "/" + selectedCreature + ".traits.dat");
		File.Delete (Application.persistentDataPath + "/" + selectedCreature + ".champ.xml");
		File.Delete (Application.persistentDataPath + "/" + selectedCreature + ".pop.xml");

		showTraits = false;
		GetFiles ();
	}
}
