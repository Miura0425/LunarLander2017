using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour {

	[SerializeField]
	private MENU_ITEM_ID _id ;

	public bool isSelect = false;

	[SerializeField]
	private Text _text;
	private Outline _outline;

	[SerializeField]
	private float fAnimationTime;
	private float fStartTime;
	private Vector2 vBaseScale;
	[SerializeField]
	private Vector2 vTargetScale;

	void Awake()
	{
		isSelect = false;
		_outline = this.GetComponent<Outline> ();
		_outline.enabled = isSelect;
		_text = this.GetComponent<Text> ();
		_text.enabled = false;
		vBaseScale = Vector2.one;
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

	public void Select()
	{
		fStartTime = Time.timeSinceLevelLoad;
		isSelect = true;
		_outline.enabled = isSelect;
	}
	public void NotSelect()
	{
		isSelect = false;
		_outline.enabled = isSelect;
		this.transform.localScale = vBaseScale;
	}

	public MENU_ITEM_ID Dicide()
	{
		
		return _id;
	}

	public void SetEnable(bool flag)
	{
		_text.enabled = flag;
	}
}
