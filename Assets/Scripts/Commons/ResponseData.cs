/* UnityWebRequestのレスポンステキストをJson形式にするので、変換後データを受け取るためのクラス */
using System.Collections.Generic;

/// <summary>
/// ユーザーがサインアップ・ログイン・データ削除した際に受け取るレスポンスデータ
/// </summary>
public class UserAccountResponseData
{
	public string message = "";
	public int num = 0;
	public string name = "";
}

/// <summary>
/// ユーザーデータの引き継ぎ処理を行った際に受け取るレスポンスデータ
/// </summary>
public class InheritResponseData
{
	public string message="";
	public string id = "";
	public string pass = "";
	public string name = "";
}

/// <summary>
/// メッセージのみのレスポンスデータ
/// </summary>
public class MessageResponseData
{
	public string message ="";
}

/// <summary>
/// プレイログのレスポンスデータ
/// </summary>
public class PlayLogResponseData
{
	public string message ="";
	public int High_Score =0;
	public int High_ClearStage = 0;
	public List<PlayLogData> LogData = new List<PlayLogData>();

}

public class ScoreRankingResponseData
{
	public string message="";
	public List<RankingData> Data = new List<RankingData>();
}
