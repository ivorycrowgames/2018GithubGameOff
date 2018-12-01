using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteDisplay : MonoBehaviour {

    public Text totalNotes;
    public Text notesThisRun;
    public BardController controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (controller)
        {
            if (totalNotes)
            {
                totalNotes.text = "Total Notes: " + controller.collectedNotes;
            }
            
            if (notesThisRun)
            {
                notesThisRun.text = "Notes this run: " + (controller.collectedNotes - controller.initialNotes);
            }
        }
	}
}
