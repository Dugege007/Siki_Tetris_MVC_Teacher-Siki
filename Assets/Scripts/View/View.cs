using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour
{
    private Control ctrl;

    private RectTransform titleText;
    private RectTransform menuUI;
    private GameObject restartBtn;
    private RectTransform gameUI;
    private RectTransform settingUI;
    private RectTransform rankUI;
    private RectTransform gameOverUI;
    private GameObject muteGO;

    private Text mScoreText;
    private Text mTopScoreText;
    private Text mGameOverScoreText;

    private Text mRankScore;
    private Text mRankTopScore;
    private Text mGameTimes;

    private void Awake()
    {
        ctrl = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

        titleText = transform.Find("Canvas/TitleText") as RectTransform;
        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        restartBtn = transform.Find("Canvas/MenuUI/RestartBtn").gameObject;
        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        settingUI = transform.Find("Canvas/SettingUI") as RectTransform;
        rankUI = transform.Find("Canvas/RankUI") as RectTransform;
        gameOverUI = transform.Find("Canvas/GameOverUI") as RectTransform;
        muteGO = transform.Find("Canvas/SettingUI/SettingBG/AudioBtn/Mute").gameObject;

        mScoreText = transform.Find("Canvas/GameUI/ScoreLabel/ScoreText").GetComponent<Text>();
        mTopScoreText = transform.Find("Canvas/GameUI/TopScoreLabel/TopScoreText").GetComponent<Text>();
        mGameOverScoreText = transform.Find("Canvas/GameOverUI/SettingBG/FinalScoreText").GetComponent<Text>();

        mRankScore = transform.Find("Canvas/RankUI/RankBG/ScorePanel/CurrentScoreRecordLabel/CurrentScoreRecordText").GetComponent<Text>();
        mRankTopScore = transform.Find("Canvas/RankUI/RankBG/ScorePanel/TopScoreRecordLabel/TopScoreRecordText").GetComponent<Text>();
        mGameTimes = transform.Find("Canvas/RankUI/RankBG/ScorePanel/GameTimesRecordLabel/GameTimesRecordText").GetComponent<Text>();
    }

    public void ShowMenuUI()
    {
        titleText.gameObject.SetActive(true);
        titleText.DOAnchorPosY(-200, 0.5f);

        menuUI.gameObject.SetActive(true);
        menuUI.DOAnchorPosY(150, 0.5f);
    }

    public void HideMenuUI()
    {
        // DoTween 有一个 OnComplete 的回调函数，可以在动画执行完毕后执行一段代码
        titleText.DOAnchorPosY(200, 0.5f)
            .OnComplete(delegate { titleText.gameObject.SetActive(false); });
        menuUI.DOAnchorPosY(-150, 0.5f)
            .OnComplete(delegate { menuUI.gameObject.SetActive(false); });
    }

    public void ShowRestartBtn()
    {
        restartBtn.SetActive(true);
    }

    public void ShowGameUI(int score = 0, int topScore = 0)
    {
        mScoreText.text = score.ToString();
        mTopScoreText.text = topScore.ToString();

        gameUI.gameObject.SetActive(true);
        gameUI.DOAnchorPosY(-100, 0.5f);
    }

    public void UpdateGameUI(int score = 0, int topScore = 0)
    {
        mScoreText.text = score.ToString();
        mTopScoreText.text = topScore.ToString();

        mScoreText.text = score.ToString();
        mTopScoreText.text = topScore.ToString();
    }

    public void HideGameUI()
    {
        gameUI.DOAnchorPosY(100, 0.5f)
            .OnComplete(delegate { gameUI.gameObject.SetActive(false); });
    }

    public void ShowGameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
        mGameOverScoreText.text = mScoreText.text;
    }

    public void HideGameOverUI()
    {
        gameOverUI.gameObject.SetActive(false);
    }

    public void OnHomeBtnClick()
    {
        ctrl.audioManager.PlayCursor();
        // 加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowSettingUI()
    {
        settingUI.gameObject.SetActive(true);
    }

    public void SetMuteActive(bool isActive)
    {
        muteGO.SetActive(isActive);
    }

    public void OnSettingUIClick()
    {
        ctrl.audioManager.PlayCursor();
        settingUI.gameObject.SetActive(false);
    }

    public void ShowRankUI(int score, int topScore, int gameTimes)
    {
        mRankScore.text = score.ToString();
        mRankTopScore.text = topScore.ToString();
        mGameTimes.text = gameTimes.ToString();
        rankUI.gameObject.SetActive(true);
    }

    //public void OnRankBtnClick()
    //{
    //    ctrl.audioManager.PlayCursor();
    //    rankUI.gameObject.SetActive(true);
    //}

    public void OnRankUIClick()
    {
        ctrl.audioManager.PlayCursor();
        rankUI.gameObject.SetActive(false);
    }


}
