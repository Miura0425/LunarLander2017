using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountLoginUserUI : MonoBehaviour {
	[SerializeField]
	private Text UserName;

	void Awake()
	{
		UserName.text = "NONE";
	}

	public void SetLoginUser(string Name)
	{
		UserName.text = Name;
	}
}
