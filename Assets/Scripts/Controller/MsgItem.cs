using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MsgItem : MonoBehaviour {
	private InfoText[] _InfoItem;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		_InfoItem = this.GetComponentsInChildren<InfoText> ();
	}
	/*---------------------------------------------------------------------*/
	public void SetInfo<T>(T id ,int value)where T:struct
	{
		_InfoItem [id.GetHashCode()].SetNumbers (value);
	}
	/*---------------------------------------------------------------------*/
	public void SetItemActive(bool active)
	{
		this.GetComponent<Image> ().enabled = active;
		foreach (InfoText item in _InfoItem) {
			item.SetInfoActive (active);
		}
	}
}
