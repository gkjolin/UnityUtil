using UnityEngine;
using System.Collections;

public class AndroidBackButtonManager : SingletonMonoBehaviour<AndroidBackButtonManager>
{
	[System.Serializable]
	public	enum GlobalGameStatus
	{
		TITLE,
		CLASSIC_TITLE,
		CLASSIC_GAME,
		CLASSIC_MOVING,
		CLASSIC_POSE,
		CLASSIC_POSTCARD,
		CLASSIC_POSTCARDSHOW,
		ENDLESS_TITLE,
		ENDLESS_GAME,
		ENDLESS_MOVING,
		ENDLESS_GAMEEND,
		ENDLESS_POSE,
		ENDLESS_POSTCARD,
		ENDLESS_POSTCARDSHOW,
		MULTI_TITLE,
		MULTI_GAME,
		MULTI_GAMEEND,
	}

	public static bool isMade;
	public GlobalGameStatus status;
	private Classic.GameControl classicGameControl;
	private Classic.GUIControl classicGUIControl;
	private Classic.PostCardCanvasControl classicPostCardCanvasControl;
	private Classic.CameraControl classicCameraControl;
	private Endless.GameControl endlessGameControl;
	private Endless.GameFrontCanvasControl endlessGameFrontCanvasControl;
	private Endless.GameEndFrontCanvasControl endlessGameEndFrontCanvasControl;
	private Endless.FadeGroupBackControl endlessFadegroupBackControl;
	private Multiplayer.GameControl multiGameControl;
	private Multiplayer.GameEndFrontCanvasControl multiGameEndFrontCanvasControl;
	public bool PCDebug;
	// Use this for initialization
	void Start ()
	{
		//TODO:有効化
		//Androidでなかったら不要なので削除
		if (Application.platform != RuntimePlatform.Android) {
			Destroy (gameObject);
			print ("Not Android Destroy BackButtonManager");
			return;
		} 
		//タイトルに戻ったときはダブるので削除
		print ("Make Back Button Manager");
		if (Application.loadedLevelName == "Title" && isMade == true) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
		isMade = true;

	}
	// Update is called once per frame
	void Update ()
	{
		//バックボタン
		if (Input.GetKeyDown (KeyCode.Escape)) {
			//ステータスを確認して設定
			print ("setstatus");
			SetStatus ();
			PlayBackButtonAction ();
		}

		//ホームボタン
		if (Input.GetKey (KeyCode.Home)) {
			Application.Quit ();
		}

	}

	void SetStatus ()
	{
		switch (Application.loadedLevelName) {
		//タイトルシーン
		case "Title":
			status = GlobalGameStatus.TITLE;
			break;
		//クラシックシーン
		case "MainClassic":
			classicGameControl = Camera.main.gameObject.GetComponent<Classic.GameControl> ();
			classicGUIControl = Camera.main.gameObject.GetComponent<Classic.GUIControl> ();
			classicCameraControl = Camera.main.gameObject.GetComponent<Classic.CameraControl> ();

			switch (classicGameControl.Status) {
			//ホーム
			case Classic.GameControl.GameStatus.STAT_HOME:
			case Classic.GameControl.GameStatus.STAT_NULL:
				status = GlobalGameStatus.CLASSIC_TITLE;
				//ポストカードモード
				if (classicGUIControl.isPostCardScene) {
					status = GlobalGameStatus.CLASSIC_POSTCARD;
					//ポストカード表示
					if (classicGUIControl.isPostCardShow) {
						status = GlobalGameStatus.CLASSIC_POSTCARDSHOW;
					}
				}
				if (classicCameraControl.isMoving) {
					status = GlobalGameStatus.CLASSIC_MOVING;
				}
				break;
			//ゲーム中
			case Classic.GameControl.GameStatus.STAT_GAME:
				status = GlobalGameStatus.CLASSIC_GAME;
				//ポーズ中
				if (classicGUIControl.Pose ()) {
					status = GlobalGameStatus.CLASSIC_POSE;
				}
				if (classicGameControl.isMoving) {
					status = GlobalGameStatus.CLASSIC_MOVING;
				}
				break;
			}
			break;
		//エンドレスシーン
		case "MainEndress":
			endlessGameControl = Camera.main.gameObject.GetComponent<Endless.GameControl> ();
			endlessGameFrontCanvasControl = GameObject.FindObjectOfType<Endless.GameFrontCanvasControl> ().GetComponent<Endless.GameFrontCanvasControl> ();
			endlessGameEndFrontCanvasControl = GameObject.FindObjectOfType<Endless.GameEndFrontCanvasControl> ().GetComponent<Endless.GameEndFrontCanvasControl> ();
			endlessFadegroupBackControl = GameObject.FindObjectOfType<Endless.FadeGroupBackControl> ().GetComponent<Endless.FadeGroupBackControl> ();
			switch (endlessGameControl.Status) {
			case Endless.GameControl.GameStatus.STAT_HOME:
			case Endless.GameControl.GameStatus.STAT_NULL:
				status = GlobalGameStatus.ENDLESS_TITLE;
				if (endlessFadegroupBackControl.isPostCardScene) {
					status = GlobalGameStatus.ENDLESS_POSTCARD;
				}
				if (endlessGameFrontCanvasControl.isPostCardShowing) {
					status = GlobalGameStatus.ENDLESS_POSTCARDSHOW;
				}
				if (endlessGameControl.isMovingToPostCard) {
					status = GlobalGameStatus.ENDLESS_MOVING;
				}
				break;
			case Endless.GameControl.GameStatus.STAT_GAME:
				status = GlobalGameStatus.ENDLESS_GAME;
				if (endlessGameControl.isPose == true) {
					status = GlobalGameStatus.ENDLESS_POSE;
				}
				break;
			case Endless.GameControl.GameStatus.STAT_GAMEEND:
				status = GlobalGameStatus.ENDLESS_GAMEEND;
				break;
			}
			break;
		//マルチシーン
		case "MainMultiplayer":
			multiGameControl = Camera.main.gameObject.GetComponent<Multiplayer.GameControl> ();
			multiGameEndFrontCanvasControl = GameObject.FindObjectOfType<Multiplayer.GameEndFrontCanvasControl> ().GetComponent<Multiplayer.GameEndFrontCanvasControl> ();
			switch (multiGameControl.Status) {
			case Multiplayer.GameControl.GameStatus.STAT_NULL:
			case Multiplayer.GameControl.GameStatus.STAT_HOME:
				status = GlobalGameStatus.MULTI_TITLE;
				break;
			case Multiplayer.GameControl.GameStatus.STAT_GAME:
				status = GlobalGameStatus.MULTI_GAME;
				break;
			case Multiplayer.GameControl.GameStatus.STAT_GAMEEND:
				status = GlobalGameStatus.MULTI_GAMEEND;
				break;
			}
			break;
		}

	}

	void PlayBackButtonAction ()
	{
		print ("Do Action" + status.ToString ());
		switch (status) {
		//タイトルの場合アプリ終了
		case GlobalGameStatus.TITLE:
			Application.Quit ();
			break;
		//各モードのタイトルの場合メインタイトルに戻る
		case GlobalGameStatus.CLASSIC_TITLE:
		case GlobalGameStatus.ENDLESS_TITLE:
		case GlobalGameStatus.MULTI_TITLE:
			OnTitleBack ();
			break;
		//クラシックモード
		case GlobalGameStatus.CLASSIC_GAME:
			//ポーズ
			classicGUIControl.OnPushStopButtin ();
			break;
		case GlobalGameStatus.CLASSIC_POSE:
			//ゲーム再開
			classicGUIControl.OnPushStopButtin ();
			break;
		case GlobalGameStatus.CLASSIC_POSTCARD:
			//モードホームに戻る
			classicGUIControl.OnPushPostCardBlinkButton ();
			break;
		case GlobalGameStatus.CLASSIC_POSTCARDSHOW:
			//ポストカード非表示
			classicPostCardCanvasControl = GameObject.FindObjectOfType<Classic.PostCardCanvasControl> ().GetComponent<Classic.PostCardCanvasControl> ();
			classicGUIControl.HidePostCard ();
			break;
		case GlobalGameStatus.CLASSIC_MOVING:
			break;
		//エンドレスモード
		case GlobalGameStatus.ENDLESS_GAME:
			//ポーズ
			endlessGameFrontCanvasControl.OnStopButton ();
			break;
		case GlobalGameStatus.ENDLESS_POSE:
			//ゲーム再開
			endlessGameFrontCanvasControl.OnPlayButton ();
			break;
		case GlobalGameStatus.ENDLESS_GAMEEND:
			//モードホームに戻る
			endlessGameEndFrontCanvasControl.OnBackButton ();
			break;
		case GlobalGameStatus.ENDLESS_POSTCARD:
			//モードホームに戻る
			endlessGameFrontCanvasControl.OnPostCardBackButton ();
			break;
		case GlobalGameStatus.ENDLESS_POSTCARDSHOW:
			//ポストカード非表示
			endlessGameFrontCanvasControl.OffPostCardCanvas ();
			break;
		case GlobalGameStatus.ENDLESS_MOVING:
			//何もしない
			break;
		//マルチモード
		case GlobalGameStatus.MULTI_GAME:
			//何もしない
			break;
		case GlobalGameStatus.MULTI_GAMEEND:
			//モードホームに戻る
			multiGameEndFrontCanvasControl.OnBackButton ();
			break;
		}
	}

	public void OnTitleBack ()
	{
		SoundManager.Instance.Play ("button");
		SoundManager.Instance.Stop ("endlessBGM", true);
		SoundManager.Instance.Stop ("endlessBGMRhythm", true);
		FadeManager.Instance.LoadLevel ("Title", 0.4f);
	}
}