using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject selection;
    public vegetables type;

    public void ShowSelection()
    {
        selection.SetActive(true);
    }

    public void HideSelection()
    {
        selection.SetActive(false);
    }



}
