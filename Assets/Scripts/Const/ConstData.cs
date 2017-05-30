namespace Const{
	public static class LanderData{
		
		public const float INIT_FUEL = 91000.0f;		// 初期燃料

		public const float INIT_POS_Y = 3.0f;		// 初期Y座標
		public const int INIT_POS_X_MIN = -8;	// 初期X座標の最低値
		public const int INIT_POS_X_MAX = 9;	// 初期X座標の最高値

		public const float SPEED_VALUE_RATE = 200.0f;
		public const float ALTITUDE_VALUE_RATE = 100.0f;

		public const float LANDIGN_SUCCESS_SPEED = 100.0f;
		public const float LANDING_SUCCESS_ROTATION_Z = 1.0f;
	}

	public static class CameraData{

		public const float MODE_NORMAL_SIZE = 10.0f;
		public const float MODE_ZOOM_SIZE = 6.0f;

		public const float NORMAL_LANDER_ALTITUDE = 900.0f;
		public const float ZOOM_LUDER_ALTITUDE = 450.0f;

		public const float NORMAL_TOP_LINE = 5.0f;
		public const float ZOOM_LEFTRIGHT_LINE = 2.0f;
		public const float ZOOM_TOP_LINE = 3.0f;
		public const float ZOOM_BOTTOM_LINE = 3.0f;
	}

	public static class MainGameData{
		public const int BONUS_FUEL_RATE = 2;
		public const int SCORE_CALC_BASE = 100;
	}

}