using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour {
	// Ship maximum speed
	[Tooltip("Ship maximum movement Speed")]
	public float maxSpeed = 500;

	// Ship acceleration
	[Tooltip("Ship acceleration speed")]
	public float acceleration = 100;

	// Ship tilt angle
	[Tooltip("How far ship tilts")]
	public float tilt;

	// Movement joystick
	[Tooltip("Ship movement joystick")]
	public Joystick movementJoystick;

	// Ship boundary
	[Tooltip("Boundarys that ship can't cross")]
	public Done_Boundary boundary;

	// Bullet prefab
	[Tooltip("What kind of bullet ship will fire")]
	public GameObject shot;

	// Bullet spawn location
	[Tooltip("Where to spawn bullet")]
	public Transform shotSpawn;

	// How many times ship fires in minute
	[Tooltip("How many times ship fires in minute")]
	public float shotsPerMinute;

	// Ship movement vector
	private Vector3 movement;

	// Does ship want to fire
	private bool wantsToFire;

	// How long before we can fire again
	private float nextFire;

	// Ships current movement vector
	private Vector3 currentMovement;

	// How much time there is between shots
	private float timeBetweenShots;

	// Rigidbody Component
	private Rigidbody rigidBody;

	// AudioSource Component
	private AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		// Cache rigidbody and audio source components
		rigidBody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();

		// Initialize time between shots
		timeBetweenShots = 60.0f / shotsPerMinute;
	}
		
	void Update()
	{
		// Get ship movement vector
		movement = Vector3.zero;
		movement = Movement (Time.deltaTime);
	}

	void FixedUpdate()
	{
		// Set velocity to rigidbody component
		rigidBody.velocity = movement;

		// Clamp ship position so it can't go beyond boundarys
		rigidBody.position = new Vector3
			(
				Mathf.Clamp (rigidBody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rigidBody.position.z, boundary.zMin, boundary.zMax)
			);

		// Set ship rotation
		rigidBody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidBody.velocity.x * -tilt);
	}

	public Vector3 Movement(float deltaTime)
	{
		// Get normalized value from joystick
		Vector3 inputVector = new Vector3(movementJoystick.JoystickInput.x, 0.0f, movementJoystick.JoystickInput.y);

		// Linear interpolation to the max speed
		currentMovement = Vector3.Lerp(currentMovement, inputVector * maxSpeed, acceleration * deltaTime);

		// return current movement vector
		return currentMovement * deltaTime;
	}

	public void BeginFire()
	{
		if (!wantsToFire) 
		{
			wantsToFire = true;
			HandleFire ();
		}
	}

	public void EndFire()
	{
		if (wantsToFire) 
		{
			wantsToFire = false;
			CancelInvoke ();
		}
	}

	private void HandleFire()
	{
		// Instantitate bullet prefab
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

		// Play weapon fire audio
		audioSource.Play ();

		// Determine if refiring ship weapon is needed
		bool refiring = (wantsToFire && (timeBetweenShots > 0)) ? true : false;
		if (refiring) 
		{
			// Call weapon fire again
			Invoke ("HandleFire", timeBetweenShots);
		}
	}
}
