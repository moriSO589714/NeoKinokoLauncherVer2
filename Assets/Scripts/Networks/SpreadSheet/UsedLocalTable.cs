using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ローカル上にあるデータテーブル(List<List<string>>)を扱う為のクラス
/// </summary>
public static class UsedLocalTable
{
    /// <summary>
    /// ローカルのテーブルから任意の範囲で値を切り出す
    /// </summary>
    /// <param name="startPos">始点座標(※0から始まる配列のindex値)</param>
    /// <param name="endPos">終点座標(※0から始まる配列のindex値)</param>
    /// <returns></returns>
    public static List<List<string>> TrimValueRangeFromLocalTable(List<List<string>> localTable, Vector2 startPos, Vector2 endPos)
    {
        int lowestColumnNum = Math.Min((int)startPos.x, (int)endPos.x);
        int lowestRowNum = Math.Min((int)startPos.y, (int)endPos.y);
        int highestColumnNum = Math.Max((int)startPos.x, (int)endPos.x);
        int highestRowNum = Math.Max((int)startPos.y, (int)endPos.y);
        int allDataLastIndex = SpStTools.LengthToLastIndex(localTable.Count);
        //指定された範囲のデータがテスト用のデータに無かった場合nullを返す
        if (allDataLastIndex < lowestRowNum) return null;
        int maxListsLength = 0;
        foreach (List<string> list in localTable)
        {
            maxListsLength = maxListsLength < list.Count ? list.Count : maxListsLength;
        }
        if (SpStTools.LengthToLastIndex(maxListsLength) < lowestColumnNum) return null;

        List<List<string>> UsedDataList = new List<List<string>>();
        //リストの２次元配列から指定範囲を切り取る
        for (int i = lowestRowNum; i <= highestRowNum; i++)
        {
            //i列のデータが存在しない場合
            if (allDataLastIndex < i || i == -1)
            {
                break;
            }
            List<string> addDataList = new List<string>();
            for (int i2 = lowestColumnNum; i2 <= highestColumnNum; i2++)
            {
                if (SpStTools.LengthToLastIndex(localTable[i].Count) < i2)
                {
                    continue;
                }
                else
                {
                    addDataList.Add(localTable[i][i2]);
                }
            }
            UsedDataList.Add(addDataList);
        }

        return UsedDataList;
    }
}
