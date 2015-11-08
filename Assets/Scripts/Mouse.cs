using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {
	public delegate void TriggerDelegate(Collider otherObject);
	public event TriggerDelegate TriggerEvent;

	// Use this for initialization
	void Start () {
		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody> ();
		Vector3 colliderSize;

		if (collider == null) {
			collider = gameObject.AddComponent<BoxCollider>();
		}
		if (rigidbody == null) {
			rigidbody = gameObject.AddComponent<Rigidbody>();
		}
		colliderSize = collider.size;
		colliderSize.z = 10.0f;
		collider.size = colliderSize;
		collider.isTrigger = true;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) { 
		TriggerEvent(other);
	}
}
