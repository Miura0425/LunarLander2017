using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreRankingUIItem : MonoBehaviour {

	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Text _RankText = null;
	[SerializeField]
	Text _NameText = null;
	[SerializeField]
	Text _ScoreText = null;
	[SerializeField]
	Text _StageText = null;

	public void SetRanking(RankingData data)
	{
		_RankText.text = data.rank.ToString ();
		_NameText.text = data.name;
		_ScoreText.text = data.score.ToString ();
		_StageText.text = data.stage.ToString ();
	}

	public void SetActiveUI(bool value)
	{
		_RankText.enabled = value;
		_BackGround.enabled = value;
		_NameText.enabled = value;
		_ScoreText.enabled = value;
		_StageText.enabled = value;
	}

	public void SetColor(Color color)
	{
		//_BackGround.color = color;
		_RankText.color = color;
		_NameText.color = color;
		_ScoreText.color = color;
		_StageText.color = color;
	}
}
