using UnityEngine;
using System.Collections;

public class TitleManger : MonoBehaviour {
	[SerializeField,TooltipAttribute("スタートするためのキー")]
	private KeyCode StartKey;

	[SerializeField]
	private MenuManager _Menu;
	[SerializeField]
	private PressMsg _PressMsg;

	private bool isStart = false;

	/*---------------------------------------------------------------------*/
	// 更新処理
	void Update () {
		CheckStartKey ();
	}
	/// <summary>
	/// スタートキーが押されたかチェックする。
	/// </summary>
	private void CheckStartKey()
	{
		if(isStart == false && Input.anyKeyDown)
		{
			isStart = true;
			_PressMsg.gameObject.SetActive (false);
			_Menu.Init ();
		}
	}
	/*---------------------------------------------------------------------*/

}
