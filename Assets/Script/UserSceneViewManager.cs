
using System.Collections;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserSceneViewManager : MonoBehaviour
{
    public static UserSceneViewManager instance;
    public GameObject exitPanal;
    public GameObject viewObject;
    GameObject testingView;
    UniWebView view;
    private ScreenOrientation lastOrientation;
    private Vector2 lastResolution;
    private bool resizeWindow = false;

    
    private Vector2 startPos;
    private bool isSwiping = false;

    // Adjustable settings
    [SerializeField] private float edgeThreshold = 50f;   // Distance from left edge to start detection
    [SerializeField] private float minSwipeDistance = 100f; // Minimum horizontal distance to count as swipe

    public string homeUrl = "https://verdecasino.com/";

    private TouchScreenKeyboard keyboard;
    private string activeFieldId;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Disable Unity keyboard input
        TouchScreenKeyboard.hideInput = true;

    }

    public void LoadRequiredView(string adress)
    {
        InitializeWebView(adress);
    }

    public void LoadRequiredView()
    {
        InitializeWebView(SplashLoader.instance.finalAdress);
    }

    private void InitializeWebView(string url)
    {
        resizeWindow = true;
        lastOrientation = Screen.orientation;
        lastResolution = new Vector2(Screen.width, Screen.height);

        // Clear previous webview if exists
        if (view != null)
        {
            Destroy(view);
            view = null;
        }
      

        UniWebView.SetEnableKeyboardAvoidance(false);
        view = viewObject.AddComponent<UniWebView>();

        // Configure webview settings
        view.SetBackButtonEnabled(true);
        view.SetAcceptThirdPartyCookies(true);
        view.SetSupportMultipleWindows(true, true);
        ResizeView();

          //changing
 view.AddUrlScheme("uniwebview"); // register your custom scheme
 view.SetAllowBackForwardNavigationGestures(false); // allow JS to detect left-edge swipe

 view.OnMessageReceived += (v, message) =>
 {
     Debug.Log($"➡️ UniWebView Message Received: scheme={message.Scheme} path={message.Path} raw={message.RawMessage}");
     if (message.Scheme == "uniwebview" && message.Path == "edgeSwipeDetected")
     {
         // Received from JS swipe detection
         OnLeftEdgeSwipe();
     }
 };

 // changig end 


        view.Load(url);
        view.Show();
        view.SetAllowBackForwardNavigationGestures(true);


        view.OnShouldClose += (v) =>
        {
            
    ShowExitPopup(); // show popup or close
    return false;

        };

        view.OnOrientationChanged += (v, orientation) =>
        {
            ResizeView();
        };

                  // changing...

        view.OnPageFinished += (v, statusCode, loadedUrl) =>
        {
            string js = @"
(function() {
    // improved left-edge swipe detector
    let startX = 0;
    let startY = 0;
    let isSwiping = false;
    const edgeThreshold = 50;        // allow start a bit farther from exact edge
    const minSwipeDistance = 40;     // much shorter distance to trigger
    const maxVerticalDeviation = 120; // allow some vertical movement
    let lastFired = 0;
    const cooldownMs = 700;          // prevent multiple rapid triggers

    function now() { return (new Date()).getTime(); }

    function onStart(e) {
        const t = e.touches ? e.touches[0] : null;
        if (!t) return;
        startX = t.clientX;
        startY = t.clientY;
        isSwiping = (startX < edgeThreshold);
    }

    function tryFire(touchX, touchY) {
        const diffX = touchX - startX;
        const diffY = Math.abs(touchY - startY);

        if (diffX > minSwipeDistance && diffY < maxVerticalDeviation) {
            const nowMs = now();
            if (nowMs - lastFired > cooldownMs) {
                lastFired = nowMs;
                // notify Unity (via custom scheme)
                window.location.href = 'uniwebview://edgeSwipeDetected';
            }
            return true;
        }
        return false;
    }

    function onMove(e) {
        if (!isSwiping) return;
        const t = e.touches ? e.touches[0] : null;
        if (!t) return;
        if (tryFire(t.clientX, t.clientY)) {
            isSwiping = false; // consume this swipe
        }
    }

    function onEnd(e) {
        if (!isSwiping) return;
        const t = e.changedTouches ? e.changedTouches[0] : null;
        if (!t) { isSwiping = false; return; }
        tryFire(t.clientX, t.clientY);
        isSwiping = false;
    }

    window.addEventListener('touchstart', onStart, { passive: true });
    window.addEventListener('touchmove', onMove, { passive: true });
    window.addEventListener('touchend', onEnd, { passive: true });

    // Optional debug: you can uncomment the next line to add a small visible helper
    // document.body.insertAdjacentHTML('beforeend', '<div style=""position:fixed;left:6px;top:6px;background:rgba(0,0,0,0.5);color:#fff;padding:4px 6px;z-index:999999;font-size:11px;"">edgeSwipe ready</div>');
})();
";
            v.EvaluateJavaScript(js, (payload) => {
                Debug.Log("✅ JS improved swipe script injected into webview");
            });
        };




        // changing ending
        
    }


      // chaning ....

 private bool IsHomePage(string url)
 {
     if (string.IsNullOrEmpty(url)) return false;

     // ✅ Your base domain (don’t include https://)
     string baseDomain = "verdecasino.com";

     // Normalize for comparison
     url = url.TrimEnd('/').ToLower();

     // ✅ Must contain base domain
     if (!url.Contains(baseDomain))
         return false;

     // ✅ Check if it's the root or a language code page
     // e.g., https://verdecasino.com OR https://verdecasino.com/en
     if (url == $"https://{baseDomain}")
         return true;

     // ✅ Check if last part is a 2-letter language code
     int lastSlash = url.LastIndexOf('/');
     if (lastSlash > 0 && url.Length - lastSlash - 1 == 2)
     {
         string lang = url.Substring(lastSlash + 1);
         if (Regex.IsMatch(lang, "^[a-z]{2}$", RegexOptions.IgnoreCase))
             return true;
     }

     return false;
 }


    // changing ended...







    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (view.CanGoBack)
            {
                view.GoBack();
            }
            else
            {
                ShowExitPopup();
            }
        }

        if (resizeWindow && view != null)
        {
            if (Screen.orientation != lastOrientation ||
                Screen.width != lastResolution.x ||
                Screen.height != lastResolution.y)
            {
                lastOrientation = Screen.orientation;
                lastResolution = new Vector2(Screen.width, Screen.height);
                ResizeView();
            }
        }

        // changing added.....

#if UNITY_EDITOR || UNITY_STANDALONE
        DetectMouseSwipe();
#elif UNITY_IOS || UNITY_ANDROID
        DetectTouchSwipe();
#endif


        // chnaging ended.....

    }
    
 // changing added ......
 private void DetectMouseSwipe()
 {
     if (Input.GetMouseButtonDown(0))
     {
         Vector2 mousePos = Input.mousePosition;
         if (mousePos.x < edgeThreshold) // Start only if from left edge
         {
             startPos = mousePos;
             isSwiping = true;
         }
     }
     else if (Input.GetMouseButtonUp(0) && isSwiping)
     {
         Vector2 endPos = Input.mousePosition;
         float distanceX = endPos.x - startPos.x;
         float distanceY = Mathf.Abs(endPos.y - startPos.y);

         // Check swipe direction and distance
         if (distanceX > minSwipeDistance && distanceY < 100f && view!=null && !view.CanGoBack)
         {
             OnLeftEdgeSwipe();
         }
         isSwiping = false;
     }
 }

 private void DetectTouchSwipe()
 {
     if (Input.touchCount > 0)
     {
         Touch touch = Input.GetTouch(0);

         if (touch.phase == TouchPhase.Began && touch.position.x < edgeThreshold)
         {
             startPos = touch.position;
             isSwiping = true;
         }
         else if (touch.phase == TouchPhase.Ended && isSwiping)
         {
             Vector2 endPos = touch.position;
             float distanceX = endPos.x - startPos.x;
             float distanceY = Mathf.Abs(endPos.y - startPos.y);

             if (distanceX > minSwipeDistance && distanceY < 100f && view != null && !view.CanGoBack)
             {
                 OnLeftEdgeSwipe();
             }
             isSwiping = false;
         }
     }
 }

 private void OnLeftEdgeSwipe()
 {
     Debug.Log("📱 Left-edge swipe detected!");
     // 👉 Show your information panel here:
     // Example:
     // infoPanel.SetActive(true);

     resizeWindow = false;
     if (view != null && IsHomePage(view.Url))
     {
            ShowExitPopup();
     }
 }

 // changing added...


    private void ResizeView()
    {
        if (view != null)
        {

            // iOS → Fullscreen, let the system handle keyboard adjustments
            //view.Frame = new Rect(0, 0, Screen.width, Screen.height);

            //view.Frame = new Rect(Screen.width - Screen.safeArea.width, Screen.height - Screen.safeArea.height, Screen.safeArea.width, Screen.safeArea.height);
            //view.Frame = new Rect(0, 0, Screen.width, Screen.height);

            //view.UpdateFrame();
            var safeArea = Screen.safeArea;

        // Calculate top margin (space for status bar)
        float topMargin = Screen.height - (safeArea.y + safeArea.height);

        // Apply the margin so the webview starts below the status bar
        view.Frame = new Rect(
            0,
            topMargin,                  // Start just below the status bar
            Screen.width,
            Screen.height - topMargin   // Adjust height
        );

        view.UpdateFrame();
        }
    }

    void ShowExitPopup()
    {
        exitPanal.SetActive(true);
        resizeWindow = false;
        if (view != null)
        {
            view.Hide();
        }
    }

    public void Yes()
    {
        //  exitPanal.SetActive(false);
        //SceneManager.LoadScene(1);
        Application.Quit();
    }

    public void NoBtnClicked()
    {
        exitPanal.SetActive(false);
        resizeWindow = true;
        if (view != null)
        {
            view.Show();
            ResizeView();
        }
    }

    public void CheckFinal()
    {
        if (testingView != null)
        {
            Destroy(testingView);
        }

        testingView = new GameObject("Testing View");
        var testView = testingView.AddComponent<UniWebView>();
        testView.Frame = new Rect(0, 0, 0, 0);
        testView.Load(SplashLoader.instance.finalAdress);
        testView.Show();
        testView.OnPageFinished += OnPageFinished;
    }

    void OnPageFinished(UniWebView _view, int statusCode, string url)
    {
        if (_view.Url != SplashLoader.instance.adress)
        {
            SplashLoader.instance.ShowNextTimeViewDirectly();
        }
        else
        {
            SplashLoader.instance.LoadNative();
        }
        Destroy(testingView);
        _view = null;
    }

    private void OnApplicationPause(bool pause)
    {
        // when application are go in the bg then we open again it relaod 
        //if (!pause && view != null)
        //{
        //    // When app resumes, refresh the webview to ensure it's in sync
        //    StartCoroutine(RefreshWebViewAfterDelay());
        //}
    }

    IEnumerator RefreshWebViewAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (view != null)
        {
            view.Reload();
        }
    }
}