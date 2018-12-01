using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby;
using IvoryCrow.Extensions;

public class CameraEntity : SimpleEntity {

    public Camera controlledCamera;

    public GameObject followedEntity;

    private float totalZoomTransistion;
    private float remainingZoomTransition;
    private float targetZoom;

    private bool isInTransitionToTarget = false;
    private GameObject targetPosition;
    private Vector2 startPosition;
    private float totalTransitionTime;
    private float remainingMoveTransitionTime;

    private bool isRunningShake = false;
    CameraShakeActionData currentShake;

    private int iterationsRemaining;

    private float iterationDuration;
    private float iterationRemainingTime;

    private float iterationMagnitude;
    private float magnitudeSteps;

    private Vector2 cameraOffsetVector;

    // Use this for initialization
    protected override void OnStart () {
        base.OnStart();
        GameObjectUtilities.GetComponentIfNull(this, ref controlledCamera);
        AddActionHandler<CameraShakeActionData>(HandleCameraShake);
        AddActionHandler<CameraFollowActionData>(HandleCameraFollow);
        AddActionHandler<CameraUnfollowActionData>(HandleCameraUnfollow);
        AddActionHandler((CameraLookActionData data) => SetTarget(data.lookTarget, data.transitionTime));
        AddActionHandler<CameraZoomActionData>(HandleCameraZoom);
        targetZoom = controlledCamera.orthographicSize;
    }

    // Update is called once per frame
    protected override void OnFixedUpdate() {
        base.OnFixedUpdate();
	}

    public void Update()
    {
        Vector2 cameraShakeOffset = GetCameraShakeOffset(Time.deltaTime);
        Vector3 targetPosition = GetTargetPosition(Time.deltaTime);

        Vector3 camPosition = controlledCamera.transform.position;
        camPosition.Set(targetPosition.x + cameraShakeOffset.x, targetPosition.y + cameraShakeOffset.y, camPosition.z);
        controlledCamera.transform.position = camPosition;

        if (remainingZoomTransition > 0)
        {
            float newOrthoSize = remainingZoomTransition.Remap(totalZoomTransistion, 0, controlledCamera.orthographicSize, targetZoom);
            controlledCamera.orthographicSize = newOrthoSize;
            remainingZoomTransition -= Time.deltaTime;
        }
        else
        {
            controlledCamera.orthographicSize = targetZoom;
        }
    }

    private void ResetTargetTransition()
    {
        targetPosition = null;
        isInTransitionToTarget = false;
        remainingMoveTransitionTime = 0;
    }

    private void SetTarget(GameObject target, float transitionTime)
    {
        if (isInTransitionToTarget)
        {
            Debug.LogError("Cannot process follow action as one is in progress.");
            return;
        }

        targetPosition = target;
        totalTransitionTime = remainingMoveTransitionTime = transitionTime;
        startPosition = controlledCamera.transform.position;
        isInTransitionToTarget = true;
    }

    private void HandleCameraFollow(CameraFollowActionData data)
    {
        followedEntity = data.followEntity;
        SetTarget(data.followEntity, data.transitionTime);
    }

    private void HandleCameraUnfollow(CameraUnfollowActionData data)
    {
        followedEntity = null;
        ResetTargetTransition();
    }

    private void HandleCameraShake(CameraShakeActionData data)
    {
        if (isRunningShake)
        {
            Debug.LogError("Cannot process shake action as one is in progress.");
            return;
        }

        currentShake = data;

        float totalDuration = data.duration;
        magnitudeSteps = currentShake.magnitude / currentShake.shakeCount;

        iterationDuration = totalDuration / currentShake.shakeCount;
        iterationRemainingTime = iterationDuration;
 
        iterationMagnitude = data.magnitude;
        iterationsRemaining = data.shakeCount;


        cameraOffsetVector = Random.insideUnitCircle.normalized;
        isRunningShake = true;
    }

    private void HandleCameraZoom(CameraZoomActionData data)
    {
        totalZoomTransistion = remainingZoomTransition = data.transitionTime;
        targetZoom = data.orthographicSize;
    }

    private void AdvanceCameraIteration()
    {
        --iterationsRemaining;
        if (iterationsRemaining == 0)
        {
            isRunningShake = false;
            return;
        }

        iterationMagnitude -= magnitudeSteps;
        iterationRemainingTime = iterationDuration;
        cameraOffsetVector = Random.insideUnitCircle.normalized;
    }

    private Vector2 GetTargetPosition(float dt)
    {
        Vector2 resultPosition = controlledCamera.transform.position;
        if (isInTransitionToTarget)
        {
            remainingMoveTransitionTime -= dt;
            if (remainingMoveTransitionTime <= 0 || remainingMoveTransitionTime.IsZero())
            {
                resultPosition = targetPosition.transform.position;
                ResetTargetTransition();
            }
            else
            {
                Vector2 targetPos = targetPosition.transform.position;
                Vector2 cameraToTarget = targetPos - startPosition;
                float totalDistance = cameraToTarget.magnitude;
                cameraToTarget.Normalize();
                float progress = remainingMoveTransitionTime.Remap(totalTransitionTime, 0, 0, 1);
                cameraToTarget *= (totalDistance * progress);
                resultPosition = startPosition + cameraToTarget;
            }
        }
        else
        {
            if (followedEntity)
            {
                resultPosition = followedEntity.transform.position;
            }
        }

        return resultPosition;
    }

    private Vector2 GetCameraShakeOffset(float dt)
    {
        if (!isRunningShake)
        {
            return Vector2.zero;
        }

        iterationRemainingTime -= dt;
        if (iterationRemainingTime <= 0)
        {
            AdvanceCameraIteration();
        }

        float halfTimeframe = iterationDuration / 2f;
        float currentDistanceFromCamera = iterationRemainingTime.Remap(iterationDuration, halfTimeframe, 0, 1);
        if (iterationRemainingTime < halfTimeframe)
        {
            currentDistanceFromCamera = iterationRemainingTime.Remap(halfTimeframe, 0, 1, 0);
        }

        return  cameraOffsetVector * (currentDistanceFromCamera * iterationMagnitude);
    }
        
}
