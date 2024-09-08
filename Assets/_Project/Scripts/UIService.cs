using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{ 
    public int startLives;
    private int lives;
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject heartPrefab;
    public List<GameObject> hearts = new List<GameObject>();
    public Transform contentHearts;
    public TextMeshProUGUI timerText;
    public TextValue prefabAddScoreText,prefabTakeLiveText;
    public Transform canvasContent;
    public float hieghtFlyText;
    public Button ready;
    public TextMeshProUGUI buttonReadyText;
    public GameClose gameClose;
    public bool IsReady;
    DateTime startGameTimer;

    void Start()
    {
        lives = startLives;
        for (int i = 0; i < lives; i++)
        {
            var hearGo = Instantiate(heartPrefab, contentHearts);
            hearts.Add(hearGo);
        }
        Container.instance.Spawner.onSlice += OnSliced;
        ready.onClick.RemoveAllListeners();
        ready.onClick.AddListener(CLickReady);
        ShowText();
 
 

        timerText.text = $"TIMER:{01:00}:{00:00}";
        buttonReadyText.text = "tap to start"; 
    }

   


    private void ShowTimer()
    {
        var finish = startGameTimer.AddMinutes(1);
        var timeLeft =   finish - DateTime.Now;
        if (timeLeft.TotalSeconds > 0)
            timerText.text = $"TIMER:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
        else
        {
            Lose();
        }
     
    }

    private void CLickReady()
    {
        if (IsReady == false)
        {
            IsReady = true;
            score = 0;
            lives = startLives;
            ready.gameObject.SetActive(false);
            Spawner.IsGameProcess = true;
            ShowText();
            startGameTimer = DateTime.Now;
            ShowTimer();
        }
    }
    private void Update()
    {
        if (Spawner.IsGameProcess)
        { 
          ShowTimer();    
        }
        if(IsReady && Spawner.IsGameProcess == false && Input.GetMouseButtonDown(0))
        {
            IsReady = false;
            gameClose.CloseCurrentFrame();
           
        }
    }
    private void OnSliced(SliceTarget target)
    {
        if(target.SliceType != SliceTarget.SliceName.bomb)
        {
            score++;
            gameClose.CallAddScore(1);
            var scoreText = Instantiate(prefabAddScoreText, target.transform.position, Quaternion.identity,canvasContent);
            PlayAnimation(scoreText, Vector3.up);
        }
        else
        {
            lives--;
            lives = Mathf.Clamp(lives, 0, lives);
            Destroy(hearts[lives]);
            StartCoroutine(CorDelayCreateText());
            IEnumerator CorDelayCreateText()
            {
                yield return new WaitForSeconds(1f);
                var liveText = Instantiate(prefabTakeLiveText, Vector3.zero, Quaternion.identity, canvasContent);
                tweens.Add(liveText.transform.DOMoveY(transform.position.y + (Vector3.up.y * 1), 1));
                tweens.Add(liveText.textMeshProUGUI.DOFade(0, 1f));
                tweens.Add(liveText.textMeshProUGUI2.DOFade(0, 1f));
                Destroy(liveText.gameObject, 2f);
            }
        
        if (lives == 0)
            {
                Lose();
            }
        }
        ShowText();
    }
    List<Tween> tweens = new List<Tween>();
    public void PlayAnimation(TextValue textGo,Vector3 vector3)
    { 
        if (vector3 == Vector3.up)
        {
            tweens.Add(textGo.transform.DOMoveY(transform.position.y + (Vector3.up.y * hieghtFlyText), 1));
            tweens.Add(textGo.textMeshProUGUI.DOFade(0, 2f));
            tweens.Add(textGo.textMeshProUGUI2.DOFade(0, 2f));

        }
        else
        {
            tweens.Add(textGo.transform.DOMoveY(transform.position.y - (Vector3.up.y * hieghtFlyText), 1));
            tweens.Add(textGo.textMeshProUGUI.DOFade(0, 2f));
            tweens.Add(textGo.textMeshProUGUI2.DOFade(0, 2f));
        }
        Destroy(textGo.gameObject, 2f);
    }

    private void Lose()
    {
        Spawner.IsGameProcess = false;
        ready.onClick.RemoveAllListeners();
       // ready.onClick.AddListener(gameClose.CloseCurrentFrame);
         ready.gameObject.SetActive(true);
        timerText.text = $"TIMER:00:00";
        buttonReadyText.text = "game over";
        // Debug.LogError("lose");

    }

    public void ShowText()
    {
        scoreText.text = $"SCORE: {score}";
        //livesText.text = $"LIVES: {lives}";
    }
    private void OnDestroy()
    {
        tweens.ForEach (t=>t?.Kill());
    }
}
