using UnityEngine;
using System.Collections;

public class Brine : MonoBehaviour {

	public GameObject sphereReference;
	public MKPoolManager MKPoolManager;
	public float SpawnTime;
	// Use this for initialization
	void Start () {
		StartCoroutine (spawnSphere());
	}

	IEnumerator spawnSphere()
	{
		while (true) {
			yield return new WaitForSeconds (SpawnTime);
			GameObject unusedObj = MKPoolManager.GetUnusedObject (sphereReference);
			unusedObj.rigidbody.AddTorque (new Vector3 (Random.Range (0, 90), Random.Range (0, 90), Random.Range (0, 90)));
			unusedObj.transform.position = this.RandomPos ();
		}
	}

	public Vector3 RandomPos()
	{
		float x = Random.Range (-4, 4);
		float z = Random.Range (-4, 4);
		return new Vector3 (x, 7, z);
	}
	// Update is called once per frame
	void Update () {
	

	}
}
