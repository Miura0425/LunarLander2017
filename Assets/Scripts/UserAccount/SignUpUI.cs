using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SignUpUI : DialogUI {

	[SerializeField]
	private InputField _InputName;
	[SerializeField]
	private Button _SignUpButton;
	/*---------------------------------------------------------------------*/

	/*---------------------------------------------------------------------*/
	public virtual void Init()
	{
		this.SetActive (true);
		_InputName.text = "";
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 描画物のアクティブを切り替える
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).name != _WaitImg.name || !value)
			transform.GetChild (i).gameObject.SetActive (value);
		}
	}

	/*---------------------------------------------------------------------*/
	public void OnSignUpButton()
	{
		if (_InputName.text != "") {
			_InputName.gameObject.SetActive(false);
			_SignUpButton.gameObject.SetActive(false);
			_WaitImg.gameObject.SetActive(true);
			StartCoroutine(UserAccountManager.AutoSignUp (_InputName.text));
		}
	}
}
