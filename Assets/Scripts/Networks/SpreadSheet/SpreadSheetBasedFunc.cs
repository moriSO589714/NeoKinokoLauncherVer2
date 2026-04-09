using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System;
using System.IO;
using UnityEngine;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

public class SpreadSheetBasedFunc
{
    /// <summary>
    /// スプレッドシート接続用のAPIを作成する。
    /// </summary>
    /// <returns></returns>
    public SheetsService CreateSSAPI()
    {
        GoogleCredential credential;
        AllDirs allDirs = AllDirs.GetInstance();
        using (var stream = new FileStream(allDirs.JsonPathKey, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.ScopeConstants.Spreadsheets);
        }

        SheetsService sheetService = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Spread Sheet",
        });

        return sheetService;
    }

    /// <summary>
    /// 指定のセルの値を取得して返す
    /// </summary>
    /// <param name="startCellPosition">値を返す範囲の始めの(列,行)</param>
    /// <param name="endCellPosition">値を返す範囲の終わりの(列,行)</param>
    /// <returns>セルの値[0]A1,A2,A3...
    ///                 [1]B2,B2,B3...</returns>
    public List<List<string>> ReturnSSValue(SheetsService sheetService, Vector2 startCellPosition, Vector2 endCellPosition)
    {
        var request = sheetService.Spreadsheets.Values.Get(AllDirs.GetInstance().SpreadSheetID,"シート1!" + ChangeR1C1(startCellPosition) + ":" + ChangeR1C1(endCellPosition));
        List<List<string>> returnValues = new List<List<string>>();
        try
        {
            ValueRange response = request.Execute();
            var values = response.Values;
            foreach(IList<object> list in values)
            {
                List<string> strings = new List<string>();
                foreach(object value in list)
                {
                    strings.Add(value.ToString());
                }
                returnValues.Add(strings);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
            throw new Exception("failed to connect with SpreadSheet. Log>>>" + e);
        }

        return returnValues;
    }

    /// <summary>
    /// Vector2でのセル表記からR1C1表記に変更する
    /// </summary>
    /// <param name="cellValue">(列,行)</param>
    /// <returns></returns>
    public string ChangeR1C1(Vector2 cellValue)
    {
        string returnValue = "R" + cellValue.x.ToString() + "C" + cellValue.y.ToString();
        return returnValue;
    }
}
