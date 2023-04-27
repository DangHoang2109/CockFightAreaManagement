using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TableMoneyItem : MonoBehaviour
{
    public Color32 _positiveValueColor, _negativeValueColor;

    public TextMeshProUGUI _tmpIDPlayer;
    public List<TextMeshProUGUI> _tmpCockMoneys;

    public void ParseData(string idPlayer, List<decimal> moneys)
    {
        this._tmpIDPlayer.SetText(idPlayer);
        for (int i = 0; i < moneys.Count; i++)
        {
            _tmpCockMoneys[i].SetText(moneys[i].FormatMoneyDot());
            _tmpCockMoneys[i].color = moneys[i] >= 0 ? _positiveValueColor : _negativeValueColor;
        }
    }
    
}
