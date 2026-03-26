using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パス関係の処理で共通する変数をプロパティ化してまとめたインターフェース
/// </summary>
public class DirInfo
{
    //基底ディレクトリ(すべてのデータがこのディレクト下に保存される)
    public string BaseDirectory;
    //ゲームが保存されるディレクトリ
    public string GameFilePath;
    //jsonFileが保存されるディレクトリ
    public string JsonFolderPath;
    //イメージ写真が保存されるディレクトリ
    public string ImageFolderPath;
}
