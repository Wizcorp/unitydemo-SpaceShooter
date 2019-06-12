using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Done_JoyStick : MonoBehaviour, IDragHandler, IEndDragHandler {
	
	public RectTransform joystick;

	public float sensitivity;

	public void OnDrag(PointerEventData pointerEventData)
	{
		joystick.anchoredPosition += (pointerEventData.delta * sensitivity);
		
		if(Vector2.Distance(Vector2.zero, joystick.anchoredPosition) > 150f)
		{
			joystick.anchoredPosition = joystick.anchoredPosition.normalized * 150f;
		}
	}
	
	public void OnEndDrag(PointerEventData pointerEventData)
	{
		joystick.anchoredPosition = Vector2.zero;
	}

	public Vector2 GetJoystickValue()
	{
		return (joystick.anchoredPosition - Vector2.zero) / 150f;
	}
}
