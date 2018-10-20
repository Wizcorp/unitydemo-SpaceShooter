using UnityEngine;
using System.Collections;

public class Done_EvasiveManeuver : MonoBehaviour
{
	public Done_Boundary boundary;
	public float tilt;
	public float dodge;
	public float smoothing;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;

	private float currentSpeed;
	private float targetManeuver;

	Rigidbody rbody;

	void Start ()
	{
		rbody = GetComponent<Rigidbody>();
		currentSpeed = rbody.velocity.z;
		StartCoroutine(Evade());
	}
	
	IEnumerator Evade ()
	{
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
		while (true)
		{
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}
	
	void FixedUpdate ()
	{
		float newManeuver = Mathf.MoveTowards (rbody.velocity.x, targetManeuver, smoothing * Time.deltaTime);
		rbody.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		rbody.position = new Vector3
		(
			Mathf.Clamp(rbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp(rbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rbody.rotation = Quaternion.Euler (0, 0, rbody.velocity.x * -tilt);
	}
}
