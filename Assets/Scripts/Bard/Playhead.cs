using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Extensions;

[ExecuteInEditMode]
public class Playhead : MonoBehaviour {

    public float time;
    public float distanceOffset = 0;
    public TrackManager manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (manager)
        {
            float trackTime = time.Clamp(0, manager.totalTrackDuration);
            float trackStartX = manager.levelBounds.bounds.min.x;
            float totalTrackSize = manager.levelBounds.bounds.max.x - trackStartX;

            float unitsPerSecond = totalTrackSize / manager.totalTrackDuration;
            Vector3 position = transform.position;
            position.x = trackStartX + (trackTime * unitsPerSecond) - distanceOffset;
            transform.position = position;
        }
	}
}
