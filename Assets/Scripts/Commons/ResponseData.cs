/* UnityWebRequestのレスポンステキストをJson形式にするので、変換後データを受け取るためのクラス */


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

public class PlayDataResponseData
{
	public string message="";
	public int high_score=0;
	public int high_stage=0;

	public string PlayLog="";
}
