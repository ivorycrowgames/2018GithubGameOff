using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreamteck.Splines;

public class TrackSegment : SimpleEntity {

    public SplineComputer trackSpline;
    public TrackSegment nextTrackSegment;

    public virtual Vector3 GetPositionForProgress(float progress)
    {
        return trackSpline.EvaluatePosition(progress);
    }

    public virtual Vector3 GetPositionForDistance(float distance)
    {
        return trackSpline.EvaluatePosition(trackSpline.Travel(0, distance, Spline.Direction.Forward));
    }

    public virtual TrackSegment GetNextTrackSegment()
    {
        return nextTrackSegment;
    }
}
