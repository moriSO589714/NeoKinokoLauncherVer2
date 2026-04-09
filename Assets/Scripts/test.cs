using Google.Apis.Sheets.v4;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class test : MonoBehaviour
{
    [SerializeField] GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        new EachDataLoad().LocalDataLoad();
        SpreadSheetBasedFunc spreadSheetBasedFunc = new SpreadSheetBasedFunc();
        SheetsService sheetService =  spreadSheetBasedFunc.CreateSSAPI();
        List<List<string>> values = spreadSheetBasedFunc.ReturnSSValue(sheetService, new Vector2(1, 1), new Vector2(3, 2));

        /*
        new EachDataLoad().LocalDataLoad();
        obj.GetComponent<GameBoxsManager>().SetGameBoxsByGameDataList(GameDatasSingleton.Instance.GameDatas);
        Debug.Log("End");
        */
    }
}
