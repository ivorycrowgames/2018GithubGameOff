using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreamteck.Splines;
using IvoryCrow.Extensions;

[System.Serializable]
public class TrackManagerDebug
{
    public bool EnableDebugging;
    public GameObject mouseIndicator;
}

public class TrackManager : SimpleEntity
{

    public TrackSegment initialTrackSegment;
    public Rigidbody2D rabbit;
    public BoxCollider2D levelBounds;
    public float totalTrackDuration;
    public AudioSource musicTrack;

    public TrackManagerDebug debug;

    private bool isRunningTrack = false;
    private float currentPositionInBounds;
    private float unitsPerSecond;
    private float totalTrackLength;

    private const float minimumAcceptableSplineDistance = 0.01f;
    private const int maxSplineIterations = 16;

    private Vector3 _rabbitPos;

    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        GameObjectUtilities.GetComponentIfNull(this, ref musicTrack);
        AddActionHandler((StartTrackData data) => StartTrackRunner());
        AddActionHandler((StopTrackData data) => StopTrackRunner());
    }

    private void Update()
    {
        if (debug.EnableDebugging)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPosPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                debug.mouseIndicator.transform.position = new Vector3(worldPosPoint.x, worldPosPoint.y, 0);
            }
        }
    }

    TrackSegment GetTrackSectionByXPosition(float xPosition)
    {
        TrackSegment currentTrack = initialTrackSegment;
        while (currentTrack)
        {
            Vector3 leftBound = currentTrack.GetPositionForProgress(0);
            Vector3 rightBound = currentTrack.GetPositionForProgress(1);
            if (xPosition >= leftBound.x && xPosition <= rightBound.x)
            {
                break;
            }

            currentTrack = currentTrack.GetNextTrackSegment();
        }

        return currentTrack;
    }

    Vector3 GetPositionOnSplineWithXValue(TrackSegment track, float xValue)
    {
        int maxIterations = maxSplineIterations;
        float min = 0;
        float max = 1;
        Vector3 splineWorldPos = track.GetPositionForProgress((max - min) / 2);
        while (maxIterations > 0 && Mathf.Abs(xValue - splineWorldPos.x) > minimumAcceptableSplineDistance)
        {
            float halfRange = (max - min) / 2;
            if (splineWorldPos.x < xValue)
            {
                min += halfRange;
            }
            else
            {
                max -= halfRange;
            }

            splineWorldPos = track.GetPositionForProgress(min + ((max - min) / 2));
            --maxIterations;
        }

        return splineWorldPos;
    }

    // Update is called once per frame
    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (debug.EnableDebugging)
        {
            rabbit.MovePosition(_rabbitPos);
        }

        if (!isRunningTrack)
        {
            return;
        }


        currentPositionInBounds += (Time.fixedDeltaTime * unitsPerSecond);
        if (currentPositionInBounds > levelBounds.bounds.max.x)
        {
            isRunningTrack = false;
            return;
        }

        TrackSegment currentSegment = GetTrackSectionByXPosition(currentPositionInBounds);
        if (currentSegment)
        {
            Vector3 rabbitPosition = GetPositionOnSplineWithXValue(currentSegment, currentPositionInBounds);
            rabbit.MovePosition(rabbitPosition);
        }
    }

    public void StartTrackRunner()
    {
        CalculateUnitsPerSecond();
        currentPositionInBounds = levelBounds.bounds.min.x;
        musicTrack.Play();
        isRunningTrack = true;
    }

    public void StopTrackRunner()
    {
        musicTrack.Stop();
        isRunningTrack = false;
    }

    private void CalculateUnitsPerSecond()
    {
        totalTrackLength = levelBounds.bounds.max.x - levelBounds.bounds.min.x;
        unitsPerSecond = totalTrackLength / totalTrackDuration;
    }
}
