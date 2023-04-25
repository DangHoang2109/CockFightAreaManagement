using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PopupAddTicket _popupAddTicket;
    public ScreenFightManageHandler _screenFightsManager;
    public ScreenFightStatHandler _screenFightStat;
    public ScreenAddBunchTicket _screenBunchTicket;

    public List<BaseUIPopup> _listUI;

    private UserDatas _userDatas;
    public UserDatas UserDatas
    {
        get
        {
            if (_userDatas == null)
                _userDatas = GameDataManager.Instance.UserDatas;
            return _userDatas;
        }
    }
    private System.DateTime _todayDate;

    public void OpenGame()
    {
        _todayDate = GameUtils.GetTodayDate();

        this.ShowUI<ScreenFightManageHandler>();


    }
    public T GetUI<T>() where T : BaseUIPopup
    {
        for (int i = 0; i < _listUI.Count; i++)
        {
            if (_listUI[i].TryGetComponent<T>(out T comp))
                return comp;
        }
        return null;
    }
    public T ShowUI<T>() where T : BaseUIPopup
    {
        T ob = this.GetUI<T>();
        if(ob != null)
        {
            ob.gameObject.SetActive(true);
            ob.OnShow();
        }
        return ob;
    }
    public void HideUI<T>() where T : BaseUIPopup
    {
        T ob = this.GetUI<T>();
        if (ob != null)
        {
            ob.Hide();
        }
    }

    #region Setting

    public static string GetCockName(int cockID)
    {
        return GameDataManager.Instance.SettingDatas.GetCockName(cockID);
    }
    public static int GetCockID(string cockName)
    {
        return GameDataManager.Instance.SettingDatas.GetCockID(cockName);
    }
    public TicketData GetEmptyTicket(int fightID)
    {
        return UserDatas.CreateEmptyTicket(fightID);
    }
    public TicketData GetEmptyTicket(System.DateTime date, int fightID)
    {
        return UserDatas.CreateEmptyTicket(date,fightID);
    }

    #endregion Setting

    #region Screen Fights Management

    public bool OnCreateNewFight(out FightData newFightData)
    {
        if(this.UserDatas.TryAddFight(out newFightData))
        {

            return true;
        }

        return false;
    }

    public List<FightData> GetFightsInToday()
    {
        return this.UserDatas.GetFightInADay(GameUtils.GetTodayDate());
    }

    #endregion Screen Fights Management

    #region Ticket

    #endregion

    public void OnEndFight(int idCockWining, int fightID, List<TicketData> tickets)
    {
        ShowUI<ScreenFightManageHandler>();
        HideUI<ScreenAddBunchTicket>();

        //calculate the winning
        if(tickets != null && tickets.Count > 0)
        {
            CalculateWiningOfAFight(idCockWining,fightID, tickets);
        }
    }
    private void CalculateWiningOfAFight(int idCockWining,int fightID,List<TicketData> tickets)
    {
        //setup the data to userdatas
        UserDatas.PushBackDataTicket(fightID,tickets);

        //calculate the wining
        if(UserDatas.EndFight(fightID,idCockWining, out FightData fight))
        {
            //print these final ticket
            PrintBillForAFight(_todayDate, fight);
        }


    }
    private void PrintBillForAFight(System.DateTime date,string fightName, List<TicketData> tickets)
    {
        Debug.Log("IN BILL");
        Debug.Log($"Ngày: {date.ToShortDateString()} - Độ: {fightName}");

        tickets = ValidatingTickets(tickets);
        foreach (TicketData ticket in tickets)
        {
            string res = ticket._isThisTicketWon ? $"Thắng {ticket._wonMoney}" : "Thua";

            Debug.Log($"ID: {ticket._id} - Đặt gà: {GetCockName(ticket._cockID)} {ticket._betMoney}vnd - {res}");
        }
    }
    private void PrintBillForAFight(System.DateTime date, FightData fight)
    {
        PrintBillForAFight(date, fight.FightName, fight.Tickets);
    }
    private List<TicketData> ValidatingTickets(List<TicketData> tickets)
    {
        //gom lại các phiếu của cùng 1 người chơi
        List<TicketData> result = new List<TicketData>();
        Dictionary<string, TicketData> dicTicket = new Dictionary<string, TicketData>();
        for (int i = 0; i < tickets.Count; i++)
        {
            if (dicTicket.ContainsKey(tickets[i]._buyerID))
            {
                dicTicket[tickets[i]._buyerID] = dicTicket[tickets[i]._buyerID].CombineTicket(tickets[i]);
            }
        }

        foreach (string id in dicTicket.Keys)
        {
            result.Add(dicTicket[id]);
        }

        return result;
    }
}
