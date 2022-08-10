using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	public delegate void GameDelegate ();
	public static event GameDelegate OnGameStarted;
	public static event GameDelegate OnGameOverConfirmed;

	public static GameManager Instance;

	[SerializeField] private GameObject startPage;
	[SerializeField] private GameObject gameOverPage;
	[SerializeField] private GameObject countdownPage;
	[SerializeField] private Text scoreText;

	enum PageState{
		None,
		Start,
		GameOver,
		Countdown
	}

	private int score = 0;
	private bool gameOver = true;

	public bool GameOver { get { return gameOver; } }

	void Awake(){
		Instance = this;
	}

	void OnEnable(){
		CountdownText.OnCountdownFinished += OnCountdownFinished;
		TapController.OnPlayerDied += OnPlayerDied;
		TapController.OnPlayerScored += OnPlayerScored;
	
	}

	void OnDisable(){
		CountdownText.OnCountdownFinished -= OnCountdownFinished;
		TapController.OnPlayerDied -= OnPlayerDied;
		TapController.OnPlayerScored -= OnPlayerScored;
	
	}

	void OnCountdownFinished(){
		SetPageState (PageState.None);
		OnGameStarted ();
		score = 0;
		gameOver = false;
	}

	void OnPlayerDied(){
		gameOver = true;
		int savedScore = PlayerPrefs.GetInt ("highscore");
		if (score > savedScore) {
			PlayerPrefs.SetInt ("highscore", score);
		
		}
		SetPageState (PageState.GameOver);
	}

	void OnPlayerScored(){
		score++;
		scoreText.text = score.ToString();
	}

	void SetPageState(PageState state){

		switch (state) {

		case PageState.None:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.Start:
			startPage.SetActive (true);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (false);
			break;
		case PageState.GameOver:
			startPage.SetActive (false);
			gameOverPage.SetActive (true);
			countdownPage.SetActive (false);
			break;
		case PageState.Countdown:
			startPage.SetActive (false);
			gameOverPage.SetActive (false);
			countdownPage.SetActive (true);
			break;

		}
	}

	public void ConfirmedGameOver(){
		OnGameOverConfirmed();
		scoreText.text="0";
		SetPageState (PageState.Start);
	}
	public void StartGame(){
		SetPageState(PageState.Countdown);
	}
}
