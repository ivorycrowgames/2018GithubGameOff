using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour {

    public TrackBardController controller;
    public Text textOutput;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (controller && textOutput)
        {
            if (controller.remainingLives >= 0)
            {
                textOutput.text = "x" + controller.remainingLives;
            }
        }
	}
}
