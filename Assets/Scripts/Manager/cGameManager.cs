using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GAME_SCENE
{
	TITLE=0,
	MAINGAME,
};

public class cGameManager : MonoBehaviour {

	// スタティックインスタンス
	static private cGameManager instance;
	static public cGameManager Instance {
		get {
			return instance;
		}
	}
	// ユーザーデータ
	private UserAccountData m_UserData = new UserAccountData();
	public UserAccountData UserData {
		get{ return m_UserData; }
	}

	public bool IsOffline = false; // オフラインフラグ

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// インスタンスが存在するなら自身を削除する。
		if (instance != null) {
			Destroy (this.gameObject);
		} else { // インスタンスが存在しないなら自信を登録する。
			instance = this;
		}

		// シーンのロード時に削除されないように設定する。
		DontDestroyOnLoad (this);

		StartCoroutine (Load ()); // ローカルデータのロード
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ローカルデータのロード処理
	/// </summary>
	public IEnumerator Load()
	{
		// ユーザーデータのロード
		m_UserData.LoadUserData ();
		yield return null;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// シーンを切り替える
	/// </summary>
	public void ChangeScene(GAME_SCENE scene)
	{
		SceneManager.LoadScene ((int)scene);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// アプリ終了を検出
	/// </summary>
	void OnApplicationQuit() {
		// 切断リクエスト
		StartCoroutine (UserAccountManager.DisconnectRequest ());
	}

	/// <summary>
	/// アプリのバックグラウンドへの移行と復帰を検知する。
	/// </summary>
	/// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			Debug.Log("applicationWillResignActive or onPause");
		} else {
			Debug.Log("applicationDidBecomeActive or onResume");
			// 引き継ぎ処理待ち中ならば処理待ちを終了する。
			if (UserAccountManager.IsInheritWait) {
				UserAccountManager.IsInheritWait = false;
			}
		}
	}
	/*---------------------------------------------------------------------*/
}
