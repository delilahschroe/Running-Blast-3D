using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrashPrice
{
    public TypeOfTrash type;
    public int price;
}

public class GameManager : MonoBehaviour
{
    //Private
    bool cameraMoving = false;
    Rigidbody boatRb;

    //Public
    public static GameManager Instance;
    [Header("JoyStick")]
    [Space(10)]
    public Joystick joystick;

    [Header("Camera")]
    [Space(10)]
    public CameraFollow cameraFollow;
    public Vector3 offSet;

    [Header("Boat")]
    [Space(10)]
    public GameObject boat;
    public float speed;
    public float boatRotationalSpeed;
    public int boatTotalTrash;
    public int currentTrash = 0;
    public bool collect = true;

    [Header("Trash Quantity")]
    [Space(10)]
    public List<TrashPrice> trashPrice;
    public int currentTrashQuantity;
    [Header("Levels")]
    [Space(10)]
    public GameObject[] levels;


    [Header("LookAt")]
    [Space(10)]
    public GameObject objectToLookAt;
    public GameObject trashPlaceLocation;
    public GameObject completePlaceLocation;
    Coroutine lookAtCorotineStorage = null;
    Coroutine lookAtCompleteCorotineStorage = null;


    [Space(10)]
    [HideInInspector] public bool levelCompleteStatus = false;
    public GameObject confettiParticals;
    private void Awake()
    {
        Time.timeScale = 1;
        levels[SaveSystem.CurrentLevelNumber].SetActive(true);
        Instance = this;
        boatRb = boat.GetComponent<Rigidbody>();
        UpdateBoatSpeed();
        FindingLocationOfCompleteAndTrash();
}
    private void FixedUpdate()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;
        if (horizontalInput != 0 || verticalInput != 0)
        {
            if (!cameraMoving)
            {
                cameraFollow.CameraMove();
                cameraMoving = true;
            }
        }
        else
        {
            if (cameraMoving)
            {
                boatRb.linearVelocity = Vector3.zero;
                cameraFollow?.StopCameraMovement();
                cameraMoving = false;
            }
        }
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        //boatRb.AddForce(movement * speed, ForceMode.Force);
        boatRb.linearVelocity = movement * speed;
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            boatRb.rotation = Quaternion.Slerp(boatRb.rotation, targetRotation, boatRotationalSpeed * Time.fixedDeltaTime);
        }
    }
    public void UpdateBoatSpeed()
    {
        speed = SaveSystem.BoatSpeed;
    }
    public void TrashUpdate(TypeOfTrash type)
    {
        foreach (TrashPrice tp in trashPrice)
        {
            if (tp.type == type)
            {
                currentTrash += tp.price;
                if (currentTrash % BoatManager.Instance.trashBagCapatity == 0)
                {
                    for (int i = 0; i < currentTrash / BoatManager.Instance.trashBagCapatity; i++)
                    {
                        BoatManager.Instance.trashInBoat[i].SetActive(true);
                    }
                }
                if ((BoatManager.Instance.trashInBoat.Length * BoatManager.Instance.trashBagCapatity) <= currentTrash)
                {
                    for (int i = 0; i < BoatManager.Instance.trashInBoat.Length; i++)
                    {
                        BoatManager.Instance.trashInBoat[i].SetActive(true);
                    }
                    GamePlayUIManager.instance.fullText.SetActive(true);
                    if (lookAtCorotineStorage == null)
                    {
                        CallStartLookAt();
                    }
                    collect = false;
                }
            }
        }
    }
    public IEnumerator TrashEmpty()
    {
        GamePlayUIManager.instance.fullText.SetActive(false);
        StopLooking();
        GamePlayUIManager.instance.BoatTrashEmptyAnimation();
        currentTrashQuantity = 0;
        while (currentTrash > 0)
        {
            if (currentTrash % BoatManager.Instance.trashBagCapatity == 0)
            {
                for (int i = BoatManager.Instance.trashInBoat.Length - 1; i > currentTrash / BoatManager.Instance.trashBagCapatity; i--)
                {
                    BoatManager.Instance.trashInBoat[i].SetActive(false);
                }
            }
            currentTrash--;
            currentTrashQuantity++;
            GamePlayUIManager.instance.UpdateTopSlider(1);
            collect = true;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < BoatManager.Instance.trashInBoat.Length; i++)
        {
            BoatManager.Instance.trashInBoat[i].SetActive(false);
        }
        yield return null;
        GamePlayUIManager.instance.StopCorotineOfTrashEmpty();
        GamePlayUIManager.instance.StartCoinsAnimation(currentTrashQuantity);
        currentTrashQuantity = 0;
    }
    void CallStartLookAt()
    {
        lookAtCorotineStorage = StartCoroutine(StartLookAt());
    }
    void StopLooking()
    {
        if (lookAtCorotineStorage != null)
        {
            StopCoroutine(lookAtCorotineStorage);
            lookAtCorotineStorage = null;
            objectToLookAt.SetActive(false);
        }
    }
    IEnumerator StartLookAt()
    {
        objectToLookAt.SetActive(true);
        while (true)
        {
            objectToLookAt.transform.LookAt(trashPlaceLocation.transform);
            objectToLookAt.transform.position=boat.transform.position+new Vector3(1.2f,3.73f,-4);
            yield return null;
        }
        yield return null;
    }
    void FindingLocationOfCompleteAndTrash()
    {
        trashPlaceLocation = GameObject.FindGameObjectWithTag("EmptyTrash");
        completePlaceLocation = GameObject.FindGameObjectWithTag("Complete");
    }
    public void GameCompletedOperations()
    {
        //StopLookingAtComplete();
        confettiParticals.SetActive(true);
        speed = 0;
        
        StartCoroutine(GamePlayUIManager.instance.PlayLevelCompleteScreenAnimation());
    }
    public void CallLookAtComplete()
    {
        lookAtCompleteCorotineStorage = StartCoroutine(LookAtComplete());
    }
    IEnumerator LookAtComplete()
    {
        objectToLookAt.SetActive(true);
        while (true)
        {
            objectToLookAt.transform.LookAt(completePlaceLocation.transform);
            objectToLookAt.transform.position = boat.transform.position + new Vector3(1.2f, 3.73f, -4);
            yield return null;
        }
        yield return null;
    }
    void StopLookingAtComplete()
    {
        StopCoroutine(lookAtCompleteCorotineStorage);
    }
}
