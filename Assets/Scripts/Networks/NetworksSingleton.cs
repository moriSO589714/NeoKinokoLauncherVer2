using Google.Apis.Sheets.v4;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ネットワーク関係の値を保持しておくシングルトンクラス
/// API等などにより取得に時間がかかるものは基本的にこのクラスに入れておく
/// </summary>
public class NetworksSingleton : BasedSingleton<NetworksSingleton>
{
    private List<string> SpreadSheetElementOrder = new List<string>();
    private int LiminalRow = -1;

    public List<string> ReturnElementOrder()
    {
        if (SpreadSheetElementOrder.Count != 0)
        {
            return new List<string>(SpreadSheetElementOrder);
        }
        else
        {
            GetElementOrder();
            return new List<string>(SpreadSheetElementOrder);
        }
    }

    public void GetElementOrder()
    {
        AllDirs allDirs = AllDirs.GetInstance();
        List<string> sheetElementOrder = new CollectivelyGetFromSpSt().GetElementTypeArray(allDirs.JsonPathKey, allDirs.SpreadSheetID);
        if (sheetElementOrder == null || sheetElementOrder.Count <= 0) throw new System.Exception("failed to get sheetElement");

        SpreadSheetElementOrder = new List<string>(sheetElementOrder);
    }

    public int ReturnLiminalRow()
    {
        if(LiminalRow != -1)
        {
            return LiminalRow;
        }
        else
        {
            GetLiminalRow();
            return LiminalRow;
        }
    }

    public void GetLiminalRow()
    {
        NetworksSingleton networksSingleton = NetworksSingleton.Instance;
        List<string> spreadSheetElementOrder = networksSingleton.ReturnElementOrder();
        AllDirs allDirs = AllDirs.GetInstance();

        //GameIDが記載されている列数を取得する
        int numberofColumns = new SpreadSheetTools().IndextoSSColumn(spreadSheetElementOrder.IndexOf("GameID"));
        Vector2 gameIDStartCell = new Vector2(numberofColumns, allDirs.SpreadSheetStartCellPos.y);
        SheetsService sheetsService = new SpreadSheetBased().CreateSpStAPI(allDirs.JsonPathKey);
        int liminalRow = new SpreadSheetBased().ReturnRowTableLastCell(sheetsService, allDirs.SpreadSheetID, new Vector2(numberofColumns, 3),(int)SearchUnit.LargeRange);

        LiminalRow = liminalRow;
    }
}
