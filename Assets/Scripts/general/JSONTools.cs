using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

/// <summary>
/// JSON関係の汎用的な処理をまとめたクラス
/// </summary>
public class JSONTools
{
    /// <summary>
    /// jsonデータを読み取って任意の型に代入する
    /// </summary>
    /// <param name="jsonFilePath"></param>
    /// <returns></returns>
    public T ReadJSON<T>(string jsonFilePath)
    {
        string stringDataByJSON = "";

        try
        {
            //指定パスのファイルを開く
            using (FileStream filestream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read))
            {
                //StreamReaderでFileStreamから文字を読み取る
                using (StreamReader streamreader = new StreamReader(filestream))
                {
                    stringDataByJSON = streamreader.ReadToEnd();
                }
            }
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        //UnityのJsonUtilityを利用してJSON形式のデータをGameDataクラスに代入する
        //JSONデータに不足がある場合、gameDataの各変数にはnullが入る
        //逆にGameDataクラス側に不足がある場合は代入は行われない -> GameDataの変数項目に追加が合った場合でも下位互換が可能
        //このときstringDataByJSONにはjson形式のテキストがそのまま入っている状態
        var inDataClass = JsonUtility.FromJson<T>(stringDataByJSON);

        return inDataClass;
    }
}
