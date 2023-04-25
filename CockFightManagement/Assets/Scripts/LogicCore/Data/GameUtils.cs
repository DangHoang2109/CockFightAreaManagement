using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static System.DateTime GetTodayDate()
    {
        return System.DateTime.Now.Date;
    }
}

public static class GameDefine
{
    public const string USER_INFO_DATA = "USER_INFO_DATA";
    public const string USER_SETTING_DATA = "USER_SETTING_DATA";

}
