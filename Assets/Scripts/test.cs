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
        
        List<string> testList = new List<string>
        {
            "weather clear",
            "weather rain",
            "weather cloud",
            "will clear",
            "kill me",
            "kill you",
            "tp you me",
            "tp you you",
            "summon cat 2",
            "summon cat 4",
            "summon cat 43",
            "summon cat 45",
            "tp you you me",
            "skill me add water",
            "skill me add wall",
            "skill me add waly",
            "skill me add fire",
            "skill me atract you",
            "skill me",
            "skill you atract me"
        };

        Dictionary<string, int> testDic = new Dictionary<string, int>() 
        {
            { "action", 3},
            { "actor", 2},
            { "ander", 0},
            { "actionslabe", 5}
        };
        
        //数字でディクショナリ分けしてくれない問題 summon cat 4...
        WordEmtCell wecLib = WECLibCreater.CreateLibFromStrList(testList, " ");
        WordEstimater wordEstimater = new WordEstimater(wecLib, " ");
        List<string> strs = wordEstimater.ReturnEstimatedStrs("skill me", -1);
        List<string> strs2 = wordEstimater.ReturnEstimatedStrs("skill me ", -1);
        

        WordEmtCell wecLib2 = WECLibCreater.CreateLibFromLineAndPriority(testDic);
        WordEstimater wordEstimater2 = new WordEstimater(wecLib2, " ");
        List<string> strs3 = wordEstimater2.ReturnEstimatedStrs("act", 0);
        List<string> strs4 = wordEstimater2.ReturnEstimatedStrs("a", 1);

        Debug.Log("end");
    }
}
