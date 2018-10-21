using UnityEngine;
using System.Collections;

public class Done_WeaponController : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;
	AudioSource audioSrc;

	void Start ()
	{
		audioSrc = GetComponent<AudioSource>();
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire ()
	{
		Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		audioSrc.Play();
	}
}
