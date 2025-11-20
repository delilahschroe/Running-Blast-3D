using UnityEngine;

public class Position : MonoBehaviour
{
   public bool Active=false;
   public Transform trigger=null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        trigger=other.transform;
        Active=true;
        //trigger.SetSiblingIndex(transform.GetSiblingIndex());
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       trigger=null;
        Active=false;
        
    }
}
