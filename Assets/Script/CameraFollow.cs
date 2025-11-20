using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Pirvate
    Vector3 offet;
    GameObject camera;
    GameObject boat;
    Coroutine cameraMovement;

    void Start()
    {
        camera = Camera.main.transform.gameObject;
        offet = GameManager.Instance.offSet;
        boat = GameManager.Instance.boat;
    }
    public void CameraMove()
    {
        cameraMovement = StartCoroutine(StartCameraMove());
    }
    public void StopCameraMovement()
    {
        StopCoroutine(cameraMovement);
    }
    IEnumerator StartCameraMove()
    {
        while (true)
        {
            camera.transform.position = boat.transform.position - offet;
            yield return null;
        }
        yield return null;
    }
}
