using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.name);
		if (other.CompareTag ("Bound")) {
			this.gameObject.SetActive(false);
		}
	}
}
