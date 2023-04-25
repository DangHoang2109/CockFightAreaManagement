using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    private UserDatas userDatas;
    public UserDatas UserDatas
    {
        get
        {
            if (this.userDatas == null)
            { }
            return this.userDatas;
        }
    }

    private UserAppSetting settingDatas;
    public UserAppSetting SettingDatas
    {
        get
        {
            if (this.settingDatas == null)
            { }
            return this.settingDatas;
        }
    }
    private void Start()
    {
        StartCoroutine(OnLoadData());
    }

    /// <summary>
    /// Bắt đầu load thông tin user
    /// </summary>
    /// <returns></returns>
    public IEnumerator OnLoadData()
    {
        yield return new WaitForEndOfFrame();
        ///Load user info
        this.LoadUserData();
        while (this.userDatas == null)
        {
            yield break;
        }
        yield return new WaitForEndOfFrame();

        this.LoadSettingData();
        yield return new WaitForEndOfFrame();

        this.OpenGame();
    }
    /// <summary>
    /// Load thông tin user
    /// </summary>
    private void LoadUserData()
    {
        try
        {
            if (PlayerPrefs.HasKey(GameDefine.USER_INFO_DATA))
            {
                string jsonData = PlayerPrefs.GetString(GameDefine.USER_INFO_DATA);
                if (!string.IsNullOrEmpty(jsonData))
                {
                    this.userDatas = JsonUtility.FromJson<UserDatas>(jsonData);
                }
                else
                {
                    Debug.LogError("CAN NOT PARSE USER DATA: " + jsonData);
                    return;
                }
            }
            else
            {
                //Create New User;
                this.CreateUser();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    /// <summary>
    /// Load thông tin user
    /// </summary>
    private void LoadSettingData()
    {
        try
        {
            if (PlayerPrefs.HasKey(GameDefine.USER_SETTING_DATA))
            {
                string jsonData = PlayerPrefs.GetString(GameDefine.USER_SETTING_DATA);
                if (!string.IsNullOrEmpty(jsonData))
                {
                    this.settingDatas = JsonUtility.FromJson<UserAppSetting>(jsonData);
                }
                else
                {
                    Debug.LogError("CAN NOT PARSE SETTING DATA: " + jsonData);
                    return;
                }
            }
            else
            {
                settingDatas = UserAppSetting.CreateBase();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    /// <summary>
    /// Tạo mới user
    /// </summary>
    private void CreateUser()
    {
        this.userDatas = UserDatas.Create();
        settingDatas = UserAppSetting.CreateBase();

        SaveUserData();
        SaveSetting();
    }

    public void OpenGame()
    {
        this.userDatas.OpenGame();

        GameManager.Instance.OpenGame();
    }


    public void SaveUserData()
    {
        string jsonData = JsonUtility.ToJson(this.userDatas);
        PlayerPrefs.SetString(GameDefine.USER_INFO_DATA, jsonData);
        PlayerPrefs.Save();

    }
    public void SaveSetting()
    {
        string jsonData = JsonUtility.ToJson(this.settingDatas);
        PlayerPrefs.SetString(GameDefine.USER_SETTING_DATA, jsonData);
        PlayerPrefs.Save();

    }
}
