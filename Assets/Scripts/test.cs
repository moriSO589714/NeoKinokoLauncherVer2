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

    void Start()
    {
        new LoadFlexibleDir().SetFlexibleDirByJson();
        AllDirs allDirs = AllDirs.GetInstance();
        DriveService service = new CreateAPIService(allDirs.JsonPathKey).CreateDriveAPIService();
        OnNetDriveMetaData onNetDriveMetaData = new OnNetDriveMetaDatafromDv(service);
        OnNetDriveGetFile onNetDriveGetFile = new OnNetDriveGetFilefromDV(service);
        string driveId = "1OYVPHDX4IPq2r4ZVWzQI3cyjLGPS_36o";
        GameData testGameData = new GameData();
        testGameData.GameDriveId = driveId;
        testGameData.GameID = "aaaaaaaaaaaaaa";

        GameDLProc gameDLProc = new GameDLProc(onNetDriveMetaData, onNetDriveGetFile);
        gameDLProc.DLGame(testGameData);
        Debug.Log("end");
    }
}