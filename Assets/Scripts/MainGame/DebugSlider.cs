using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugSlider : MonoBehaviour {

	private Slider _slider;
	[SerializeField]
	private Text _value;

	void Awake()
	{
		_slider = this.GetComponent<Slider> ();
	}

	public void OnChangedValue()
	{
		_value.text = _slider.value.ToString ();
	}
}
