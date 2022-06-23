using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    private GameObject _outputPanel;


    public GameObject OutputPanel
    {
        get { return _outputPanel; }
        set { _outputPanel = value; }
    }

    private void Awake()
    {
        //_outputPanel = GameObject.FindWithTag("OutputPanel");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_outputPanel = GameObject.FindWithTag("OutputPanel");
        //_outputPanel = new GameObject();

    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
