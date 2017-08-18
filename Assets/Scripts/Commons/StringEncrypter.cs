//暗号方式(AES:ラインダール)による文字列の暗号化・複号化
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Collections;

public class StringEncrypter {
	const string SALT_STRING = "166285996a0f986efc";
	const string PASS_STRING = "219705996a1340041a";
	/// 暗号化
	/// <param name="sourceString">暗号化する文字列</param>
	/// <param name="saltString">パスワードに付加する文字</param>
	/// <param name="password">暗号化に使用するパスワード</param>
	/// <returns>暗号化された文字列</returns>
	public static string EncryptString(string sourceString)
	{
		// RijndaelManagedオブジェクトを作成する
		RijndaelManaged rijndael = new RijndaelManaged();
		rijndael.Padding = PaddingMode.Zeros;
		rijndael.Mode = CipherMode.CBC;
		rijndael.KeySize = 256;
		rijndael.BlockSize = 256;

		// パスワードから共有キーと初期化ベクタを作成する
		byte[] key, iv;
		GenerateKeyFromPassword(rijndael.KeySize, out key, rijndael.BlockSize, out iv);

		rijndael.Key = key;
		rijndael.IV = iv;

		// 文字列をバイト型配列に変換する
		byte[] strBytes = Encoding.UTF8.GetBytes(sourceString);

		// 対称暗号化オブジェクトを作成する
		ICryptoTransform encryptor = rijndael.CreateEncryptor();

		// バイト型配列を暗号化する
		byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

		// 閉じる
		encryptor.Dispose();

		// バイト型配列を文字列に変換して返す
		return System.Convert.ToBase64String(encBytes);
	}

	///復号化する
	/// <param name="sourceString">暗号化された文字列</param>
	/// <param name="saltString">パスワードに付加する文字</param>
	/// <param name="password">暗号化に使用したパスワード</param>
	/// <returns>復号化された文字列</returns>
	public static string DecryptString(string sourceString)
	{
		// RijndaelManagedオブジェクトを作成する
		RijndaelManaged rijndael = new RijndaelManaged();
		rijndael.Padding = PaddingMode.Zeros;
		rijndael.Mode = CipherMode.CBC;
		rijndael.KeySize = 256;
		rijndael.BlockSize = 256;

		// パスワードから共有キーと初期化ベクタを作成する
		byte[] key, iv;
		GenerateKeyFromPassword(rijndael.KeySize, out key, rijndael.BlockSize, out iv);

		rijndael.Key = key;
		rijndael.IV = iv;

		// 文字列をバイト型配列に戻す
		byte[] strBytes = System.Convert.FromBase64String(sourceString);

		// 対称暗号化オブジェクトを作成する
		ICryptoTransform decryptor = rijndael.CreateDecryptor();

		// バイト型配列を復号化する
		byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

		// 閉じる
		decryptor.Dispose();

		// バイト型配列を文字列に戻して返す
		return Encoding.UTF8.GetString(decBytes);
	}

	// パスワードから共有キーと初期化ベクタを生成する
	private static void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv)
	{
		// パスワードから共有キーと初期化ベクタを作成する
		// saltを決める
		byte[] salt = Encoding.UTF8.GetBytes(SALT_STRING);

		// Rfc2898DeriveBytesオブジェクトを作成する
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(PASS_STRING, salt);

		// 反復処理回数を指定する
		deriveBytes.IterationCount = 1000;

		// 共有キーと初期化ベクタを生成する
		key = deriveBytes.GetBytes(keySize / 8);
		iv = deriveBytes.GetBytes(blockSize / 8);
	}
}
