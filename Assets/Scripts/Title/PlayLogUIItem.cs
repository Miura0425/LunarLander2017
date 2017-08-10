using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayLogUIItem : MonoBehaviour {

	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Text _ScoreText = null;
	[SerializeField]
	Text _StageText = null;
	[SerializeField]
	Text _DateText = null;

	public void SetPlayLog(PlayLogData data)
	{
		_ScoreText.text = data.score.ToString ();
		_StageText.text = data.stage.ToString ();
		_DateText.text = data.date.ToString ();
	}

	public void SetActiveUI(bool value)
	{
		_BackGround.enabled = value;
		_ScoreText.enabled = value;
		_StageText.enabled = value;
		_DateText.enabled = value;
	}
}
