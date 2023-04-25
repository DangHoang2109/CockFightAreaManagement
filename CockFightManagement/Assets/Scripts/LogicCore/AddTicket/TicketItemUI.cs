using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TicketItemUI : MonoBehaviour
{
    public TextMeshProUGUI _tmpID;
    public TMP_Dropdown _dropboxCock;
    public TMP_InputField _iptBetMoney;
    public Button _btnDeleteTicket;

    public void ParseData(TicketData tick, bool isAllowEdit = false)
    {
        this.SetAllowEdit(isAllowEdit);
    }
    public void SetAllowEdit(bool isAllowEdit)
    {
        this._dropboxCock.interactable = isAllowEdit;
        this._iptBetMoney.interactable = isAllowEdit;
        this._btnDeleteTicket.interactable = isAllowEdit;
    }
}
