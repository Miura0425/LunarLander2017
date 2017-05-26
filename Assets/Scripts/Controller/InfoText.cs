using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

	// 数値用のText
	[SerializeField]
	private Text txtNumbers;

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 数値用のTextの設定
	/// </summary>
	/// <param name="num">設定値</param>
	public void SetNumbers(int num)
	{
		// 数値用Textがnullなら処理しない
		if (txtNumbers == null) {
			return;
		}
		// 値を設定
		txtNumbers.text = num.ToString ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 自身と数値用Textのアクティブを切り替える。
	/// </summary>
	/// <param name="active">設定値 true or false</param>
	public void SetInfoActive(bool active)
	{
		// 自身のTextコンポーネントのアクティブを切り替える。
		this.GetComponent<Text> ().enabled = active;
		// 数値用Textがnullでなければアクティブを切り替える。
		if (txtNumbers != null) {
			txtNumbers.enabled = active;
		}
	}
}
