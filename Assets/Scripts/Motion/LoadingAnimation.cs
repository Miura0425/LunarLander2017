using UnityEngine;
using System.Collections;

public class LoadingAnimation : MonoBehaviour {

	[SerializeField]
	private float RotSpeed;

	
	// Update is called once per frame
	void Update () {
		float rotZ = this.transform.rotation.eulerAngles.z;
		this.transform.rotation = Quaternion.Euler (0, 0,rotZ+RotSpeed);
	}
}
