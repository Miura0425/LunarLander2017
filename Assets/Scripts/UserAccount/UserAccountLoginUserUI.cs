using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountLoginUserUI : MonoBehaviour {
	[SerializeField]
	private Text UserName;

	public void SetLoginUser(string Name)
	{
		string name = Name;
		if (Name == "") {
			name = "NONE";
		}
		UserName.text = Name;
	}
}
