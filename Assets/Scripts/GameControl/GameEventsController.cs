using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
    public static GameEventsController current;

    private int _score;
    private float powerUpTime;
    private bool powerUp;

    private int[] scores = new int[3];

    void Awake()
    {
        current = this;
    }

    void Start() {
        _score = 0;
        powerUpTime = 0;
        powerUp = false;
    }

    void Update() {
        if (powerUp) {
            if (powerUpTime > 0) {
                powerUpTime -= Time.deltaTime;
            }
            else {
                powerUp = false;
                lostPowerUp();
            }
        }
    }

    public event Action onGetCoin;
    public event Action onGetPowerUp;
    public event Action onLostPowerUp;

    public event Action onGameOver;

    public void getCoin() {
        if (onGetCoin != null) {
            onGetCoin();
        }
        _score += 5;
    }

    public void getPowerUp() {
        if (onGetPowerUp != null) {
            onGetPowerUp();
        }
        powerUp = true;
        powerUpTime = 5;
    }

    public void lostPowerUp() {
        if (onLostPowerUp != null) {
            onLostPowerUp();
        }
    }

    public void GameOver() {
        if (onGameOver != null) {
            onGameOver();
        }
        saveScoreData();
    }

    private void loadScoreData() {
        scores[0] = PlayerPrefs.GetInt("highscore1");
        scores[1] = PlayerPrefs.GetInt("highscore2");
        scores[2] = PlayerPrefs.GetInt("highscore3");
        Array.Sort(scores);
        Array.Reverse(scores);
    }

    private void saveScoreData() {
        loadScoreData();
        for (int i = 0; i < scores.Length; i++) {
            if (scores[i] < _score) {
                scores[i] = _score;
                break;
            }
        }
        PlayerPrefs.SetInt("highscore3", scores[0]);
        PlayerPrefs.SetInt("highscore2", scores[1]);
        PlayerPrefs.SetInt("highscore1", scores[2]);

        foreach (int i in scores) {
            Debug.Log("Scores: "+i);
        }
        Debug.Log("-----------------------");
    }
}
