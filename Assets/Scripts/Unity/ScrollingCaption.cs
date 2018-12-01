using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using IvoryCrow.Extensions;

public class ScrollingCaption : MonoBehaviour {

    public AudioSource AudioClip;
    public ScrollRect scrollRect;

    public float scrollSpeed = 20.0f;
    private float _remainingScrollTime = 0;

    // Use this for initialization
    void Start () {
        _remainingScrollTime = scrollSpeed;
        AudioClip.Play();
    }
	
	// Update is called once per frame
	void Update () {
        _remainingScrollTime -= Time.deltaTime;
        scrollRect.verticalNormalizedPosition = _remainingScrollTime.Remap(scrollSpeed, 0, 1.0f, 0.0f);
    }
}
