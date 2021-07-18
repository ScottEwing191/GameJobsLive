using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager> {
    [Header("Input UI")]
    public float alphaWhileUsing = 0.5f;          // the alpha value the image will have while it is in use
    Image climbDownImage;
    Image climbUpImage;
    Image moveRightImage;
    Image jumpImage;
    Image moveLeftImage;

    [Header("Reset UI")]
    Image resetCircle;
    IEnumerator resetCircleIncrease;
    IEnumerator resetCircleDecrease;

    float resetCircleMaxTime = 1;
    const float MAX_RESET_IMAGE_SCALE = 42;
    bool terminateNestedRoutine = false;




    protected override void Awake() {
        base.Awake();
        climbDownImage = GameObject.Find("ClimbDownImage").GetComponent<Image>();
        climbUpImage = GameObject.Find("ClimbUpImage").GetComponent<Image>();
        moveRightImage = GameObject.Find("MoveRightImage").GetComponent<Image>();
        jumpImage = GameObject.Find("JumpImage").GetComponent<Image>();
        moveLeftImage = GameObject.Find("MoveLeftImage").GetComponent<Image>();
        resetCircle = GameObject.Find("ResetCircle").GetComponent<Image>();
    }
    public void UsedClimbDown() {
        SetImageAlpha(climbDownImage, alphaWhileUsing);
    }
    public void UsedClimbUp() {
        SetImageAlpha(climbUpImage, alphaWhileUsing);
    }

    public void UsedRight() {
        SetImageAlpha(moveRightImage, alphaWhileUsing);
    }

    public void UsedJump() {
        SetImageAlpha(jumpImage, alphaWhileUsing);
    }

    public void UsedLeft() {
        SetImageAlpha(moveLeftImage, alphaWhileUsing);
    }

    public void ResetUI() {
        ResetInputIcons();
        ResetResetCircle();
    }


    private void SetImageAlpha(Image icon, float alpha) {
        Color newAlphaColor = icon.color;
        newAlphaColor.a = alpha;
        icon.color = newAlphaColor;
    }

    private void ResetInputIcons() {
        SetImageAlpha(climbDownImage, 1);
        SetImageAlpha(climbUpImage, 1);
        SetImageAlpha(moveRightImage, 1);
        SetImageAlpha(jumpImage, 1);
        SetImageAlpha(moveLeftImage, 1);

    }
    private void ResetResetCircle() {
        resetCircleIncrease = null;
        resetCircleDecrease = null;
        resetCircle.transform.localScale = Vector3.zero;
    }

    public void StartResetImage(float time) {
        StartCoroutine(StartResetImageRoutine(time));
    }

    public IEnumerator StartResetImageRoutine(float time) {
        if (resetCircleDecrease != null) {
            StopCoroutine(resetCircleDecrease);
        }

        resetCircleMaxTime = time;
        terminateNestedRoutine = false;
        float currentScale = resetCircle.transform.localScale.x;
        float scaleLeft = MAX_RESET_IMAGE_SCALE - currentScale;
        float timeForOneScaleUnit = resetCircleMaxTime / MAX_RESET_IMAGE_SCALE;
        float timeForCurrentScaleToMaxScale = scaleLeft * timeForOneScaleUnit;
        Vector3 startScale = resetCircle.transform.localScale;
        Vector3 targetScale = new Vector3(MAX_RESET_IMAGE_SCALE, MAX_RESET_IMAGE_SCALE, 0);

        resetCircleIncrease = SetResetCircle(startScale, targetScale, timeForCurrentScaleToMaxScale);
        //StartCoroutine(resetCircleIncrease);

        // https://answers.unity.com/questions/1182632/problem-with-stopping-nested-coroutines-control-ne.html
        IEnumerator nested = resetCircleIncrease;
        while (!terminateNestedRoutine && nested.MoveNext()) {
            yield return nested.Current;
        }
        if (!terminateNestedRoutine) {
            GameManager.Instance.ResetLevel();
        }
    }
    public void StopResetImage() {
        if (resetCircleIncrease != null) {
            StopCoroutine(resetCircleIncrease);
            //resetCircleIncrease = null;
            terminateNestedRoutine = true;
        }

        float scaleLeft = resetCircle.transform.localScale.x;
        float timeForOneScaleUnit = resetCircleMaxTime / MAX_RESET_IMAGE_SCALE;
        float timeForCurrentScaleToMaxScale = scaleLeft * timeForOneScaleUnit;
        Vector3 startScale = resetCircle.transform.localScale;

        resetCircleDecrease = SetResetCircle(startScale, Vector3.zero, timeForCurrentScaleToMaxScale);
        StartCoroutine(resetCircleDecrease);
    }

    

    IEnumerator SetResetCircle(Vector3 startScale, Vector3 targetScale, float moveTime) {
        float time = 0;
        while (time < moveTime) {
            resetCircle.transform.localScale = Vector3.Lerp(startScale, targetScale, time / moveTime);
            time += Time.deltaTime;
            yield return null;
        }
        print("Complete");
        resetCircle.transform.localScale = targetScale;
        
        yield return null;
    }

}
