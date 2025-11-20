using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Items Data",menuName ="ItemsData/SO")]
public class Data : ScriptableObject
{
    public List<ItemsData> itemData;
}

[System.Serializable]
public class ItemsData
{
    public Transform prefabs;
    public vegetables type;
    public Color effectColor;
}
