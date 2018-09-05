using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	// How far the joystick can move
	[Tooltip("Joystick Movement Range")]
	public float movementRange = 50.0f; 

	// Joystick tint colour when pressed
	public Color pressedColour;

	// Getter for joystick position
	public Vector3 JoystickInput{
		get { return joystickInput; }
	}

	// Joystick graphic
	private Image joystick;

	// Joystick start position
	private Vector3 startPosition;

	// Joystick position from [-1, 1] range in x and y
	private Vector3 joystickInput;

	void Start()
	{
		// Cache joystick start position
		startPosition = transform.position;

		// Get Image component
		joystick = GetComponent<Image> ();
	}

	public virtual void OnPointerDown(PointerEventData pointerEvent)
	{
		SetJoystickTintColour (pressedColour);
		OnDrag (pointerEvent);
	}

	public virtual void OnPointerUp(PointerEventData pointerEvent)
	{
		SetJoystickTintColour (Color.white);
		ResetJoystick ();
	}

	public virtual void OnDrag(PointerEventData pointerEvent)
	{
		// Calculate delta position 
		Vector3 deltaPosition = new Vector3 (pointerEvent.position.x - startPosition.x, pointerEvent.position.y - startPosition.y, 0.0f);

		// Clamp joystick movement range
		Vector3 clampedVector = (deltaPosition.sqrMagnitude > movementRange * movementRange) ? deltaPosition.normalized * movementRange : deltaPosition;

		// Add new position to joystick
		joystick.transform.position = clampedVector + startPosition;

		// get a value between -1 and 1 based on the joystick graphic location
		joystickInput.x = (joystick.rectTransform.position.x + movementRange - startPosition.x) / movementRange - 1;
		joystickInput.y = (joystick.rectTransform.position.y + movementRange - startPosition.y) / movementRange - 1;
	}

	public void ResetJoystick()
	{
		// Set joystick position back to original position
		joystick.transform.position = startPosition;
		joystickInput = Vector3.zero;
	}

	public void SetJoystickTintColour(Color tintColour)
	{
		joystick.color = tintColour;
	}
}