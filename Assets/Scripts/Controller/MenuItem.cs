using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler{

	public MenuManager _MenuManger = null;

	[SerializeField]
	private MENU_ITEM_ID _id ;

	public MENU_ITEM_ID ID
	{
		get{ return _id; }
		set{ _id = value; }
	}

	public bool isSelect = false;

	[SerializeField]
	private Text _text;
	private Outline _outline;

	[SerializeField]
	private float fAnimationTime;
	private float fStartTime;
	private Vector2 vBaseScale;
	private Vector2 vBaseSize;
	[SerializeField]
	private Vector2 vTargetScale;


	public Vector2 LeftTop{
		get{ return new Vector2(transform.position.x - vBaseSize.x/2,transform.position.y); }
	}
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
		_MenuManger.SelectMenuType (_id);
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
