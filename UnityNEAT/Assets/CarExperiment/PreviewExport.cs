using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using System;
using System.Xml;

public class PreviewExport : MonoBehaviour {

	//SimpleExperiment experiment;
	public GameObject Creature;
	string bestPath;
	string traitPath;

	public PreviewExport(string name, GameObject Creature){
		this.Creature = Creature;
		//experiment = new SimpleExperiment ();
		bestPath = Application.persistentDataPath + "/" + name + ".champ.xml";
		traitPath = Application.persistentDataPath + "/" + name + ".traits.dat";
	}

	public void LoadCreature(){

		NeatGenome genome = null;

		//allways true because we are always using acyclic activiation
		NeatGenomeParameters ngp = new NeatGenomeParameters();
		ngp.FeedforwardOnly = true;

		//temp, need to make more malleable
		int inputs = 72;
		int outputs = 2;

		var ngf = new NeatGenomeFactory (inputs, outputs, ngp);
		// Try to load the genome from the XML document.
		using (XmlReader xr = XmlReader.Create(bestPath))
			genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)ngf)[0];
		
		// Get a genome decoder that can convert genomes to phenomes.
		var genomeDecoder = new NeatGenomeDecoder(NetworkActivationScheme.CreateAcyclicScheme());

		// Decode the genome into a phenome (neural network).
		var phenome = genomeDecoder.Decode(genome);

		GameObject obj = Instantiate(Creature, Creature.transform.position, Creature.transform.rotation) as GameObject;
		UnitController controller = obj.GetComponent<UnitController>();
		//send the network as a black box to the 
		controller.Activate(phenome);
	}
}
