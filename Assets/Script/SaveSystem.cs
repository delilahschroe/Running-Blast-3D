using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    #region SetterAndGetters
    public static int CurrentLevelNumber
    {
        set
        {
            PlayerPrefs.SetInt("levelNumber", value);
        }
        get
        {
            return PlayerPrefs.GetInt("levelNumber", 0);
        }
    }
    public static float BoatSpeed
    {
        set
        {
            PlayerPrefs.SetFloat("BoatSpeed", value);
        }
        get
        {
            return PlayerPrefs.GetFloat("BoatSpeed", 4);
        }
    }
    public static int BoatSpeedCashValue
    {
        set
        {
            PlayerPrefs.SetInt("BoatSpeedCash", value);
        }
        get
        {
            return PlayerPrefs.GetInt("BoatSpeedCash", 10);
        }
    }
    public static float BoatSpeedUpgradeSliderValue
    {
        set
        {
            PlayerPrefs.SetFloat("BoatSpeedUpgradeSliderValue", value);
        }
        get
        {
            return PlayerPrefs.GetFloat("BoatSpeedUpgradeSliderValue", 0.2f);
        }
    }
    public static int BoatIndex
    {
        set
        {
            PlayerPrefs.SetInt("BoatIndex", value);
        }
        get
        {
            return PlayerPrefs.GetInt("BoatIndex", 0);
        }
    }
    public static int BoatIndexCash
    {

        set
        {
            PlayerPrefs.SetInt("BoatIndexCash", value);
        }
        get
        {
            return PlayerPrefs.GetInt("BoatIndexCash", 10);
        }
    }
    public static float BoatIndexSliderValue
    {
        set
        {
            PlayerPrefs.SetFloat("BoatIndexSliderValue", value);
        }
        get
        {
            return PlayerPrefs.GetFloat("BoatIndexSliderValue", 0);
        }
    }
    public static int BoatCapacityPerbag
    {
        set
        {
            PlayerPrefs.SetInt("BoatCapacity", value);
        }
        get
        {
            return PlayerPrefs.GetInt("BoatCapacity", 10);
        }
    }
    public static int BoatTrashCapacityCashValue
    {
        set
        {
            PlayerPrefs.SetInt("BoatTrashCapacityCashValue", value);
        }
        get
        {
            return PlayerPrefs.GetInt("BoatTrashCapacityCashValue", 10);
        }
    }
    public static float BoatTrashCapacitySliderValue
    {
        set
        {
            PlayerPrefs.SetFloat("BoatTrashCapacitySliderValue", value);
        }
        get
        {
            return PlayerPrefs.GetFloat("BoatTrashCapacitySliderValue", 0);
        }
    }
    public static int TotalCash
    {
        set
        {
            PlayerPrefs.SetInt("TotalCash", value);
        }
        get
        {
            return PlayerPrefs.GetInt("TotalCash", 10);
        }
    }
    public static bool SoundOn
    {
        set
        {
            PlayerPrefs.SetInt("SoundOn", value?1:0) ;
        }
        get
        {
            return PlayerPrefs.GetInt("SoundOn",1)==1;
        }
    }
    public static bool MusicOn
    {
        set
        {
            PlayerPrefs.SetInt("MusicOn", value?1:0) ;
        }
        get
        {
            return PlayerPrefs.GetInt("MusicOn", 1)==1;
        }
    }
    public static bool VibaratinOn
    {
        set
        {
            PlayerPrefs.SetInt("VibaratinOn", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("VibaratinOn", 1) == 1;
        }
    }

    #endregion
}
