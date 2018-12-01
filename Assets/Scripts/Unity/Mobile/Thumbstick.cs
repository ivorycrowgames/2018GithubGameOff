using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Extensions;
using IvoryCrow.Utilities;

public enum ThumbstickEnum
{
    X,
    XY
}

public class Thumbstick : MonoBehaviour {

    public bool IsControlled { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }
    public Vector2 Direction
    {
        get
        {
            return new Vector2(X, Y).normalized;
        }
        private set { }
    }

    public delegate void ControlChanged(Thumbstick stick);
    public ControlChanged OnControlStarted;
    public ControlChanged OnControlEnded;

    public RectTransform StickGraphic;
    public ThumbstickEnum ThumbstickType;

    public int MaxVisualDistance = 1;
    public int MaxControlDistance = 1;
    public int ScreenWidthBaseline = 100;

    public int DeadZone = 0;

    private ValueChangeDetector<bool> isControlledChangeDetector = new ValueChangeDetector<bool>(false);
    private const int StateChangeBufferSize = 3;
    private int currentTouchId = -1;
    private Vector2 initialPosition = new Vector2();

    // Use this for initialization
    void Start () {
        initialPosition = new Vector2(StickGraphic.position.x, StickGraphic.position.y);
        IsControlled = isControlledChangeDetector.Value;
        isControlledChangeDetector.OnValueChanged += (bool wasControlled, bool controlled) =>
        {
            IsControlled = controlled;
            if (controlled)
            {
                if (OnControlStarted != null)
                {
                    OnControlStarted(this);
                }
            }
            else
            {
                if (OnControlEnded != null)
                {
                    OnControlEnded(this);
                }
            }
        };
    }
	
	// Update is called once per frame
	void Update () {
        // Check android touch if we are on android, otherwise default to mouse
        if (Application.platform == RuntimePlatform.Android)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    ResetState(touch.fingerId);
                }
                else
                {
                    HandleScreenInteraction(touch.position, touch.fingerId);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                HandleScreenInteraction(Input.mousePosition, -1);
            }
            else
            {
                ResetState(-1);
            }
        }
    }

    private int ScaleValueForScreen(int value)
    {
        float scaledValue = (float)value * ((float)Screen.width / (float)ScreenWidthBaseline);
        return (int) scaledValue;
    }

    private bool IsInControlBounds(Vector2 screenPosition)
    {
        Vector2 currentOffset = screenPosition - initialPosition;
        if (ThumbstickType != ThumbstickEnum.XY)
        {
            currentOffset.y = 0;
        }

        return currentOffset.magnitude < ScaleValueForScreen(MaxControlDistance);
    }

    private void HandleScreenInteraction(Vector2 screenPosition, int touchId)
    {
        bool currentlyControlled = IsInControlBounds(screenPosition);
        if (currentlyControlled)
        {
            HandleControlInteraction(screenPosition, touchId);
        }
        else
        {
            ResetState(touchId);
        }
    }

    private void HandleControlInteraction(Vector2 screenPosition, int touchId)
    {
        if (currentTouchId != touchId && isControlledChangeDetector.Value)
        {
            return;
        }

        float scaledMaxControl = ScaleValueForScreen(MaxControlDistance);

        Vector2 currentOffset = screenPosition - initialPosition;
        if (Mathf.Abs(currentOffset.x) < ScaleValueForScreen(DeadZone))
        {
            currentOffset.x = 0;
        }

        if (ThumbstickType == ThumbstickEnum.XY && Mathf.Abs(currentOffset.y) > ScaleValueForScreen(DeadZone))
        {
            Y = currentOffset.y.Remap(-scaledMaxControl, scaledMaxControl, -1, 1);
        }
        else
        {
            Y = currentOffset.y = 0;
        }

        X = currentOffset.x.Remap(-scaledMaxControl, scaledMaxControl, -1, 1);

        float visualDistanceMagnitude = currentOffset.magnitude.Clamp(0, ScaleValueForScreen(MaxVisualDistance));
        Vector2 imagePosition = currentOffset.normalized * visualDistanceMagnitude;
        StickGraphic.position = new Vector2(initialPosition.x + imagePosition.x, initialPosition.y + imagePosition.y);

        currentTouchId = touchId;
        isControlledChangeDetector.Value = true;
    }

    private void ResetState(int touchId)
    {
        if (currentTouchId != touchId)
        {
            return;
        }

        currentTouchId = -1;
        isControlledChangeDetector.Value = false;
        X = Y = 0;
        StickGraphic.position = new Vector2(initialPosition.x, initialPosition.y);
    }
}
