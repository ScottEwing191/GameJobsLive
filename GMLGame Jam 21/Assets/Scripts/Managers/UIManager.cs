using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public float alphaWhileUsing = 0.5f;          // the alpha value the image will have while it is in use
    Image climbDownImage;
    Image climbUpImage;
    Image moveRightImage;
    Image jumpImage;
    Image moveLeftImage;


    


    protected override void Awake() {
        base.Awake();
        climbDownImage = GameObject.Find("ClimbDownImage").GetComponent<Image>();
        climbUpImage = GameObject.Find("ClimbUpImage").GetComponent<Image>();
        moveRightImage = GameObject.Find("MoveRightImage").GetComponent<Image>();
        jumpImage = GameObject.Find("JumpImage").GetComponent<Image>();
        moveLeftImage = GameObject.Find("MoveLeftImage").GetComponent<Image>();


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

}
