using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public static BoatManager Instance;
    public GameObject[] trashInBoat;
    public int trashBagCapatity;
    Coroutine empty;
    SoundManager soundManager;
    [Header("Boats")]
    [Space(10)]
    public GameObject[] boats;
    private void Awake()
    {
        Instance = this;
        boats[SaveSystem.BoatIndex].SetActive(true);
        soundManager = SoundManager.instance;
    }
    private void Start()
    {
        trashBagCapatity = SaveSystem.BoatCapacityPerbag;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            if (!GameManager.Instance.levelCompleteStatus)
            {
                if (GameManager.Instance.collect)
                {
                    GameManager.Instance.TrashUpdate(other.GetComponent<TrashType>().type);
                    Destroy(other.gameObject);
                    soundManager?.TrashSoundAndVibration();
                }
            }
        }
        if (other.gameObject.CompareTag("EmptyTrash"))
        {
                empty = StartCoroutine(GameManager.Instance.TrashEmpty());
        }
        if (other.gameObject.CompareTag("Upgrade"))
        {
            GamePlayUIManager.instance.UpgradePanalActivatorAndValueSetter();
        }
        if (other.gameObject.CompareTag("Complete"))
        {
            if (GameManager.Instance.levelCompleteStatus)
            {
                GameManager.Instance.GameCompletedOperations();

            }
        }
    }
    public void BoatUpgradeCheck()
    {
        for (int i = 0; i < boats.Length; i++)
        {
            boats[i].SetActive(false);
        }
        boats[SaveSystem.BoatIndex].SetActive(true);

    }
    public void BoatTrashCapacityIncrease(int value)
    {
        SaveSystem.BoatCapacityPerbag += value;
        trashBagCapatity = SaveSystem.BoatCapacityPerbag;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EmptyTrash"))
        {
            StopCoroutine(empty);
            GamePlayUIManager.instance.StopCorotineOfTrashEmpty();
            GamePlayUIManager.instance.StartCoinsAnimation(GameManager.Instance.currentTrashQuantity);
            GameManager.Instance.currentTrashQuantity = 0;
        }
        if (other.gameObject.CompareTag("Upgrade"))
        {
            GamePlayUIManager.instance.ExitUpgradePanal();
        }
    }
}
