/* UnityWebRequestのレスポンステキストがJsonなので、変換後データを受け取るためのクラス */


/// <summary>
/// ユーザーがサインアップ・ログイン・データ削除した際に受け取るレスポンスデータ
/// </summary>
public class UserAccountResponseData
{
	public string Message = "";
	public int num = 0;
	public string name = "";
}

/// <summary>
/// ユーザーデータの引き継ぎ処理を行った際に受け取るレスポンスデータ
/// </summary>
public class InheritResponseData
{
	public string Message="";
	public string id = "";
	public string pass = "";
	public string name = "";
}
