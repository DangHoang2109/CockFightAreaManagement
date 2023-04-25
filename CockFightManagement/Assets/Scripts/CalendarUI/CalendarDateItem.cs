using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarDateItem : MonoBehaviour
{
    public void OnDateItemClick()
    {
        CalendarController.Instance.OnDateItemClick(gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
    }
}