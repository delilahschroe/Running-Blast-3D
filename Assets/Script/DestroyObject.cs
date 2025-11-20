using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
