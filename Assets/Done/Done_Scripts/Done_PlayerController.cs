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
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		
		//check for touches.. move ship towards first touch if present
		if(Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) 
			{
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0 ));
				
				if( touchPosition.x > transform.position.x )
				{
					moveHorizontal = 1f;
				}
				else
				{
					moveHorizontal = -1f;
				}
				
				if( touchPosition.z > transform.position.z )
				{
					moveVertical = 1f;
				}
				else
				{
					moveVertical = -1f;
				}
				
			}
			//return;
		}
		
		
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
}
