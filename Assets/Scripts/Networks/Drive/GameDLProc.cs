using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDLProc
{
    private OnNetDriveMetaData _onNetDriveMetaData = null;
    private OnNetDriveGetFile _onNetDriveFetFile = null;

    public GameDLProc(OnNetDriveMetaData metaData, OnNetDriveGetFile onNetDriveFetFile)
    {
        _onNetDriveMetaData = metaData;
        _onNetDriveFetFile = onNetDriveFetFile;
    }

    /// <summary>
    /// 固有IDからゲームデータをローカルに保存する
    /// </summary>
    public void DLGame(GameData gamedata)
    {
        DirectoryActs directoryActs = new DirectoryActs();
        string gameId = gamedata.GameID;
        string driveId = gamedata.GameDriveId;
        AllDirs allDirs = AllDirs.GetInstance();

        //他のゲームがダウンロードされているディレクトリに同じIDのゲームが保存されていないか確認する処理
        if(File.Exists(Path.Combine(allDirs.GameFilePath, gameId)))
        {
            throw new System.Exception("既にダウンロードフォルダに同じIDのゲームが保存されています。削除し、再度実行してください。ID >>>" + gameId);
        }
        //既に途中までダウンロードしていないかを確認する処理

        //保存用一時ディレクトリの作成
        string tempDirPath = AllDirs.GetInstance().TempPath;
        directoryActs.CreateAndCheckDir(tempDirPath);
        string tempGameDLPath = Path.Combine(tempDirPath, gameId);
        string tempSlicedGameDLPath = Path.Combine(tempGameDLPath, "sliced");
        directoryActs.CreateAndCheckDir(tempSlicedGameDLPath);

        //メタデータを取得してくる
        Dictionary<string, string> metaDic = _onNetDriveMetaData.GetFileList(driveId);

        Dictionary<string, string> dlDataFileNameAndDriveId = metaDic.Where(x => x.Key.EndsWith(".000")).ToDictionary(x => x.Key, x => x.Value);
        //.000ファイルの数が不正な場合
        if (dlDataFileNameAndDriveId == null || dlDataFileNameAndDriveId.Count == 0 || dlDataFileNameAndDriveId.Count > 1)
        {
            throw new System.Exception("ドライブへの接続に失敗しました。ドライブにフォルダが存在しないか、ドライブにある'.00'形式のファイル数が不正です");
        }
        string dlDataFileName = dlDataFileNameAndDriveId.Keys.First();
        string dlDataDriveId = dlDataFileNameAndDriveId.Values.First();
        //.000ファイル(DLDataが書かれたファイル)をダウンロードする
        _onNetDriveFetFile.GetFile(dlDataDriveId, dlDataFileName, tempSlicedGameDLPath);

        //.000ファイルからインストールするゲームの情報を取得
        DLData gameDLData = new DLData();
        gameDLData.DeserializeDataByFilePath(Path.Combine(tempSlicedGameDLPath, dlDataFileName));

        //ダウンロードするファイルの数を取得
        long maxDLfiles = gameDLData.SplitedFileNum;
        //ダウンロード済みのファイルの数
        long nowDLedFileCounts = 0;
        //拡張子が.000以外のファイルを進捗を表示させながら保存する
        foreach(var pair in metaDic)
        {
            if (pair.Key.EndsWith(".000")) continue;
            _onNetDriveFetFile.GetFile(pair.Value, pair.Key, tempSlicedGameDLPath);
            nowDLedFileCounts++;
            Debug.Log(nowDLedFileCounts + "まで終了");
        }

        //保存したファイル群をマージする(FileCombineに不足ファイルが合った際に呼ぶ処理を登録できる)
        string gameTempPath = new FileCombine().MergeSplitedFile(tempSlicedGameDLPath, tempGameDLPath);
        string gameFileName = Path.GetFileName(gameTempPath);
        string newGamePath = Path.Combine(allDirs.GameFilePath, gameFileName);
        //ゲームデータを一時保存用のフォルダから他のゲームも保存されているフォルダに移動する
        Directory.Move(gameTempPath, newGamePath);

        gamedata.Status = GameStatus.Downloaded;
        string thisGameJsonPath = Path.Combine(allDirs.JsonsDirPath, gameId + ".json");
        //ダウンロード済みデータとしてjsonに保存
        JSONTools.SerializeJson(gamedata, thisGameJsonPath);

        //ダウンロードに利用した一時保存関係のファイル・フォルダを全て削除する
        new DirectoryActs().CompleteDirDelete(tempGameDLPath);
    }
}
