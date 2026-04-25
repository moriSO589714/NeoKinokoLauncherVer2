using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 保存されているゲームのデータを管理するシングルトンクラス
/// </summary>
public class GameDatasSingleton : BasedSingleton<GameDatasSingleton>
{
    public List<GameData> GameDatas { get; private set; } = new List<GameData>();
    
    //ゲームデータをリストにセット
    public void AddGameData(GameData gameData)
    {
        //追加
        if(CheckGameData(gameData)) GameDatas.Add(gameData);
    }
    public void AddGameDataList(List<GameData> gameDatas)
    {
        List<GameData> addGameDataList = new List<GameData>();
        foreach(GameData gameData in gameDatas)
        {
            if(CheckGameData(gameData)) addGameDataList.Add(gameData);
        }
        GameDatas.AddRange(addGameDataList);
    }

    private bool CheckGameData(GameData gameData)
    {
        foreach(GameData singletonGameData in GameDatas)
        {
            //シングルトンに既に同じゲームIDのゲーム情報が登録されている場合
            if(singletonGameData.GameID == gameData.GameID)
            {
                //追加されるゲームのバージョンが新しい場合
                if(int.Parse(singletonGameData.GameVersion) < int.Parse(gameData.GameVersion))
                {
                    singletonGameData.Status = GameStatus.UpdateAvailable;
                }
                return false;
            }
        }

        //ゲーム情報として扱う最低要件を満たせていない場合は追加しない
        if (!GameDataQualityCheck.CheckQuality(gameData)) return false;
        return true;
    }



    //リストのリセット
    public void ResetGameDataList()
    {
        GameDatas = new List<GameData>();
    }
}

