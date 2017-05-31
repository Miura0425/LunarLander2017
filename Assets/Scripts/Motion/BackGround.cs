using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackGround : MonoBehaviour {
	// 判別用タイプ
	private enum TYPE
	{
		SPRITE,	// スプライト
		IMAGE, // イメージ
	}
	private TYPE type;

	[SerializeField]
	private SpriteRenderer _sprite;	// SpriteRenderer
	[SerializeField]
	private Image _image;			// Image

	[SerializeField]
	private float Speed;			// 速さ
	private float fStartTime;		// 開始時間
	[SerializeField][Range(0,1)]
	private float Max_RGB;			// カラーRGB 最大値
	[SerializeField][Range(0,1)]
	private float Min_RGB;			// カラーRGB 最小値
	private Color _color = new Color(1,1,1,1); // 変更用カラー
	/*---------------------------------------------------------------------*/
	void Awake () {
		// SpriteRendererとImageどちらが登録されているかチャックして、タイプを設定する。
		if (_sprite != null && _image == null) {
			type = TYPE.SPRITE;
		} else if (_image != null && _sprite == null) {
			type = TYPE.IMAGE;
		} else {
			Debug.LogError ("SpriteRendererとImageの両方が設定されています。");
		}
	}

	void Start()
	{
		// 初回リセット
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		ColorUpdate ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 処理リセット処理
	/// </summary>
	private void Reset()
	{
		fStartTime = Time.timeSinceLevelLoad;
	}
	/// <summary>
	/// 色を徐々に変更し、また戻していく処理
	/// </summary>
	private void ColorUpdate()
	{
		// 色更新処理
		float diff = Time.timeSinceLevelLoad - fStartTime;
		if (diff > Speed*2) {
			Reset ();
		}
		float rate = diff / (Speed*2);
		if (diff < Speed) {
			_color.r = Mathf.Lerp (Min_RGB, Max_RGB, rate);
			_color.g = Mathf.Lerp (Min_RGB, Max_RGB, rate);
			_color.b = Mathf.Lerp (Min_RGB, Max_RGB, rate);
		} else {
			_color.r = Mathf.Lerp (Max_RGB, Min_RGB, rate);
			_color.g = Mathf.Lerp (Max_RGB, Min_RGB, rate);
			_color.b = Mathf.Lerp (Max_RGB, Min_RGB, rate);
		}
		if (type == TYPE.IMAGE) {
			_image.color = _color;
		} else if (type == TYPE.SPRITE) {
			_sprite.color = _color;
		}
	}
}
