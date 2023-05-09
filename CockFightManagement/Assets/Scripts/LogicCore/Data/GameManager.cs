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
    public System.DateTime _todayDate;
    public System.DateTime _currentChoseDate;

    public void OpenGame()
    {
        _todayDate = GameUtils.GetTodayDate();

        this.ShowUI<ScreenFightManageHandler>();
    }
    public void CloseGame()
    {
#if UNITY_EDITOR
#else
Application.Quit();
#endif
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
            HideUI(ob);
        }
    }
    public void HideUI(BaseUIPopup t)
    {
        if(t != null)
        {
            t.Hide();
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
        return this.UserDatas.GetFightInADay(_todayDate);
    }
    public List<FightData> GetFightsInDay(System.DateTime day)
    {
        return this.UserDatas.GetFightInADay(day);
    }
#endregion Screen Fights Management

#region Ticket

#endregion

    public void OnEndFight(int idCockWining, int fightID, List<TicketData> tickets)
    {
        ScreenFightManageHandler manage = ShowUI<ScreenFightManageHandler>();
        HideUI<ScreenAddBunchTicket>();

        //calculate the winning
        if(tickets != null && tickets.Count > 0)
        {
            CalculateWiningOfAFight(idCockWining,fightID, tickets);
        }

        manage.Refresh();
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
    public void PrintBillForAFight(System.DateTime date, string fightName, List<TableMultipleCockTicketData> _combinedTicketsData)
    {
        FightDocument doc = new FightDocument(date, fightName, _combinedTicketsData);

        PrintingManager.Instance.GenerateAndPrintDocument(doc.ToDocument());
    }
    private void PrintBillForAFight(System.DateTime date,string fightName, List<TicketData> tickets)
    {
        List<TableMultipleCockTicketData>  _combinedTicketsData = GameUtils.ValidateAndCombineTicketToSinglePlayer(tickets);
        PrintBillForAFight(date, fightName, _combinedTicketsData);
    }
    private void PrintBillForAFight(System.DateTime date, FightData fight)
    {
        PrintBillForAFight(date, fight.FightName, fight.Tickets);
    }

    public void TempSaveAFight(System.DateTime date,int fightID, List<TicketData> tickets)
    {
        UserDatas.PushBackDataTicket(date,fightID, tickets);
    }
}
