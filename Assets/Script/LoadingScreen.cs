
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider a;
    void Start()
    {
        a.value = 0;
        Invoke(nameof(PlayBtn),1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            a.value+=Time.deltaTime*50;
        }
    }
    public void PlayBtn()
    {
        
        SceneManager.LoadScene(1);
        
    }
}
