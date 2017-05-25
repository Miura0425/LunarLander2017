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

	private LunderManager _Lunder;	// ランダー
	private MoonManager _Moon;		// 月面
	private CameraManager _Camera;	// カメラ

	// 現在のゲームモード
	[SerializeField]
	private MAIN_GAME_MODE _Mode;

	// モード更新処理のデリゲート
	private delegate void ModeUpdate();
	ModeUpdate modeupdate = null;

	private bool isClear = false;


	// テスト用
	[SerializeField]
	private Text txtGameStart;
	[SerializeField]
	private Text txtGameOver;
	[SerializeField]
	private Text txtGameClear;

	private float fScore;
	private float fBonusFuel;

	[SerializeField]
	private float fStartWaitTime;
	[SerializeField]
	private float fResultWaitTime;
	private float fStartTime;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		_Lunder = FindObjectOfType<LunderManager> ();
		_Moon = FindObjectOfType<MoonManager> ();
		_Camera = FindObjectOfType<CameraManager> ();

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
		_Lunder.isInit = false;
		_Moon.isInit = false;
		_Camera.isInit = false;
		// クリアフラグがTrueならばステージクリア後とし、ランダーを初期化する。
		if (isClear == true) {
			// ボーナス燃料を加算した値を渡す。
			_Lunder.Init (_Lunder._Status.GetStatus().fuel+fBonusFuel);
		} else {
			_Lunder.Init ();
		}
		_Moon.Init ();	// 月面初期化処理
		_Camera.Init (); // カメラ初期化処理

		// クリアフラグをFalseに設定
		isClear = false;
		fStartTime = Time.timeSinceLevelLoad;
		txtGameStart.enabled = true;
		// 更新処理を設定
		modeupdate = new ModeUpdate (GameStart_Update);
	}
	/// <summary>
	/// ゲームクリア時の処理
	/// </summary>
	private void GameClear()
	{
		// スコアとボーナス燃料の計算
		float score=0;
		ScoreCalc(_Lunder._Status,_Lunder._LandingPoint,out score,out fBonusFuel);
		fScore += score;
		// GameClearUIの値設定と表示
		txtGameClear.enabled = true;
		// クリアフラグをTrueに設定
		isClear = true;
		fStartTime = Time.timeSinceLevelLoad;
		// 更新処理の設定
		modeupdate = new ModeUpdate (GameClear_Update);
	}
	/// <summary>
	/// ゲームオーバー時の処理
	/// </summary>
	private void GameOver()
	{
		txtGameOver.enabled = true;
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
		// 各オブジェクトの初期化が完了しているならゲームを開始する。
		if (_Lunder.isInit == true && _Moon.isInit == true && _Camera.isInit == true) {
			float fTime = Time.timeSinceLevelLoad - fStartTime;
			if (fTime > fStartWaitTime) {
				// ランダーの開始処理
				_Lunder.PlayStart ();
				txtGameStart.enabled = false;
				// ゲームプレイモードへ変更する。
				ChangeGameMode (MAIN_GAME_MODE.GAME_PLAYING);
			}
		}
	}
	/// <summary>
	/// GAME_PLAYINGモードの更新処理
	/// </summary>
	private void GamePlaying_Update ()
	{
		// 着陸成功ならクリアモードへ変更する。
		if (_Lunder.isLanding == true) {
			ChangeGameMode (MAIN_GAME_MODE.GAME_END_CLEAR);
		}
		// 着陸失敗ならゲームオーバーモードへ変更する。
		if (_Lunder.isDestroy == true) {
			ChangeGameMode (MAIN_GAME_MODE.GAME_END_OVER);
		}
	}
	/// <summary>
	/// GAME_END_CLEARモードの更新処理
	/// </summary>
	private void GameClear_Update()
	{
		float fTime = Time.timeSinceLevelLoad - fStartTime;

		// 待ち時間が経過した場合、次のステージへ
		if (fTime > fResultWaitTime) {
			// CLEARUIを非表示にする。
			txtGameClear.enabled = false;
			// ゲームスタートモードへ変更する。
			ChangeGameMode (MAIN_GAME_MODE.GAME_START);
		}
	}
	/// <summary>
	/// GAME_END_OVERモードの更新処理
	/// </summary>
	private void GameOver_Update()
	{
		float fTime = Time.timeSinceLevelLoad - fStartTime;
		// 待ち時間が経過した場合、タイトルシーンへ遷移する。
		if (fTime > fResultWaitTime) {
			TransitionManager.Instance.ChangeScene (GAME_SCENE.TITLE);
		}
	}
	/*---------------------------------------------------------------------*/
	private void ScoreCalc(LunderStatus status,LandingPoint point,out float score,out float bonusfuel)
	{
		LunderStatus.STATUS _status = status.GetStatus ();
		int speed =(int)(new Vector2 (_status.horizontal_speed, _status.vertical_speed).magnitude/2);

		score = (Const.MainGameData.SCORE_CALC_BASE-speed)*point.GetBonusRate();
		bonusfuel = score*Const.MainGameData.BONUS_FUEL_RATE;
	}
}
