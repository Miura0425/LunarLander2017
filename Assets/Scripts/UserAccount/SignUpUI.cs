using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SignUpUI : MonoBehaviour {

	private Image _Panel;
	private Text _TitleName;
	private InputField _InputName;
	private Button _SignUpButton;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		_Panel = this.GetComponent<Image> ();
		_TitleName = this.GetComponentInChildren<Text> ();
		_InputName = this.GetComponentInChildren<InputField> ();
		_SignUpButton = this.GetComponentInChildren<Button>();
	}
	/*---------------------------------------------------------------------*/
	public void Init()
	{
		this.SetActive (true);
		_InputName.text = "";
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 描画物のアクティブを切り替える
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void SetActive(bool value)
	{
		_Panel.enabled = value;
		_TitleName.enabled = value;
		_InputName.enabled = value;
		_SignUpButton.enabled = value;
	}

	/*---------------------------------------------------------------------*/
	public void OnSignUpButton()
	{
		if (_InputName.text != "") {
			UserAccountManager.AutoSignUp (_InputName.text);
		}
	}
}
