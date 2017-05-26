using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MsgItem : MonoBehaviour {

	// 情報アイテム
	private InfoText[] _InfoItem;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// 情報アイテムの取得
		_InfoItem = this.GetComponentsInChildren<InfoText> ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 情報アイテムの更新
	/// </summary>
	/// <param name="id">更新する情報アイテムの種類</param>
	/// <param name="value">設定値</param>
	/// <typeparam name="T">MsgItemを継承するクラス内のEnum</typeparam>
	public void SetInfo<T>(T item ,int value)where T:struct
	{
		_InfoItem [item.GetHashCode()].SetNumbers (value);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 自身と情報アイテムのアクティブを切り替える。
	/// </summary>
	/// <param name="active">設定値 true or false</param>
	public void SetItemActive(bool active)
	{
		// 自身のImageコンポーネントのアクティブを切り替える。
		this.GetComponent<Image> ().enabled = active;
		// 情報アイテムのアクティブを切り替える。
		foreach (InfoText item in _InfoItem) {
			item.SetInfoActive (active);
		}
	}
}
