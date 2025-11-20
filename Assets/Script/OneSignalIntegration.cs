using UnityEngine;
using OneSignalSDK;
using System.Threading.Tasks;

public class OneSignalIntegration : MonoBehaviour
{
    public string ONESIGNAL_APP_ID;

    async void Start()
    {
        OneSignal.Default.Initialize(ONESIGNAL_APP_ID);
        bool permissionGranted = await OneSignal.Notifications.RequestPermissionAsync(true);
        if (permissionGranted)
        {
            string userId = SplashLoader.GetOrCreateUserID();
            OneSignal.Default.Login(userId);
        }
    }
}
