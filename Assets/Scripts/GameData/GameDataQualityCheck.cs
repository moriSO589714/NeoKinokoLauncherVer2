using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using Unity.VisualScripting.Dependencies.NCalc;

/// <summary>
/// GameDataオブジェクトがゲームとして表示する最低要件をクリアしているか確認する
/// </summary>
public static class GameDataQualityCheck
{
    public static bool CheckQuality(GameData gameData)
    {
        //GameDataクラスのフィールドを取得
        FieldInfo[] gameDataFieldInfo = typeof(GameData).GetFields();
        foreach(var field in gameDataFieldInfo)
        {
            //対象とされるフィールドにMustItem属性が付いているかを確認(必要とされる条件か？)
            if(Attribute.IsDefined(field, typeof(MustItemAttribute)))
            {
                var value = field.GetValue(gameData);
                if(value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
