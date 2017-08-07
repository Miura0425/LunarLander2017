using UnityEngine;
using System.Collections;

public class GenericUIManager : MonoBehaviour {
	private static GenericUIManager instance = null;
	public static GenericUIManager Instance{
		get{
			return instance;
		}
	}

	public enum eType{
		MESSAGE=0,
		YESNO,
	}
	private eType mode;

	[SerializeField]
	private DialogUI[] _DialogUIs; 

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}
	/*---------------------------------------------------------------------*/
	public void SetActiveUI(bool value)
	{
		switch(mode)
		{
		case eType.MESSAGE:
			(_DialogUIs [(int)mode] as UserAccountMessageUI).SetActive (value);
			break;
		case eType.YESNO:
			(_DialogUIs [(int)mode] as UserAccountYesNoUI).SetActive (value);
			break;
		default:
			break;
		}
	}
	/*---------------------------------------------------------------------*/
	public void ShowMessageDialog(string title ,string message,UserAccountMessageUI.OKFunc func=null)
	{
		SetActiveUI (false);
		mode = eType.MESSAGE;
		(_DialogUIs [(int)mode] as UserAccountMessageUI).SetTitleAndMessage (title, message,func);
		SetActiveUI(true);
	}
	public void ShowYesNoDialog(string title,string message,UserAccountYesNoUI.YesNoFunc func=null)
	{
		SetActiveUI (false);
		mode = eType.YESNO;
		(_DialogUIs [(int)mode] as UserAccountYesNoUI).SetTitleAndMessage (title, message, func);
		SetActiveUI (true);
	}
}
