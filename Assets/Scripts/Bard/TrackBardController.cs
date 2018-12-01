using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Extensions;
using IvoryCrow.Utilities;

public class TrackBardController : MonoBehaviour {

    SpringJoint2D rabbitJoint;
    public LevelManager levelManager;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D playerBody;
    public int warnCount = 3;
    public float warnFadeTime = 0.25f;
    public float deathGhostAlpha = 0.5f;

    public ProxyTrigger bump;
    public ProxyTrigger door;
    public ProxyTrigger onDied;

    public int notesPerLive = 5;
    public int remainingLives
    {
        get;
        private set;
    }

    private GhostState curGhostState = GhostState.NotGhost;
    private float ghostTimer;
    private int remainingWarnings;

    private string bumperTag = "Bumper";
    private string deathTag = "Death";

    private enum GhostState
    {
        NotGhost,
        Ghosted,
        GhostWarning
    }

	// Use this for initialization
	void Start () {
        GameObjectUtilities.GetComponentIfNull(this, ref playerBody);
        GameObjectUtilities.GetComponentIfNull(this, ref spriteRenderer);

        List<string> collectedNotes;
        if (levelManager.GetCollectedNotes(out collectedNotes))
        {
            remainingLives = collectedNotes.Count / notesPerLive;
        }
        else
        {
            remainingLives = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        HandleGhostState(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            bump.Fire();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            door.Fire();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        string gameObjectTag = collider.gameObject.tag;
        if (gameObjectTag.Equals(bumperTag))
        {
            float magnitude = collider.gameObject.GetComponentInChildren<Bumper>().BumpMagnitude;
            Vector2 playerVelocity = playerBody.velocity;
            playerVelocity.y = 1 * magnitude;
            playerBody.velocity = playerVelocity;
        }
        else if (gameObjectTag.Equals(deathTag) && curGhostState == GhostState.NotGhost)
        {
            float ghostTime = collider.gameObject.GetComponentInChildren<Death>().GhostTime;
            float minGhostTime = warnCount * warnFadeTime;
            ghostTime = Mathf.Max(minGhostTime, ghostTime);
            HandleDeathHit(ghostTime);
        }
    }

    private void HandleGhostState(float dt)
    {
        ghostTimer -= dt;
        if (ghostTimer <= 0)
        {
            if (curGhostState == GhostState.Ghosted)
            {
                curGhostState = GhostState.GhostWarning;
                ghostTimer = warnFadeTime;
            }
            else if (curGhostState == GhostState.GhostWarning)
            {
                --remainingWarnings;
                if (remainingWarnings == 0)
                {
                    curGhostState = GhostState.NotGhost;
                    SetSpriteAlpha(1);
                    return;
                }
            }
        }
        else
        {
            if (curGhostState == GhostState.Ghosted)
            {
                SetSpriteAlpha(deathGhostAlpha);
            }
            else if (curGhostState == GhostState.GhostWarning)
            {
                float halfGhostWarning = warnFadeTime / 2;
                if (ghostTimer >= halfGhostWarning)
                {
                    float a = ghostTimer.Remap(warnFadeTime, halfGhostWarning, deathGhostAlpha, 1);
                    SetSpriteAlpha(a);
                }
                else
                {
                    // If it's the last warning just leave it solid, otherwise back to ghost
                    if (remainingWarnings > 1)
                    {
                        float a = ghostTimer.Remap(halfGhostWarning, 0, 1, deathGhostAlpha);
                        SetSpriteAlpha(a);
                    }
                }
            }
            else
            {
                SetSpriteAlpha(1);
            }
        }
    }

    private void HandleDeathHit(float ghostTime)
    {
        remainingLives--;
        if (remainingLives < 0)
        {
            onDied.Fire();
            rabbitJoint.breakForce = 0;
            rabbitJoint.breakTorque = 0;
        }
        else
        {
            remainingWarnings = warnCount;
            ghostTimer = ghostTime - (warnCount * warnFadeTime);
            curGhostState = ghostTimer <= 0 ? GhostState.GhostWarning : GhostState.Ghosted;
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
