using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {
	[SerializeField]
	private Text txtNumbers;
	/*---------------------------------------------------------------------*/
	public void SetNumbers(int num)
	{
		txtNumbers.text = num.ToString ();
	}
	/*---------------------------------------------------------------------*/
	public void SetInfoActive(bool active)
	{
		this.GetComponent<Text> ().enabled = active;
		txtNumbers.enabled = active;
	}
}
