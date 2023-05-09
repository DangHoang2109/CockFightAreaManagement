using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenFightManageHandler : BaseUIPopup
{
    public Button _btnCreateNewFight;

    public FightItemUI _prefab;
    public List<FightItemUI> _items;
    public Transform _tfPanelItems;

    private System.DateTime _currentChoseDate;
    public override void OnShow()
    {
        base.OnShow();

        ParseData();

        CalendarController.Instance._onChangeDate -= this.ParseDayData;
        CalendarController.Instance._onChangeDate += this.ParseDayData;
    }

    public void ParseData()
    {
        ParseDayData(GameManager.Instance._todayDate);
    }
    public void Refresh()
    {
        foreach (FightItemUI item in this._items)
        {
            item.Refresh();
        }
    }

    public void ParseDayData(System.DateTime day)
    {
        this._currentChoseDate = GameManager.Instance._currentChoseDate = day;

        List<FightData> fightsInADay = GameManager.Instance.GetFightsInDay(day);
        _items ??= new List<FightItemUI>();

        int amountToSpawn = fightsInADay.Count - _items.Count;
        if (amountToSpawn >= 0)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                AddItem();
            }
        }

        for (int i = 0; i < _items.Count; i++)
        {
            if (i >= fightsInADay.Count)
            {
                _items[i].gameObject.SetActive(false);
            }
            else
            {
                _items[i].gameObject.SetActive(true);
                _items[i].OnParseData(fightsInADay[i]);
            }
        }

        this._btnCreateNewFight.transform.SetAsFirstSibling();
    }

    public FightItemUI AddItem()
    {
        FightItemUI item = Instantiate<FightItemUI>(this._prefab, this._tfPanelItems);
        _items.Insert(0,item);
        item.transform.SetAsFirstSibling();
        this._btnCreateNewFight.transform.SetAsFirstSibling();

        return item;
    }
    public void OnClickNewFight()
    {
        bool isCreateSuccess = GameManager.Instance.OnCreateNewFight(out FightData fightData);
        if (isCreateSuccess)
        {
            FightItemUI item = AddItem();
            if(item != null)
            {
                item.OnParseData(fightData);
            }
        }
    }
}
