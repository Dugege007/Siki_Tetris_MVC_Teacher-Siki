using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private Control ctrl;
    private GameManager gameManager;
    private Transform pivot;

    private bool isPause = false;
    private float stepTimer = 0;
    private float stepTime = 0.5f;
    private float tempTime;
    private float stepRush = 0.1f;
    private bool isSpeedUp = false;

    private void Awake()
    {
        pivot = transform.Find("Pivot");
        tempTime = stepTime;
    }

    private void Update()
    {
        if (isPause) return;

        stepTimer += Time.deltaTime;
        if (stepTimer >= stepTime)
        {
            stepTimer = 0;
            Fall();
        }

        InputControl();
    }

    public void Init(Color color, Control ctrl, GameManager gameManager)
    {
        foreach (Transform t in transform)
        {
            if (t.CompareTag("Block"))
            {
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }

        this.ctrl = ctrl;
        this.gameManager = gameManager;
    }

    private void Fall()
    {
        Vector3 pos = transform.position;
        pos.y--;
        transform.position = pos;

        // 如果下个不是有效格子
        if (ctrl.model.IsValidMapPosition(transform) == false)
        {
            pos.y++;
            transform.position = pos;
            isPause = true;
            
            bool isRowClear = ctrl.model.PlaceShape(transform);
            if (isRowClear)
                ctrl.audioManager.PlayRowClear();

            gameManager.FallDown();
            return;
        }

        ctrl.audioManager.PlayDrop();
    }

    private void InputControl()
    {
        float h = 0;
        //float v = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            h = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            h = 1;
        if (h != 0)
        {
            Vector3 pos = transform.position;
            pos.x += h;
            transform.position = pos;

            if (ctrl.model.IsValidMapPosition(transform) == false)
            {
                pos.x -= h;
                transform.position = pos;
            }
            else
            {
                ctrl.audioManager.PlayMove();
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(pivot.position,Vector3.forward, -90);
            if (ctrl.model.IsValidMapPosition(transform) == false)
            {
                transform.RotateAround(pivot.position, Vector3.forward, 90);
            }
            else
            {
                ctrl.audioManager.PlayMove();
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isSpeedUp = false;
            stepTime = tempTime;
        }

        //if (isSpeedUp) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSpeedUp = true;
            tempTime = stepTime;
            stepTime *= stepRush;
        }
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
        stepTimer = 0;
    }
}
