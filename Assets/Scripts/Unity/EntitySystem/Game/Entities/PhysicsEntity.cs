using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.Tween;
using IvoryCrow.Utilities;
using IvoryCrow.Extensions;

public class PhysicsEntity : BaseEntity
{
    public Rigidbody2D physicsBody;

    private bool isMoving = false;
    private bool finishMoving = false;
    private Vector2 _currentPosition;
    private Vector2 _initialPosition;

    protected override void OnStart()
    {
        if (!physicsBody)
        {
            physicsBody = GetComponent<Rigidbody2D>();
        }

        AddActionHandler<MoveActionData>(handleMoveAction);
        AddActionHandler<SizeActionData>(handleSizeAction);
        physicsBody.isKinematic = true;
        physicsBody.freezeRotation = true;
        isMoving = finishMoving = false;
        _currentPosition = physicsBody.position;
    }

    protected override void OnFixedUpdate()
    {
        if (isMoving || finishMoving)
        {
            physicsBody.MovePosition(_currentPosition);
            if (finishMoving)
            {
                isMoving = finishMoving = false;
            }
        } 
    }

    private void handleMoveAction(MoveActionData actionData)
    {
        if (isMoving)
        {
            return;
        }

        _initialPosition = _currentPosition;
        Vector2 endPoint = _currentPosition + (actionData.Direction.normalized * actionData.Distance);
        float animationDuration = actionData.Distance / actionData.Speed;

        isMoving = true;
        TweenFactory.Tween(this, _currentPosition, endPoint, animationDuration, TweenScaleFunctions.Linear,
        (ITween<Vector2> current) => {
            _currentPosition = current.CurrentValue;
        },
        (ITween<Vector2> final) => {
            _currentPosition = final.CurrentValue;
            if (actionData.ReturnToStart)
            {
                float holdTime = actionData.HoldTime.Clamp(0, float.MaxValue);
                StartCoroutine(handleReturnAfterDelay(holdTime, animationDuration));
            }
            else
            {
                finishMoving = true;
            }
        });
    }

    private IEnumerator handleReturnAfterDelay(float delay, float animationDuration)
    {
        yield return new WaitForSeconds(delay);
        TweenFactory.Tween(this, _currentPosition, _initialPosition, animationDuration, TweenScaleFunctions.Linear,
        (ITween<Vector2> current) => {
            _currentPosition = current.CurrentValue;
        },
        (ITween<Vector2> final) => {
            _currentPosition = final.CurrentValue;
            finishMoving = true;
        });
    }


    private void handleSizeAction(SizeActionData actionData)
    {
        Vector2 finalPosition = transform.position;
        Vector2 finalSize = transform.localScale;
        foreach(MoveSide move in actionData.SidesToMove)
        {
            float distance = move.Distance;
            float quartDist = distance / 4.0f;
            switch (move.Direction)
            {
                case Direction.Left:
                    finalSize.x += distance;
                    finalPosition.x -= quartDist;
                    break;
                case Direction.Right:
                    finalSize.x += distance;
                    finalPosition.x += quartDist;
                    break;
                case Direction.Up:
                    finalSize.y += distance;
                    finalPosition.y += quartDist;
                    break;
                case Direction.Down:
                    finalSize.y += distance;
                    finalPosition.y -= quartDist;
                    break;
            }
        }

        TweenFactory.Tween(this.transform, transform.localScale, finalSize, actionData.Speed, TweenScaleFunctions.Linear,
        (ITween<Vector2> current) => {
            transform.localScale = new Vector3(current.CurrentValue.x, current.CurrentValue.y, 1);
        },
        (ITween<Vector2> final) => {
            transform.localScale = new Vector3(final.CurrentValue.x, final.CurrentValue.y, 1);
        });

        TweenFactory.Tween(this, transform.position, finalPosition, actionData.Speed, TweenScaleFunctions.Linear,
        (ITween<Vector2> current) => {
            transform.position = new Vector3(current.CurrentValue.x, current.CurrentValue.y, 0);
        },
        (ITween<Vector2> final) => {
            transform.position = new Vector3(final.CurrentValue.x, final.CurrentValue.y, 0);
        });
    }
}
