using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dotween : MonoBehaviour
{
    public bool buttons;
    public bool loop;
    public float defaultScale;
    public float buttonScaleZero;
    public float buttonScaleOne;
    public float buttonScaleTime;
    public float buttonScaleTimeDelay;
    public Ease buttonScaleEase;

    private void OnEnable()
    {
        this.transform.localScale = new Vector3(defaultScale,defaultScale,defaultScale);
        if (loop)
        {
            if (buttons)
            {
                transform.DOScale(buttonScaleZero, buttonScaleTime).SetEase(buttonScaleEase).SetUpdate(true).OnComplete(Rescale).SetDelay(buttonScaleTimeDelay);
            }
        }
        else
        {
            if (buttons)
            {
                transform.DOScale(buttonScaleZero, buttonScaleTime).SetEase(buttonScaleEase).SetUpdate(true).SetDelay(buttonScaleTimeDelay);
            }
        }
    }
    void Rescale()
    {
        buttonScaleTimeDelay = 0;
        transform.DOScale(buttonScaleOne, buttonScaleTime).SetEase(buttonScaleEase).SetUpdate(true).OnComplete(OnEnable).SetDelay(buttonScaleTimeDelay);
    }
}
