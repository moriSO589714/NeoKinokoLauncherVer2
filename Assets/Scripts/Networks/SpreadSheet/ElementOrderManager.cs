using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スプレッドシートの項目名を扱うクラス
/// 各ゲームデータの処理とは独立しており、取得時は毎回APIを叩く
/// </summary>
public class ElementOrderManager
{
    OnNetGameInfo _onNetGameInfo = null;
    AllDirs _allDirs;

    public ElementOrderManager(OnNetGameInfo onNetGameInfo)
    {
        _onNetGameInfo = onNetGameInfo;
        _allDirs = AllDirs.GetInstance();
    }

    /// <summary>
    /// スプレッドシートの項目名を順番通りのリストとして取得
    /// </summary>
    /// <returns></returns>
    public List<string> GetElementOrder()
    {
        //スプレッドシートで項目名がどの位置で始まっているかの位置をスプシ上の座標(1から始まる)で取得
        Vector2 elementStartPos = _allDirs.SpStElementStartCellPos;

        List<string> elementOrderList = new List<string>();
        //項目名が書かれた行の値を全て取得
        new LastCellManager().ReturnLastCellPos(_onNetGameInfo, elementStartPos, SearchUnit.NarrowRange, DirectionOnSpSt.column ,elementOrderList);

        if (elementOrderList == null || elementOrderList.Count == 0)
        {
            throw new System.Exception("failed to get SpSt element order");
        }

        return elementOrderList;
    }
}
