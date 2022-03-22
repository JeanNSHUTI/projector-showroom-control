using System.Collections;
using UnityEngine;

public class ActivateConnectedMonitors : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate.
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
