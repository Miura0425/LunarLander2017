using UnityEngine;
using System.Collections;

public class UserAccountUIManager : MonoBehaviour {

	private enum eMode{
		SGIN_UP=0,
		LOGIN,
	}
	private eMode mode = eMode.SGIN_UP;

	[SerializeField]
	private UserAccountUI[] _UserAccountUIs; 
	[SerializeField]
	private UserAccountLoginUserUI _LoginUserUI;

	/*---------------------------------------------------------------------*/
	public void CheckUserAccount()
	{
		//PlayerPrefs.SetString ("ID", "");
		//PlayerPrefs.SetString ("PASS", "");
		if (PlayerPrefs.GetString ("ID") == "") {
			mode = eMode.SGIN_UP;
			(_UserAccountUIs [(int)mode] as SignUpUI).Init ();
		} else {
			mode = eMode.LOGIN;
			(_UserAccountUIs [(int)mode] as LoginUI).Init ();
		}


	}
	/*---------------------------------------------------------------------*/
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
		default:
			break;
		}
	}
	/*---------------------------------------------------------------------*/
	public void SetLoginUser()
	{
		_LoginUserUI.SetLoginUser (PlayerPrefs.GetString (Const.UserAccount.USER_NAME_KEY));
	}
}
