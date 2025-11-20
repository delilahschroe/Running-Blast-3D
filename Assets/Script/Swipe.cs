using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeSelection : MonoBehaviour
{
    public float maxDistance = 1.0f; // Adjust this value based on your object's layout and spacing

    public ParticleSystem effect;

    private Camera mainCamera;
    private vegetables initialType = vegetables.None;
    private List<GameObject> selectedObjects = new List<GameObject>();
    private List<Vector3> objectPositions = new List<Vector3>();  // List to store positions of selected objects
    private GameObject previousChildObject = null;
    private GameObject currentSelectedObject = null;
    private LineRenderer lineRenderer;
    private bool lineActive = false;

    // Reference to the prefab options (drag and drop in the inspector)
    public GameObject type1;
    public GameObject type2;
    public GameObject type3;
    public GameObject type4;
    public GameObject type5;
    public GameObject type6;
    public GameObject type7;
    public GameObject type8;
    public GameObject type9;
    public GameObject type10;

    [Header("Range position")]
    [SerializeField] private float x = 1;
    [SerializeField] private float z = 1;

    [Header("Rotation")]
    [SerializeField] private float minAngley = 45f;
    [SerializeField] private float maxAngley = 90f;

    // Reference to DrawLineController (drag and drop in the inspector)
    public DrawLineController drawLineController;
    private Vector3 selectionDirection = Vector3.zero; // Direction of the selection
    private const float directionTolerance = 0.1f; // Tolerance for direction alignment

    public Transform destination;
    public PackManager packManager;
    void Start()
    {
        mainCamera = Camera.main;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log("Touch detected");

            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchStart(touch);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                HandleTouchMove(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                ResetSelection();
            }
        }

        if (lineActive && selectedObjects.Count > 0)
        {
            lineRenderer.positionCount = selectedObjects.Count;

            for (int i = 0; i < selectedObjects.Count; i++)
            {
                lineRenderer.SetPosition(i, selectedObjects[i].transform.position);
            }
        }

    }

    private void HandleTouchStart(Touch touch)
    {
       
        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != null)
            {
                if (hitObject != currentSelectedObject)
                {
                    DeactivatePreviousChild();
                    currentSelectedObject = hitObject;
                    ActivateChildObject(hitObject);
                }

                //save the type
                initialType = hitObject.GetComponent<Item>().type;

                // Add to selectedObjects if it's not already in the list
                if (!selectedObjects.Contains(hitObject))
                {
                    selectedObjects.Add(hitObject);
                }

                if (selectedObjects.Count == 1)
                {
                    lineRenderer.enabled = true;
                    lineActive = true;
                }
            }
        }
    }
    private void HandleTouchMove(Touch touch)
    {
        Ray ray = mainCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Ensure the hit object is valid and of the same type
            if (hitObject != null && hitObject.GetComponent<Item>().type == initialType)
            {
                // Avoid selecting the same object multiple times
                if (!selectedObjects.Contains(hitObject))
                {
                    if (selectedObjects.Count > 0)
                    {
                        // Get the previous selected object's position
                        GameObject lastSelected = selectedObjects[selectedObjects.Count - 1];
                        Vector3 start = lastSelected.transform.position;
                        Vector3 end = hitObject.transform.position;

                        // Perform a raycast between the last selected object and the current one
                        if (IsPathClear(start, end))
                        {
                            // Check alignment if necessary
                            Vector3 currentDirection = (end - start).normalized;

                            if (selectedObjects.Count == 1)
                            {
                                selectionDirection = currentDirection;
                            }
                            else if (!IsDirectionAligned(selectionDirection, currentDirection))
                            {
                                // If not aligned, reject the selection
                                return;
                            }

                            // Add the object to the selection and activate its child
                            selectedObjects.Add(hitObject);
                            ActivateChildObject(hitObject);
                        }
                        else
                        {
                            // Path is not clear, reset selection
                            ResetSelection();
                        }
                    }
                    else
                    {
                        // First object selection
                        selectedObjects.Add(hitObject);
                        ActivateChildObject(hitObject);
                    }
                }
            }
            else
            {
                ResetSelection();
            }
        }
    }

    private bool IsPathClear(Vector3 start, Vector3 end)
    {
        // Cast a ray between the start and end points to check for collisions
        Ray ray = new Ray(start, (end - start).normalized);
        float distance = Vector3.Distance(start, end);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            // Ensure the hit object is the intended next object
            if (hit.collider.gameObject != selectedObjects[selectedObjects.Count - 1]
                && hit.collider.GetComponent<Item>().type != initialType)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsDirectionAligned(Vector3 baseDirection, Vector3 currentDirection)
    {
        // Check if the directions are aligned within the tolerance
        float dotProduct = Vector3.Dot(baseDirection, currentDirection);
        return Mathf.Abs(dotProduct - 1) < directionTolerance; // Dot product close to 1 means aligned
    }
    private void ActivateChildObject(GameObject parentObject)
    {
        Item item = parentObject.GetComponent<Item>();
        if(item!=null)
        {
            item.ShowSelection();
            previousChildObject = parentObject;
        }
        //Transform childTransform = parentObject.transform.GetChild(0);
        //if (childTransform != null)
        //{
        //    childTransform.gameObject.SetActive(true);
        //    previousChildObject = childTransform.gameObject;
        //}
    }
    private void DeactivatePreviousChild()
    {
        if (previousChildObject != null)
        {
            previousChildObject.GetComponent<Item>().HideSelection();
        }
    }
    private void DestroySelectedObjects()
    {
        if (selectedObjects.Count == 0) return;

        // Store the tag of the first object in the selection
        if (initialType==vegetables.None && selectedObjects[0] != null)
        {
            initialType = selectedObjects[0].GetComponent<Item>().type;
            Debug.Log($"Stored tag for instantiation: {initialType}");
        }

        foreach (GameObject obj in selectedObjects)
        {
           // GameHandler.Instance.RemoveItemFromList(obj.GetComponent<Item>());
            if (obj != null)
            {
                // Store the position before destruction
                objectPositions.Add(obj.transform.position);

                // Destroy the object
                Destroy(obj);
            }
        }

        Debug.Log($"Destroyed {selectedObjects.Count} selected objects.");
        selectedObjects.Clear();


        // Call the DrawLineController's SetPositions method to draw the line from first to last object
        if (objectPositions.Count > 0 && drawLineController != null)
        {
            drawLineController.SetPositions(objectPositions);
        }

        //LevelManager.Instance.PlayCutSound();
    }
    private void ResetSelection()
    {
        if (selectedObjects.Count >= 2)
        {
            DestroySelectedObjects();
            InstantiateObjectsAtPositions(); // Instantiate new objects at stored positions
        }

        foreach (GameObject obj in selectedObjects)
        {
            Item item = obj.GetComponent<Item>();
            if(item!=null)
            {
                item.HideSelection();
            }
        }

        initialType = vegetables.None;
        selectedObjects.Clear();
        currentSelectedObject = null;
        lineRenderer.enabled = false;
        lineActive = false;
    }
    private void InstantiateObjectsAtPositions()
    {
        float yOffset = 2f; // How high the objects will move up before falling

        // Store the positions of instantiated objects to draw the line
        List<Vector3> instantiatedPositions = new List<Vector3>();

        foreach (Vector3 position in objectPositions)
        {
            // Instantiate the correct objects based on the selected tag
            List<GameObject> newObjects = InstantiateObjectsBasedOnTag(initialType, position);
            //CutEffectHandler.Instance.SetupEffect(initialType, position);

            foreach (GameObject newObject in newObjects)
            {

                // Add Rigidbody component to the instantiated object if it doesn't already have one
                Rigidbody rb = newObject.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = newObject.AddComponent<Rigidbody>();
                }

                // Disable gravity initially
                rb.useGravity = false;

                // Add the object's position to the list
                instantiatedPositions.Add(newObject.transform.position);

                // Start coroutine to slowly move the object up, then enable gravity
                StartCoroutine(MoveUpAndEnableGravity(newObject, rb, yOffset, 0.1f,
                    0.5f,0.5f,5,0,5)); // 0.1 seconds for upward movement
            }
        }

        // Clear the positions list after instantiating
        objectPositions.Clear();

        // Send the instantiated positions to the DrawLineController to draw the line
        if (drawLineController != null)
        {
            drawLineController.SetPositions(instantiatedPositions);
        }
    }
    private List<GameObject> InstantiateObjectsBasedOnTag(vegetables type, Vector3 position)
    {
        GameObject prefabToInstantiate = null;
        List<GameObject> instantiatedObjects = new List<GameObject>();

        // Log the tag to check what is being passed
        Debug.Log($"Tag received: {type}");

        switch (type)
        {
            case vegetables.V1:
                prefabToInstantiate = type1;
                break;
            case vegetables.V2:
                prefabToInstantiate = type2;
                break;
            case vegetables.V3:
                prefabToInstantiate = type3;
                break;
            case vegetables.V4:
                prefabToInstantiate = type4;
                break;
            case vegetables.V5:
                prefabToInstantiate = type5;
                break;
            case vegetables.V6:
                prefabToInstantiate = type6;
                break;
            case vegetables.V7:
                prefabToInstantiate = type7;
                break;
            case vegetables.V8:
                prefabToInstantiate = type8;
                break;
            case vegetables.V9:
                prefabToInstantiate = type9;
                break;
            case vegetables.V10:
                prefabToInstantiate = type10;
                break;
            default:
                Debug.LogWarning($"No prefab set for tag: {type}");
                break;
        }

        if (prefabToInstantiate != null)
        {

            // Instantiate the first object
            Vector3 offsetPosition = position + new Vector3(-0.5f, 0, 0);
            GameObject firstObject = Instantiate(prefabToInstantiate, offsetPosition, Quaternion.Euler(new Vector3(0,-90,0)));
            instantiatedObjects.Add(firstObject);

            // Instantiate the second object at an offset position (e.g., slightly above)
            Vector3 offsetPosition1 = position + new Vector3(0.5f, 0, 0); // Offset by 0.5 units along X-axis
            GameObject secondObject = Instantiate(prefabToInstantiate, offsetPosition1, Quaternion.Euler(new Vector3(0, 90, 0)));
            instantiatedObjects.Add(secondObject);
        }

        return instantiatedObjects; // Return the list of instantiated objects
    }
    private IEnumerator MoveUpAndEnableGravity(GameObject obj, Rigidbody rb, float yOffset, float duration,
        float x,float z,float xR,float yR,float zR)
    {
        Vector3 startPosition = obj.transform.position;

        // Add randomness to the position on the x and z axes
        float randomX = Random.Range(-x, x);
        float randomZ = Random.Range(-z, z);

        Vector3 targetPosition = startPosition + new Vector3(randomX, yOffset, randomZ);

        // Generate a random rotation
        Quaternion startRotation = obj.transform.rotation;
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(-xR, xR), // Random rotation around the X-axis
            obj.transform.localEulerAngles.y, // Random rotation around the Y-axis
            Random.Range(-zR, zR)  // Random rotation around the Z-axis
        );

        float elapsedTime = 0;

        // Smoothly move the object and apply rotation over the duration
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Interpolate position
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // Interpolate rotation
            obj.transform.rotation = Quaternion.Slerp(startRotation, randomRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the target position and final rotation
        obj.transform.position = targetPosition;
        obj.transform.rotation = randomRotation;

        // Enable gravity on the Rigidbody
        rb.useGravity = true;

        yield return new WaitForSeconds(0.8f);
        rb.isKinematic = true;
        PackBox packBox = packManager.GetAvailablePackBox();
        if(packBox!=null)
        {
            Transform position = packBox.GetAvailablePosition();
            float angel = packBox.GetAngle();
            if (position != null)
            {
                obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                obj.GetComponent<SlicedPieces>().Move(position,angel);
                
            }
        }
        //obj.SetActive(false);
    }
}