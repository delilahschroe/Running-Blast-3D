using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum vegetables
{
    V1,
    V2,
    V3,
    V4,
    V5,
    V6,
    V7,
    V8,
    V9,
    V10,
    None,
}
public class Vegetable : MonoBehaviour
{
    public vegetables type;

    private void Awake()
    {
        for(int i=0;i<ItemDataHandler.Instance.itemData.itemData.Count;i++)
        {
            if(ItemDataHandler.Instance.itemData.itemData[i].type==type)
            {
                Transform item = Instantiate(ItemDataHandler.Instance.itemData.itemData[i].prefabs,
                    this.transform.position, Quaternion.identity, this.transform);
               // GameHandler.Instance.AddItemToItemList(item.GetComponent<Item>());
                return;
            }
        }
    }
}
