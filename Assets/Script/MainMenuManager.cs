using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [Space(10)]
    public Button playBtn;
    public Button settingBtn;
    public Button settingCloseBtn;

    [Header("Panals")]
    [Space(10)]
    public GameObject settingPanal;
    public GameObject settingPanalBlackScreen;
    public GameObject mainMenuUI;
    public GameObject gamePlayUI;

    [Header("Text")]
    [Space(10)]
    public Text levelNumber;
    private void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            playBtn.transform.DOMoveY(-800, 1).OnComplete(() =>
            {
                mainMenuUI.SetActive(false);
                settingPanal.SetActive(false);
                gamePlayUI.SetActive(true);
            }).SetEase(Ease.OutBounce);
            settingBtn.transform.DOLocalMoveY(1800, 1).SetEase(Ease.OutBounce);
            levelNumber.transform.parent.transform.DOLocalMoveY(1800, 1).SetEase(Ease.OutBounce);
        });

        settingBtn.onClick.AddListener(() =>
        {
            settingPanal.SetActive(true);
            settingPanalBlackScreen.SetActive(true);
            settingPanalBlackScreen.GetComponent<Image>().DOFade(0.85f, 1).SetEase(Ease.Flash).OnComplete(() =>
            {
                settingPanal.transform.DOLocalMoveY(0, 1).SetEase(Ease.InOutBounce);
            });
        });
        settingCloseBtn.onClick.AddListener(() =>
        {

            settingPanal.transform.DOLocalMoveY(3000, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
            {
                settingPanalBlackScreen.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    settingPanalBlackScreen.SetActive(false);
                });
            });
        });


        levelNumber.text = "Level "+ (SaveSystem.CurrentLevelNumber+1);
    }

}
