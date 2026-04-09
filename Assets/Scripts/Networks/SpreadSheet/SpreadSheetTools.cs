using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using UnityEngine;

public class SpreadSheetTools : SpreadSheetBased
{
    /// <summary>
    /// スプレッドシートから要素名(GameDataのjsonと共通)を取り出してきて、スプシにある順番で配列に格納して返す
    /// (重い処理なので複数走らせない)
    /// </summary>
    public List<string> GetElementTypeArray(string jsonKeyPath, string sheetID)
    {
        SheetsService sheetService = CreateSSAPI(jsonKeyPath);

        Vector2 targetPos = new Vector2(2, 2);
        List<string> elementTypeArray = new List<string>();

        bool flag = true;
        while (flag)
        {
            List<List<string>> rangeValues = ReturnSSValue(sheetService, sheetID, targetPos, targetPos);

            if(rangeValues == null)
            {
                flag = false;
            }
            else
            {
                string value = rangeValues[0][0];
                elementTypeArray.Add(value);
                targetPos.y += 1;
            }
        }

        if(elementTypeArray.Count == 0)
        {
            throw new System.Exception("empty or failed to get elementTyoe from SpreadSheet.");
        }

        return elementTypeArray;
    }


}
