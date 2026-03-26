using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 自由に変更することができるディレクトリ(ゲームが入っているフォルダなど)の場所を取得する
/// </summary>
public class LoadFreedamDir
{
    //jsonファイルからDirectoryPathsに各フォルダのパスをセットする
    public void SetFreedamDirByJson()
    {
        DirInfo dirInfo = new DirInfo();
        //パスが記載されたjsonファイルのパスを取得する
        string jsonFilePath = new DirPathsSingleton().DefinedDataPath;
        //jsonデータを読み取って代入
        dirInfo = new JSONTools().ReadJSON<DirInfo>(jsonFilePath);

        /*========================================================================================
         *ローカルファイル(jsonデータ)から読み取ったパスをシングルトンとしてフォルダの場所を指定しているDirectoryPathsクラスに代入していく
         *以下の処理を挟むのは、JsonUtilityはJSON側で指定されていない変数が代入側のクラスにあった場合nullを代入していしまうため。
         *(代入されるシングルトンで元々変数に入っているデータがnullに上書きされるのを防止するため)　※Qiitaに移動
        ==========================================================================================*/
        //typeof演算子を使い、System.Type(型情報)としてクラスを取得、GetFields()でフィールドを取得する。
        PropertyInfo[] props = typeof(DirInfo).GetProperties();
        FieldInfo[] fields = typeof(DirInfo).GetFields();
        
        DirPathsSingleton dirPaths = DirPathsSingleton.GetInstance();

        //インターフェースから取得できたプロパティ(ここでは定義されている変数)を利用し、FreedamDirPathsクラスとDirectoryPathsクラスとを比較する
        foreach (FieldInfo field in fields)
        {
            var targetValue = field.GetValue(dirInfo);
            if (targetValue != "" || targetValue != null)
            {
                field.SetValue(dirPaths, targetValue);
            }
        }
    }

}
