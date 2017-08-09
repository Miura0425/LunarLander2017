using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// ダイアログベースクラス
public class DialogUI : MonoBehaviour {

	[SerializeField]
	protected Image _BackBord; // 背景イメージ
	[SerializeField]
	protected Text _TitleName; // タイトルテキスト
	[SerializeField]
	protected Image _WaitImg; // 待ち用イメージ


	public virtual void Init (){}

	public virtual void SetActive (bool value){}

	public virtual void SetTitle(string title)
	{
		_TitleName.text = title;
	}

}
