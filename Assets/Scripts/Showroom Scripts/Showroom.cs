using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Showroom : MonoBehaviour
{
    PlayerControls playerControls;
    //private int defWidth;
    //private int defHeight;

    private void Awake()
    {
        //defWidth = Screen.width;
        //defHeight = Screen.height;
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

    public void HandleUserExit()
    {
        bool isEscapeKeyPressed = playerControls.UI.Cancel.ReadValue<float>() > 0.1f;

        /*if (Input.GetKeyDown(KeyCode.Esc))
        {
            SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_NAME);
        }*/
        if (isEscapeKeyPressed)
        {
            SceneManager.LoadScene(TagManager.MAIN_MENU_SCENE_NAME);
        }
    }


}
