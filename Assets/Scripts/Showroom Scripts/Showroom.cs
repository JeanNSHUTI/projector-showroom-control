using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Showroom : MonoBehaviour
{
    PlayerControls playerControls;

    private void Awake()
    {
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
