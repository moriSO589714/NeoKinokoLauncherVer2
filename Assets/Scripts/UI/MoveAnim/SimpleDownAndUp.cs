using DG.Tweening;
using UnityEngine;

/// <summary>
/// DoTweenを用いたオブジェクトの上昇/下降と初期位置へ戻るアニメーション
/// </summary>
public class SimpleDownAndUp
{
    private GameObject _targetObj;
    private Vector2 _firstPosition;
    private Vector2 _moveDistance;
    private float _moveSeconds;
    private float _removeSeconds;

    private Tween _runtimeTween;
    public SimpleDownAndUp(GameObject obj, Vector2 moveDistance, float moveSeconds, float removeSeconds)
    {
        _targetObj = obj;
        _firstPosition = obj.transform.position;
        _moveDistance = moveDistance;
        _moveSeconds = moveSeconds;
        _removeSeconds = removeSeconds;
    }

    /// <summary>
    /// 対象のオブジェクトを移動先まで動かす
    /// </summary>
    public void MoveObject()
    {
        if (_runtimeTween != null) _runtimeTween.Kill();
        //目的先の座標を出す
        Vector2 targetPos = _firstPosition + _moveDistance;
        _runtimeTween = _targetObj.transform.DOMove(targetPos, _moveSeconds);
    }

    /// <summary>
    /// 対象のオブジェクトを開始座標に戻す
    /// </summary>
    public void RemoveObject()
    {
        if (_runtimeTween != null) _runtimeTween.Kill();
        _runtimeTween = _targetObj.transform.DOMove(_firstPosition, _moveSeconds);
    }
}
