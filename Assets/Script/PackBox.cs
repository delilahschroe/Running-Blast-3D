using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PackBox : MonoBehaviour
{
    public Transform upperPart;
    public ParticleSystem ps;
    public List<Transform> availablePosition;

    private List<float> angel = new List<float>
    {
    -90, 90, -90, 90
    };

    private int index = -1;
    private bool isUpperPartActive = false;
    public Transform GetAvailablePosition()
    {
        // Reset index if no positions are available
        for (int i = 0; i < availablePosition.Count; i++)
        {
            if (availablePosition[i].GetComponentInChildren<SlicedPieces>() == null)
            {
                index = i; // Set index to the current available position
                if(i==availablePosition.Count-1 && ! isUpperPartActive)
                {
                   StartCoroutine(ActivateUpperPart());
                    isUpperPartActive = true;
                }
                return availablePosition[i];
            }
        }

        index = -1; // Reset index if no available position is found
        return null;
    }

    public float GetAngle()
    {
        // Check if index is valid before accessing the angle
        if (index >= 0 && index < angel.Count)
        {
            return angel[index];
        }

        // Default or fallback angle if index is invalid
        return 0f;
    }

    public void PerformEffect()
    {
        transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f).OnComplete(() =>
           {
               transform.DOScale(new Vector3(1, 1, 1), 0.1f);
               //LevelManager.Instance.PlayPlacedSound();
           });
    }

    private IEnumerator ActivateUpperPart()
    {
        yield return new WaitForSeconds(1f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(upperPart.transform.DOScale(1.01f, 0.15f).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOMoveY(6, 0.3f).SetEase(Ease.OutBounce));
        sequence.Append(transform.DOMoveX(-15, 0.3f).SetEase(Ease.InOutBounce));
        ps.gameObject.SetActive(true);
        ps.Play();
        //GameHandler.Instance.currentCompletedBox++;
    }
}
