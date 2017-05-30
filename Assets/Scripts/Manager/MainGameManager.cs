using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MAIN_GAME_MODE
{
	GAME_START = 0,
	GAME_PLAYING,
	GAME_END_CLEAR,
	GAME_END_OVER,
}

public class MainGameManager : MonoBehaviour {

	// ゲームオブジェクト
	private LanderManager _Lander;	// ランダー
	private MoonManager _Moon;		// 月面
	private CameraManager _Camera;	// カメラ
	private InfoUIManager _UIManager; // UI

	// 現在のゲームモード
	[SerializeField]
	private MAIN_GAME_MODE _Mode;

	// モード更新処理のデリゲート
	private delegate void ModeUpdate();
	ModeUpdate modeupdate = null;

	// 処理用変数
	private bool isClear = false; 	// ステージのクリアフラグ
	private int nStage=0;			// 現在のステージ番号
	private float fScore=0;			// 現在の総合スコア
	private float fBonusFuel=0;		// クリア時に取得するボーナス燃料
	[SerializeField]
	private float fStartWaitTime;	// ゲーム開始待ち処理の待ち時間
	[SerializeField]
	private float fResultWaitTime;	// ゲーム終了待ち処理の待ち時間
	private float fStartTime;		// 待ち処理の待ち開始時間
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// ゲームオブジェクトの取得
		_Lander = FindObjectOfType<LanderManager> ();
		_Moon = FindObjectOfType<MoonManager> ();
		_Camera = FindObjectOfType<CameraManager> ();
		_UIManager = FindObjectOfType<InfoUIManager> ();

		// 現在のゲームモード初期化
		ChangeGameMode(MAIN_GAME_MODE.GAME_START);
	}
	
	// 更新処理
	void Update () {
		if (modeupdate == null)
			return;
		// モード更新処理
		modeupdate ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲームモードを変更する。
	/// </summary>
	/// <param name="mode">Mode.</param>
	public void ChangeGameMode(MAIN_GAME_MODE mode)
	{
		_Mode = mode;
		SwitchMode ();
	}

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲームモードに変更に伴う処理を実行する。
	/// </summary>
	private void SwitchMode()
	{
		switch (_Mode) {
		case MAIN_GAME_MODE.GAME_START:
			GameStart ();
				break;
		case MAIN_GAME_MODE.GAME_PLAYING:
			modeupdate = new ModeUpdate (GamePlaying_Update);
				break;
		case MAIN_GAME_MODE.GAME_END_CLEAR:
			GameClear ();
				break;
		case MAIN_GAME_MODE.GAME_END_OVER:
			GameOver ();
				break;
			default:
				break;
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲーム開始処理
	/// </summary>
	private void GameStart()
	{
		// 初期化フラグをFalseに設定
		_Lander.isInit = false;
		_Moon.isInit = false;
		_Camera.isInit = false;

		// クリアフラグがTrueならばステージクリア後とし、ランダーを初期化する。
		if (isClear == true) {
			// ボーナス燃料を加算した値を渡す。
			_Lander.Init (_Lander._Status.GetStatus().fuel+fBonusFuel);
		} else {
			_Lander.Init ();
		}
		_Camera.Init (); // カメラ初期化処理
		_Moon.Init ();	// 月面初期化処理


		// クリアフラグをFalseに設定
		isClear = false;

		// ステージ番号の更新
		nStage++;

		// 開始待ち処理の待ち開始時間の取得
		fStartTime = Time.timeSinceLevelLoad;

		// ＵＩのステージ番号と総合スコアの更新
		_UIManager.SetInfoItem (UI_INFO_ITEM.STAGE, nStage);
		_UIManager.SetInfoItem (UI_INFO_ITEM.SCORE, (int)fScore);

		// 表示する開始メッセージの情報更新と表示
		_UIManager.SetMsgItem<GameStartMsg.INFO_ITEM> (UI_MSG_ITEM.GAME_START,GameStartMsg.INFO_ITEM.STAGE, nStage);
		_UIManager.SetMsgActive (UI_MSG_ITEM.GAME_START, true);

		// 更新処理を設定
		modeupdate = new ModeUpdate (GameStart_Update);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲームクリア時の処理
	/// </summary>
	private void GameClear()
	{
		// スコアとボーナス燃料の取得
		float score=0;
		ScoreCalc(_Lander._Status,_Lander._LandingPoint,out score,out fBonusFuel);

		// 総合スコアの更新
		fScore += score;

		// メッセージの情報更新と表示
		_UIManager.SetMsgItem<GameClearMsg.INFO_ITEM>(UI_MSG_ITEM.GAME_CLEAR,GameClearMsg.INFO_ITEM.SCORE,(int)score);
		_UIManager.SetMsgItem<GameClearMsg.INFO_ITEM>(UI_MSG_ITEM.GAME_CLEAR,GameClearMsg.INFO_ITEM.BONUSFUEL,(int)fBonusFuel);
		_UIManager.SetMsgActive(UI_MSG_ITEM.GAME_CLEAR,true);

		// クリアフラグをTrueに設定
		isClear = true;

		// 終了待ち処理の待ち開始時間の取得
		fStartTime = Time.timeSinceLevelLoad;

		// 更新処理の設定
		modeupdate = new ModeUpdate (GameClear_Update);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲームオーバー時の処理
	/// </summary>
	private void GameOver()
	{
		// メッセージの情報更新と表示
		_UIManager.SetMsgItem<GameOverMsg.INFO_ITEM>(UI_MSG_ITEM.GAME_OVER,GameOverMsg.INFO_ITEM.CLEARSTAGE,nStage-1);
		_UIManager.SetMsgItem<GameOverMsg.INFO_ITEM>(UI_MSG_ITEM.GAME_OVER,GameOverMsg.INFO_ITEM.SCORE,(int)fScore);
		_UIManager.SetMsgActive(UI_MSG_ITEM.GAME_OVER,true);

		// 終了待ち処理の待ち開始時間の取得
		fStartTime = Time.timeSinceLevelLoad;

		// 更新処理の設定
		modeupdate = new ModeUpdate (GameOver_Update);
	}

	/*---------------------------------------------------------------------*/
	/* モード更新処理 */
	/// <summary>
	/// GAME_START モードの更新処理
	/// </summary>
	private void GameStart_Update()
	{
		// ＵＩ情報の更新処理
		UIUpdate ();

		// 各オブジェクトの初期化が完了しているならゲームを開始する。
		if (_Lander.isInit == true && _Moon.isInit == true && _Camera.isInit == true) {
			float fTime = Time.timeSinceLevelLoad - fStartTime;
			if (fTime > fStartWaitTime) {
				// ランダーの開始処理
				_Lander.PlayStart ();

				// メッセージを非表示にする。
				_UIManager.SetMsgActive(UI_MSG_ITEM.GAME_START,false);

				// ゲームプレイモードへ変更する。
				ChangeGameMode (MAIN_GAME_MODE.GAME_PLAYING);
			}
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// GAME_PLAYINGモードの更新処理
	/// </summary>
	private void GamePlaying_Update ()
	{
		// ＵＩ情報の更新処理
		UIUpdate ();

		// 着陸成功ならクリアモードへ変更する。
		if (_Lander.isLanding == true) {
			ChangeGameMode (MAIN_GAME_MODE.GAME_END_CLEAR);
		}

		// 着陸失敗ならゲームオーバーモードへ変更する。
		if (_Lander.isDestroy == true) {
			ChangeGameMode (MAIN_GAME_MODE.GAME_END_OVER);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// GAME_END_CLEARモードの更新処理
	/// </summary>
	private void GameClear_Update()
	{
		// 待ちの経過時間を取得する。
		float fTime = Time.timeSinceLevelLoad - fStartTime;

		// 待ち時間が経過した場合、次のステージへ
		if (fTime > fResultWaitTime) {
			// メッセージを非表示にする。
			_UIManager.SetMsgActive(UI_MSG_ITEM.GAME_CLEAR,false);

			// ゲームスタートモードへ変更する。
			ChangeGameMode (MAIN_GAME_MODE.GAME_START);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// GAME_END_OVERモードの更新処理
	/// </summary>
	private void GameOver_Update()
	{
		// 待ちの経過時間を取得する。
		float fTime = Time.timeSinceLevelLoad - fStartTime;

		// 待ち時間が経過した場合、タイトルシーンへ遷移する。
		if (fTime > fResultWaitTime) {
			TransitionManager.Instance.ChangeScene (GAME_SCENE.TITLE);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ＵＩ情報更新処理
	/// </summary>
	private void UIUpdate()
	{
		// 燃料・高度・水平速度・垂直速度の更新
		_UIManager.SetInfoItem (UI_INFO_ITEM.FUEL, (int)_Lander._Status.GetStatus ().fuel);
		_UIManager.SetInfoItem (UI_INFO_ITEM.ALTITUDE, (int)_Lander._Status.GetStatus ().altitude);
		_UIManager.SetInfoItem (UI_INFO_ITEM.HORIZONTAL_SPEED, (int)_Lander._Status.GetStatus ().horizontal_speed);
		_UIManager.SetInfoItem (UI_INFO_ITEM.VERTICAL_SPEED, (int)_Lander._Status.GetStatus ().vertical_speed);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// スコアとボーナス燃料の計算
	/// </summary>
	/// <param name="status">ランダーのステータス</param>
	/// <param name="point">着陸地点</param>
	/// <param name="score">スコア受け取り用変数</param>
	/// <param name="bonusfuel">ボーナス燃料受け取り用変数</param>
	private void ScoreCalc(LanderStatus status,LandingPoint point,out float score,out float bonusfuel)
	{
		// ステータス情報の取得
		LanderStatus.STATUS _status = status.GetStatus ();

		// 水平速度と垂直速度をベクトルとし、ベクトルの長さの半分をスピード値として取得する。
		int speed =(int)(new Vector2 (_status.horizontal_speed, _status.vertical_speed).magnitude/2);

		// 計算ベース値からスピード値を引いた値に着陸地点のボーナス倍率をかけた値をスコアとする。
		score = (Const.MainGameData.SCORE_CALC_BASE-speed)*point.GetBonusRate();

		// スコアにボーナス燃料倍率をかけた値をボーナス燃料とする。
		bonusfuel = score*Const.MainGameData.BONUS_FUEL_RATE;
	}
	/*---------------------------------------------------------------------*/
}
