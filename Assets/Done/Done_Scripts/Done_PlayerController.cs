using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;
	
	private Rigidbody rigidbody;
	private AudioSource audio;

	void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
	}

	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}

	void AccelerometerMovement()
	{
		Vector3 dir = Vector3.zero;
		dir.x = Mathf.Clamp (Input.acceleration.x, boundary.xMin, boundary.xMax);
		dir.z = Mathf.Clamp (Input.acceleration.y, boundary.zMin, boundary.zMax);

		if (dir.sqrMagnitude > 1)
			dir.Normalize();

		dir *= Time.deltaTime;

		rigidbody.transform.Translate (dir * speed * 2);
	}

	void KeyboardMovement()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}

	void FixedUpdate ()
	{
		AccelerometerMovement();
		KeyboardMovement();		
	}
}
