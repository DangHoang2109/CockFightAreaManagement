using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFightStatHandler : BaseUIPopup
{
    private FightData _fightData;

    public List<TableMoney> _tableDatas;
    private List<TableMultipleCockTicketData> _combinedTicketsData;
    public void ParseData(FightData fight)
    {
        this._fightData = fight;

        _combinedTicketsData = GameUtils.ValidateAndCombineTicketToSinglePlayer(_fightData.Tickets);

        foreach (TableMoney table in this._tableDatas)
        {
            table.ParseData(_combinedTicketsData);
        }
    }
    public void PrintDocument()
    {
        GameManager.Instance.PrintBillForAFight
            (
            GameManager.Instance._currentChoseDate,
            _fightData.FightName,
            _combinedTicketsData
            );
    }
}

