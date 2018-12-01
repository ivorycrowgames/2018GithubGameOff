using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Utilities;

public class BardControllerView : MonoBehaviour {

    public bool debug = false;
    public Animator playerAnimator;

    public void OnIdle()
    {
        logStateIfDebug("OnIdle");
        ResetStates();
        playerAnimator.SetBool("Jumped", false);
        playerAnimator.SetBool("IsIdle", true);
    }

    public void OnFalling()
    {
        logStateIfDebug("OnFalling");
        ResetStates();
        playerAnimator.SetBool("IsFalling", true);
    }

    public void OnRunning(DirectionLR direction)
    {
        logStateIfDebug("OnRunning " + direction.ToString());
        ResetStates();
        playerAnimator.SetBool("Jumped", false);
        playerAnimator.SetBool("IsRunning", true);
    }

    public void OnJump()
    {
        logStateIfDebug("OnJump");
        ResetStates();
        playerAnimator.SetBool("Jumped",true);
    }

    public void OnDeath()
    {
        logStateIfDebug("OnDeath");
    }

    public void OnGenericAnimation(string animation)
    {
        logStateIfDebug("OnGenericAnimation " + animation);
    }

    public void OnBumperTouched()
    {
        logStateIfDebug("OnBumperTouched");
        ResetStates();
        playerAnimator.SetBool("Jumped", true);
    }

    public void OnNoteCollected()
    {
        logStateIfDebug("OnNoteCollected");
        playerAnimator.SetTrigger("Collected");
    }

    public void OnBump()
    {
        logStateIfDebug("OnBump");
        playerAnimator.SetTrigger("Bumped");
    }

    public void OnDoor()
    {
        logStateIfDebug("OnDoor");
        playerAnimator.SetTrigger("Doored");
    }

    public void logStateIfDebug(string state)
    {
        if (debug)
        {
            Debug.Log(state);
        }
    }

    public void ResetStates()
    {
        playerAnimator.SetBool("IsIdle", false);
        playerAnimator.SetBool("IsRunning", false);
        playerAnimator.SetBool("IsFalling", false);
        
    }
}
