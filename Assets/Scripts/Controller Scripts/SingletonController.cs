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

    [SerializeField]
    private GameObject handler;

    private int _charIndex;
    private Texture2D[] _imageTextures;
    private GameObject _outputPanel;
    private string[] _URLs;
    private string[] _fileExtensions;
    private int noOfelements;
    private bool screenDivisionby2;
    private bool screenDivisionby1;
    private string[] isSpecialTemplate;
    private bool loadAfile, reset;
    private string loadAfilePath;

    public string supportRSS = ".xml";
    public string[] supportedImageExts = { ".png", ".jpg"};
    public string[] supportedVideoExts = { ".mov", ".mp4", ".asf", ".avi", ".m4v", ".dv", ".mpg", ".mpeg", ".ogv", ".vp8", ".webm", ".wmv" };

    public bool ScreenDivisionby2
    {
        get { return screenDivisionby2; }
        set { screenDivisionby2 = value; }
    }

    public bool ScreenDivisionby1
    {
        get { return screenDivisionby1; }
        set { screenDivisionby1 = value; }
    }

    public bool Reset
    {
        get { return reset; }
        set { reset = value; }
    }

    public bool LoadAFile
    {
        get { return loadAfile; }
        set { loadAfile = value; }
    }

    public string LoadAFilePath
    {
        get { return loadAfilePath; }
        set { loadAfilePath = value; }
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

    public GameObject Handler
    {
        get { return handler; }
        set { handler = value; }
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

    public string getSpecialTemplate(int index)
    {
        return isSpecialTemplate[index];
    }

    public void setSpecialTemplate(string templateType, int index)
    {
        isSpecialTemplate[index] = templateType;
    }

    private void Awake()
    {
        //Instantiate information arrays
        ImageTextures = new Texture2D[5];
        URLs = new string[5];
        FileExtensions = new string[5];
        isSpecialTemplate = new string[5];

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

    GameObject GetMediaTemplate(int index, string mediaTemplate)
    {
        GameObject mediaPrefab = null;
        if (mediaTemplate == "I")
        {
            if (getSpecialTemplate(index) == TagManager.SPECIAL_T_CIRCLE)
            {
                mediaPrefab = AddTemplate(index, TagManager.I_CIRCLE_PREFAB_INDEX);
            }
            else if (getSpecialTemplate(index) == TagManager.SPECIAL_T_TRIANGLE)
            {
                mediaPrefab = AddTemplate(index, TagManager.I_TRIANGLE_PREFAB_INDEX);
            }
            else
            {
                mediaPrefab = AddTemplate(index, TagManager.IMAGE_PREFAB_INDEX);
            }
        }
        if (mediaTemplate == "V")
        {
            if (getSpecialTemplate(index) == TagManager.SPECIAL_T_CIRCLE)
            {
                mediaPrefab = AddTemplate(index, TagManager.V_CIRCLE_PREFAB_INDEX);
            }
            else if (getSpecialTemplate(index) == TagManager.SPECIAL_T_TRIANGLE)
            {
                mediaPrefab = AddTemplate(index, TagManager.V_TRIANGLE_PREFAB_INDEX);
            }
            else
            {
                mediaPrefab = AddTemplate(index, TagManager.VIDEO_PREFAB_INDEX);
            }
        }

        if (mediaTemplate == "W")
        {

            string time = DateTime.Now.ToString("h:mm:ss tt");
            Debug.Log("Your time: " + time);
            int hour = int.Parse(time.Split(':')[0]);
            string dayOrnight = time.Split(' ')[1];

            if (dayOrnight.Equals("pm") && hour >= TagManager.NIGHT_TIME)
            {
                mediaPrefab = AddTemplate(index, TagManager.W_NIGHT_PREFAB_INDEX);
            }
            else
            {
                mediaPrefab = AddTemplate(index, TagManager.W_DAY_PREFAB_INDEX);
            }
        }
        return mediaPrefab;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        //if (scene.name == TagManager.SHOWROOM_SCENE_NAME)
        if (scene.name == TagManager.SHOWROOM_SCENE_NAME && NoOfElements > 0)
        {

            if (Reset)
            {
                Debug.Log("Resetting: ");
                ClearShowroomDisplay();
                Reset = false;
            }
            //GameObject display_panel = GameObject.FindWithTag("DisplayPanel").transform.Find("Background").gameObject;
            GameObject display_panel = GameObject.FindWithTag("DisplayBackground");
            //Debug.Log("Elements: " + NoOfElements);

            for (int i = 0; i < NoOfElements; i++)
            {
                if (supportedImageExts.Contains(FileExtensions[i]))
                {
                    GameObject imagePrefab;
                    imagePrefab = GetMediaTemplate(i, "I");
                    //imagePrefab = AddTemplate(i, TagManager.IMAGE_PREFAB_INDEX);

                    if (ScreenDivisionby2)
                    {
                        int x = -1;
                        if (i == 1) { x = 1; }
                        Set_Size(imagePrefab, TagManager.NEXT_POS_DISPLAY*2, TagManager.Y_RESOLUTION, i);
                        imagePrefab.transform.position = new Vector3(((x) * TagManager.NEXT_POS_DISPLAY), 0, 0);
                    }
                    else if (ScreenDivisionby1)
                    {
                        Set_Size(imagePrefab, TagManager.NEXT_POS_DISPLAY*4, TagManager.Y_RESOLUTION, i);
                        imagePrefab.transform.position = new Vector3(0, 0, 0);
                    }
                    else if (LoadAFile)
                    {
                        string[] lines = System.IO.File.ReadAllLines(LoadAFilePath);
                        //int x = 0;
                        string[] parts = lines[i+1].Split(','); //ignore csv headers

                        float savedwidth = float.Parse(parts[2]);
                        float savedheight = float.Parse(parts[3]);
                        float savedPosX = float.Parse(parts[4]);
                        float savedPosY = float.Parse(parts[5]);

                        Set_Size(imagePrefab, savedwidth, savedheight, i);
                        //mediaTemplate.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
                        imagePrefab.transform.position = new Vector3(savedPosX * 2f, savedPosY * 2f, 0);

                    }
                    else
                    {
                        Set_Size(imagePrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    }
                    //Set_Size(imagePrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    DisplayPicture(imagePrefab, display_panel, i);
                }

                if (supportedVideoExts.Contains(FileExtensions[i]))
                {
                    GameObject videoPrefab;
                    videoPrefab = GetMediaTemplate(i, "V");
                    //videoPrefab = AddTemplate(i, TagManager.VIDEO_PREFAB_INDEX);
                    if (ScreenDivisionby2)
                    {
                        int x = -1;
                        if (i == 1) { x = 1; }
                        Set_Size(videoPrefab, TagManager.NEXT_POS_DISPLAY * 2, TagManager.Y_RESOLUTION, i);
                        videoPrefab.transform.position = new Vector3(((x) * TagManager.NEXT_POS_DISPLAY), 0, 0);
                    }
                    else if (ScreenDivisionby1)
                    {
                        Set_Size(videoPrefab, TagManager.NEXT_POS_DISPLAY * 4, TagManager.Y_RESOLUTION, i);
                        videoPrefab.transform.position = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        Set_Size(videoPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    }
                    //Set_Size(videoPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    DisplayVideo(videoPrefab, display_panel, i);
                }

                if (supportRSS.Equals(FileExtensions[i]))
                {
                    GameObject weatherPrefab;
                    weatherPrefab = GetMediaTemplate(i, "W");
                        
                    if (ScreenDivisionby2)
                    {
                        int x = -1;
                        if (i == 1) { x = 1; }
                        Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY * 2, TagManager.Y_RESOLUTION, i);
                        weatherPrefab.transform.position = new Vector3(((x) * TagManager.NEXT_POS_DISPLAY), 0, 0);
                    }
                    else if (ScreenDivisionby1)
                    {
                        Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY * 4, TagManager.Y_RESOLUTION, i);
                        weatherPrefab.transform.position = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    }
                    //Set_Size(weatherPrefab, TagManager.NEXT_POS_DISPLAY, TagManager.Y_RESOLUTION, i);
                    DisplayRssFeed(weatherPrefab, display_panel, i);
                }

                //Debug.Log("File extension: " + getFileExtension(0));
            }
        }
        /*if (scene.name == TagManager.MAIN_MENU_SCENE_NAME)
        {
            //Reset special shape templates

        }*/

    }
    
    public void DisplayRssFeed(GameObject weatherPrefab, GameObject displaypanel, int i)
    {
        FillInDummyData(weatherPrefab);
        weatherPrefab.transform.SetParent(displaypanel.transform, false);
        GetRSSdata(weatherPrefab, getURL(i));
        
    }

    public void DisplayPicture(GameObject imagePrefab, GameObject displaypanel, int i)
    {
        if (getSpecialTemplate(i) == TagManager.SPECIAL_T_CIRCLE)
        {
            imagePrefab.transform.Find("Image").GetComponent<RawImage>().texture = getImageTexture(i);
            //ResetShapeSelectFlags();
        }
        else if (getSpecialTemplate(i) == TagManager.SPECIAL_T_TRIANGLE)
        {
            imagePrefab.transform.Find("Image").GetComponent<RawImage>().texture = getImageTexture(i);
            //ResetShapeSelectFlags();
        }
        else
        {
            imagePrefab.GetComponent<RawImage>().texture = getImageTexture(i);
        }
        //imagePrefab.GetComponent<RawImage>().texture = getImageTexture(i);
        imagePrefab.transform.SetParent(displaypanel.transform, false);
        
    }

    public void DisplayVideo(GameObject videoPrefab, GameObject displaypanel, int i)
    {
        RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);
        {
            videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
            videoPrefab.GetComponent<RawImage>().texture = rt;
            videoPrefab.GetComponent<VideoPlayer>().source = VideoSource.Url;
            videoPrefab.GetComponent<VideoPlayer>().url = getURL(i);
        }
        videoPrefab.transform.SetParent(displaypanel.transform, false);
        //ResetShapeSelectFlags();
    }

    public void ResetShapeSelectFlags()
    {
        //LoadAFile = false;
        //LoadAFilePath = false;
        for(int i = 0; i < isSpecialTemplate.Length; i++)
        {
            setSpecialTemplate("", i);
        }

    }

    public void ClearShowroomDisplay()
    {
        //Clear display panel of elements
        GameObject displaypanel = GameObject.FindWithTag("DisplayBackground");
        Debug.Log("display Elements: " + displaypanel.transform.childCount);
        if (displaypanel.transform.childCount > 0)
        {
            //Delete existing object
            //foreach(GameObject child in SingletonController.instance.OutputPanel.gameObject)
            for (int x = 0; x < displaypanel.transform.childCount; x++)
            {
                Debug.Log("Deleting..");
                DestroyImmediate(displaypanel.transform.Find(x.ToString()).gameObject);
                //DestroyImmediate(child);
                //NoOfElements = displaypanel.transform.childCount;
                //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
            }
        }
    }

    public void ClearOutputDisplay()
    {
        //Clear display panel of elements
        //Debug.Log("display output Elements: " + OutputPanel.transform.childCount);
        int noOfElements = OutputPanel.transform.childCount;
        if (OutputPanel.transform.childCount > 0)
        {
            //Delete existing object
            //foreach(GameObject child in SingletonController.instance.OutputPanel.gameObject)
            for (int x = 0; x < noOfElements; x++)
            {
                Debug.Log("Deleting..");
                DestroyImmediate(OutputPanel.transform.Find(x.ToString()).gameObject);
                //DestroyImmediate(child);
                //NoOfElements = displaypanel.transform.childCount;
                //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
            }

            NoOfElements = 0;
        }
    }

    public static void Set_Size(GameObject gameObject, float width, float height, int i)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, height);
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
                //mediaPrefab.name = "0";
                break;

            case 1:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                //mediaPrefab.name = "1";
                break;

            case 2:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                //mediaPrefab.name = "2";
                break;

            case 3:
                mediaPrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.DISPLAY_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_DISPLAY), 0, 0), Quaternion.identity);
                //mediaPrefab.name = "3";
                break;

            default:
                Debug.Log("Not more than 4 templates can be added to ouput");
                //mediaPrefab.name = "4";
                break;
        }
        return mediaPrefab;
    }

    //Fill in dummy data
    public void FillInDummyData(GameObject weatherprefab)
    {
        weatherprefab.transform.Find("txtCity").GetComponent<Text>().text = "City: " + TagManager.NAN;
        weatherprefab.transform.Find("txtTemp").GetComponent<Text>().text = "Temp: " + TagManager.NAN;
        weatherprefab.transform.Find("txtDate").GetComponent<Text>().text = "Date: " + TagManager.NAN;
        weatherprefab.transform.Find("txtLink").GetComponent<Text>().text = "Link: " + TagManager.NAN;
        weatherprefab.transform.Find("txtDescription").GetComponent<Text>().text = TagManager.NAN;
    }

    //For demonstration purposes as I did not find a cheap/free RSS feed for weather
    //public async void GetRSSdata(GameObject weatherprefab, string path)
    public void GetRSSdata(GameObject weatherprefab, string path)
    {
        //UnityEngine.Object[] sprites;
        UnityEngine.Sprite[] sprites = Resources.LoadAll<Sprite>("RuntimeSprites/2");

        // connect to the rss feed and pull it
        rssreader rdr = new rssreader(path);

        string description = rdr.rowNews.item[0].description;
        string[] info = description.Split('<');
        //string test = info[0] + " " + info[1] + " " + info[2] + " " info[3] + " " + info[4] + " " + info[5];
        string[] cityinfo = rdr.rowNews.title.Split('-');

        //Update weather widget
        weatherprefab.transform.Find("txtTemp").GetComponent<Text>().text = rdr.rowNews.item[0].title;
        weatherprefab.transform.Find("txtCity").GetComponent<Text>().text = cityinfo[0];
        weatherprefab.transform.Find("txtDate").GetComponent<Text>().text = rdr.rowNews.item[0].pubDate;
        weatherprefab.transform.Find("txtDescription").GetComponent<Text>().text = info[0];
        weatherprefab.transform.Find("txtLink").GetComponent<Text>().text = rdr.rowNews.item[0].link;

        if (rdr.rowNews.item[0].title.Contains("Sunny"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[3];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
        else if (rdr.rowNews.item[0].title.Contains("Cloudy"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[22];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
        else if (rdr.rowNews.item[0].title.Contains("Windy"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[31];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
        else if (rdr.rowNews.item[0].title.Contains("Rain"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[21];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
        else if (rdr.rowNews.item[0].title.Contains("Snow"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[24];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
        else if (rdr.rowNews.item[0].title.Contains("storm"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[32];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }


        //var feed = await CodeHollow.FeedReader.FeedReader.ReadAsync(path);
        //string description = feed.Items[0].Description;
        //string[] info = description.Split('<');
        //string test = info[0] + " " + info[1] + " " + info[2] + " " info[3] + " " + info[4] + " " + info[5];
        //string[] city = info[0].Split(' ');
        //city = city[0].Split(' ');

        //Debug.Log("Feed Image: " + feed.ImageUrl);

        /*foreach (var item in feed.Items)
        {
            Console.WriteLine(item.Title + " - " + item.Link);
        }*/

        //Update weather widget
        //weatherprefab.transform.Find("txtTemp").GetComponent<Text>().text = feed.Items[0].Title;
        //weatherprefab.transform.Find("txtCity").GetComponent<Text>().text = info[2] + info[3];
        //weatherprefab.transform.Find("txtDate").GetComponent<Text>().text = feed.Items[0].Date;
        //weatherprefab.transform.Find("txtDescription").GetComponent<Text>().text = info[0];
        //weatherprefab.transform.Find("txtLink").GetComponent<Text>().text = feed.Items[0].Link;


        //Test for example if rainy cloud is present in feed
        /*if (feed.Title.Contains(".com"))
        {
            weatherprefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[33];
            weatherprefab.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }*/

    }


}
