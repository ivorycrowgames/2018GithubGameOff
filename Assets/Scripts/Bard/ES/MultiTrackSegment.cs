using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreamteck.Splines;

public class MultiTrackSegment : TrackSegment {

    public TrackSegment alternateNextTrackSegment;

    private bool isOnAlternateSection = false;

    protected override void OnStart()
    {
        base.OnStart();
        AddActionHandler((ChangeTrackActionData data) => isOnAlternateSection = data.useAlternateTrack);
    }

    public override TrackSegment GetNextTrackSegment()
    {
        if (isOnAlternateSection)
        {
            return alternateNextTrackSegment;
        }

        return base.GetNextTrackSegment();
    }
}
