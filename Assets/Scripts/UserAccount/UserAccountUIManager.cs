using UnityEngine;
using System.Collections;

/// <summary>
/// ユーザーアカウントに関するUIを管理する。
/// </summary>
public class UserAccountUIManager : MonoBehaviour {
	private static UserAccountUIManager instance = null;
	public static UserAccountUIManager Instance{
		get{
			return instance;
		}
	}

	// 表示モード
	public enum eMode{
		SGIN_UP=0,
		LOGIN,
		ERROR,
	}
	private eMode mode = eMode.SGIN_UP;

	[SerializeField]
	private DialogUI[] _UserAccountUIs; // 使用するダイアログ
	[SerializeField]
	private UserAccountLoginUserUI _LoginUserUI; // ログインユーザーUI

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}


	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ユーザーアカウントUIを表示する
	/// </summary>
	public void ShowUserAccountUI()
	{
		// ユーザーデータにIDが無い場合 サインアップ
		if (cGameManager.Instance.UserData.Data.id == "") {
			mode = eMode.SGIN_UP;
			(_UserAccountUIs [(int)mode] as SignUpUI).Init ();

		} else{ // ある場合 ログイン
			mode = eMode.LOGIN;
			(_UserAccountUIs [(int)mode] as LoginUI).Init ();
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// UIのアクティブ設定
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void SetActiveUI(bool value)
	{
		switch(mode)
		{
		case eMode.SGIN_UP:
			(_UserAccountUIs [(int)mode] as SignUpUI).SetActive (value);
			break;
		case eMode.LOGIN:
			(_UserAccountUIs [(int)mode] as LoginUI).SetActive (value);
			break;
		case eMode.ERROR:
			(_UserAccountUIs [(int)mode] as UserAccountErrorUI).SetActive (value);
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// モードを切り替える
	/// </summary>
	/// <param name="next">Next.</param>
	public void ChangeMode(eMode next)
	{
		SetActiveUI (false);
		mode = next;
		SetActiveUI (true);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ログインユーザーUIを更新する
	/// </summary>
	public void SetLoginUser()
	{
		_LoginUserUI.SetLoginUser (cGameManager.Instance.UserData.Data.name);
	}
	/*---------------------------------------------------------------------*/
}
