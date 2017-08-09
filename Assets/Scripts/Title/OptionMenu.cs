using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour {

	// ウィンドウ設定モード
	private enum WINDOW_MODE
	{
		WINDOW=0,
		FULL_SCREEN,

		MODE_NUM,
	};
	private WINDOW_MODE _mode;

	private Image _image; // 自身のイメージ
	[SerializeField]
	private Text _text;	// 子のテキスト
	[SerializeField]
	private Text _arrow; // 子の矢印テキスト

	public bool isEnable = false; // 表示中かどうか

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// 自身のイメージを取得
		_image = this.GetComponent<Image> ();
	}
	void Start()
	{
		// 非表示にする。
		SetEnable (false);
	}

	// 更新処理
	void Update () {
		InputSelect ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 表示・非表示を切り替える。
	/// </summary>
	/// <param name="flag">true:表示　false:非表示</param>
	public void SetEnable(bool flag)
	{
		isEnable = flag;
		_image.enabled = isEnable;
		_text.enabled = isEnable;
		_arrow.enabled = isEnable;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 入力処理
	/// </summary>
	private void InputSelect()
	{
		// 非表示なら処理しない
		if (isEnable == false) {
			return;
		}
		// →キーが押されたら次のモードへ
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			_mode = (WINDOW_MODE)(((int)_mode+1)%(int)WINDOW_MODE.MODE_NUM);
			_text.text = _mode.ToString ();
		}
		// ←キーが押されてたら前のモードへ
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			_mode = (WINDOW_MODE)(((int)_mode+(int)WINDOW_MODE.MODE_NUM-1)%(int)WINDOW_MODE.MODE_NUM);
			_text.text = _mode.ToString ();
		}
		// エンターキーが押されたら選択しているモードへ切り替える。
		if (Input.GetKeyDown (KeyCode.Return)) {
			switch (_mode) {
			case WINDOW_MODE.WINDOW: // ウィンドウモード
				Screen.SetResolution (960, 540, false, 60);
				break;
			case WINDOW_MODE.FULL_SCREEN: // フルスクリーンモード
				Screen.SetResolution (1920, 1080, true, 60);
				break;
			}
			// 非表示にする。
			SetEnable (false);
		}
	}
	/*---------------------------------------------------------------------*/
}
