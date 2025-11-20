using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUIManager : MonoBehaviour
{
    public static GamePlayUIManager instance;


    public Image topGameCompleteSlider;
    public GameObject fullText;
    public GameObject completeText;
    public Text Totalcoins;
    public int totalScores = 0;
    public int score;

    [Header("Speed Increase Panal")]
    [Space(10)]
    public GameObject upgradePanal;
    public Button speedBuyButton;
    public Image speedBuyInnerSlider;
    public Text speedBuyButtonPriceText;

    [Header("Boat Upgrade Panal")]
    [Space(10)]
    public Button boatUpgradeBtn;
    public Image boatUpgradeSliderImage;
    public Text boatBuyButtonPriceText;

    [Header("Trash Upgrade Panal")]
    [Space(10)]
    public Button trashUpgeadeButton;
    public Image trashUpgeadesliderImage;
    public Text trashBuyButtonPriceText;


    [Header("Coins and Trash Animation")]
    [Space(10)]
    public Sprite trashImage;
    public Sprite cashCoinImage;
    public GameObject canvas;
    public GameObject coinsBar;
    Coroutine trashAnimationRotine = null;
    Coroutine coinsAnimationCorotine = null;
    GameObject trashCollider;
    int totalTrashBags = 0;


    [Header("Game Panals")]
    [Space(10)]
    public GameObject levelCompleteBlackScreen;
    public GameObject levelCompletePanal;
    public GameObject levelPauseBlackScreen;
    public GameObject levelPausePanal;

    [Header("Game Buttons")]
    [Space(10)]
    public Button pauseBtn;
    public Button resumeBtn;


    [Header("Boat Particle System")]
    public ParticleSystem blueGlow;
    private void Awake()
    {
        instance = this;
        UpgragePanalValueSetter();
        CoinsUpdate();
        PauseResuneBtnClickListeners();
    }
    
    private void Start()
    {
        Invoke(nameof(TopSliderMaximumValueFinder), 0.2f);
        trashCollider = GameObject.FindGameObjectWithTag("EmptyTrash");
    }
    public void CoinsUpdate()
    {
        Totalcoins.text = SaveSystem.TotalCash.ToString("00");
    }
    public void UpgradePanalActivatorAndValueSetter()
    {
        UpgragePanalValueSetter();
        upgradePanal.SetActive(true);
    }
    public void ExitUpgradePanal()
    {
        upgradePanal.SetActive(false);
    }
    void UpgragePanalValueSetter()
    {
        if (SaveSystem.TotalCash >= SaveSystem.BoatSpeedCashValue)
        {
            speedBuyButton.onClick.RemoveAllListeners();
            speedBuyButton.onClick.AddListener(SpeedBuy);
            speedBuyButtonPriceText.text = SaveSystem.BoatSpeedCashValue.ToString();
            speedBuyInnerSlider.fillAmount = SaveSystem.BoatSpeedUpgradeSliderValue;
            speedBuyButton.interactable = true;
        }
        else
        {
            speedBuyButton.onClick.RemoveAllListeners();
            speedBuyButton.interactable = false;
            speedBuyButtonPriceText.text = SaveSystem.BoatSpeedCashValue.ToString();
            speedBuyInnerSlider.fillAmount = SaveSystem.BoatSpeedUpgradeSliderValue;
        }


        if (SaveSystem.TotalCash >= SaveSystem.BoatIndexCash)
        {
            boatUpgradeBtn.onClick.RemoveAllListeners();
            boatUpgradeBtn.onClick.AddListener(BuyUpgradeBoat);
            boatBuyButtonPriceText.text = SaveSystem.BoatIndexCash.ToString();
            boatUpgradeSliderImage.fillAmount = SaveSystem.BoatIndexSliderValue;
            boatUpgradeBtn.interactable = true;
        }
        else
        {
            boatUpgradeBtn.onClick.RemoveAllListeners();
            boatUpgradeBtn.interactable = false;
            boatBuyButtonPriceText.text = SaveSystem.BoatIndexCash.ToString();
            boatUpgradeSliderImage.fillAmount = SaveSystem.BoatIndexSliderValue;
        }
        if (SaveSystem.TotalCash >= SaveSystem.BoatTrashCapacityCashValue)
        {
            trashUpgeadeButton.onClick.RemoveAllListeners();
            trashUpgeadeButton.onClick.AddListener(BuyTrashUpgradeCapacity);
            trashBuyButtonPriceText.text = SaveSystem.BoatTrashCapacityCashValue.ToString();
            trashUpgeadesliderImage.fillAmount = SaveSystem.BoatTrashCapacitySliderValue;
            trashUpgeadeButton.interactable = true;
        }
        else
        {
            trashUpgeadeButton.onClick.RemoveAllListeners();
            trashUpgeadeButton.interactable = false;
            trashBuyButtonPriceText.text = SaveSystem.BoatTrashCapacityCashValue.ToString();
            trashUpgeadesliderImage.fillAmount = SaveSystem.BoatTrashCapacitySliderValue;
        }

    }
    void SpeedBuy()
    {
        SoundManager.instance.ButtonClicked();
        SaveSystem.TotalCash = SaveSystem.TotalCash - SaveSystem.BoatSpeedCashValue;
        SaveSystem.BoatSpeed += 0.2f;
        SaveSystem.BoatSpeedCashValue *= 2;
        SaveSystem.BoatSpeedUpgradeSliderValue += 0.2f;
        if (SaveSystem.BoatSpeedUpgradeSliderValue > 1)
        {
            SaveSystem.BoatSpeedUpgradeSliderValue = 0f;
        }
        PlayerPrefs.Save();
        speedBuyButtonPriceText.text = SaveSystem.BoatSpeedCashValue.ToString();
        speedBuyInnerSlider.fillAmount = SaveSystem.BoatSpeedUpgradeSliderValue;
        GameManager.Instance.UpdateBoatSpeed();
        UpgragePanalValueSetter();
        CoinsUpdate();
    }
    void BuyUpgradeBoat()
    {
        SoundManager.instance.ButtonClicked();
        SaveSystem.TotalCash = SaveSystem.TotalCash - SaveSystem.BoatIndexCash;
        SaveSystem.BoatIndexCash *= 2;
        SaveSystem.BoatIndexSliderValue += 0.2f;
        if (SaveSystem.BoatIndexSliderValue > 1)
        {
            SaveSystem.BoatIndexSliderValue = 0f;
            if (BoatManager.Instance.boats.Length - 1 > SaveSystem.BoatIndex)
            {
                Debug.Log("boat buy");
                SaveSystem.BoatIndex++;
                StartCoroutine(BoatUnlockAnimationCorotine());
            }
        }
        PlayerPrefs.Save();
        boatBuyButtonPriceText.text = SaveSystem.BoatIndexCash.ToString();
        boatUpgradeSliderImage.fillAmount = SaveSystem.BoatIndexSliderValue;
        BoatManager.Instance.BoatUpgradeCheck();
        UpgragePanalValueSetter();
        CoinsUpdate();
    }
    IEnumerator BoatUnlockAnimationCorotine()
    {
        blueGlow.gameObject.SetActive(true);
        blueGlow.Play();
        yield return new WaitForSeconds(2);
        blueGlow.gameObject.SetActive(false);
        blueGlow.Pause();
    }
    void BuyTrashUpgradeCapacity()
    {
        SoundManager.instance.ButtonClicked();
        SaveSystem.TotalCash = SaveSystem.TotalCash - SaveSystem.BoatTrashCapacityCashValue;
        SaveSystem.BoatTrashCapacityCashValue *= 2;
        SaveSystem.BoatTrashCapacitySliderValue += 0.2f;
        if (SaveSystem.BoatIndexSliderValue > 1)
        {
            SaveSystem.BoatIndexSliderValue = 0f;
        }
        PlayerPrefs.Save();
        trashBuyButtonPriceText.text = SaveSystem.BoatTrashCapacityCashValue.ToString();
        trashUpgeadesliderImage.fillAmount = SaveSystem.BoatTrashCapacitySliderValue;
        BoatManager.Instance.BoatTrashCapacityIncrease(2);
        UpgragePanalValueSetter();
        CoinsUpdate();
    }
    void TopSliderMaximumValueFinder()
    {
        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        for (int i = 0; i < trashObjects.Length; i++)
        {
            foreach (TrashPrice tp in GameManager.Instance.trashPrice)
            {
                if (tp.type == trashObjects[i].GetComponent<TrashType>().type)
                {
                    totalScores += tp.price;
                }
            }
        }
        totalScores = (totalScores * 90) / 100;
    }
    public void UpdateTopSlider(int value)
    {
        score++;
        float fillerValue = (float)score / totalScores;
        topGameCompleteSlider.fillAmount = fillerValue;
        if (fillerValue >= 1)
        {
            GameManager.Instance.levelCompleteStatus = true;
            completeText.SetActive(true);
            GameManager.Instance.CallLookAtComplete();
            GameManager.Instance.currentTrash = 0;
            Debug.Log("Game win");
        }
    }
    public void BoatTrashEmptyAnimation()
    {
        trashAnimationRotine = StartCoroutine(BoatTrashClean());
    }
    public void StopCorotineOfTrashEmpty()
    {
        StopCoroutine(trashAnimationRotine);
    }
    IEnumerator BoatTrashClean()
    {
        totalTrashBags = 0;
        while (true)
        {
            GameObject trashImageOnCanvas = new GameObject("image");
            trashImageOnCanvas.transform.parent = canvas.transform;
            trashImageOnCanvas.AddComponent<Image>().sprite = trashImage;
            trashImageOnCanvas.GetComponent<Image>().SetNativeSize();
            trashImageOnCanvas.transform.SetSiblingIndex(4);
            trashImageOnCanvas.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.boat.transform.position);
            trashImageOnCanvas.transform.DOMove(Camera.main.WorldToScreenPoint(trashCollider.transform.position), 1).OnComplete(() =>
            {
                Destroy(trashImageOnCanvas);
            });
            totalTrashBags++;
            yield return new WaitForSeconds(1);
        }
    }
    #region Animations
    public void StartCoinsAnimation(int index)
    {
        coinsAnimationCorotine = StartCoroutine(CoinsAnimation(index));
    }
    IEnumerator CoinsAnimation(int index)
    {
        SaveSystem.TotalCash += index;
        int newIndex = (int)index / 7;
        for (int i = 0; i < newIndex; i++)
        {
            GameObject trashImageOnCanvas = new GameObject("image");
            trashImageOnCanvas.transform.parent = canvas.transform;
            trashImageOnCanvas.AddComponent<Image>().sprite = cashCoinImage;
            trashImageOnCanvas.transform.SetSiblingIndex(4);
            //trashImageOnCanvas.GetComponent<Image>().SetNativeSize();

            trashImageOnCanvas.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.boat.transform.position);
            trashImageOnCanvas.transform.DOMove(coinsBar.transform.position+new Vector3(-100,0,0), 0.5f).OnComplete(() =>
            {
                Destroy(trashImageOnCanvas);
            });
            yield return new WaitForSeconds(0.2f);
        }
        CoinsUpdate();
        if (coinsAnimationCorotine != null)
        {
            StopCoroutine(coinsAnimationCorotine);
        }
    }
    void PauseResuneBtnClickListeners()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 0;
            levelPauseBlackScreen.SetActive(true);
            levelPauseBlackScreen.GetComponent<Image>().DOFade(0.85f, 1).SetEase(Ease.Flash).SetUpdate(true).OnComplete(() =>
            {
                levelPausePanal.transform.DOLocalMoveY(0, 1).SetEase(Ease.InOutBounce).SetUpdate(true);
            });
        });
        resumeBtn.onClick.AddListener(() =>
        {
            levelPausePanal.transform.DOMoveY(4000, 0.5f).SetEase(Ease.Flash).SetUpdate(true).OnComplete(() =>
            {
                levelPauseBlackScreen.GetComponent<Image>().DOFade(0, 0.5f).SetUpdate(true).OnComplete(() =>
                {
                    levelPauseBlackScreen.SetActive(false);
                    Time.timeScale = 1;
                });
            });
        });
    }
    public IEnumerator PlayLevelCompleteScreenAnimation()
    {
        levelCompleteBlackScreen.SetActive(true);
        levelCompletePanal.SetActive(true);
    yield return new WaitForSeconds(3);
        levelCompleteBlackScreen.GetComponent<Image>().DOFade(0.85f, 1).SetEase(Ease.Flash).OnComplete(() =>
        {
            levelCompletePanal.transform.DOLocalMoveY(0,1).SetEase(Ease.InOutBounce);
        });
    }
    #endregion
    public void NextLevelBtn()
    {
        SaveSystem.CurrentLevelNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReplayOrMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
