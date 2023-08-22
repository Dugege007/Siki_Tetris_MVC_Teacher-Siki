using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public const int MaxRows = 23;
    public const int MaxColumns = 10;

    // [ 列，行 ]
    private Transform[,] map = new Transform[MaxColumns, MaxRows];

    private int score = 0;
    private int topScore = 0;
    private int gameTimes = 0;
    private string scoreKey = "Score";
    private string topScoreKey = "TopScore";
    private string gameTimesKey = "GameTimes";

    public bool isDataUpdate = false;

    public int Score { get { return score; } }
    public int TopScore { get { return topScore; } }
    public int GameTimes { get { return gameTimes; } }

    private void Awake()
    {
        LoadData();
    }

    /// <summary>
    /// 判断是否在有效格子上
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public bool IsValidMapPosition(Transform trans)
    {
        //transform.GetComponentsInChildren<Transform>(); //无法获取SetActive为false的子物体
        //transform.GetComponentsInChildren<Transform>(true); //获取全部子物体，无论SetActive是否为true

        // 这里似乎可以用 trans 直接遍历子物体
        foreach (Transform t in trans)
        {
            // 判断是否为 Block
            if (t.CompareTag("Block") == false)
                continue;

            Vector2 pos = t.position.Round();   // Round() 为扩展方法，近似值取整，详见 Verctor3Extension 类

            // 判断是否在地图内
            if (IsInMap(pos) == false)
                return false;

            // 判断当前格是否为空
            if (map[(int)pos.x, (int)pos.y] != null)
                return false;
        }

        return true;
    }

    public bool IsGameOver()
    {
        for (int i = 0; i < MaxColumns; i++)
        {
            for (int j = MaxRows - 3; j < MaxRows; j++)
            {
                if (map[i, j] != null)
                {
                    gameTimes++;
                    SaveData();
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsInMap(Vector2 pos)
    {
        return pos.x >= 0
            && pos.x < MaxColumns
            && pos.y >= 0
            && pos.y < MaxRows;
    }

    public bool PlaceShape(Transform trans)
    {
        foreach (Transform t in trans)
        {
            if (t.CompareTag("Block") == false)
                continue;

            Vector2 pos = t.position.Round();
            map[(int)pos.x, (int)pos.y] = t;
        }

        return CheckMap();
    }

    // 检查地图是否需要消除行
    private bool CheckMap()
    {
        int rowClearNum = 0;
        for (int i = 0; i < MaxRows; i++)
        {
            if (CheckIsRowFull(i))
            {
                rowClearNum++;
                DeleteRow(i);
                MoveDownRowsAbove(i + 1);
                i--;    // 当上方行降下来后，在该行再判断一次
            }
        }

        if (rowClearNum > 0)
        {
            // 增加分数
            score += rowClearNum * 100;
            if (score > topScore)
            {
                topScore = score;
            }
            isDataUpdate = true;
            return true;
        }
        else return false;
    }

    // 检查行是否满了
    private bool CheckIsRowFull(int rowIndex)
    {
        for (int i = 0; i < MaxColumns; i++)
        {
            if (map[i, rowIndex] == null)
                return false;
        }
        return true;
    }

    // 删除行
    private void DeleteRow(int rowIndex)
    {
        for (int i = 0; i < MaxColumns; i++)
        {
            Destroy(map[i, rowIndex].gameObject);
            map[i, rowIndex] = null;
        }
    }

    // 移动删除行以上的所有行
    private void MoveDownRowsAbove(int rowIndex)
    {
        for (int i = rowIndex; i < MaxRows; i++)
        {
            MoveDownRow(i);
        }
    }

    // 移动行
    private void MoveDownRow(int rowIndex)
    {
        for (int i = 0; i < MaxColumns; i++)
        {
            map[i, rowIndex - 1] = map[i, rowIndex];

            if (map[i, rowIndex - 1] == null)
                continue;

            map[i, rowIndex] = null;
            map[i, rowIndex - 1].position += Vector3.down;
        }
    }

    private void LoadData()
    {
        topScore = PlayerPrefs.GetInt(topScoreKey, 0);
        gameTimes = PlayerPrefs.GetInt(gameTimesKey, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(topScoreKey, topScore);
        PlayerPrefs.SetInt(gameTimesKey, gameTimes);
    }

    public void Restart()
    {
        for (int i = 0; i < MaxColumns; i++)
        {
            for (int j = 0; j < MaxRows; j++)
            {
                if (map[i, j] != null)
                    Destroy(map[i, j].gameObject);
                map[i, j] = null;
            }
        }
        score = 0;
    }

    public void ClearData()
    {
        score = 0;
        topScore = 0;
        gameTimes = 0;
        SaveData();
    }
}
