using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
public class Line : MonoBehaviour,IPointerClickHandler
{

    public Transform Up;
    bool flag,Done;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount==5)
        {
            if(!flag)
            {
                checker();
                flag=true;
            }
        }
        if(transform.childCount<5)
        {
            flag=false;
        }
    }
    void checker()
    {
        for(int i=2;i<5;++i)
        {
            if(transform.GetChild(1).GetComponent<ItemManager>().itemType!=transform.GetChild(i).GetComponent<ItemManager>().itemType)
            {
                return;
            }
        }
        /*GetComponentInParent<Level>().Done++;
        Done=true;
        Color a=GetComponentInParent<Level>().GetColor(transform.GetChild(1).GetComponent<ItemManager>().itemType);
        GetComponent<SpriteRenderer>().color=a;
        Destroy(GetComponent<Collider>());*/
    }
    public Transform GetPos()
    {
        Transform pos=transform.GetChild(0);
        for(int i=0;i<pos.childCount-1;++i)
        {
            if(!pos.GetChild(i).GetComponent<Position>().Active)
            {
                return pos.GetChild(i);
            }
        }
        return null;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
       /* if(GameManager.Instance.Drag ||Done)
        {
            return;
        }
        GameManager.Instance.PlaySound(1);
        Transform a=transform.GetChild(transform.childCount-1);
        a.GetComponent<ItemManager>().fun();
        GameManager.Instance.Onitemclick(a);*/
    }
}
