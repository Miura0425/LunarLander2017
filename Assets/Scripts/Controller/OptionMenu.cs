using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour {

	private enum WINDOW_MODE
	{
		WINDOW=0,
		FULL_SCREEN,

		MODE_NUM,
	};
	private WINDOW_MODE _mode;

	private Image _image;
	[SerializeField]
	private Text _text;
	[SerializeField]
	private Text _arrow;

	public bool isEnable = false;

	void Awake()
	{
		_image = this.GetComponent<Image> ();
	}
	void Start()
	{
		SetEnable (false);
	}

	// Update is called once per frame
	void Update () {
		if (isEnable == false) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			_mode = (WINDOW_MODE)(((int)_mode+1)%(int)WINDOW_MODE.MODE_NUM);
			_text.text = _mode.ToString ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			_mode = (WINDOW_MODE)(((int)_mode+(int)WINDOW_MODE.MODE_NUM-1)%(int)WINDOW_MODE.MODE_NUM);
			_text.text = _mode.ToString ();
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			switch (_mode) {
			case WINDOW_MODE.WINDOW:
				Screen.SetResolution (960, 540, false, 60);
				break;
			case WINDOW_MODE.FULL_SCREEN:
				Screen.SetResolution (1920, 1080, true, 60);
				break;
			}

			SetEnable (false);
		}
	}

	public void SetEnable(bool flag)
	{
		isEnable = flag;
		_image.enabled = isEnable;
		_text.enabled = isEnable;
		_arrow.enabled = isEnable;
	}
}
