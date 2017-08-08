using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// メニュー項目
/// </summary>
public class MenuItem : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler{

	// メニューマネージャ
	public MenuManager _MenuManger = null;

	// メニューの種類ID
	[SerializeField]
	private MENU_ITEM_ID _id ;


	// 選択フラグ
	public bool isSelect = false;

	[SerializeField]
	private Text _text; // テキスト
	private Outline _outline; // アウトラインコンポーネント

	[SerializeField]
	private float fAnimationTime; // アニメーション時間
	private float fStartTime; // アニメーション開始時間
	private Vector2 vBaseScale; // 変化前スケール
	private Vector2 vBaseSize; // 変化前オブジェクトサイズ
	[SerializeField]
	private Vector2 vTargetScale; // 変化後スケール


	// プロパティ
	public MENU_ITEM_ID ID
	{
		get{ return _id; }
		set{ _id = value; }
	}
	// オブジェクトの左上座標
	public Vector2 LeftTop{
		get{ return new Vector2(transform.position.x - vBaseSize.x/2,transform.position.y); }
	}
	// オブジェクトの右下座標
	public Vector2 RightBottom{
		get{ return new Vector2(transform.position.x + vBaseSize.x/2,transform.position.y); }
	}
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		isSelect = false;
		_outline = this.GetComponent<Outline> ();
		_outline.enabled = isSelect;
		_text = this.GetComponent<Text> ();
		_text.enabled = false;
		vBaseScale = Vector2.one;
		vBaseSize = GetComponent<RectTransform> ().sizeDelta;
	}

	void Update()
	{
		if (isSelect == true) {
			float diff = Time.timeSinceLevelLoad - fStartTime;
			if (diff > fAnimationTime) {
				fStartTime = Time.timeSinceLevelLoad;
				this.transform.localScale = vBaseScale;
			}
			float rate = diff / fAnimationTime;
			if (diff < fAnimationTime / 2) {
				this.transform.localScale = Vector2.Lerp (vBaseScale, vTargetScale, rate);
			} else {
				this.transform.localScale = Vector2.Lerp (vTargetScale, vBaseScale, rate);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Select ();
		_MenuManger.nSelectIdx = (int)_id;
		_MenuManger.AnotherMenuNotSelect (_id);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		NotSelect ();
	}

	/*---------------------------------------------------------------------*/
	public void Select()
	{
		fStartTime = Time.timeSinceLevelLoad;
		isSelect = true;
		_outline.enabled = isSelect;
	}
	/*---------------------------------------------------------------------*/
	public void NotSelect()
	{
		isSelect = false;
		_outline.enabled = isSelect;
		this.transform.localScale = vBaseScale;
	}

	/*---------------------------------------------------------------------*/
	public void SetEnable(bool flag)
	{
		_text.enabled = flag;
	}
	/*---------------------------------------------------------------------*/
}
