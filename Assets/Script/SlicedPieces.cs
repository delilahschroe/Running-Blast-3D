using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SlicedPieces : MonoBehaviour
{
    public float duration=0.4f;
    public void Move(Transform destination,float angle)
    {
        this.transform.SetParent(destination);
        this.transform.DOMove(destination.position, duration).SetEase(Ease.Linear).OnComplete(()=>
        {
            this.transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y,
                transform.localEulerAngles.z);
            destination.GetComponentInParent<PackBox>().PerformEffect();
        });
    }
}
