using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PackManager : MonoBehaviour
{
    public List<PackBox> packBoxes;

    public List<Transform> vege = new List<Transform>();
    private void Start()
    {
        StartEffect();
    }
    public int GetAvailablePackBoxIndex()
    {
        for(int i=0;i<packBoxes.Count;i++)
        {
            if(packBoxes[i].GetAvailablePosition()!=null)
            {
                return i;
            }
        }

        return -1;
    }

    public PackBox GetAvailablePackBox()
    {
        int index = GetAvailablePackBoxIndex();

        return packBoxes[index];
    }

    public void StartEffect()
    {
        for (int i = 0; i < vege.Count; i++)
        {
            vege[i].DOScale(1, 0.2f).SetEase(Ease.OutBounce);
        }
    }
}
