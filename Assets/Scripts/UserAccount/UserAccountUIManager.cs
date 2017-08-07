using UnityEngine;
using System.Collections;

public class UserAccountUIManager : MonoBehaviour {
	private static UserAccountUIManager instance = null;
	public static UserAccountUIManager Instance{
		get{
			return instance;
		}
	}

	public enum eMode{
		SGIN_UP=0,
		LOGIN,
		ERROR,
	}
	private eMode mode = eMode.SGIN_UP;

	[SerializeField]
	private DialogUI[] _UserAccountUIs; 
	[SerializeField]
	private UserAccountLoginUserUI _LoginUserUI;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}


	/*---------------------------------------------------------------------*/
	public void ShowUserAccountUI()
	{
		if (cGameManager.Instance.UserData.Data.id == "") {
			mode = eMode.SGIN_UP;
			(_UserAccountUIs [(int)mode] as SignUpUI).Init ();
		} else{
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
		case eMode.ERROR:
			(_UserAccountUIs [(int)mode] as UserAccountErrorUI).SetActive (value);
			break;
		default:
			break;
		}
	}
	public void ChangeMode(eMode next)
	{
		SetActiveUI (false);
		mode = next;
		SetActiveUI (true);
	}
	/*---------------------------------------------------------------------*/
	public void SetLoginUser()
	{
		_LoginUserUI.SetLoginUser (cGameManager.Instance.UserData.Data.name);
	}
	/*---------------------------------------------------------------------*/
}
