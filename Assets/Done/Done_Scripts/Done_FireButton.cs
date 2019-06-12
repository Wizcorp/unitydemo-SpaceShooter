using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Done_FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Image image;
	public bool pressed = false;

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		pressed = true;

		Color color = image.color;
		color.a = 0.25f;
		image.color = color;
	}

	public void OnPointerUp(PointerEventData pointerEventData)
	{
		pressed = false;

		Color color = image.color;
		color.a = 0.75f;
		image.color = color;
	}
}
