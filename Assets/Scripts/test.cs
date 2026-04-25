using DG.Tweening.Plugins;
using Google.Apis.Drive.v3;
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
        new LoadFlexibleDir().SetFlexibleDirByJson();
        string jsonPathKey = AllDirs.GetInstance().JsonPathKey;
        string spID = AllDirs.GetInstance().SpreadSheetID;
        
        SpreadSheetBased spreadSheetBased = new SpreadSheetBased();
        SheetsService sheetService = spreadSheetBased.CreateSpStAPI(jsonPathKey);
        spreadSheetBased.ReturnRowTableLastCell(sheetService, spID, new Vector2(2,1), 10);
        

        //List<GameData> getData = new SpreadSheetDataGet().AllGameDataFromSpSt(jsonPathKey, spID);
        //new GameDataManager().OverallGameDataLoad();
        //GameDatasSingleton gameDatasSingleton = GameDatasSingleton.Instance;
        Debug.Log("end");
    }
}
