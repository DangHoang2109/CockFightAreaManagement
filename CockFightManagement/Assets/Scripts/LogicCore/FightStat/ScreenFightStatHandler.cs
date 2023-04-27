using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFightStatHandler : BaseUIPopup
{
    private FightData _fightData;

    public List<TableMoney> _tableDatas;

    public void ParseData(FightData fight)
    {
        this._fightData = fight;

        foreach (TableMoney table in this._tableDatas)
        {
            table.ParseData(fight.Tickets);
        }
    }
}

