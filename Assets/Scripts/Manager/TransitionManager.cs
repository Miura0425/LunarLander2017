using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GAME_SCENE
{
	TITLE=0,
	MAINGAME,
};

public class TransitionManager : MonoBehaviour {
	// スタティックインスタンス
	static private TransitionManager instance;
	static public TransitionManager Instance {
		get {
			return instance;
		}
	}
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
	}

	/// <summary>
	/// シーンを切り替える
	/// </summary>
	public void ChangeScene(GAME_SCENE scene)
	{
		SceneManager.LoadScene ((int)scene);
	}
	/*---------------------------------------------------------------------*/
}
