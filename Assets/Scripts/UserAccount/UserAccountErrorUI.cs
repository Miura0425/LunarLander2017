using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountErrorUI : UserAccountUI {

	[SerializeField]
	private Button _ReturnButton;
	[SerializeField]
	private Button _OfflineButton;

	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (value);
		}
	}

	public void OnReturnButton()
	{
		cGameManager.Instance.ChangeScene (GAME_SCENE.TITLE);
	}

	public void OnOfflineButton()
	{
		cGameManager.Instance.IsOffline = true;
		cGameManager.Instance.ChangeScene (GAME_SCENE.TITLE);
	}
}
