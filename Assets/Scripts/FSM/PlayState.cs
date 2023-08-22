using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : FSMState
{
    private void Awake()
    {
        stateID = StateID.Play;
        AddTransition(Transition.PauseBtnClick, StateID.Menu);
    }

    public override void DoBeforeEntering()
    {
        ctrl.cameraManager.ZoomIn();
        ctrl.view.ShowGameUI(ctrl.model.Score, ctrl.model.TopScore);
        ctrl.gameManager.StartGame();
    }

    public override void DoBeforeLeaving()
    {
        ctrl.view.HideGameUI();
        ctrl.gameManager.PauseGame();
    }

    public void OnPauseBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        fsm.PerformTransition(Transition.PauseBtnClick);
        ctrl.view.ShowRestartBtn();
    }

    public void OnRestartBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        ctrl.view.HideGameOverUI();
        ctrl.view.ShowGameUI(0, ctrl.model.TopScore);
        ctrl.model.Restart();
        ctrl.gameManager.StartGame();
    }
}
