using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    public float waveTimescale = 1f;
    public float waveMagnitude = 1f;

    private const float angleMax = Mathf.PI * 2;
    private float _currentAngle;
    private Vector2 _initialPosition;

	// Use this for initialization
	void Start () {
        _initialPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        _currentAngle += (waveTimescale * Time.deltaTime);
        if (_currentAngle > angleMax)
        {
            _currentAngle -= angleMax;
        }

        float magnitude = Mathf.Sin(_currentAngle) * waveMagnitude;
        transform.position = _initialPosition + (Vector2.up * magnitude);
	}
}
