using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using IvoryCrow.Unity;
using IvoryCrow.Utilities;
using IvoryCrow.Extensions;

public class BardController : SimpleEntity {

    public float playerMinRunSpeed = 4.0f;
    public float playerMaxRunSpeed = 4.0f;
    public float runSpeedRampTime = 2.0f;
    public float jumpMagnitude = 5.0f;
    public float jumpReductionOnKeyup = 0.2f;

    private ValueChangeDetector<bool> mobileIsJumping = new ValueChangeDetector<bool>(false);
    public bool forceMobile = false;
    public float mobileJumpThreshold = 0.2f;
    public Thumbstick mobileThumbstick;
    public Button mobileBumpButton;
    public Button mobileDoorButton;

    public ProxyTrigger bumpTrigger;
    public ProxyTrigger doorTrigger;
    public ProxyTrigger onBumperTouched;
    public ProxyTrigger onCollectNote;

    public CaptionManager captionManager;
    public BardControllerView view;
    public ColliderSensor groundSensor;
    public ColliderSensor bodySensor;
    public Rigidbody2D playerBody;

    private bool hasHadFirstFixedUpdate;

    public LevelManager levelManager;

    public int collectedNotes
    {
        get { return _collectedNotes.Count;  }
        set { }
    }

    public int initialNotes
    {
        get;
        private set;
    }

    private bool isPlayerFrozen = false;

    private List<string> _collectedNotes = new List<string>();

    private bool jumpKeyDown = false;
    private bool jumpKeyUp = false;
    
    private ValueChangeDetector<bool> isOnGround;
    private ValueChangeDetector<int> playerRunScalar;
    private float playerRunTime = 0;

    private const string bumperName = "Bumper";
    private const string noteTag = "Note";

    // Use this for initialization
    protected override void OnStart() {
        base.OnStart();
        GameObjectUtilities.GetComponentIfNull(this, ref view);
        bodySensor.onTriggerEnter += OnBodyTrigger;
        isOnGround = new ValueChangeDetector<bool>(false);
        playerRunScalar = new ValueChangeDetector<int>(0);
        SetupStateChangeListeners();
        SetupControls();
        hasHadFirstFixedUpdate = false;

        AddActionHandler((FreezePlayerActionData data) => isPlayerFrozen = data.isFrozen);
        AddActionHandler<StartCaptionOnceActionData>(HandleOnceCaption);
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlayerFrozen)
        {
            return;
        }

        if (IsOnMobile())
        {
            ProcessAndroidControls();
        }
        else
        {
            ProcessDesktopControls();
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        CheckFirstFixedUpdate();
        UpdateGroundFlag();
        CheckForBumper();
        CheckForDeath();

        if (isPlayerFrozen)
        {
            playerBody.velocity = Vector2.zero;
            playerRunScalar.Value = 0;
            return;
        }

        if (!isOnGround.Value || playerRunScalar.Value == 0)
        {
            playerRunTime = 0;
        }

        Vector2 playerVelocity = playerBody.velocity;
        if (playerRunScalar.Value != 0)
        {
            playerRunTime = (playerRunTime + Time.fixedDeltaTime).Clamp(0, runSpeedRampTime);
            playerVelocity.x = playerRunScalar.Value * playerRunTime.Remap(0, runSpeedRampTime, playerMinRunSpeed, playerMaxRunSpeed);
        }
        else
        {
            playerVelocity.x = 0;
        }

        playerBody.velocity = playerVelocity;

        if (jumpKeyDown)
        {
            JumpPlayer();
            jumpKeyDown = false;
        }

        if (jumpKeyUp)
        {
            if (playerVelocity.y > 0)
            {
                playerVelocity.y -= playerVelocity.y * jumpReductionOnKeyup;
                playerBody.velocity = playerVelocity;
            }

            jumpKeyUp = false;
        }
    }

    private void CheckFirstFixedUpdate()
    {
        if (!hasHadFirstFixedUpdate)
        {
            Vector2 spawnPosition;
            if (levelManager.GetLastCheckpoint(out spawnPosition))
            {
                playerBody.position = spawnPosition;
            }

            levelManager.GetCollectedNotes(out _collectedNotes);
            initialNotes = _collectedNotes.Count;

            foreach(string noteName in _collectedNotes)
            {
                GameObject.Find(noteName)?.SetActive(false);
            }

            onCollectNote?.Fire();
            view?.OnNoteCollected();
            hasHadFirstFixedUpdate = true;
        }
    }

    private void LateUpdate()
    {
    }

    private void OnBodyTrigger(Collider2D collision)
    {
        var go = collision.gameObject;
        string otherGameobjectTag = go.tag;
        if (otherGameobjectTag.Equals(noteTag))
        {
            if (_collectedNotes.Contains(go.name))
            {
                throw new System.Exception("Notes must have unique names DUDE");
            }
            
            levelManager.StoreNote(go.name);
            levelManager.GetCollectedNotes(out _collectedNotes);
            go.SetActive(false);
            onCollectNote?.Fire();
        }
    }

    private bool IsOnMobile()
    {
        return Application.platform == RuntimePlatform.Android || forceMobile;
    }

    private void SetupControls()
    {
        if (IsOnMobile())
        {
            mobileIsJumping.OnValueChanged += (bool wasJumping, bool isJumping) =>
            {
                if (!wasJumping && isJumping)
                {
                    jumpKeyDown = true;
                }

                if (wasJumping && !isJumping)
                {
                    jumpKeyUp = true;
                }
            };

            mobileBumpButton.onClick.AddListener(() => BumpPlayer());
            mobileDoorButton.onClick.AddListener(() => OpenDoors());
        }
    }

    private void ProcessDesktopControls()
    {
        if (Input.GetKey(KeyCode.A))
        {
            playerRunScalar.Value = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerRunScalar.Value = 1;
        }
        else
        {
            playerRunScalar.Value = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround.Value == true)
        {
            jumpKeyDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyUp = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            BumpPlayer();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenDoors();
        }
    }

    private void BumpPlayer()
    {
        bumpTrigger?.Fire();
        view?.OnBump();
    }

    private void OpenDoors()
    {
        doorTrigger?.Fire();
        view?.OnDoor();
    }

    private void ProcessAndroidControls()
    {
        if (mobileThumbstick.X < 0)
        {
            playerRunScalar.Value = -1;
        }
        else if (mobileThumbstick.X > 0)
        {
            playerRunScalar.Value = 1;
        }
        else
        {
            playerRunScalar.Value = 0;
        }

        if (mobileThumbstick.Y >= mobileJumpThreshold && isOnGround.Value == true)
        {
            mobileIsJumping.Value = true;
        }

        if (mobileThumbstick.Y < mobileJumpThreshold)
        {
            mobileIsJumping.Value = false;
        }
    }

    private void JumpPlayer()
    {
        Vector2 playerVelocity = playerBody.velocity;
        playerVelocity.y = jumpMagnitude;
        playerBody.velocity = playerVelocity;
        view?.OnJump();
    }

    private void UpdateGroundFlag()
    {
        var ground = groundSensor.CurrentTouchingEntities().Find((Collider2D col) => !col.isTrigger);
        isOnGround.Value = ground != null;
    }

    private void CheckForBumper()
    {
        var bumper = groundSensor.CurrentTouchingEntities().Find((Collider2D col) => col.gameObject.CompareTag(bumperName));
        if (bumper) {
            Vector2 playerVelocity = playerBody.velocity;
            playerVelocity.y = bumper.gameObject.GetComponent<Bumper>().BumpMagnitude;
            playerBody.velocity = playerVelocity;
            onBumperTouched?.Fire();
            view?.OnBumperTouched();
        }
    }

    private void CheckForDeath()
    {
    }

    private void HandleOnceCaption(StartCaptionOnceActionData data)
    {
        string assetFilename = AssetDatabase.GetAssetPath(data.caption);
        var action = new CaptionStartData();
        action.caption = data.caption;
        action.audioClip = data.audioClip;
        captionManager.HandleAction(action);
    }

    private void SetupStateChangeListeners()
    {
        isOnGround.OnValueChanged += (bool wasOnGround, bool isOnGround) =>
        {
            if (wasOnGround && !isOnGround)
            {
                view?.OnFalling();
            }
            else if (!wasOnGround && isOnGround)
            {
                if (playerRunScalar.Value == 0)
                {
                    view?.OnIdle();
                }
                else
                {
                    view?.OnRunning(playerRunScalar.Value == 1 ? DirectionLR.Right : DirectionLR.Left);
                }
            }
        };

        playerRunScalar.OnValueChanged += (int oldDirection, int newDirection) =>
        {
            if (newDirection != 0)
            {
                Vector3 rotation = transform.rotation.eulerAngles;
                rotation.y = newDirection == -1 ? 180 : 0;
                playerBody.transform.rotation = Quaternion.Euler(rotation);
            }

            if (isOnGround.Value)
            {
                if (newDirection == 0)
                {
                    view?.OnIdle();
                }
                else
                {
                    view?.OnRunning(newDirection == 1 ? DirectionLR.Right : DirectionLR.Left);
                }
            }
        };
    }
}
