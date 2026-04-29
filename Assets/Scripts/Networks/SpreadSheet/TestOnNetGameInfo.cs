using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// API通信を行わず、スプレッドシートの値を仮で返す
/// </summary>
public class TestOnNetGameInfo : OnNetGameInfo
{
    public List<List<string>> GetGameInfoFromNet(Vector2 startPos, Vector2 endPos)
    {
        List<List<string>> alternaDataSet = AlternaDatas.SpStAlternaDatas;
        int lowestColumnNum = startPos.x <= endPos.x ? (int)startPos.x :(int)endPos.x;
        int lowestRowNum = startPos.y <= endPos.y ? (int)startPos.y :(int) endPos.y;
        int highestColumnNum = startPos.x >= endPos.x ? (int)startPos.x : (int)endPos.x;
        int highestRowNum = startPos.y >= endPos.y ? (int)startPos.y :(int)endPos.y;

        //指定された範囲のデータがテスト用のデータに無かった場合nullを返す
        if (alternaDataSet.Count < lowestRowNum) return null;
        int maxListsLength = 0;
        foreach(List<string> list in alternaDataSet)
        {
            maxListsLength = maxListsLength < list.Count ? list.Count : maxListsLength;
        }
        if (maxListsLength < lowestColumnNum) return null;

        List<List<string>> UsedDataList = new List<List<string>>();
        //テスト用のデータ(リストの２次元配列)から指定範囲を切り取る
        for(int i = lowestRowNum; i <= highestRowNum; i++)
        {
            //i列のデータが存在しない場合
            if(alternaDataSet.Count < i || i == 0)
            {
                break;
            }
            List<string> addDataList = new List<string>();
            for(int i2 = lowestColumnNum; i2 <= highestColumnNum; i2++)
            {
                if (alternaDataSet[i - 1].Count < i2)
                {
                    continue;
                }
                else
                {
                    addDataList.Add(alternaDataSet[i - 1][i2 - 1]);
                }
            }
            UsedDataList.Add(addDataList);
        }

        return new List<List<string>>(UsedDataList);
    }
}
