using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandleMoveMedia : MonoBehaviour
{
    //public Vector3 parentCenter = Vector3.zero;
    //private Camera mainCam;
    //private float startDistance, currentScale, currentAngle;
    //private Vector3 startPosition;

    public GameObject ui_canvas;
    GraphicRaycaster ui_raycaster;
    PointerEventData click_data;
    List<RaycastResult> click_results;

    List<GameObject> clicked_elements;

    //Resize variables
    /*public float lineWidth = .1f;
    public float handlerSize = 0.2f;
    private Bounds bounds;
    private LineRenderer line;
    private int segments = 360;
    private int pointCount;
    private float radius;
    private Vector3[] points;
    private float zPosition = 0.0f;
    private GameObject raycastPlane;
    private float initialScale;
    private Quaternion initialRotation;*/

    bool dragging = false;
    public bool isBeingResized = false;
    GameObject drag_element;

    Vector3 mouse_position;
    Vector3 previous_mouse_position;

    [SerializeField]
    private float m_panelResizableAreaLength = 12f;
    public float PanelResizableAreaLength { get { return m_panelResizableAreaLength; } }

    // Start is called before the first frame update
    void Start()
    {
        //mainCam = Camera.main;
        //startDistance = (transform.position - parentCenter).magnitude;
        //currentScale = (transform.position - parentCenter).magnitude / startDistance;
        //startPosition = transform.position;
        //startPosition = transform.position;

        ui_raycaster = ui_canvas.GetComponent<GraphicRaycaster>();
        click_data = new PointerEventData(EventSystem.current);
        click_results = new List<RaycastResult>();
        clicked_elements = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseDragUi();
    }

    void MouseDragUi()
    {
        /** Houses the main mouse dragging logic. **/

        mouse_position = Mouse.current.position.ReadValue();
        //previous_mouse_position = mouse_position;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DetectUi();
            //Debug.Log("Right click");
            //AddLineRenderer(drag_element);
            //bounds = result.gameObject.transform.GetChild(0).GetComponent<Renderer>().bounds;
            //bounds = GameObject.FindWithTag("DisplayBackground").transform.GetChild(0).GetComponent<Renderer>().bounds;
            //bounds = drag_element.GetComponent<Renderer>().bounds;
            //RemoveLineRenderer(drag_element);
            //DrawCircle(drag_element);
        }

        if (Mouse.current.rightButton.isPressed && isBeingResized)
        {
            //DragElement();
            //Debug.Log("Resizing..");
            ResizeElement();
            //DrawCircle(drag_element);

        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectUi();
        }

        if (Mouse.current.leftButton.isPressed && dragging)
        {
            DragElement();
        }
        else
        {
            dragging = false;
            isBeingResized = false;
            
        }

        previous_mouse_position = mouse_position;
    }

    void DetectUi()
    {
        /** Detect if the mouse has been clicked on a UI element, and begin dragging **/

        GetUiElementsClicked();

        if (clicked_elements.Count > 0)
        {
            dragging = true;
            /*if(mouse_position != previous_mouse_position)
            {
                isBeingResized = true;
            }*/
            isBeingResized = true;
            drag_element = clicked_elements[0];
        }
    }

    void GetUiElementsClicked()
    {
        /** Get all the UI elements clicked, using the current mouse position and raycasting. **/

        click_data.position = mouse_position;
        click_results.Clear();
        ui_raycaster.Raycast(click_data, click_results);

        // Optimised version
        //clicked_elements = (from result in click_results select result.gameObject).ToList();

        // Foreach version
        clicked_elements.Clear();
        foreach (RaycastResult result in click_results)
        {
            clicked_elements.Add(result.gameObject);
            //bounds = result.gameObject.transform.GetChild(0).GetComponent<Renderer>().bounds;
        }

    }

    void DragElement()
    {
        /** Drag a UI element across the screen based on the mouse movement. **/

        //Controll what can be moved by user
        if(drag_element.tag != TagManager.DISPLAY_BACKGROUND)
        {
            RectTransform element_rect = drag_element.GetComponent<RectTransform>();

            Vector2 drag_movement = mouse_position - previous_mouse_position;
            element_rect.anchoredPosition = element_rect.anchoredPosition + drag_movement;
            //Debug.Log("Tag: " + drag_element.name);
        }

    }

    void ResizeElement()
    {
        //if (SingletonController.instance.Handler.GetComponent<ResizeHandler>().Scale() > 0)
        //    drag_element.transform.localScale = Vector3.one * initialScale * SingletonController.instance.Handler.GetComponent<ResizeHandler>().Scale();

        if (drag_element.tag != TagManager.DISPLAY_BACKGROUND)
        {
            //Mouse.current.position.ReadValue();
            /*Ray castPoint = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("UI");
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, mask))
            {
                transform.position = hit.point;
            }
            currentScale = (transform.position - parentCenter).magnitude / startDistance;
            currentAngle = CalculateAngle();*/

            //Debug.Log(drag_element.name);
            RectTransform rectTransform = drag_element.GetComponent<RectTransform>();
            float newSizeX = rectTransform.rect.width; 
            float newSizeY = rectTransform.rect.height;
            //mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
            //previous_mouse_position = Camera.main.ScreenToWorldPoint(previous_mouse_position);
            //Vector3 mouse_p = Camera.main.ScreenToWorldPoint(mouse_position);
            //Vector3 p_mouse_p = Camera.main.ScreenToWorldPoint(previous_mouse_position);
            if (rectTransform != null)
            {
                Vector2 drag_movement = mouse_position - previous_mouse_position;
                //Vector2 drag_movement = mouse_p - p_mouse_p;
                newSizeX = newSizeX + (drag_movement.x * 100f);
                newSizeY = newSizeY + (drag_movement.y * 100f);

                //rectTransform.sizeDelta = new Vector2(rectTransform.rect.width + drag_movement.x, rectTransform.rect.height + drag_movement.y);
                rectTransform.sizeDelta = new Vector2(newSizeX, newSizeY);
                //Debug.Log("X:" + newSizeX);
                //Debug.Log("Y:" + newSizeY);
                //Debug.Log("Drag:" + drag_movement);
                //Debug.Log("Mouse:" + mouse_position);
                //Debug.Log("Previous Mouse:" + previous_mouse_position);
            }

        }
    }

    public void StopResizing()
    {
        isBeingResized = false;
        //RemoveLineRenderer(drag_element);
    }

    /*private void AddLineRenderer(GameObject gO)
    {
        if (line == null)
        {
            points = new Vector3[pointCount];
            gO.AddComponent<LineRenderer>();
            line = gO.GetComponent<LineRenderer>();
            line.startWidth = lineWidth; line.endWidth = lineWidth;
            line.positionCount = pointCount;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.red; line.endColor = Color.red;

            SingletonController.instance.Handler = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SingletonController.instance.Handler.transform.localScale = new Vector3(handlerSize, handlerSize, handlerSize);
            SingletonController.instance.Handler.gameObject.AddComponent<ResizeHandler>();
            SingletonController.instance.Handler.GetComponent<ResizeHandler>().parentCenter = gO.transform.position;

            Vector3 cameraDirection = gO.transform.position - Camera.main.transform.position;
            radius = bounds.extents.magnitude * gO.transform.localScale.x * 1.2f;
            float rad = Mathf.Deg2Rad * (360f / segments);
            Vector3 handlerPos = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, zPosition) + gO.transform.position;
            handlerPos = RotatePointAroundPivot(handlerPos, gO.transform.position, cameraDirection);
            SingletonController.instance.Handler.transform.position = handlerPos;

            raycastPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Destroy(raycastPlane.GetComponent<MeshRenderer>());
            raycastPlane.transform.LookAt(Camera.main.transform);
            raycastPlane.transform.Rotate(new Vector3(90, 0, 0));
            raycastPlane.transform.localScale = new Vector3(1000f, 1000f, 1000f);
            raycastPlane.layer = 5;
        }
    }

    private void RemoveLineRenderer(GameObject gO)
    {
        if (gO.gameObject.GetComponent<LineRenderer>() != null)
        {
            Destroy(gO.gameObject.GetComponent<LineRenderer>());
            points = null;
            Destroy(SingletonController.instance.Handler.gameObject);
            Destroy(raycastPlane.gameObject);
            initialScale = gO.transform.localScale.x;
            initialRotation = gO.transform.rotation;
        }
    }

    void DrawCircle(GameObject gO)
    {
        Vector3 cameraDirection = gO.transform.position - Camera.main.transform.position;

        radius = bounds.extents.magnitude * this.transform.localScale.x * 1.2f;
        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, zPosition) + this.transform.position;
            points[i] = RotatePointAroundPivot(points[i], transform.position, cameraDirection);
        }

        line.SetPositions(points);
        line.alignment = LineAlignment.View;
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.LookRotation(angles, Vector3.up) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }*/

    /*private float CalculateAngle()
    {
        Vector3 startVector = startPosition - parentCenter;
        Vector3 endVector = transform.position - parentCenter;
        float angle = Vector3.Angle(startVector, endVector);

        float sign = Mathf.Sign(Vector3.Dot(mainCam.transform.forward, Vector3.Cross(startVector, endVector)));
        return angle * sign;
    }

    public float Scale()
    {
        return currentScale;
    }
    public float Angle()
    {
        return currentAngle;
    }*/

    /*private void OnRectTransformDimensionsChange()
    {
        updateBounds = true;
    }

    private void UpdateBounds()
    {
        Size = RectTransform.rect.size;

        RootPanelGroup.Internal.UpdateBounds(Vector2.zero, Size);
        UnanchoredPanelGroup.Internal.UpdateBounds(Vector2.zero, Size);
    }

    private void ResizeAnchoredPanelsRecursively(PanelGroup group, Dictionary<Panel, Vector2> initialSizes)
    {
        if (group == null)
            return;

        int count = group.Count;
        for (int i = 0; i < count; i++)
        {
            Panel panel = group[i] as Panel;
            if (panel != null)
            {
                Vector2 initialSize;
                if (initialSizes.TryGetValue(panel, out initialSize))
                    panel.ResizeTo(initialSize, Direction.Right, Direction.Top);
            }
            else
                ResizeAnchoredPanelsRecursively(group[i] as PanelGroup, initialSizes);
        }
    }*/

}
