using System;
using UnityEngine;

using IvoryCrow.Unity;

[System.Serializable]
public struct CaptionStartData
{
    public AudioClip audioClip;
    public TextAsset caption;
};

public class CaptionStartAction : SimpleGameAction<CaptionStartData> { }
