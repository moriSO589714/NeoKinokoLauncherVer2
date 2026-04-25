using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using System;
using System.IO;
using UnityEngine;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SpreadSheetBased
{
    //スプレッドシート上で2つの座標から間の個数を求める際に値を修正するため
    const int FixVectorDiffenceToArrayLength = 1;

    /// <summary>
    /// スプレッドシート接続用のAPIを作成する。
    /// </summary>
    /// <returns></returns>
    public SheetsService CreateSpStAPI(string jsonKeyPath)
    {
        GoogleCredential credential;
        using (var stream = new FileStream(jsonKeyPath, FileMode.Open, FileAccess.Read))
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
    /// 指定したセルの値を取得して返す
    /// </summary>
    /// <param name="startCellPosition">値を返す範囲の始めの(列,行)</param>
    /// <param name="endCellPosition">値を返す範囲の終わりの(列,行)</param>
    /// <returns>セルの値[0]A1,A2,A3...
    ///                 [1]B2,B2,B3...
    ///                 ※空セルの場合nullを返す！注意！</returns>
    public List<List<string>> ReturnSSValue(SheetsService sheetService, string sheetID, Vector2 startCellPosition, Vector2 endCellPosition)
    {
        if (sheetID == null) throw new Exception("failed to get cell value. dont load ssid");
        //リクエストのシート名は省略することで、1番始めのシートが自動で選択される(qiita行き？)
        var request = sheetService.Spreadsheets.Values.Get(sheetID, ChangeToR1C1(startCellPosition) + ":" + ChangeToR1C1(endCellPosition));
        List<List<string>> returnValues = new List<List<string>>();
        try
        {
            ValueRange response = request.Execute();
            var values = response.Values;

            if (values == null) return null;
            foreach(IList<object> list in values)
            {
                List<string> strings = new List<string>();
                foreach(object value in list)
                {
                    strings.Add(value.ToString());
                }
                returnValues.Add(strings);
            }

            //spread sheet APIで無視される末尾の空セルを埋める(行方向のみ)
            int columnLength = ((int)endCellPosition.x - (int)startCellPosition.x) + 1;
            foreach(List<string> value in returnValues)
            {
                if(value.Count < columnLength)
                {
                    for(int i = 0; i < columnLength - value.Count; i++)
                    {
                        value.Add("");
                    }
                }
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
    /// ２次元配列のリスト(ReturnSSValueで取得したセルバリュー)を座標をKeyにしたDictionaryに変換して返す
    /// </summary>
    /// <param name="wList"></param>
    /// <returns>[セル座標(列,行),セルの値]のDictionary</returns>
    public Dictionary<Vector2,string> ConvertWListintoDictionary(List<List<string>> wList)
    {
        Dictionary<Vector2,string> convertedDictionary = new Dictionary<Vector2,string>();
        SpreadSheetTools spreadSheetTools = new SpreadSheetTools();
        for(int i = 0; i < wList.Count; i++)
        {
            for(int i2 = 0; i2 < wList[i].Count; i2++)
            {
                Vector2 cellPos = new Vector2(spreadSheetTools.IndextoSSColumn(i2), spreadSheetTools.IndextoSSRow(i));
                convertedDictionary[cellPos] = wList[i][i2];
            }
        }

        return convertedDictionary;
    }

    /// <summary>
    /// 特定のセルから列または行方向において最後にあるセル座標を取得する
    /// </summary>
    /// <param name="startPos">検索の開始地点とするセルの座標</param>
    /// <param name="oneTimeSearchUnit">1回で検索するセルの長さ。最後に到達しない場合はもう一度この長さで検索する</param>
    /// <returns></returns>
    public int ReturnRowTableLastCell(SheetsService sheetService, string sheetId, Vector2 startPos, int oneTimeSearchUnit)
    {
        bool loopFlag = true;
        Vector2 endSearchPos = Vector2.zero;
        int addNum = oneTimeSearchUnit - 1; //足す際にセル数とズレてしまうため

        int lastListLength = 0;
        while (loopFlag)
        {
            endSearchPos = new Vector2(startPos.x, startPos.y + addNum);
            if (isInLine(startPos, endSearchPos)) throw new Exception("range is not in one line");
            
            List<List<string>> returnValues = ReturnSSValue(sheetService, sheetId, startPos, endSearchPos);
            if(returnValues == null)
            {
                lastListLength = 0;
                loopFlag = false;
            }

            //セルの範囲がstartPos～endPosの範囲に収まっているかの確認
            if(isInRange(returnValues.Count, (int)startPos.y, (int)endSearchPos.y))
            {
                lastListLength = returnValues.Count;
                loopFlag = false; //収まっていたらループを抜ける
            }
            else
            {
                startPos = new Vector2(endSearchPos.x, endSearchPos.y + 1);
            }
        }
        //スプレッドシートの最後の列数を求める
        int lastCellRow = (int)endSearchPos.y - (oneTimeSearchUnit - lastListLength);
        return lastCellRow;
    }

    public int ReturnColumnTableLastCell(SheetsService sheetService, string sheetId, Vector2 startPos, int oneTimeSearchUnit)
    {
        bool loopFlag = true;
        Vector2 endSearchPos = Vector2.zero ;

        int lastListLength = 0;
        while (loopFlag)
        {
            endSearchPos = new Vector2(startPos.x + oneTimeSearchUnit, startPos.y);
            if (isInLine(startPos, endSearchPos)) throw new Exception("range is not in one line");

            List<List<string>> returnValues = ReturnSSValue(sheetService, sheetId, startPos, endSearchPos);
            if (returnValues == null)
            {
                lastListLength = 0;
                loopFlag = false;
            }

            //セルの範囲がstartPos～endPosの範囲に収まっているかの確認
            if (isInRange(returnValues[0].Count, (int)startPos.x, (int)endSearchPos.x))
            {
                lastListLength = returnValues[0].Count;
                loopFlag = false; //収まっていたらループを抜ける
            }
            else
            {
                startPos = new Vector2(endSearchPos.x + 1, endSearchPos.y);
            }
        }
        int lastCellColumn = (int)endSearchPos.x - (oneTimeSearchUnit - lastListLength);
        return lastCellColumn;
    }

    private bool isInRange(int listLength, int startPos, int endPos)
    {
        //引数から必要な要素の個数を取得する
        int needLength = (endPos - startPos) + FixVectorDiffenceToArrayLength;

        if(needLength > listLength) //取得してきた値が範囲内に収まっている場合
        {
            return true;
        }
        else if (needLength <= listLength)
        {
            return false;
        }
        else
        {
            throw new Exception("range error");
        }
    }

    /// <summary>
    /// startPosとendPosがシート内で直線に並んでいるか確かめる
    /// </summary>
    private bool isInLine(Vector2 startPos, Vector2 endPos)
    {
        if (startPos.x != endPos.x && startPos.y != endPos.y) return true;
        return false;
    }

    /// <summary>
    /// Vector2でのセル表記からR1C1表記に変更する
    /// </summary>
    /// <param name="cellValue">(列,行)</param>
    /// <returns></returns>
    public string ChangeToR1C1(Vector2 cellValue)
    {
        string returnValue = "R" + cellValue.y.ToString() + "C" + cellValue.x.ToString();
        return returnValue;
    }
}

public enum SearchUnit 
{
    NarrowRange = 10,
    LargeRange = 100,
}

