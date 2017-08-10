using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayLogData{
	public int score=0;
	public int stage=0;
	public string date="";
}

public class PlayData  {

	private int m_Score = 0;
	private int m_ClearStage = 0;
	private int m_Date = 0;

	private int m_HighScore = 0;
	private int m_HighStage = 0;

	private List<PlayLogData> m_PlayLogData =null;
	public List<PlayLogData> PlayLogData {
		get{ return m_PlayLogData; }
		set{ m_PlayLogData = value; }
	}

	// プロパティ
	public int Score {
		get { return m_Score; }
		set { m_Score = value; }
	}
	public int ClearStage {
		get{ return m_ClearStage; }
		set{ m_ClearStage = value; }
	}
	public int Date{
		get{ return m_Date; }
		set{m_Date = value;}
	}

	public int HighScore{
		get{ return m_HighScore; }
		set{ m_HighScore = value; }
	}
	public int HighStage{
		get{ return m_HighStage; }
		set{ m_HighStage = value; }
	}
	/*---------------------------------------------------------------------*/
	public PlayData()
	{
		
	}
	/*---------------------------------------------------------------------*/

	/*---------------------------------------------------------------------*/
	public void Save(int score,int stage)
	{
		m_Score = score;
		m_ClearStage = stage;

		if (m_HighScore < m_Score) {
			m_HighScore = m_Score;
		}
		if (m_HighStage < m_ClearStage) {
			m_HighStage = m_ClearStage;
		}
	}
	/*---------------------------------------------------------------------*/
}
