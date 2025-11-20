using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemType
{
    None,One,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten
}
public class ItemManager : MonoBehaviour
{
    public ItemType itemType;
    Vector3 current; 
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    public void fun()
    {
        current=transform.position;
        transform.DORotate(new Vector3(0, 0, 90), 0.5f, RotateMode.LocalAxisAdd);
        transform.DOScale(new Vector3(1.1f,1.1f,1.1f), 0.5f);
        transform.DOMove(transform.GetComponentInParent<Line>().Up.position,0.2f);
    }
    public void back(bool a)
    {
       // GameManager.Instance.Drag=false;
        if(!a)
        {
            transform.DOMove(current,0.2f);
        }
        transform.DORotate(new Vector3(0, 0, -90), 0.5f, RotateMode.LocalAxisAdd);
        transform.DOScale(new Vector3(1f,1f,1f), 0.5f).OnComplete(()=>
        {
            
        });
       // GameManager.Instance.Drag=false;
        
        
    }
    
}
