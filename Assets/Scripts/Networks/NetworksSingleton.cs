using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ネットワーク関係の値を保持しておくシングルトンクラス
/// API等などにより取得に時間がかかるものは基本的にこのクラスに入れておく
/// </summary>
public class NetworksSingleton : BasedSingleton<NetworksSingleton>
{
    private List<string> _spreadSheetElementOrder = null;
    private List<List<string>> _allGameDataOnSpSt = null;
    private int _liminalRow = -1;

    public List<string> ReturnElementOrder(bool forceLoad)
    {
        if (_spreadSheetElementOrder != null && !forceLoad)
        {
            return new List<string>(_spreadSheetElementOrder);
        }
        else
        {
            try
            {
                _spreadSheetElementOrder = new GetDataFromSpStAPI().GetElementOrder();
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            return _spreadSheetElementOrder;
        }
    }

    public int ReturnLiminalRow(bool forceLoad)
    {
        if(_liminalRow != -1 && !forceLoad)
        {
            return _liminalRow;
        }
        else
        {
            try
            {
                List<string> elementOrder = ReturnElementOrder(false);
                _liminalRow = new GetDataFromSpStAPI().GetLiminalRow(elementOrder);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return _liminalRow;
        }
    }


    public List<List<string>> ReturnGameInfoAllData(bool forceLoad)
    {
        if(_allGameDataOnSpSt != null && !forceLoad)
        {
            return _allGameDataOnSpSt;
        }
        else
        {
            try
            {
                int liminalRow = ReturnLiminalRow(false);
                List<string> elementOrder = ReturnElementOrder(false);
                _allGameDataOnSpSt = new GetDataFromSpStAPI().GetAllGameData(liminalRow, elementOrder);
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            return _allGameDataOnSpSt;
        }
    }
}
