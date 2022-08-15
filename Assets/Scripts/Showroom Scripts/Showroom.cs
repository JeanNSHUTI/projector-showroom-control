using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Text;
using SFB;

public class Showroom : MonoBehaviour
{
    PlayerControls playerControls;
    private Camera cam;
    //private int defWidth;
    //private int defHeight;

    private void Awake()
    {
        //defWidth = Screen.width;
        //defHeight = Screen.height;
        cam = Camera.main;
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Make sure showroom is full screen even in windowed mode
        /*if (!Screen.fullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            Screen.SetResolution(defWidth, defHeight, false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        HandleUserExit();

    }

    void LateUpdate()
    {
        CheckSave();
    }

    public void HandleUserExit()
    {
        bool isEscapeKeyPressed = playerControls.UI.Cancel.ReadValue<float>() > 0.1f;

        if (isEscapeKeyPressed)
        {
            SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_NAME);
        }
    }

    public void CheckSave()
    {

        if (playerControls.UI.Save.ReadValue<float>() > 0.1f)
        {
            // Save file with filter
            var extensionList = new[]{new ExtensionFilter("Text", "csv")};
            GameObject displayPanel = GameObject.FindWithTag("DisplayBackground");
            string data = ToCSV(displayPanel);
            //Debug.Log(data);
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "MyPlan", extensionList);
            Debug.Log(path);
            path.ToString();

            //if (path != null)
            //if (path != null && path.Length == 0)
            if (path.Length != 0)
            {
                using (var writer = new System.IO.StreamWriter(path, false))
                {
                    writer.Write(data);
                }
            }
            else
            {
                Debug.Log("Save cancelled");
            }

        }

        
    }

    public string ToCSV(GameObject displayPanel)
    {
        int i = 0;
        var sb = new StringBuilder("Index,Type,Width,Height,PositionX,PositionY");
        foreach (Transform child in displayPanel.transform)
        {
            RectTransform rt = (RectTransform)child;
            Vector3 screenPos = cam.WorldToScreenPoint(child.position);
            sb.Append('\n').Append(i.ToString()).Append(',').Append(child.name).Append(',').Append(rt.rect.width).Append(',').Append(rt.rect.height).Append(',').Append((screenPos.x - TagManager.PANEL_X_RESOLUTION)/2).Append(',').Append((TagManager.PANEL_Y_RESOLUTION - screenPos.y)/2);
            i++;
        }

        return sb.ToString();
    }


}
