using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerPrefsEditor :EditorWindow{
	class PrefInfo{
		public string Name;
		public string Key;
		public string Type;
	}
	PrefInfo[] keys = new PrefInfo[]{
		new PrefInfo{Name="ID", Key = Const.UserAccount.USER_ID_KEY,Type="String"},
		new PrefInfo{Name="PASS", Key = Const.UserAccount.USER_PASS_KEY,Type="String"},
		new PrefInfo{Name="NAME", Key = Const.UserAccount.USER_NAME_KEY,Type="String"},
		new PrefInfo{Name="NUM", Key = Const.UserAccount.USER_NUM_KEY,Type="Int"},
	};

	[UnityEditor.MenuItem("Tools/PlayerPrefs/ShowPrefs")]
	static void ShowPrefs()
	{
		EditorWindow window = EditorWindow.GetWindow<PlayerPrefsEditor> ("PlayerPrefs");
	}

	void OnGUI()
	{
		foreach (var keyInfo in keys) {
			switch (keyInfo.Type) {
			case "String":
				var originalString = PlayerPrefs.GetString(keyInfo.Key);
				var currentString = EditorGUILayout.TextField(keyInfo.Key, originalString);
				/*if (currentString != originalString) {
					PlayerPrefs.SetString(keyInfo.Key, currentString);
					Debug.Log ("*** update " + originalString + " => " + currentString);
				}*/
				break;

			case "int":
				var originalInteger = PlayerPrefs.GetInt (keyInfo.Key);
				var currentInteger = System.Convert.ToInt32(EditorGUILayout.TextField(keyInfo.Key, originalInteger.ToString()));
				/*if (currentInteger != originalInteger) {
					PlayerPrefs.SetInt(keyInfo.Key, currentInteger);
					Debug.Log ("*** update " + originalInteger.ToString() + " => " + currentInteger.ToString());
				}*/
				break;
			default:
				break;
			}
		}
	}


	[UnityEditor.MenuItem ("Tools/PlayerPrefs/DeleteAll")]
	static void DeleteAll()
	{
		PlayerPrefs.DeleteAll ();
		Debug.Log ("Complete PlayerPrefs DeleteAll");
	}

}