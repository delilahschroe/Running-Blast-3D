using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataHandler : MonoBehaviour
{
    public static ItemDataHandler Instance;
    public Data itemData;

    private void Awake()
    {
        Instance = this;
    }
}
