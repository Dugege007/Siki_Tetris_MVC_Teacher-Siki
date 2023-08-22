using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Control ctrl;

    public Shape[] shapes;
    public Color[] colors;

    // 游戏是否暂停，默认在暂停中
    private bool isPause = true;
    // 装载当前生成的图形
    private Shape currentShape = null;

    private Transform blockHolder;

    private void Awake()
    {
        ctrl = GetComponent<Control>();
        blockHolder = transform.Find("BlockHolder");
    }

    private void Update()
    {
        if (isPause) return;

        // 检测到currentShape为空时，生成图形
        if (currentShape == null)
            SpawnShape();
    }

    public void ClearShape()
    {
        if (currentShape!=null)
        {
            Destroy(currentShape.gameObject);
            currentShape = null;
        }
    }

    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
            currentShape.Resume();
    }

    public void PauseGame()
    {
        isPause = true;
        if (currentShape != null)
            currentShape.Pause();
    }

    private void SpawnShape()
    {
        int indexS = Random.Range(0, shapes.Length);
        int indexC = Random.Range(0, colors.Length);

        currentShape = Instantiate(shapes[indexS]);
        currentShape.Init(colors[indexC], ctrl, this);
        currentShape.transform.SetParent(blockHolder);

        // 当图形落地时，将currentShape设置为空
    }

    public void FallDown()
    {
        currentShape = null;
        if (ctrl.model.isDataUpdate)
        {
            ctrl.view.UpdateGameUI(ctrl.model.Score, ctrl.model.TopScore);
        }

        foreach(Transform t in blockHolder)
        {
            if(t.childCount <= 1)
                Destroy(t.gameObject);
        }

        if (ctrl.model.IsGameOver())
        {
            PauseGame();
            ctrl.view.HideGameUI();
            ctrl.view.ShowGameOverUI();
        }
    }
}
