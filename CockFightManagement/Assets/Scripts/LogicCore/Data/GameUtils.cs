using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static System.DateTime GetTodayDate()
    {
        return System.DateTime.Now.Date;
    }
    public static string FormatMoneyDot(this long money, string separator = ".")
    {
        bool isNegative = false;
        if (money < 0)
        {
            money = -money;
            isNegative = true;
        }

        string result = money.ToString();
        int index = result.Length - 1;
        int split = 0;
        while (index > 0)
        {
            split++;
            if (split % 3 == 0)
            {
                result = result.Insert(index, "" + separator);
                split = 0;
            }
            index--;
        }
        if (isNegative)
            result = "-" + result;
        return result;
    }
    public static string FormatMoneyDot(this decimal money, string separator = ".")
    {
        bool isNegative = false;
        if (money < 0)
        {
            money = -money;
            isNegative = true;
        }

        string result = money.ToString("#,##0.00", new System.Globalization.CultureInfo("vi-VN"));

        if (isNegative)
            result = "-" + result;
        return result;
    }
}

public static class GameDefine
{
    public const string USER_INFO_DATA = "USER_INFO_DATA";
    public const string USER_SETTING_DATA = "USER_SETTING_DATA";

}
