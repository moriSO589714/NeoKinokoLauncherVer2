using System;
using UnityEngine;

/// <summary>
/// MainUIでのデリゲート設定などの管理を行うクラス
/// </summary>
public class ManageMainUI : MonoBehaviour
{
    [SerializeField] private MonitorPlayerInput _monitorPlayerInput;

    [SerializeField] private GameBoxsManager _gameBoxManager;
    [SerializeField] private GameObject _wifiMrkObj;
    [SerializeField] private GameObject _filterMrkObj;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        CommonStateManager commonStateManager = CommonStateManager.Instance;

        //Wifiボタンのデリゲート設定======================================================
        DownTab wifiMrkDownTab = _wifiMrkObj.GetComponent<DownTab>();
        if(wifiMrkDownTab != null)
        {
            Vector2 moveDistance = wifiMrkDownTab.ReturnMoveDis();
            float moveSeconds = wifiMrkDownTab.ReturnMoveSeconds();
            float removeSeconds = wifiMrkDownTab.ReturnRemoveSeconds();
            SimpleDownAndUp simpleDownAndUp = new SimpleDownAndUp(_wifiMrkObj, moveDistance, moveSeconds, removeSeconds);
            wifiMrkDownTab.PointerEnterAct = simpleDownAndUp.MoveObject;
            wifiMrkDownTab.PointerExitAct = simpleDownAndUp.RemoveObject;
        }
        //================================================================================

        //フィルターボタンのデリゲート設定================================================
        DownTab filterMrkDowntab = _filterMrkObj.GetComponent<DownTab>();
        if(filterMrkDowntab != null)
        {
            Vector2 moveDistance = filterMrkDowntab.ReturnMoveDis();
            float moveSeconds = filterMrkDowntab.ReturnMoveSeconds();
            float removeSeconds = filterMrkDowntab.ReturnRemoveSeconds();
            SimpleDownAndUp simpleDownAndUp = new SimpleDownAndUp(_filterMrkObj, moveDistance, moveSeconds, removeSeconds);
            filterMrkDowntab.PointerEnterAct = simpleDownAndUp.MoveObject;
            filterMrkDowntab.PointerExitAct = simpleDownAndUp.RemoveObject;
        }
        //================================================================================

        //マウススクロールの割り当て======================================================
        if (_monitorPlayerInput != null)
        {
            Action<float> act = _gameBoxManager.OnScroll;
            _monitorPlayerInput.onMouseScroll += act;
            //画面変更及び、ローディング時に外す
            commonStateManager.AddOnMainLoadingFunc(() => { _monitorPlayerInput.onMouseScroll -= act; });
            commonStateManager.AddOnMiniLoadingFunc(() => { _monitorPlayerInput.onMouseScroll -= act; });
            //ローディングから戻った際にもう一度設定
            commonStateManager.AddOutLoadingFunc(() => { _monitorPlayerInput.onMouseScroll += act; });
        }
        //================================================================================


    }
}
