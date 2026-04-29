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
        Vector2 startPos = new Vector2(20,20);
        Vector2 endPos = new Vector2(30,30);
        List<List<string>> listlist = new TestOnNetGameInfo().GetGameInfoFromNet(startPos,endPos);

        /*
        new LoadFlexibleDir().SetFlexibleDirByJson();
        string jsonPathKey = AllDirs.GetInstance().JsonPathKey;
        string spID = AllDirs.GetInstance().SpreadSheetID;
        
       
        SheetsService sheetService = new CreateAPIService(jsonPathKey).CreateSheetAPIService();
        OnNetGameInfo onNetGameInfo = new TestOnNetGameInfo();

        CollectivelyGetFromSpSt collectivelyGetFromSpSt = new CollectivelyGetFromSpSt();
        List<GameData> gotGameData = collectivelyGetFromSpSt.AllGameDataFromSpSt(onNetGameInfo);
        */
        //List<GameData> getData = new SpreadSheetDataGet().AllGameDataFromSpSt(jsonPathKey, spID);
        //new GameDataManager().OverallGameDataLoad();
        //GameDatasSingleton gameDatasSingleton = GameDatasSingleton.Instance;
        Debug.Log("end");
    }
}
