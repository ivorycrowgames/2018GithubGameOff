using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 360f;

	// Update is called once per frame
	void Update () {
        float angle = Time.deltaTime * speed;
        transform.Rotate(new Vector3(0, 0, -angle));
	}
}
