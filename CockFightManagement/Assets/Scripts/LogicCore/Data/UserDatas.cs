using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class UserDatas 
{
    public UserFightDatas _fightDatas;
    private System.DateTime _todayDate;

    public UserDatas()
    {

    }
    public static UserDatas Create()
    {
        return new UserDatas()
        {
            _fightDatas = new UserFightDatas()
        };
    }
    public void OpenGame()
    {
        _todayDate = GameUtils.GetTodayDate();

        _fightDatas ??= new UserFightDatas();
        _fightDatas.OpenGame();
    }

    public List<FightData> GetFightInADay(System.DateTime date)
    {
        _fightDatas ??= new UserFightDatas();
        return _fightDatas.GetFightInADay(date);
    }
    public bool TryAddFight(out FightData fight)
    {
        return _fightDatas.TryAddFight(this._todayDate, out fight);
    }

    public TicketData CreateEmptyTicket(int fightID)
    {
        return _fightDatas.CreateEmptyTicket(_todayDate, fightID);
    }
    public TicketData CreateEmptyTicket(System.DateTime date,int fightID)
    {
        return _fightDatas.CreateEmptyTicket(date, fightID);
    }
    public void PushBackDataTicket(int fightID, List<TicketData> tickets)
    {
        _fightDatas.PushBackDataTicket(_todayDate, fightID, tickets);
    }
    public void PushBackDataTicket(System.DateTime date, int fightID,List<TicketData> tickets)
    {
        _fightDatas.PushBackDataTicket(date, fightID, tickets);
    }
    public bool EndFight(int fightID, int winingCockID, out FightData fightFinal)
    {
        return EndFight(_todayDate, fightID, winingCockID,out fightFinal);
    }
    public bool EndFight(System.DateTime date, int fightID, int winingCockID, out FightData fightFinal)
    {
        return _fightDatas.EndFight(date, fightID, winingCockID, out fightFinal);
    }
}

#region Fight 

[System.Serializable]
public class UserFightDatas
{
    public List<FightDatasInDate> _datas;
    public void OpenGame()
    {
        System.DateTime todayDate = GameUtils.GetTodayDate();
        if (!TryGetDateData(todayDate, out _))
        {
            this.NewDate(todayDate);
        }
    }
    public bool NewDate(System.DateTime todayDate)
    {
        try
        {
            _datas ??= new List<FightDatasInDate>();

            FightDatasInDate fightDate = new FightDatasInDate()
            {
                _timestampDate = todayDate.ToFileTime(),
            };
            _datas.Add(fightDate);
            SaveData();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            return false;
        }
    }
    public bool TryGetDateData(System.DateTime date, out FightDatasInDate dateData)
    {
        try
        {
            _datas ??= new List<FightDatasInDate>();
            dateData = _datas.Find(x => x.Date.Equals(date));

            return dateData != null;
        }
        catch (System.Exception)
        {
            dateData = null;
            return false;
        }
    }
    public bool TryAddFight(System.DateTime date, out FightData fight)
    {
        fight = null;

        try
        {
            if (TryGetDateData(date, out FightDatasInDate dateFight))
            {
                bool res = dateFight.TryAddFight(out fight);
                return res;
            }
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            return false;
        }
    }
    public List<FightData> GetFightInADay(System.DateTime date)
    {
        if(TryGetDateData(date, out FightDatasInDate dateData))
            return dateData?._fightsInThisDate ?? new List<FightData>();

        return new List<FightData>();
    }

    public TicketData CreateEmptyTicket(System.DateTime date, int fightID)
    {
        if (TryGetDateData(date, out FightDatasInDate dateData))
        {
            return dateData.CreateEmptyTicket(fightID);
        }

        return null;
    }
    public void PushBackDataTicket(System.DateTime date, int fightID, List<TicketData> tickets)
    {
        if (TryGetDateData(date, out FightDatasInDate dateData))
        {
            dateData.PushBackDataTicket(fightID, tickets);
        }
        SaveData();
    }
    public bool EndFight(System.DateTime date, int fightID, int winingCockID,  out FightData fightFinal)
    {
        if (TryGetDateData(date, out FightDatasInDate dateData))
        {
            bool res = dateData.EndFight(fightID, winingCockID, out fightFinal);
            SaveData();
            return res;
        }
        fightFinal = null;
        return false;
    }
    public void SaveData()
    {
        GameDataManager.Instance.SaveUserData();
    }
}
[System.Serializable]
public class FightDatasInDate
{
    public long _timestampDate;
    public System.DateTime Date => System.DateTime.FromFileTime(_timestampDate);
    public List<FightData> _fightsInThisDate;

    public bool TryAddFight(out FightData fightData)
    {
        fightData = null;
        try
        {
            _fightsInThisDate ??= new List<FightData>();
            fightData = new FightData()
            {
                _id = _fightsInThisDate.Count,
            };

            _fightsInThisDate.Add(fightData);
            SaveData();
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            return false;
        }
    }

    public TicketData CreateEmptyTicket(int fightID)
    {
        _fightsInThisDate ??= new List<FightData>();
        FightData fight = _fightsInThisDate.Find(x => x._id == fightID);
        if (fight != null)
        {
            return fight.CreateEmptyTicket();
        }

        return null;
    }
    public void PushBackDataTicket(int fightID, List<TicketData> tickets)
    {
        _fightsInThisDate ??= new List<FightData>();
        FightData fight = _fightsInThisDate.Find(x => x._id == fightID);

        if (fight != null)
        {
            fight.PushBackDataTicket(tickets);
        }
    }
    public bool BuyTicket(int fightID, string buyerID, int cockID, long betMoney)
    {
        try
        {
            _fightsInThisDate ??= new List<FightData>();
            FightData fight = _fightsInThisDate.Find(x => x._id == fightID);
            if (fight != null)
            {
                fight.BuyTicket(buyerID, cockID, betMoney);
                SaveData();
                return true;
            }
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            return false;
        }
    }
    public bool EndFight(int fightID, int winingCockID, out FightData fightFinal)
    {
        try
        {
            _fightsInThisDate ??= new List<FightData>();
            FightData fight = _fightsInThisDate.Find(x => x._id == fightID);
            if (fight != null)
            {
                fightFinal = fight.EndFight(winingCockID);
                SaveData();
                return true;
            }

            fightFinal = null;
            return false;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.StackTrace);
            fightFinal = null;
            return false;
        }

    }
    public void SaveData()
    {
        GameDataManager.Instance.SaveUserData();
    }
}
[System.Serializable]
public class FightData
{
    public int _id;
    public string FightName => $"{_id + 1}";
    public List<TicketData> _tickets;
    public List<TicketData> Tickets => _tickets;

    public bool _isEnded;
    public bool IsEnded => _isEnded;

    public int _idCockWin ;
    public int IDCockWIn => _idCockWin;

    public long TotalBetMoney
    {
        get
        {
            if (_tickets == null || _tickets.Count == 0)
                return 0;
            return _tickets.Sum(x => x._betMoney);
        }
    }
    public long TotalBetMoneyOfACock(int cockID)
    {
        if (_tickets == null || _tickets.Count == 0)
            return 0;

        IEnumerable<TicketData> tick = _tickets.Where(x => x._cockID == cockID);

        if(tick != null && tick.Any())
        {
            return tick.Sum(x => x._betMoney);
        }
        return 0;
    }

    public FightData()
    {

    }
    public FightData(FightData f)
    {
        this._id = f._id;
        this._tickets = new List<TicketData>(f.Tickets);
        this._isEnded = f.IsEnded;
        this._idCockWin = f.IDCockWIn;
    }

    public TicketData CreateEmptyTicket()
    {
        _tickets ??= new List<TicketData>();
        return new TicketData()
        {
            _id = _tickets.Count,
        };
    }
    public void PushBackDataTicket(List<TicketData> tickets)
    {
        _tickets = new List<TicketData>(tickets);
    }
    public void BuyTicket(string buyerID, int cockID, long betMoney)
    {
        _tickets ??= new List<TicketData>();
        TicketData tick = new TicketData()
        {
            _id = _tickets.Count,
            _buyerID = buyerID,
            _cockID = cockID,
            _betMoney = betMoney
        };
        _tickets.Add(tick);
    }
    public FightData EndFight(int winingCockID)
    {
        _tickets ??= new List<TicketData>();
        if(_tickets.Count > 0)
        {
            for (int i = 0; i < this._tickets.Count; i++)
            {
                _tickets[i].EndGame(_tickets[i]._cockID == winingCockID);
            }
        }

        this._isEnded = true;
        this._idCockWin = winingCockID;

        return new FightData(this);
    }

}
#endregion


#region Ticket 

[System.Serializable]
public class TicketData
{
    public int _id;

    public string _buyerID;
    public int _cockID;

    public long _betMoney;

    //these var will be set when the game ended, by manager;
    public bool _isThisTicketWon;
    public long _wonMoney;

    public TicketData()
    {

    }
    public TicketData(TicketData t)
    {
        this._id = t._id;
        this._buyerID = t._buyerID;
        this._cockID = t._cockID;
        this._betMoney = t._betMoney;
        this._isThisTicketWon = t._isThisTicketWon;
        this._wonMoney = t._wonMoney;
    }
    public void EndGame(bool isWon)
    {
        this._isThisTicketWon = isWon;
        this._wonMoney = (long)(this._betMoney * GameDataManager.Instance.SettingDatas._ratioCutOfWining);
    }

    public TicketData CombineTicket(TicketData other)
    {
        if(other._cockID == this._cockID && this._buyerID == other._buyerID)
        {
            TicketData tick = new TicketData(this);
            tick._betMoney = this._betMoney + other._betMoney;
            tick._wonMoney = this._wonMoney + other._wonMoney;
            return tick;
        }

        return this;
    }
}
#endregion


#region Setting
[System.Serializable]
public class UserAppSetting
{
    public double _ratioCutOfWining = 0.95;
    public List<CockData> _cocks;

    public static UserAppSetting CreateBase()
    {
        return new UserAppSetting()
        {
            _ratioCutOfWining = 0.95,
            _cocks = new List<CockData>()
            {
                new CockData() { _cockID = 0, _cockName = "Xanh" },
                new CockData() { _cockID = 1, _cockName = "Đỏ" },
            }
        };
    }
    public string GetCockName(int id)
    {
        _cocks ??= new List<CockData>();
        CockData c = this._cocks.Find(x => x._cockID == id);
        return c?._cockName ?? "";
    }
    public int GetCockID(string cockName)
    {
        _cocks ??= new List<CockData>();
        CockData c = this._cocks.Find(x => x._cockName.Equals(cockName));
        return c?._cockID ?? 0;
    }
}

[System.Serializable]
public class CockData
{
    public int _cockID;
    public string _cockName;
}
#endregion