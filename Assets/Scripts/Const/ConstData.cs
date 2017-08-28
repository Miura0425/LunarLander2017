namespace Const{
	// ランダーで扱う定数
	public static class LanderData{

		public const float INIT_FUEL = 1500.0f;		// 初期燃料

		public const float INIT_POS_Y = 3.0f;		// 初期Y座標
		public const int INIT_POS_X_MIN = -15;	// 初期X座標の最低値
		public const int INIT_POS_X_MAX = 15;	// 初期X座標の最高値

		public const float INIT_VELOCITY_X = 0.5f; // 初速度X値

		public const float SPEED_VALUE_RATE = 200.0f;		// UIに送る速度値にかける倍率
		public const float ALTITUDE_VALUE_RATE = 100.0f;	// UIに送る高度値にかける倍率

		public const float LANDIGN_SUCCESS_SPEED = 100.0f;		// 着陸成功とする速度
		public const float LANDING_SUCCESS_ROTATION_Z = 1.0f;	// 着陸成功とする回転値

		public const float ALERT_FUEL = 200.0f; // 燃料警報を鳴らす残り燃料数
	}
	// カメラで扱う定数
	public static class CameraData{

		public const float MODE_NORMAL_SIZE = 10.0f;	// NORMALモードのサイズ
		public const float MODE_ZOOM_SIZE = 6.0f;		// ZOOMモードのサイズ

		public const float NORMAL_LANDER_ALTITUDE = 900.0f;	// NORMALモードに切り替えるランダーの高さ
		public const float ZOOM_LUDER_ALTITUDE = 450.0f;	// ZOOMモードに切り替えるランダーの高さ

		public const float NORMAL_TOP_LINE = 5.0f;		// NORMALモードで上方向にカメラを動かしだすY座標
		public const float ZOOM_LEFTRIGHT_LINE = 2.0f;	// ZOOMモードで左右方向にカメラを動かしだすX座標
		public const float ZOOM_TOP_LINE = 3.0f;		// ZOOMモードで上方向にカメラを動かしだすY座標
		public const float ZOOM_BOTTOM_LINE = 3.0f;		// ZOOMモードで下方向にカメラを動かしだすY座標
	}
	// メインゲームで扱う定数
	public static class MainGameData{
		public const int BONUS_FUEL_RATE = 2;	// ボーナス燃料を決めるスコアにかける倍率
		public const int SCORE_CALC_BASE = 100; // スコアを計算する際に利用するベース値
	}

	// ウェブリクエスト用定数
	public static class WebRequest{
		public const string BASE_URL = "http://ec2-54-149-25-62.us-west-2.compute.amazonaws.com/LunarLanderServer/index.php/LLS/";
		public const string HEADER_NAME_COOKIE = "Cookie";
	}

	// ユーザーアカウント用定数
	public static class UserAccount{
		public const string USER_ID_KEY = "ID";
		public const string USER_PASS_KEY = "PASS";
		public const string USER_NAME_KEY = "NAME";
		public const string USER_NUM_KEY = "NUM";

	}

	public static class PlayData{
		public const string PLAY_HIGHSCORE_KEY = "HIGHSCORE";
		public const string PLAY_HIGHSTAGE_KEY = "HIGHSTAGE";
	}
}
