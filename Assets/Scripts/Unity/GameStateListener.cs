using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IvoryCrow.Unity;

public interface GameStateListener {
    void OnStateStarted(GameState state);
    void OnStateEnded(GameState state);
}
