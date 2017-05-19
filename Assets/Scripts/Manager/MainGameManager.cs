using UnityEngine;
using System.Collections;

public enum MAIN_GAME_MODE
{
	GAME_START = 0,
	GAME_PLAYING,
	GAME_END_CLEAR,
	GAME_END_OVER,
}

public class MainGameManager : MonoBehaviour {
	// 現在のゲームモード
	[SerializeField]
	private MAIN_GAME_MODE _Mode;

	// モード更新処理のデリゲート
	private delegate void ModeUpdate();
	ModeUpdate modeupdate = null;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// 現在のゲームモード初期化
		_Mode = MAIN_GAME_MODE.GAME_START;
		// 更新処理の設定
		SwitchModeUpdate ();
	}
	
	// 更新処理
	void Update () {
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
		SwitchModeUpdate ();
	}

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲームモードによって更新処理を変更する。
	/// </summary>
	private void SwitchModeUpdate()
	{
		switch (_Mode) {
		case MAIN_GAME_MODE.GAME_START:
			modeupdate = new ModeUpdate (GameStart_Update);
				break;
		case MAIN_GAME_MODE.GAME_PLAYING:
			modeupdate = new ModeUpdate (GamePlaying_Update);
				break;
		case MAIN_GAME_MODE.GAME_END_CLEAR:
			modeupdate = new ModeUpdate (GameClear_Update);
				break;
		case MAIN_GAME_MODE.GAME_END_OVER:
			modeupdate = new ModeUpdate (GameOver_Update);
				break;
			default:
				break;
		}
	}
	/*---------------------------------------------------------------------*/
	/* モード更新処理 */
	/// <summary>
	/// GAME_START モードの更新処理
	/// </summary>
	private void GameStart_Update()
	{
		Debug.Log ("GameStart");
	}
	/// <summary>
	/// GAME_PLAYINGモードの更新処理
	/// </summary>
	private void GamePlaying_Update ()
	{
		Debug.Log ("GamePlaying");
	}
	/// <summary>
	/// GAME_END_CLEARモードの更新処理
	/// </summary>
	private void GameClear_Update()
	{
		Debug.Log ("GameClear");
	}
	/// <summary>
	/// GAME_END_OVERモードの更新処理
	/// </summary>
	private void GameOver_Update()
	{
		Debug.Log ("GameOver");
	}
	/*---------------------------------------------------------------------*/

}
