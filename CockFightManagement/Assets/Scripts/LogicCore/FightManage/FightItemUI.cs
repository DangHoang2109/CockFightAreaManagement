using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FightItemUI : MonoBehaviour
{
    public TextMeshProUGUI _tmpFightName;
    public TextMeshProUGUI _tmpFightResult;

    private FightData _fightData;

    public void OnParseData(FightData data)
    {
        _fightData = data;

        _tmpFightName.SetText($"Độ {data.FightName}");

        OnEndFight(data.IsEnded, data.IDCockWIn);
    }
    public void Refresh()
    {
        OnEndFight(this._fightData.IsEnded, _fightData.IDCockWIn);
    }
    public void OnEndFight(bool isEnd, int idCockWin)
    {
        _tmpFightResult.gameObject.SetActive(isEnd);
        _tmpFightResult.SetText($"{GameManager.GetCockName(idCockWin)} thắng");
    }
    public void OnClickItem()
    {
        if (this._fightData.IsEnded)
        {
            GameManager.Instance.ShowUI<ScreenFightStatHandler>()
                .ParseData(this._fightData);
        }
        else
        {
            ScreenAddBunchTicket scre = GameManager.Instance.ShowUI<ScreenAddBunchTicket>()
                .ParseData(this._fightData);
        }
    }
}
