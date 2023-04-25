using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class TicketItemInputUI : MonoBehaviour
{
    public TextMeshProUGUI _tmpID;
    public CustomInputField _iptIDPlayer;
    public CustomInputField _iptNameCock;
    public CustomInputField _iptBetMoney;
    public Button _btnDelete;

    public List<CustomInputField> _listInputs;
    public CustomInputField _selectingInput;

    public System.Action<bool> _onCbCannotMoveLocalInput;
    private TicketData _thisTicketData;
#if UNITY_EDITOR
    private void OnValidate()
    {
        _listInputs = new List<CustomInputField>() { _iptIDPlayer, _iptNameCock, _iptBetMoney };
    }
#endif
    public void Setup(TicketData emptyStartTicket)
    {
        SetupInput();
        _thisTicketData = emptyStartTicket;
    }
    public void SelectFirstInput()
    {
        _listInputs[0].Select();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.End))
        //{
        //    bool isCanNext = OnNextInputField();
        //    Debug.Log("IsCanNext " + isCanNext);
        //}
    }
    [ContextMenu("Setup Input")]
    public  void SetupInput()
    {
        foreach (CustomInputField item in _listInputs)
        {
            item.onCustomSelected -= OnInput_Selected;
            item.onCustomSelected += OnInput_Selected;

            item.onCustomSubmit -= OnInput_Submit;
            item.onCustomSubmit += OnInput_Submit;
        }

        this._iptNameCock.onCustomValueChanged -= OnInput_NameCock;
        this._iptNameCock.onCustomValueChanged += OnInput_NameCock;

        this._iptBetMoney.onCustomValueChanged -= OnInput_BetMoney;
        this._iptBetMoney.onCustomValueChanged += OnInput_BetMoney;
    }
    private void OnInput_Submit(CustomInputField sender, string currentText)
    {
        bool isCanNext = OnNextInputField();
        Debug.Log("IsCanNext " + isCanNext);

        _onCbCannotMoveLocalInput?.Invoke(!isCanNext);
    }
    public bool OnNextInputField()
    {
        if(this._selectingInput != null)
        {
            int index = this._listInputs.IndexOf(this._selectingInput);

            bool isCanNext = ++index < _listInputs.Count;

            if (isCanNext)
            {
                _listInputs[index].Select();
            }

            return isCanNext;
        }
        return false;
    }

    public void OnInput_Selected(CustomInputField sender, string currentText)
    {
        _selectingInput = sender;
    }
    public void OnInput_NameCock(CustomInputField sender, string input)
    {
        string iptLower = input.ToLower();
        if (iptLower.StartsWith("x"))
        {
            _iptNameCock.SetTextWithoutNotify("Xanh");
        }
        else if (iptLower.StartsWith("d"))
        {
            _iptNameCock.SetTextWithoutNotify("Đỏ");
        }
        else
        {
            _iptNameCock.SetTextWithoutNotify("Xanh");
        }
    }
    public void OnInput_BetMoney(CustomInputField sender, string input)
    {
        int money = 0;
        if (int.TryParse(input, out money))
        {
            if (money < 0)
                money = Mathf.Abs(money);
        }

        sender.SetTextWithoutNotify(money.ToString());
    }

    public TicketData GenerateThisTicketData()
    {
        if(string.IsNullOrEmpty(this._iptIDPlayer.text) || string.IsNullOrWhiteSpace(this._iptIDPlayer.text))
        {
            Debug.Log("ERROR: EMPTY ID PLAYER");
            return null;
        }

        this._thisTicketData._buyerID = _iptIDPlayer.text;

        if (string.IsNullOrEmpty(this._iptNameCock.text) || string.IsNullOrWhiteSpace(this._iptNameCock.text))
        {
            Debug.Log("ERROR: EMPTY COCK NAME");
            return null;
        }
        _thisTicketData._cockID = GameManager.GetCockID(_iptNameCock.text);

        if (string.IsNullOrEmpty(this._iptBetMoney.text) || string.IsNullOrWhiteSpace(this._iptBetMoney.text))
        {
            Debug.Log("ERROR: EMPTY BET MONEY");
            return null;
        }
        if(!int.TryParse(this._iptBetMoney.text, out int money))
        {
            Debug.Log("ERROR: ERROR BET MONEY");
            return null;
        }
        if (money <= 0)
        {
            Debug.Log("ERROR: MONEY INVALID");
            return null;
        }

        _thisTicketData._betMoney = money;
        return this._thisTicketData;
    }
}

