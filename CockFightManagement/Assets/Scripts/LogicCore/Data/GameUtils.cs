using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static System.DateTime GetTodayDate()
    {
        return System.DateTime.Now.Date;
    }
    public static string FormatMoneyDot(this long money, string separator = ".")
    {
        bool isNegative = false;
        if (money < 0)
        {
            money = -money;
            isNegative = true;
        }

        string result = money.ToString();
        int index = result.Length - 1;
        int split = 0;
        while (index > 0)
        {
            split++;
            if (split % 3 == 0)
            {
                result = result.Insert(index, "" + separator);
                split = 0;
            }
            index--;
        }
        if (isNegative)
            result = "-" + result;
        return result;
    }
    public static string FormatMoneyDot(this decimal money, string separator = ".")
    {
        bool isNegative = false;
        if (money < 0)
        {
            money = -money;
            isNegative = true;
        }

        string formatString = "#,##0";

        if (money % 1 != 0)
        {
            int decimalPlaces = System.BitConverter.GetBytes(decimal.GetBits(money)[3])[2];
            formatString += "." + new string('0', decimalPlaces);
        }
        string result = money.ToString(formatString, new System.Globalization.CultureInfo("vi-VN"));
        if (isNegative)
            result = "-" + result;

        return result;
    }

    /// <summary>
    /// Gộp tất cả các phiếu lại thành phiếu record theo từng buyer
    /// Format sẽ trông như sau:
    /// IDPlayer - TotalBetMoneyOfBlueCock - TotalBetMoneyOfRedCock
    /// Hoặc
    /// IDPlayer - TotalWonMoneyOfBlueCock - TotalWonMoneyOfRedCock
    /// </summary>
    /// <returns></returns>
    public static List<TableMultipleCockTicketData> ValidateAndCombineTicketToSinglePlayer(List<TicketData> ticketDatas)
    {
        List<TableMultipleCockTicketData> res = new List<TableMultipleCockTicketData>();
        Dictionary<string, TableMultipleCockTicketData> dicPlayers = new Dictionary<string, TableMultipleCockTicketData>();
        foreach (TicketData item in ticketDatas)
        {
            if (dicPlayers.TryGetValue(item._buyerID, out TableMultipleCockTicketData combineTicket))
            {
                combineTicket.CombineTicket(item);
            }
            else
            {
                TableMultipleCockTicketData newCombine = new TableMultipleCockTicketData()
                {
                    _id = res.Count,
                    _buyerID = item._buyerID,
                };
                newCombine.CombineTicket(item);
                res.Add(newCombine);
                dicPlayers.Add(item._buyerID, newCombine);
            }
        }
        return new List<TableMultipleCockTicketData>(dicPlayers.Values);
    }
}

public static class GameDefine
{
    public const string USER_INFO_DATA = "USER_INFO_DATA";
    public const string USER_SETTING_DATA = "USER_SETTING_DATA";

}

public class FightDocument
{
    public System.DateTime _date;
    public string _fightName;
    List<TableMultipleCockTicketData> _ticketsCombinedByPlayers;

    public FightDocument()
    {

    }
    public FightDocument(System.DateTime date, string fightName, List<TableMultipleCockTicketData> ticketsCombinedByPlayers)
    {
        this._date = date;
        this._fightName = fightName;
        this._ticketsCombinedByPlayers = ticketsCombinedByPlayers;
    }

    public List<string> ToDocument()
    {
        List<string> strings = new List<string>();
        strings.Add($"Ngày: {this._date.ToShortDateString()}");
        strings.Add($"Tran: {_fightName}");
        strings.Add("");
        strings.Add("");

        //strings.Add($"Người chơi\t\t Thu \t\t Chung \t\t Tổng");

        foreach (TableMultipleCockTicketData item in this._ticketsCombinedByPlayers)
        {
            strings.Add(string.Format(TicketCombineToDocument(item)));
            strings.Add("");
        }

        return strings;

        string TicketCombineToDocument(TableMultipleCockTicketData ticket)
        {
            TicketData TicketOfACock(TableMultipleCockTicketData combinedData, int cockID)
            {
                TicketData combineTick = combinedData.GetCombinedTickOfACock(cockID: cockID);
                decimal wonIfWinOrBetIfLose = combineTick?._wonMoney?.Value ?? 0;
                return combineTick;
            }

            TicketData tickBlue = TicketOfACock(ticket, cockID: 0);
            TicketData tickRed = TicketOfACock(ticket, cockID: 1);

            decimal betBlue = tickBlue?._betMoney?.Value ?? 0;
            decimal betRed = tickRed?._betMoney?.Value ?? 0;
            decimal winBlue = tickBlue?._wonMoney?.Value ?? 0;
            decimal winRed = tickRed?._wonMoney?.Value ?? 0;

            decimal actualWinBlue = winBlue + betBlue;
            decimal actualWinRed = winRed + betRed;

            string Line0 = $"Nguoi choi: {ticket._buyerID}";
            string Line1 = $"                   \t\t Xanh       :\t\tMua:\t\t  {betBlue.FormatMoneyDot()}\t\t   Chung:\t\t  {(actualWinBlue.FormatMoneyDot())}";
            string Line2 = $"                   \t\t Do          :\t\tMua:\t\t  {betRed.FormatMoneyDot()}\t\t   Chung:\t\t  {(actualWinRed.FormatMoneyDot())}";
            string Line3 = $"                   \t\t Tong       :\t\tThu:\t\t  {(betBlue + betRed).FormatMoneyDot()}\t\t    Tra Lai:\t\t  {(actualWinBlue + actualWinRed).FormatMoneyDot()}";

            return $"{string.Format(Line0)}\n{string.Format(Line1)}\n{string.Format(Line2)}\n{string.Format(Line3)}";
        }
    }
}
