using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.Tween;

public class ZoomEffect
{
    public float ZoomInAmount;
    public float ZoomOutAmount;

    public float AnimationTime;
    public int RippleCount;
}

public class CameraInfo : MonoBehaviour {

    public bool OriginLowerLeft = true;

    private Vector2 lowerLeftCorner;
    public Vector2 LowerLeftCorner
    {
        get
        {
            return lowerLeftCorner;
        }
        private set { }
    }

    private Vector2 lowerRightCorner;
    public Vector2 LowerRightCorner
    {
        get
        {
            return lowerRightCorner;
        }
        private set { }
    }

    private Vector2 upperRightCorner;
    public Vector2 UpperRightCorner
    {
        get
        {
            return upperRightCorner;
        }
        private set { }
    }

    private Vector2 upperLeftCorner;
    public Vector2 UpperLeftCorner
    {
        get
        {
            return upperLeftCorner;
        }
        private set { }
    }

    public Vector2 Center
    {
        get
        {
            return camera.transform.position;
        }
        set
        {
            camera.transform.position = value;
        }
    }

    private float verticalSize;
    public float VerticalSize
    {
        get
        {
            return verticalSize;
        }
        private set { }
    }

    private float horizontalSize;
    public float HorizontalSize
    {
        get
        {
            return horizontalSize;
        }
        private set { }
    }


    public Camera camera;
    private float defaultOrthoSize;
    private ZoomEffect currentZoomEffect = null;
    private int initialRippleCount = 0;

    // Use this for initialization
    void Awake() {
        camera = GetComponent<Camera>();

        if (OriginLowerLeft)
        {
            updateScreenSize();
            camera.transform.position = new Vector3(horizontalSize / 2, verticalSize / 2, camera.transform.position.z);
        }

        defaultOrthoSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update() {
        updateScreenSize();
        if (OriginLowerLeft)
        {
            camera.transform.position = new Vector3(horizontalSize / 2, verticalSize / 2, camera.transform.position.z);
        }
        computeCorners();
    }

    public void StartZoomEffect(ZoomEffect effect)
    {
        if (currentZoomEffect != null)
        {
            TweenFactory.RemoveTweenKey(currentZoomEffect, TweenStopBehavior.Complete);
        }

        if (effect.RippleCount > 0)
        {
            currentZoomEffect = effect;
            initialRippleCount = effect.RippleCount;
            playOneRipple();
        }
    }

    private void onRippleEnded()
    {
        if (currentZoomEffect != null)
        {
            --currentZoomEffect.RippleCount;
            if (currentZoomEffect.RippleCount == 0)
            {
                currentZoomEffect = null;
                initialRippleCount = 0;
                return;
            }

            playOneRipple();
        }

    }

    private void playOneRipple()
    {
        float rippleFactor = 1.0f / (float) ((initialRippleCount - currentZoomEffect.RippleCount) + 1);
        TweenFactory.Tween(
            currentZoomEffect,
            camera.orthographicSize,
            defaultOrthoSize - (currentZoomEffect.ZoomInAmount * rippleFactor),
            currentZoomEffect.AnimationTime * rippleFactor,
            TweenScaleFunctions.SineEaseInOut,
            updateCameraZoom,
            (ITween<float> c) =>
            {
                if (currentZoomEffect == null)
                {
                    camera.orthographicSize = defaultOrthoSize;
                }


                TweenFactory.Tween(
                    currentZoomEffect,
                    camera.orthographicSize,
                    defaultOrthoSize,
                    (currentZoomEffect.AnimationTime) * rippleFactor,
                    TweenScaleFunctions.SineEaseInOut,
                    updateCameraZoom,
                    (ITween<float> c1) =>
                    {
                        camera.orthographicSize = defaultOrthoSize;
                        onRippleEnded();
                    });
    });
    }

    private void updateCameraZoom(ITween<float> tween)
    {
        camera.orthographicSize = tween.CurrentValue;
    }

    private void updateScreenSize()
    {
        verticalSize = camera.orthographicSize * 2.0f;
        horizontalSize = verticalSize * camera.aspect;
    }

    private void computeCorners()
    {
        Vector3 cameraPosition = camera.transform.position;
        float halfW = horizontalSize / 2;
        float halfH = verticalSize / 2;
  
        lowerLeftCorner = new Vector2(cameraPosition.x - halfW, cameraPosition.y - halfW);
        lowerRightCorner = new Vector2(cameraPosition.x + halfW, cameraPosition.y - halfW);
        upperLeftCorner = new Vector2(cameraPosition.x - halfW, cameraPosition.y + halfW);
        upperRightCorner = new Vector2(cameraPosition.x + halfW, cameraPosition.y + halfW);
    }
}
