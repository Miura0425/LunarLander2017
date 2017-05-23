using UnityEngine;
using System.Collections;

public class RocketFire : MonoBehaviour {

	private SpriteRenderer _SpriteRenderer;

	private Color firecolor;

	private float fStartTime = 0;
	[SerializeField]
	private float fInterval;
	[SerializeField]
	private Color StartColor;
	[SerializeField]
	private Color EndColor;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		_SpriteRenderer = this.GetComponent<SpriteRenderer> ();
		fStartTime = Time.timeSinceLevelLoad;
	}

	// 更新処理
	void Update () {

		float fdiff = Time.timeSinceLevelLoad - fStartTime;
		float frate = fdiff / fInterval;
		if (fdiff > fInterval) {
			fStartTime = Time.timeSinceLevelLoad;
			Color tmp = StartColor;
			StartColor = EndColor;
			EndColor = tmp;
		} else {
			firecolor.r = Mathf.Lerp (StartColor.r, EndColor.r, frate);
			firecolor.g = Mathf.Lerp (StartColor.g, EndColor.g, frate);
			firecolor.b = Mathf.Lerp (StartColor.b, EndColor.b, frate);
			firecolor.a = Mathf.Lerp (StartColor.a, EndColor.a, frate);

			_SpriteRenderer.color = firecolor;
		}

	}
	/*---------------------------------------------------------------------*/
}
