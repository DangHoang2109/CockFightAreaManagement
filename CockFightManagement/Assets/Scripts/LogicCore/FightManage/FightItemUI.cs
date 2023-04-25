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

        _tmpFightResult.gameObject.SetActive(data.IsEnded);
        _tmpFightResult.SetText($"{GameManager.GetCockName(data.IDCockWIn)} thắng");
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
            GameManager.Instance.ShowUI<ScreenAddBunchTicket>()
                .ParseData(this._fightData._id);
        }
    }
}
