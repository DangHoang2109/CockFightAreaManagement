using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFightStatHandler : BaseUIPopup
{
    private FightData _fightData;
    public void ParseData(FightData fight)
    {
        this._fightData = fight;
    }
    public void OnClickBuyNewTicket()
    {
        GameManager.Instance.ShowUI<PopupAddTicket>();
    }
}

