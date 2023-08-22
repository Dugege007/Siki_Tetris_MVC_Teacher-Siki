using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 每个状态只做和其相关的事情
public class MenuState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Menu;
        AddTransition(Transition.StartBtnClick, StateID.Play);
    }

    public override void DoBeforeEntering()
    {
        ctrl.view.ShowMenuUI();
        ctrl.cameraManager.ZoomOut();
    }

    public override void DoBeforeLeaving()
    {
        ctrl.view.HideMenuUI();
        // 相机放大在PlayState中控制
    }

    public void OnStartBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        fsm.PerformTransition(Transition.StartBtnClick);
    }

    public void OnSettingBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.view.ShowSettingUI();
    }

    public void OnRankBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.view.ShowRankUI(ctrl.model.Score, ctrl.model.TopScore, ctrl.model.GameTimes);
    }

    public void OnDeletDataBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.model.ClearData();
        OnRankBtnClick();
    }

    public void OnRestartBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.model.Restart();
        ctrl.gameManager.ClearShape();
        fsm.PerformTransition(Transition.StartBtnClick);
    }
}
