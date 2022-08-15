using UnityEngine;
using UnityEngine.EventSystems;
public class WindowBorder : MonoBehaviour, IDragHandler
{//probably shouldnt named this class resizer. dont feel like changing it.
	public enum Border
	{
		top,
		bottom,
		left,
		right,
		topLeft,
		topRight,
		bottomLeft,
		bottomRight
	}
	public Border border;
	public WindowBase window;
	public void OnDrag(PointerEventData data)
	{//simply passes drag events to windowbase.
		switch(border)
		{
			case Border.top : window.OnTopResize(data.delta.y); break;
			case Border.bottom : window.OnBottomResize(data.delta.y); break;
			case Border.left : window.OnLeftResize(data.delta.x); break;
			case Border.right : window.OnRightResize(data.delta.x); break;
			case Border.topLeft : window.OnTopLeftResize(new Vector2(data.delta.x,data.delta.y)); break;
			case Border.topRight : window.OnTopRightResize(new Vector2(data.delta.x,data.delta.y)); break;
			case Border.bottomLeft : window.OnBottomLeftResize(new Vector2(data.delta.x,data.delta.y)); break;
			case Border.bottomRight : window.OnBottomRightResize(new Vector2(data.delta.x,data.delta.y)); break;
		}
	}
}