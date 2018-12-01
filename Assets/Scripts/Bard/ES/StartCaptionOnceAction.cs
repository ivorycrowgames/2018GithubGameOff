using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartCaptionOnceActionData
{
    public AudioClip audioClip;
    public TextAsset caption;
};

public class StartCaptionOnceAction : SimpleGameAction<StartCaptionOnceActionData> { }
