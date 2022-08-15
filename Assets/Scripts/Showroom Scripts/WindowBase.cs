using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WindowBase : MonoBehaviour, IDragHandler
{
	//TODO: fix the choppiness if resizing in relative mode and you drag too quickly.
	//TODO: test how this works with children elements
	public enum ResizingStyle
	{
		Uniform,
		Relative
	}
	public bool draggable;
	public ResizingStyle horizontalStyle;
	public ResizingStyle verticalStyle;
	public ResizingStyle cornerStyle;
	public bool topResizer;
	public bool bottomResizer;
	public bool leftResizer;
	public bool rightResizer;
	public bool topLeftResizer;
	public bool topRightResizer;
	public bool bottomLeftResizer;
	public bool bottomRightResizer;
	public float minWidth;
	public float minHeight;
	public float maxWidth;
	public float maxHeight;
	float verticalResize = 0f;
	float horizontalResize = 0f;
	RectTransform rectTransform;
	RectTransform topRectTransform;
	RectTransform bottomRectTransform;
	RectTransform leftRectTransform;
	RectTransform rightRectTransform;
	RectTransform topLeftRectTransform;
	RectTransform topRightRectTransform;
	RectTransform bottomLeftRectTransform;
	RectTransform bottomRightRectTransform;
	WindowBorder topWindowBorder;
	WindowBorder bottomWindowBorder;
	WindowBorder leftWindowBorder;
	WindowBorder rightWindowBorder;
	WindowBorder topLeftWindowBorder;
	WindowBorder topRightWindowBorder;
	WindowBorder bottomLeftWindowBorder;
	WindowBorder bottomRightWindowBorder;



	GameObject BorderDummy; //The border dummy is simply a gameobject with recttransform and a WindowBorder script component.
	void Awake()
	{
		BorderDummy = GameObject.FindGameObjectWithTag("Border");
		BorderDummy.SetActive(false);//need to rework this for multiple windows, as other WindowBases won't be able to find it with tag if it's disabled.


		rectTransform = GetComponent<RectTransform>();
		verticalResize = rectTransform.rect.height;
		horizontalResize = rectTransform.rect.width;
		if(topResizer)
		{
			topRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			topRectTransform.SetParent(rectTransform);
			topRectTransform.gameObject.SetActive(true);
			topRectTransform.anchorMax = new Vector2(1f,1f);
			topRectTransform.anchorMin = new Vector2(0f,1f);
			topRectTransform.position = new Vector3(rectTransform.position.x,topRectTransform.position.y,topRectTransform.position.z);
			topRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
			topRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -10f, 10f);
			topWindowBorder = topRectTransform.GetComponent<WindowBorder>();
			topWindowBorder.window = this;
			topWindowBorder.border = WindowBorder.Border.top;
			topWindowBorder.gameObject.name = "TopWindowBorder";
		}
		if(bottomResizer)
		{
			bottomRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			bottomRectTransform.SetParent(rectTransform);
			bottomRectTransform.gameObject.SetActive(true);
			bottomRectTransform.anchorMax = new Vector2(1f,0f);
			bottomRectTransform.anchorMin = new Vector2(0f,0f);
			bottomRectTransform.position = new Vector3(rectTransform.position.x,bottomRectTransform.position.y,bottomRectTransform.position.z);
			bottomRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width);
			bottomRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, -10f, 10f);
			bottomWindowBorder = bottomRectTransform.GetComponent<WindowBorder>();
			bottomWindowBorder.window = this;
			bottomWindowBorder.border = WindowBorder.Border.bottom;
			bottomWindowBorder.gameObject.name = "BottomWindowBorder";
		}
		if(leftResizer)
		{
			leftRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			leftRectTransform.SetParent(rectTransform);
			leftRectTransform.gameObject.SetActive(true);
			leftRectTransform.anchorMax = new Vector2(0f,1f);
			leftRectTransform.anchorMin = new Vector2(0f,0f);
			leftRectTransform.position = new Vector3(leftRectTransform.position.x,rectTransform.position.y,leftRectTransform.position.z);
			leftRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
			leftRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -10f, 10f);
			leftWindowBorder = leftRectTransform.GetComponent<WindowBorder>();
			leftWindowBorder.window = this;
			leftWindowBorder.border = WindowBorder.Border.left;
			leftWindowBorder.gameObject.name = "LeftWindowBorder";
		}
		if(rightResizer)
		{
			rightRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			rightRectTransform.SetParent(rectTransform);
			rightRectTransform.gameObject.SetActive(true);
			rightRectTransform.anchorMax = new Vector2(1f,1f);
			rightRectTransform.anchorMin = new Vector2(1f,0f);
			rightRectTransform.position = new Vector3(rightRectTransform.position.x,rectTransform.position.y,rightRectTransform.position.z);
			rightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.height);
			rightRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, -10f, 10f);
			rightWindowBorder = rightRectTransform.GetComponent<WindowBorder>();
			rightWindowBorder.window = this;
			rightWindowBorder.border = WindowBorder.Border.right;
			rightWindowBorder.gameObject.name = "RightWindowBorder";
		}
		if(topLeftResizer)
		{
			topLeftRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			topLeftRectTransform.SetParent(rectTransform);
			topLeftRectTransform.gameObject.SetActive(true);
			topLeftRectTransform.anchorMax = new Vector2(0f,1f);
			topLeftRectTransform.anchorMin = new Vector2(0f,1f);
			topLeftRectTransform.anchoredPosition = new Vector2(-5f,5f);
			topLeftRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
			topLeftRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
			topLeftWindowBorder = topLeftRectTransform.GetComponent<WindowBorder>();
			topLeftWindowBorder.window = this;
			topLeftWindowBorder.border = WindowBorder.Border.topLeft;
			topLeftWindowBorder.gameObject.name = "TopLeftWindowBorder";
		}
		if(topRightResizer)
		{
			topRightRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			topRightRectTransform.SetParent(rectTransform);
			topRightRectTransform.gameObject.SetActive(true);
			topRightRectTransform.anchorMax = new Vector2(1f,1f);
			topRightRectTransform.anchorMin = new Vector2(1f,1f);
			topRightRectTransform.anchoredPosition = new Vector2(5f,5f);
			topRightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
			topRightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
			topRightWindowBorder = topRightRectTransform.GetComponent<WindowBorder>();
			topRightWindowBorder.window = this;
			topRightWindowBorder.border = WindowBorder.Border.topRight;
			topRightWindowBorder.gameObject.name = "TopRightWindowBorder";
		}
		if(bottomLeftResizer)
		{
			bottomLeftRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			bottomLeftRectTransform.SetParent(rectTransform);
			bottomLeftRectTransform.gameObject.SetActive(true);
			bottomLeftRectTransform.anchorMax = new Vector2(0f,0f);
			bottomLeftRectTransform.anchorMin = new Vector2(0f,0f);
			bottomLeftRectTransform.anchoredPosition = new Vector2(-5f,-5f);
			bottomLeftRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
			bottomLeftRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
			bottomLeftWindowBorder = bottomLeftRectTransform.GetComponent<WindowBorder>();
			bottomLeftWindowBorder.window = this;
			bottomLeftWindowBorder.border = WindowBorder.Border.bottomLeft;
			bottomLeftWindowBorder.gameObject.name = "BottomLeftWindowBorder";
		}
		if(bottomRightResizer)
		{
			bottomRightRectTransform = (Instantiate(BorderDummy) as GameObject).GetComponent<RectTransform>();
			bottomRightRectTransform.SetParent(rectTransform);
			bottomRightRectTransform.gameObject.SetActive(true);
			bottomRightRectTransform.anchorMax = new Vector2(1f,0f);
			bottomRightRectTransform.anchorMin = new Vector2(1f,0f);
			bottomRightRectTransform.anchoredPosition = new Vector2(5f,-5f);
			bottomRightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
			bottomRightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10f);
			bottomRightWindowBorder = bottomRightRectTransform.GetComponent<WindowBorder>();
			bottomRightWindowBorder.window = this;
			bottomRightWindowBorder.border = WindowBorder.Border.bottomRight;
			bottomRightWindowBorder.gameObject.name = "BottomRightWindowBorder";
		}
	}
	public void OnDrag(PointerEventData eventData)
	{
		if(draggable)
			rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y);
	}
	public void OnTopResize(float amount)
	{
		if(verticalStyle == ResizingStyle.Uniform)
		{
			amount *= 2f;
			if(amount < 0f && (verticalResize+amount < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount > 0f && (verticalResize+amount > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
		}
		else if (verticalStyle == ResizingStyle.Relative)
		{
			if(amount < 0f && (verticalResize+amount < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount > 0f && (verticalResize+amount > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount;
				rectTransform.Translate(0f,amount/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
		}
	}
	public void OnBottomResize(float amount)
	{
		if(verticalStyle == ResizingStyle.Uniform)
		{
			amount *= -2f;
			if(amount < 0f && (verticalResize+amount < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount > 0f && (verticalResize+amount > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
		}
		else if (verticalStyle == ResizingStyle.Relative)
		{
			amount = -amount;
			if(amount < 0f && (verticalResize+amount < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount > 0f && (verticalResize+amount > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount;
				rectTransform.Translate(0f,-amount/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
		}
	}
	public void OnLeftResize(float amount)
	{
		if(horizontalStyle == ResizingStyle.Uniform)
		{
			amount *= -2f;
			if(amount < 0f && (horizontalResize+amount < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount > 0f && (horizontalResize+amount > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if (horizontalStyle == ResizingStyle.Relative)
		{
			amount = -amount;
			if(amount < 0f && (horizontalResize+amount < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount > 0f && (horizontalResize+amount > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount;
				rectTransform.Translate(-amount/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
	public void OnRightResize(float amount)
	{
		if(horizontalStyle == ResizingStyle.Uniform)
		{
			amount *= 2f;
			if(amount < 0f && (horizontalResize+amount < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount > 0f && (horizontalResize+amount > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if (horizontalStyle == ResizingStyle.Relative)
		{
			if(amount < 0f && (horizontalResize+amount < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount > 0f && (horizontalResize+amount > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount;
				rectTransform.Translate(amount/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
	public void OnTopLeftResize(Vector2 amount)
	{
		if(cornerStyle == ResizingStyle.Uniform)
		{
			amount *= 2f;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			amount = -amount;
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if(cornerStyle == ResizingStyle.Relative)
		{
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.Translate(0f,amount.y/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			amount = -amount;
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.Translate(-amount.x/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
	public void OnTopRightResize(Vector2 amount)
	{
		if(cornerStyle == ResizingStyle.Uniform)
		{
			amount *= 2f;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if(cornerStyle == ResizingStyle.Relative)
		{
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.Translate(0f,amount.y/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.Translate(amount.x/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
	public void OnBottomLeftResize(Vector2 amount)
	{
		if(cornerStyle == ResizingStyle.Uniform)
		{
			amount *= -2f;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if(cornerStyle == ResizingStyle.Relative)
		{
			amount = -amount;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.Translate(0f,-amount.y/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.Translate(-amount.x/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
	public void OnBottomRightResize(Vector2 amount)
	{
		if(cornerStyle == ResizingStyle.Uniform)
		{
			amount *= -2f;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			amount = -amount;
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
		else if(cornerStyle == ResizingStyle.Relative)
		{
			amount = -amount;
			if(amount.y < 0f && (verticalResize+amount.y < minHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minHeight);
				verticalResize = minHeight;
			}
			else if(amount.y > 0f && (verticalResize+amount.y > maxHeight))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
				verticalResize = maxHeight;
			}
			else
			{
				verticalResize += amount.y;
				rectTransform.Translate(0f,-amount.y/2f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, verticalResize);
			}
			
			amount = -amount;
			if(amount.x < 0f && (horizontalResize+amount.x < minWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minWidth);
				horizontalResize = minWidth;
			}
			else if(amount.x > 0f && (horizontalResize+amount.x > maxWidth))
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
				horizontalResize = maxWidth;
			}
			else
			{
				horizontalResize += amount.x;
				rectTransform.Translate(amount.x/2f,0f,0f);
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontalResize);
			}
		}
	}
}