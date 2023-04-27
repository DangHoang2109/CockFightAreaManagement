using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CalendarController : MonoSingleton<CalendarController>
{
    public GameObject _calendarPanel;
    public TextMeshProUGUI _yearNumTextMeshProUGUI;
    public TextMeshProUGUI _monthNumTextMeshProUGUI;
    public TextMeshProUGUI _target;

    public GameObject _prefabItem;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;

    public System.Action<DateTime> _onChangeDate;

    public override void Init()
    {
        base.Init();

        Vector3 startPos = _prefabItem.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_prefabItem);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_prefabItem) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_prefabItem.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 31 + startPos.x, startPos.y - (i / 7) * 25, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;
        //_target.text = $"Ngày {_dateTime.Date} Tháng {_dateTime.Month} Năm {_dateTime.Year}"; ;

        CreateCalendar();

        OnDateItemClick(_dateTime.Day.ToString());
        //_calendarPanel.SetActive(false);
    }
    void Start()
    {
        
    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            TextMeshProUGUI label = _dateItems[i].GetComponentInChildren<TextMeshProUGUI>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumTextMeshProUGUI.text = _dateTime.Year.ToString();
        _monthNumTextMeshProUGUI.text = _dateTime.Month.ToString();
    }

    int GetDays(DayOfWeek day)
    {
        return (int)day;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    public void ShowCalendar(TextMeshProUGUI target)
    {
        _calendarPanel.SetActive(true);
        _target = target;
        //_calendarPanel.transform.position = Input.mousePosition - new Vector3(0, 120, 0);
    }

    public void OnDateItemClick(string day)
    {
        _target.text = $"Ngày {day} Tháng {_monthNumTextMeshProUGUI.text} Năm {_yearNumTextMeshProUGUI.text}"; ;
        //_calendarPanel.SetActive(false);

        _onChangeDate?.Invoke(new DateTime(int.Parse(_yearNumTextMeshProUGUI.text), int.Parse(_monthNumTextMeshProUGUI.text), int.Parse(day)));
    }
}
