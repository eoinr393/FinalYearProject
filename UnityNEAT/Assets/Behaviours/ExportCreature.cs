using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;

/// <summary>
/// 
/// Script for exporting both the selected creature, which includes
/// the genome XML file, the creature controller with the creature traits,
/// as well as the sharpNEAT files necassary for running these files in a 
/// seperate Unity3D project
/// 
/// --Eoin Raeside 04/2017
/// </summary>
public class ExportCreature : MonoBehaviour {

	private TextAsset skele;
	private CreatureData cd;
	private string cn = "";
	private string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\EMBU";
	private ControllerSkeleton cs = new ControllerSkeleton();

	public ExportCreature(string cn, CreatureData cd){
		this.cd = cd;
		this.cn = cn;
		skele = (TextAsset)Resources.Load ("Skeleton");

		Debug.Log ("Directory path = " + path);
	}


	public void export(){

		//Create directory
		if (!Directory.Exists (path)) {
			try{
				System.IO.Directory.CreateDirectory(path);
	
				copySharpNEAT(UnityEngine.Application.dataPath + "\\SharpNEAT", path + "\\SharpNEAT");
			}
			catch{
				Debug.Log ("error creating directory");
				return;
			}
		}

		//write file
		if (skele != null) {
			//if there is a file already there, then delete it so that it can be updated
			if (File.Exists (path + "\\" + cn + ".cs")) {
				//File.Delete (path + "\\" + cn + ".cs");
			}

			//change values of the controller and save file
			using (StreamWriter file = new StreamWriter (path + "\\" + cn + ".cs")) {
				foreach (string s in Regex.Split(skele.text, "\n|\r|\r\n")) {
					
					string line = s;
					if (line.Contains ("private float maxSpeed")) {
						line += " " + cd.maxSpeed + ";";
					}
					if (line.Contains ("private float Speed")) {
						line += " " + cd.Speed + ";";
					}
					if (line.Contains ("private float TurnSpeed")) {
						line += " " + cd.TurnSpeed + ";";
					}
					if (line.Contains ("private float stamina")) {
						line += " " + cd.stamina + ";";
					}
					if (line.Contains ("private float curstamina")) {
						line += " " + cd.curstamina + ";";
					}
					if (line.Contains ("private float sightlenght")) {
						line += " " + cd.sightLength + ";";
					}
					if (line.Contains ("private float fov")) {
						line += " " + cd.fov + ";";
					}

					file.WriteLine (line);
				}
			}

			//save the xml
			try{
				string champPath = Application.persistentDataPath + "/" + cn + ".champ.xml";
				string champNewPath = path + "\\" + cn + ".champ.xml";

				XmlDocument champ = new XmlDocument ();
				champ.Load (champPath);
				champ.Save (champNewPath);
			}
			catch{
				Debug.Log ("error copying XML champ file");
				return;
			}
		} else
			Debug.Log ("skele is null");
	}

	//class for getting all of the files from the sharpNEAT folder and copying them to the 
	//right destination
	void copySharpNEAT(string source, string dest){
		DirectoryInfo dir = new DirectoryInfo(source);
		DirectoryInfo[] dirs = dir.GetDirectories();
		//create new folder
		if(!File.Exists(dest)){
			System.IO.Directory.CreateDirectory(dest);
		}
		//get files from directory and copy the files
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			string temppath = Path.Combine(dest, file.Name);
			file.CopyTo(temppath, false);
		}
		//get directorys and copy directories
		foreach (DirectoryInfo subs in dirs)
		{
			string newDest = Path.Combine(dest, subs.Name);
			//Debug.Log ("sub: "+ subs.FullName + " newdest: " + newDest);
			copySharpNEAT(subs.FullName, newDest);
		}

	}

}
