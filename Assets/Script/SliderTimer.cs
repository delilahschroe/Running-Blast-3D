
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    Text timerText; 
    public float []totalTime;
    [HideInInspector]public float temp;
    public Transform Pos;
    void Awake()
    {
    
        timerText=transform.GetChild(0).GetComponent<Text>();
        temp=totalTime[PlayerPrefs.GetInt("LevelNumber")-1];
    }
    void Start()
    {
        UpdateTimerText();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            temp-=Time.deltaTime;
            UpdateTimerText();

            if (temp<=0)
            {
                //GameManager.Instance.GetComponent<UiManager>().GameOver();
                gameObject.SetActive(false);
            }
        }
    }
    void UpdateTimerText()
    {
        timerText.text = Mathf.RoundToInt(temp).ToString();
    }

   
    void OnDisable()
    {
        transform.DOMove(transform.position+new Vector3(800,0,0),0.8f);
    }

    void OnEnable()
    {
        
        transform.DOMove(Pos.position,0.8f);
    }
}
