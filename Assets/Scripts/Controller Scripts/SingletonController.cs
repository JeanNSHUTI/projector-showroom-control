using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SingletonController : MonoBehaviour
{
    public static SingletonController instance;

    [SerializeField]
    private GameObject[] importMedia;

    private int _charIndex;
    private Texture2D[] _imageTextures;
    private GameObject _outputPanel;
    private string[] _URLs;
    private string[] _fileExtensions;
    private int noOfelements;
    private bool isResizablePanel;

    public string supportRSS = ".xml";
    public string[] supportedImageExts = { ".png", ".jpg"};
    public string[] supportedVideoExts = { ".mov", ".mp4", ".asf", ".avi", ".m4v", ".dv", ".mpg", ".mpeg", ".ogv", ".vp8", ".webm", ".wmv" };

    public bool IsResizablePanel
    {
        get { return isResizablePanel; }
        set { isResizablePanel = value; }
    }

    public int NoOfElements
    {
        get { return noOfelements; }
        set { noOfelements = value; }
    }

    public GameObject OutputPanel
    {
        get { return _outputPanel; }
        set { _outputPanel = value; }
    }

    public int CharIndex
    {
        get { return _charIndex; }
        set { _charIndex = value; }
    }

    public Texture2D[] ImageTextures
    {
        get { return _imageTextures; }
        set { _imageTextures = value; }
    }

    public string[] URLs
    {
        get { return _URLs; }
        set { _URLs = value; }
    }

    public string[] FileExtensions
    {
        get { return _fileExtensions; }
        set { _fileExtensions = value; }
    }

    public GameObject[] getMediaObjects
    {
        get { return importMedia; }
        set { importMedia = value; }
    }

    public GameObject getMediaObject(int index)
    {
        return importMedia[index];
    }

    public void setMediaObject(GameObject gameObject, int index)
    {
        importMedia[index] = gameObject;
    }

    public Texture2D getImageTexture(int index)
    {
        return _imageTextures[index];
    }

    public void setImageTexture(Texture2D tex, int index)
    {
        _imageTextures[index] = tex;
    }

    public string getURL(int index)
    {
        return _URLs[index];
    }

    public void setURL(string url, int index)
    {
        _URLs[index] = url;
    }

    public string getFileExtension(int index)
    {
        return _fileExtensions[index];
    }

    public void setFileExtension(string path, int index)
    {
        _fileExtensions[index] = path;
    }

    private void Awake()
    {
        //Instantiate information arrays
        ImageTextures = new Texture2D[5];
        URLs = new string[5];
        FileExtensions = new string[5];


        //OutputPanel = GameObject.FindWithTag("OutputPanel");
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    /*void Update()
    {
        NoOfElements = OutputPanel.transform.childCount;
    }*/

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        //if (scene.name == TagManager.SHOWROOM_SCENE_NAME)
        if (scene.name == TagManager.SHOWROOM_SCENE_NAME & NoOfElements > 0)
        {
            GameObject display_panel = GameObject.FindWithTag("DisplayPanel");

            for (int i = 0; i < NoOfElements; i++)
            {
                if (supportedImageExts.Contains(FileExtensions[i]))
                {
                    GameObject imagePrefab;
                    if (IsResizablePanel)
                    {
                        imagePrefab = AddTemplate(i, (TagManager.IMAGE_PREFAB_INDEX + TagManager.JUMP_PREFAB_PICKER));
                    }
                    else
                    {
                        imagePrefab = AddTemplate(i, TagManager.IMAGE_PREFAB_INDEX);
                    }
                    //GameObject imagePrefab = AddTemplate(i, TagManager.IMAGE_PREFAB_INDEX);
                    Set_Size(imagePrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION);
                    DisplayPicture(imagePrefab, display_panel, i);
                }

                if (supportedVideoExts.Contains(FileExtensions[i]))
                {
                    GameObject videoPrefab = AddTemplate(i, TagManager.VIDEO_PREFAB_INDEX);
                    Set_Size(videoPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION);
                    DisplayVideo(videoPrefab, display_panel, i);
                }

                if (supportRSS.Equals(FileExtensions[i]))
                {
                    string time = DateTime.Now.ToString("h:mm:ss tt");
                    Debug.Log("Your time: " + time);
                    int hour = int.Parse(time.Split(':')[0]);
                    string dayOrnight = time.Split(' ')[1];

                    if (dayOrnight.Equals("pm") && hour >= TagManager.NIGHT_TIME)
                    {
                        GameObject weatherPrefab = AddTemplate(i, TagManager.W_NIGHT_PREFAB_INDEX);
                        Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION);
                        DisplayRssFeed(weatherPrefab, display_panel, i);
                    }
                    else
                    {
                        GameObject weatherPrefab = AddTemplate(i, TagManager.W_DAY_PREFAB_INDEX);
                        Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION);
                        DisplayRssFeed(weatherPrefab, display_panel, i);
                    }
                }

                Debug.Log("File extension: " + getFileExtension(0));
            }
        }

    }
    
    public void DisplayRssFeed(GameObject weatherPrefab, GameObject displaypanel, int i)
    {
        FillInDummyData(weatherPrefab);
        weatherPrefab.transform.SetParent(displaypanel.transform, false);
        GetRSSdata(weatherPrefab, getURL(i));
        
    }

    public void DisplayPicture(GameObject imagePrefab, GameObject displaypanel, int i)
    {
        if (IsResizablePanel)
        {
            imagePrefab.transform.Find("resizablePanel").GetComponent<RawImage>().texture = getImageTexture(i);
        }
        else
        {
            imagePrefab.GetComponent<RawImage>().texture = getImageTexture(i);
        }            
        imagePrefab.transform.SetParent(displaypanel.transform, false);
    }

    public void DisplayVideo(GameObject videoPrefab, GameObject displaypanel, int i)
    {
        RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);
        videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
        videoPrefab.GetComponent<RawImage>().texture = rt;
        videoPrefab.GetComponent<VideoPlayer>().source = VideoSource.Url;
        videoPrefab.GetComponent<VideoPlayer>().url = getURL(i);
        videoPrefab.transform.SetParent(displaypanel.transform, false);
    }

    public static void Set_Size(GameObject gameObject, float width, float height)
    {
        if (gameObject != null)
        {
            if (SingletonController.instance.IsResizablePanel)
            {
                var rectTransform = gameObject.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, height);
                }
                rectTransform = gameObject.transform.Find("resizablePanel").GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width-20, height-20);
                }
            }
            else
            {
                var rectTransform = gameObject.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, height);
                }
            }

        }
    }

    GameObject AddTemplate(int noOftemplates, int mediaObject)
    {
        GameObject[] prefabs = getMediaObjects;
        GameObject mediaPrefab = new GameObject();

        switch (noOftemplates)
        {
            case 0:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION, 0, 0), Quaternion.identity);
                break;

            case 1:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                break;

            case 2:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                break;

            case 3:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                break;

            default:
                Debug.Log("Not more than 4 templates can be added to ouput");
                break;
        }
        return mediaPrefab;
    }

    //Fill in dummy data
    public void FillInDummyData(GameObject weatherprefab)
    {
        weatherprefab.transform.Find("txtCity").GetComponent<Text>().text = "City: " + TagManager.NAN;
        weatherprefab.transform.Find("txtTemp").GetComponent<Text>().text = "Temp: " + TagManager.NAN;
        weatherprefab.transform.Find("txtHumidity").GetComponent<Text>().text = "Humidity: " + TagManager.NAN;
        weatherprefab.transform.Find("txtPressure").GetComponent<Text>().text = "Pressure: " + TagManager.NAN;
        weatherprefab.transform.Find("txtWindSpeed").GetComponent<Text>().text = "Wind Speed: " + TagManager.NAN;
        weatherprefab.transform.Find("txtWindDegree").GetComponent<Text>().text = "Degree: " + TagManager.NAN;
        weatherprefab.transform.Find("txtDescription").GetComponent<Text>().text = TagManager.NAN;
    }

    //For demonstration purposes as I did not find a cheap/free RSS feed for weather
    public async void GetRSSdata(GameObject weatherprefab, string path)
    {
        UnityEngine.Object[] sprites;
        sprites = Resources.LoadAll("RuntimeSprites/2");
        var feed = await CodeHollow.FeedReader.FeedReader.ReadAsync(path);

        //Debug.Log("Feed Image: " + feed.ImageUrl);

        /*foreach (var item in feed.Items)
        {
            Console.WriteLine(item.Title + " - " + item.Link);
        }*/

        //Update weather widget
        weatherprefab.transform.Find("txtCity").GetComponent<Text>().text = feed.Title;
        weatherprefab.transform.Find("txtWindDegree").GetComponent<Text>().text = "° : " + feed.Language;
        weatherprefab.transform.Find("txtDescription").GetComponent<Text>().text = feed.Description;


        //Test for example if rainy cloud is present in feed
        if (feed.Title.Contains(".com"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[33];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }

    }




}
