﻿using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class Optimizer : MonoBehaviour {

	//input/output nodes
    private int NUM_INPUTS = 72;
    private int NUM_OUTPUTS = 2;

    public int Trials;
    public float TrialDuration;
    public float StoppingFitness;
	public string creatureName = "temp";
    bool EARunning;
    string popFileSavePath, champFileSavePath, traitsFileSavePath;

    SimpleExperiment experiment;
    static NeatEvolutionAlgorithm<NeatGenome> _ea;

	//creature
    public GameObject Unit;
	private CreatureData cd = new CreatureData();

    Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();
    private DateTime startTime;
    private float timeLeft;
    private float accum;
    private int frames;
    private float updateInterval = 12;

    private uint Generation;
    private double Fitness;

	public Optimizer(){
	}

	public Optimizer(int inputs, int outputs, GameObject unit){
		this.NUM_INPUTS = inputs;
		this.NUM_OUTPUTS = outputs;
		this.Unit = unit;
	}

	// Use this for initialization
	void Start () {
        Utility.DebugLog = true;
        experiment = new SimpleExperiment();
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("experiment.config");
        xmlConfig.LoadXml(textAsset.text);
        experiment.SetOptimizer(this);

        experiment.Initialize("Creature Experiment", xmlConfig.DocumentElement, NUM_INPUTS, NUM_OUTPUTS);

		champFileSavePath = Application.persistentDataPath + string.Format("/{0}.champ.xml", creatureName);
		popFileSavePath = Application.persistentDataPath + string.Format("/{0}.pop.xml", creatureName);   
		traitsFileSavePath = Application.persistentDataPath + string.Format("/{0}.traits.dat", creatureName);

        print(champFileSavePath);
	}

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeLeft <= 0.0)
        {
            var fps = accum / frames;
            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
            //   print("FPS: " + fps);
			if (fps < 10 && Time.timeScale > 1)
            {
                Time.timeScale = Time.timeScale - 1;
                print("Lowering time scale to " + Time.timeScale);
            }
        }
    }

    public void StartEA()
    {        
        Utility.DebugLog = true;
        Utility.Log("Starting Creature experiment");
        // print("Loading: " + popFileLoadPath);
        _ea = experiment.CreateEvolutionAlgorithm(popFileSavePath);
        startTime = DateTime.Now;

        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);

        var evoSpeed = 2;

        Time.timeScale = evoSpeed;       
        _ea.StartContinue();
        EARunning = true;

		//save creature traits;
		print("calling save traits");
		SaveTraits();
    }

    void ea_UpdateEvent(object sender, EventArgs e)
    {
        Utility.Log(string.Format("gen={0:N0} bestFitness={1:N6}",
            _ea.CurrentGeneration, _ea.Statistics._maxFitness));

        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;
    }

    void ea_PauseEvent(object sender, EventArgs e)
    {
        Time.timeScale = 1;
        Utility.Log("Done ea'ing (and neat'ing)");

        XmlWriterSettings _xwSettings = new XmlWriterSettings();
        _xwSettings.Indent = true;

        // Save genomes to xml file.        
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath);
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        using (XmlWriter xw = XmlWriter.Create(popFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, _ea.GenomeList);
        }

        // Also save the best genome
        using (XmlWriter xw = XmlWriter.Create(champFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
        }
        DateTime endTime = DateTime.Now;
        Utility.Log("Total time elapsed: " + (endTime - startTime));

        System.IO.StreamReader stream = new System.IO.StreamReader(popFileSavePath);
      
        EARunning = false;           
    }

    public void StopEA()
    {
        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            _ea.Stop();
        }
    }

    public void Evaluate(IBlackBox box)
    {
		//random position inside the box
		float x = UnityEngine.Random.Range(-40,40);
		float z = UnityEngine.Random.Range (-40, 40);
		Vector3 pos = new Vector3(x, 5, z);

        GameObject obj = Instantiate(Unit, pos, Unit.transform.rotation) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        ControllerMap.Add(box, controller);

        controller.Activate(box);
    }

    public void StopEvaluation(IBlackBox box)
    {
        UnitController ct = ControllerMap[box];

        Destroy(ct.gameObject);
    }

    public void RunBest()
    {
        Time.timeScale = 1;

        NeatGenome genome = null;

        // Try to load the genome from the XML document.
        try
        {
            using (XmlReader xr = XmlReader.Create(champFileSavePath))
                genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)experiment.CreateGenomeFactory())[0];
        }
        catch (Exception e1)
        {
            return;
        }

        // Get a genome decoder that can convert genomes to phenomes.
        var genomeDecoder = experiment.CreateGenomeDecoder();

        // Decode the genome into a phenome (neural network).
        var phenome = genomeDecoder.Decode(genome);

        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        ControllerMap.Add(phenome, controller);

        controller.Activate(phenome);
    }

    public float GetFitness(IBlackBox box)
    {
        if (ControllerMap.ContainsKey(box))
        {
            return ControllerMap[box].GetFitness();
        }
        return 0;
    }

	private void SaveTraits(){
		
		using (FileStream fs = File.Open (traitsFileSavePath, FileMode.OpenOrCreate)) {
			BinaryFormatter bf = new BinaryFormatter ();
			print("Saving Traits of " + Unit.tag);

			CreatureData cd = new CreatureData ();
			CreatureController cc = Unit.GetComponent<CreatureController> ();

			cd.maxSpeed = SelectionMenu.speed * 1.5f;
			cd.Speed = SelectionMenu.speed;
			cd.TurnSpeed = SelectionMenu.turnSpeed;
			cd.stamina = SelectionMenu.stamina;
			cd.curstamina = SelectionMenu.stamina;
			cd.sightLength = cc.sightLength;
			cd.fov = cc.fov;
			cd.predstr = cc.predstr;
			cd.foodstr = cc.foodstr;
			cd.wallstr = cc.wallstr;
			cd.mass = Unit.GetComponent<Rigidbody> ().mass;

			if (SelectionMenu.herb)
				cd.type = "Prey";
			else
				cd.type = "Predator";

			bf.Serialize (fs, cd);
			fs.Close ();
		}
	}

    void OnGUI()
    {
		if(GUI.Button (new Rect (10, 10, 75, 30), "Main Menu")){
			SceneManager.LoadScene ("MainMenu");
		}
        if (GUI.Button(new Rect(10, 40, 100, 40), "Start EA"))
        {
            StartEA();
        }
        if (GUI.Button(new Rect(10, 90, 100, 40), "Stop EA"))
        {
            StopEA();
        }
        if (GUI.Button(new Rect(10, 140, 100, 40), "Run best"))
        {
            RunBest();
        }

        GUI.Button(new Rect(10, Screen.height - 70, 100, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", Generation, Fitness));
    }
}
