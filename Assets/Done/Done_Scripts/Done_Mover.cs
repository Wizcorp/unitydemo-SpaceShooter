using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;
	private Rigidbody rigidbody;
	
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = transform.forward * speed;
	}
}
