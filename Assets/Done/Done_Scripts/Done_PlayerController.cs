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
	AudioSource audioSrc;
	Rigidbody rbody;

	void Start(){
		audioSrc = GetComponent<AudioSource>();
		rbody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			fire();
		}
		#elif UNITY_IOS || UNITY_ANDROID
		if(Input.touchCount > 0 && Time.time > nextFire){
			fire();
		}
		#endif
	}

	void fire(){
		nextFire = Time.time + fireRate;
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		audioSrc.Play ();
	}

	void FixedUpdate ()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		moving(moveHorizontal, moveVertical);
		#elif UNITY_IOS || UNITY_ANDROID
		if(Input.touchCount > 0){
			Vector3 goal = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector3 current = GetComponent<Transform>().position;
			Vector3 normalized = (goal - current).normalized;
			float scale = 1.5f;
			moving(normalized.x*scale, normalized.z*scale);
		}
		else{
			moving(0, 0);
		}
		#endif

	}

	void moving(float horizontal, float vertical){
		Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);
		rbody.velocity = movement * speed;
		
		rbody.position = new Vector3
		(
			Mathf.Clamp (rbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rbody.rotation = Quaternion.Euler (0.0f, 0.0f, rbody.velocity.x * -tilt);
	}
}
