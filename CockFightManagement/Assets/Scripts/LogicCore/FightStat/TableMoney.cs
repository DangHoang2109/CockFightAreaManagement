﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMoney : MonoBehaviour
{
    public TableMoneyItem _prefab;
    public List<TableMoneyItem> _items;
    public Transform _tfPanelItems;

    public bool _isBetPanel;

    /// <summary>
    /// Pass in the data of all tickets in a fight
    /// </summary>
    /// <param name="ticketDatas"></param>
    public void ParseData(List<TableMultipleCockTicketData> combinedData)
    {
        _items ??= new List<TableMoneyItem>();

        int amountToSpawn = combinedData.Count - _items.Count;
        if (amountToSpawn >= 0)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                AddItem();
            }
        }

        for (int i = 0; i < _items.Count; i++)
        {
            if (i >= combinedData.Count)
            {
                _items[i].gameObject.SetActive(false);
            }
            else
            {
                _items[i].gameObject.SetActive(true);

                List<decimal> value = new List<decimal>() 
                { 
                    ValueOfACock(combinedData[i], cockID: 0, isBet: _isBetPanel),
                    ValueOfACock(combinedData[i], cockID: 1, isBet: _isBetPanel)
                };

                _items[i].ParseData(combinedData[i]._buyerID, value);
            }
        }


        decimal ValueOfACock(TableMultipleCockTicketData combinedData, int cockID, bool isBet) => ((isBet ? combinedData.GetCombinedTickOfACock(cockID: cockID)?._betMoney.Value : combinedData.GetCombinedTickOfACock(cockID: cockID)?._wonMoney.Value) ?? 0);
    }
    public TableMoneyItem AddItem()
    {
        TableMoneyItem item = Instantiate<TableMoneyItem>(this._prefab, this._tfPanelItems);
        _items.Add(item);
        //item.transform.SetAsLastSibling();
        return item;
    }
}
