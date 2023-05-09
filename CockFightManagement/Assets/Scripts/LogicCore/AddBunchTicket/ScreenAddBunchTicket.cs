using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAddBunchTicket : BaseUIPopup
{
    public Button _btnCreateNewItem;

    public TicketItemInputUI _prefab;
    public List<TicketItemInputUI> _items;
    public Transform _tfPanelItems;

    private int _fightID;

    public override void OnShow()
    {
        base.OnShow();
    }
    public ScreenAddBunchTicket ParseData(FightData fightData)
    {
        this._fightID = fightData._id;

        if(fightData.Tickets != null && fightData.Tickets.Count > 0)
        {
            foreach (TicketData item in fightData.Tickets)
            {
                AddAndSetDataToItem(item);
            }
        }

        //add one default item
        AddItem();

        return this;
    }

    IEnumerator ieAddNewFocusItem(TicketItemInputUI item) 
    {
        yield return new WaitForEndOfFrame();
        //set select to the first input of this item
        item.SelectFirstInput();
    }
    private TicketItemInputUI AddNewBlankItem()
    {
        TicketItemInputUI item = Instantiate<TicketItemInputUI>(this._prefab, this._tfPanelItems);
        item._onDeleteItem -= OnItemClickDeleted;
        item._onDeleteItem += OnItemClickDeleted;

        item._onCbCannotMoveLocalInput -= OnItemSubmited;
        item._onCbCannotMoveLocalInput += OnItemSubmited;
        item.Setup(GameManager.Instance.GetEmptyTicket(_fightID));

        _items.Insert(0, item);
        item.transform.SetAsFirstSibling();
        this._btnCreateNewItem.transform.SetAsFirstSibling();

        return item;
    }
    public TicketItemInputUI AddItem()
    {
        TicketItemInputUI item =  AddNewBlankItem();
        StartCoroutine(ieAddNewFocusItem(item));

        return item;
    }
    public TicketItemInputUI AddAndSetDataToItem(TicketData existedTicket)
    {
        TicketItemInputUI item = AddNewBlankItem();
        item.SetData(existedTicket);
        return item;
    }
    public void OnClickAddItem()
    {
        AddItem();
    }
    public void AutoAddItem()
    {
        AddItem();
    }
    public void OnItemSubmited(bool isCanNotNextInputInLocalItem)
    {
        if (isCanNotNextInputInLocalItem)
        {
            //auto add new item
            AutoAddItem();
        }
    }
    public void OnItemClickDeleted(TicketItemInputUI item)
    {
        this._items ??= new List<TicketItemInputUI>();
        _items.Remove(item);
        Destroy(item.gameObject);
    }
    private List<TicketData> UpdateListTicket()
    {
        try
        {
            this._items ??= new List<TicketItemInputUI>();
            List<TicketData> ticketsOnItems = new List<TicketData>();

            foreach (TicketItemInputUI item in this._items)
            {
                TicketData tick = item.GenerateThisTicketData();
                if (tick != null)
                    ticketsOnItems.Add(tick);
                else
                {
                    Debug.Log("A TICKET IS HAS INVALID DATA");
                }
            }

            return ticketsOnItems;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            return new List<TicketData>();
        }
    }

    //private 
    //public void OnClickChooseOptionBlue(bool isON)
    //{

    //}

    public System.Action<bool, int> _onEndFight;

    public void OnClickTempSave()
    {
        GameManager.Instance.TempSaveAFight(GameManager.Instance._currentChoseDate, this._fightID, UpdateListTicket());
        Hide();
    }
    public void OnClickEndFight_Blue()
    {
        OnEndFight(0);
    }
    public void OnClickEndFight_Red()
    {
        OnEndFight(1);
    }
    public void OnEndFight(int idCockWining)
    {
        //calculate the wining money,
        GameManager.Instance.OnEndFight(idCockWining, this._fightID, UpdateListTicket());

        _onEndFight?.Invoke(true, idCockWining);
    }

    public override void OnHiding()
    {
        base.OnHiding();

        for (int i = _items.Count - 1; i >= 0 ; i--)
        {
            Destroy(_items[i].gameObject);
        }
        _items.Clear();
    }
}
