using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTab : SimpleUI
{
    [SerializeField] Vector2 _moveDistance;
    [SerializeField] float _moveSeconds;
    [SerializeField] float _removeSeconds;

    public Vector2 ReturnMoveDis()
    {
        return _moveDistance;
    }

    public float ReturnMoveSeconds()
    {
        return _moveSeconds;
    }

    public float ReturnRemoveSeconds()
    {
        return _moveSeconds;
    }
}
