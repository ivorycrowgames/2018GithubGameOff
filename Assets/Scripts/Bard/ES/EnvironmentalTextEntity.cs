using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DigitalRuby;
using IvoryCrow.Extensions;

public class EnvironmentalTextEntity : SimpleEntity {

    public Image background;
    public float backgroundMaxAlpha = 1;

    public Text text;
    public float textMaxAlpha = 1;

    public float fadeInTime;
    public float fadeOutTime;

    private TextState curTextState = TextState.Hidden;
    private float remainingFadeTime;

    private enum TextState
    {
        FadeIn,
        Showing,
        FadeOut,
        Hidden
    }

    // Use this for initialization
    protected override void OnStart () {
        base.OnStart();
        AddActionHandler<ShowEnvTextActionData>(HandleShowText);
        AddActionHandler<HideEnvTextActionData>(HandleHideText);
    }

    // Update is called once per frame
    protected override void OnFixedUpdate() {
        base.OnFixedUpdate();
	}

    public void Update()
    {
        if (curTextState == TextState.Hidden)
        {
            text.text = "";
            SetImageAlpha(0);
            SetTextAlpha(0);
        }
        else if (curTextState == TextState.FadeIn || curTextState == TextState.FadeOut)
        {
            remainingFadeTime -= Time.deltaTime;
            if (remainingFadeTime <= 0)
            {
                curTextState = (curTextState == TextState.FadeIn) ? TextState.Showing : TextState.Hidden;
            }
            else
            {
                // Fade the background
                float sourceAlpha = (curTextState == TextState.FadeIn) ? 0 : backgroundMaxAlpha;
                float targetAlpha = (curTextState == TextState.FadeIn) ? backgroundMaxAlpha : 0;
                SetImageAlpha(remainingFadeTime.Remap(fadeInTime, 0, sourceAlpha, targetAlpha));

                // Fade the text
                sourceAlpha = (curTextState == TextState.FadeIn) ? 0 : textMaxAlpha;
                targetAlpha = (curTextState == TextState.FadeIn) ? textMaxAlpha : 0;
                SetTextAlpha(remainingFadeTime.Remap(fadeInTime, 0, sourceAlpha, targetAlpha));
            }
        }
        else
        {
            SetImageAlpha(backgroundMaxAlpha);
            SetTextAlpha(textMaxAlpha);
        }
    }

    private void HandleShowText(ShowEnvTextActionData data)
    {
        curTextState = TextState.FadeIn;
        remainingFadeTime = fadeInTime;
        text.text = data.Text;
    }

    private void HandleHideText(HideEnvTextActionData data)
    {
        if (curTextState != TextState.FadeOut || curTextState != TextState.Hidden)
        {
            curTextState = TextState.FadeOut;
            remainingFadeTime = fadeOutTime;
        }
    }

    private void SetImageAlpha(float a)
    {
        if (background)
        {
            var color = background.color;
            color.a = a;
            background.color = color;
        }
    }

    private void SetTextAlpha(float a)
    {
        if (text)
        {
            var color = text.color;
            color.a = a;
            text.color = color;
        }
    }
}
