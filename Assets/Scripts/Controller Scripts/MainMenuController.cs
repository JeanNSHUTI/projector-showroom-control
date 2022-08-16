using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using SFB;

#if UNITY_EDITOR
    using UnityEditor;
#endif


public class MainMenuController : MonoBehaviour
{
    private int noOftemplates;
    private bool template0Selected, template1Selected, template2Selected, template3Selected;
    private bool circleTemplate, triangleTemplate;
    private float previousTemplateSize;
    private int editTemplate = -1;


    void Start()
    {
        SingletonController.instance.OutputPanel = GameObject.FindWithTag("OutputPanel");
    }

    // Update is called once per frame
    void Update()
    {
        ResetNoTemplates();
        UpdateTemplateFlags();
        //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
    }

    public void PlayShowroom()
    {
        SceneManager.LoadScene(TagManager.SHOWROOM_SCENE_NAME);
    }

    public void ResetNoTemplates()
    {
        if (noOftemplates > 3 && !(template0Selected || template1Selected || template2Selected || template3Selected) )
        {
            //reload
            noOftemplates = 0;
            DeactivateMediaButtons();

        }
        else
        {
            noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
            //SingletonController.instance.NoOfElements = noOftemplates;
            if ((SingletonController.instance.NoOfElements > 0) &&
                (SingletonController.instance.LoadAFile || SingletonController.instance.ScreenDivisionby2 || SingletonController.instance.ScreenDivisionby1) && 
                !(template0Selected || template1Selected || template2Selected || template3Selected))
            {
                DeactivateMediaButtons();
            }
            else
            {
                ActivateMediaButtons();
            }
        }
    }

    void DeactivateMediaButtons()
    {
        GameObject.Find("btnUploadMedia").GetComponent<Button>().interactable = false;
        GameObject.Find("btnUploadVideo").GetComponent<Button>().interactable = false;
        GameObject.Find("btnAddOnlineVideo").GetComponent<Button>().interactable = false;
        GameObject.Find("btnAddOnlineMedia").GetComponent<Button>().interactable = false;
        GameObject.Find("btnAddOnlineRSSMedia").GetComponent<Button>().interactable = false;

        if (SingletonController.instance.LoadAFile)
        {
            DeactivateAutoDivideButton();
        }
        if (SingletonController.instance.ScreenDivisionby2 || SingletonController.instance.ScreenDivisionby1)
        {
            DeactivateLoadFileButton();
        }
    }

    void DeactivateLoadFileButton()
    {
        GameObject.Find("btnLoadPlans").GetComponent<Button>().interactable = false;
    }

    void DeactivateAutoDivideButton()
    {
        GameObject.Find("dropdownAutoDivideScreen").GetComponent<Dropdown>().interactable = false;
    }

    void ActivateLoadFileButton()
    {
        GameObject.Find("btnLoadPlans").GetComponent<Button>().interactable = true;
    }

    void ActivateAutoDivideButton()
    {
        GameObject.Find("dropdownAutoDivideScreen").GetComponent<Dropdown>().interactable = true;
    }

    void ActivateMediaButtons()
    {
        GameObject.Find("btnUploadMedia").GetComponent<Button>().interactable = true;
        GameObject.Find("btnUploadVideo").GetComponent<Button>().interactable = true;
        GameObject.Find("btnAddOnlineVideo").GetComponent<Button>().interactable = true;
        GameObject.Find("btnAddOnlineMedia").GetComponent<Button>().interactable = true;
        GameObject.Find("btnAddOnlineRSSMedia").GetComponent<Button>().interactable = true;
    }

    void CheckDeactivateLoadFile()
    {
        if (!SingletonController.instance.LoadAFile)
        {
            DeactivateLoadFileButton();
        }
        if (!SingletonController.instance.ScreenDivisionby1 || !SingletonController.instance.ScreenDivisionby2)
        {
            DeactivateAutoDivideButton();
        }
    }

    //Images
    public void ImportLocalFile()
    {
        CheckDeactivateLoadFile();
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        string ext;

        var extensions = new[]{ new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )};

        //string fileName = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        var fileName = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (fileName.Length > 0)
        {
            if (File.Exists(fileName[0]))
            {
                byte[] bytes = File.ReadAllBytes(fileName[0]);
                //Texture2D tex = new Texture2D(2, 2);
                Texture2D tex = new Texture2D(240, 540);
                tex.LoadImage(bytes);
                ext = Path.GetExtension(fileName[0]);
                //Debug.Log("Selected file: " + fileName);

                if (template0Selected || template1Selected || template2Selected || template3Selected)
                {

                    //Delete existing object
                    if (SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject != null)
                    {
                        //Debug.Log("Deleting.. " + editTemplate);
                        RectTransform rt = (RectTransform)SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString());
                        previousTemplateSize = rt.rect.width;
                        DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject);
                        //Debug.Log("Deleting.. " + editTemplate);
                    }
                    PlaceImageTemplate(tex, editTemplate, ext);
                    UpdateTemplateScrollView(fileName[0], editTemplate);
                    //CheckCurrentShape(editTemplate);
                    ResetSelectedTemplateFlags();
                }
                else
                {
                    UpdateScrollView(fileName[0]);
                    PlaceImageTemplate(tex, noOftemplates, ext);
                    
                }

            }
            //ResetSelectedTemplateFlags();
        }

    }

    public void PlaceImageTemplate(Texture2D tex, int index, string ext)
    {
        GameObject imagePrefab = null;
        if (circleTemplate)
        {
            imagePrefab = AddTemplate(index, TagManager.I_CIRCLE_PREFAB_INDEX);
            imagePrefab.transform.Find("Image").GetComponent<RawImage>().texture = tex;
            //CheckCurrentShape(index);
            ResetShapeSelectFlags();
        } 
        else if (triangleTemplate)
        {
            imagePrefab = AddTemplate(index, TagManager.I_TRIANGLE_PREFAB_INDEX);
            imagePrefab.transform.Find("Image").GetComponent<RawImage>().texture = tex;
            //CheckCurrentShape(index);
            ResetShapeSelectFlags();
        }
        else
        {
            imagePrefab = AddTemplate(index, TagManager.IMAGE_PREFAB_INDEX);
            imagePrefab.GetComponent<RawImage>().texture = tex;
        }

        if (SingletonController.instance.LoadAFile)
        {
            string[] lines = System.IO.File.ReadAllLines(SingletonController.instance.LoadAFilePath);
            string[] parts = lines[editTemplate+1].Split(',');
            //mediaType = parts[1];

            float savedwidth = float.Parse(parts[2]);
            float savedheight = float.Parse(parts[3]);
            float savedPosX = float.Parse(parts[4]);
            float savedPosY = float.Parse(parts[5]);

            //Set_Size(imagePrefab, savedwidth, savedheight);
            Set_Size(imagePrefab, savedwidth - (savedwidth / 2.0f), savedheight - (savedheight / 2.0f));
            //mediaTemplate.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
            imagePrefab.transform.position = new Vector3(savedPosX , savedPosY , 0);
            RefreshTemplateType(parts[1]);

        }

        //When user selects to divide screen by 2 or 1
        if (previousTemplateSize > 240)
        {
            int i = -1;
            Set_Size(imagePrefab, previousTemplateSize, 540);
            if(editTemplate == 1) { i = 1; }
            if(previousTemplateSize > 480)
            {
                imagePrefab.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                imagePrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
            }
            
        }

        SingletonController.instance.setImageTexture(tex, index);
        SingletonController.instance.setFileExtension(ext, index);
        imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        CheckCurrentShape(index);
        //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
        //ResetShapeSelectFlags();
    }

    void RefreshTemplateType(string type)
    {
        if (type.Contains("Circle") && circleTemplate)
        {
            SingletonController.instance.setSpecialTemplate("C", editTemplate);
        }
        else if (type.Contains("Triangle") && triangleTemplate)
        {
            SingletonController.instance.setSpecialTemplate("T", editTemplate);
        }
        else
        {
            SingletonController.instance.setSpecialTemplate("", editTemplate);
        }
    }

    //Videos
    public void ImportLocalVideoFile()
    {
        CheckDeactivateLoadFile();
        var extensions = new[]{ new ExtensionFilter("Video Files", "mov", "mp4", "asf", "avi", "m4v", "dv", "mpg", "mpeg", "ogv", "vp8", "webm", "wmv" )};
        //Get output panel to view imported file
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

        //string url = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        var url = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (url.Length > 0)
        {
            string path = "file:///" + url[0];
            if (File.Exists(url[0]))
            {
                RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);
                SingletonController.instance.setURL(path, noOftemplates);
                //Debug.Log(path);
                string ext = Path.GetExtension(url[0]);

                if (template0Selected || template1Selected || template2Selected || template3Selected)
                {
                    //Delete existing object
                    if (SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject != null)
                    {
                        //Debug.Log("Deleting..");
                        RectTransform rectt = (RectTransform)SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString());
                        previousTemplateSize = rectt.rect.width;
                        DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject);
                    }

                    PlaceVideoTemplate(rt, editTemplate, ext, path);
                    UpdateTemplateScrollView(path, editTemplate);
                    ResetSelectedTemplateFlags();

                }
                else
                {
                    PlaceVideoTemplate(rt, noOftemplates, ext, path);
                    UpdateScrollView(url[0]);
                }
            }
        }
     
    }

    public void PlaceVideoTemplate(RenderTexture rt, int index, string ext, string path)
    {
        GameObject videoPrefab;

        if (circleTemplate)
        {
            videoPrefab = AddTemplate(index, TagManager.V_CIRCLE_PREFAB_INDEX);
            ResetShapeSelectFlags();
        }
        else if (triangleTemplate)
        {
            videoPrefab = AddTemplate(index, TagManager.V_TRIANGLE_PREFAB_INDEX);
            ResetShapeSelectFlags();
        }
        else
        {
            videoPrefab = AddTemplate(index, TagManager.VIDEO_PREFAB_INDEX);
        }

        //When user selects to divide screen by 2 or 1
        if (previousTemplateSize > 240)
        {
            int i = -1;
            Set_Size(videoPrefab, previousTemplateSize, 540);
            if (editTemplate == 1) { i = 1; }
            if (previousTemplateSize > 480)
            {
                videoPrefab.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                videoPrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
            }

        }

        if (SingletonController.instance.LoadAFile)
        {
            string[] lines = System.IO.File.ReadAllLines(SingletonController.instance.LoadAFilePath);
            string[] parts = lines[editTemplate + 1].Split(',');

            float savedwidth = float.Parse(parts[2]);
            float savedheight = float.Parse(parts[3]);
            float savedPosX = float.Parse(parts[4]);
            float savedPosY = float.Parse(parts[5]);

            Set_Size(videoPrefab, savedwidth - (savedwidth / 2.0f), savedheight - (savedheight / 2.0f));
            videoPrefab.transform.position = new Vector3(savedPosX, savedPosY, 0);
            SingletonController.instance.setURL(path, index);
            RefreshTemplateType(parts[1]);

        }

        videoPrefab.GetComponent<VideoPlayer>().source = VideoSource.Url;
        SingletonController.instance.setFileExtension(ext, index);
        videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
        videoPrefab.GetComponent<RawImage>().texture = rt;
        videoPrefab.GetComponent<VideoPlayer>().url = path;
        videoPrefab.GetComponent<VideoPlayer>().Pause();
        videoPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        CheckCurrentShape(index);
    }

    public void ImportOnlineFile()
    {
        CheckDeactivateLoadFile();
        GameObject urlInputField = GameObject.FindWithTag("URLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        //Debug.Log("Your URL: " + url);
        string ext = Path.GetExtension(url);

        if (template0Selected || template1Selected || template2Selected || template3Selected)
        {
            //Delete existing object
            if (SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject != null)
            {
                //Debug.Log("Deleting..");
                RectTransform rt = (RectTransform)SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString());
                previousTemplateSize = rt.rect.width;
                DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject);
            }

            PlaceOnlineImage(editTemplate, ext, url);
            UpdateTemplateScrollView(url, editTemplate);
            ResetSelectedTemplateFlags();

        }
        else
        {
            PlaceOnlineImage(noOftemplates, ext, url);
            UpdateScrollView(url);
        }

    }

    public void PlaceOnlineImage(int index, string ext, string url)
    {
        GameObject imagePrefab = null;

        if (circleTemplate)
        {
            imagePrefab = AddTemplate(index, TagManager.I_CIRCLE_PREFAB_INDEX);
            //ResetShapeSelectFlags();
        }
        else if (triangleTemplate)
        {
            imagePrefab = AddTemplate(index, TagManager.I_TRIANGLE_PREFAB_INDEX);
            //ResetShapeSelectFlags();
        }
        else
        {
            imagePrefab = AddTemplate(index, TagManager.IMAGE_PREFAB_INDEX);
        }

        //When user selects to divide screen by 2 or 1
        if (previousTemplateSize > 240)
        {
            int i = -1;
            Set_Size(imagePrefab, previousTemplateSize, 540);
            if (editTemplate == 1) { i = 1; }
            if (previousTemplateSize > 480)
            {
                imagePrefab.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                imagePrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
            }

        }


        if (SingletonController.instance.LoadAFile)
        {
            string[] lines = System.IO.File.ReadAllLines(SingletonController.instance.LoadAFilePath);
            string[] parts = lines[editTemplate + 1].Split(',');
            //mediaType = parts[1];

            float savedwidth = float.Parse(parts[2]);
            float savedheight = float.Parse(parts[3]);
            float savedPosX = float.Parse(parts[4]);
            float savedPosY = float.Parse(parts[5]);

            //Set_Size(imagePrefab, savedwidth, savedheight);
            Set_Size(imagePrefab, savedwidth - (savedwidth / 2.0f), savedheight - (savedheight / 2.0f));
            //mediaTemplate.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
            imagePrefab.transform.position = new Vector3(savedPosX, savedPosY, 0);
            RefreshTemplateType(parts[1]);

        }

        if (ext == "") { ext = ".png"; }  // if https link does not have extension => add dummy record to be treated as image
        SingletonController.instance.setFileExtension(ext, index);
        StartCoroutine(GetText(index, url, imagePrefab, SingletonController.instance.OutputPanel));
       // CheckCurrentShape(index);
    }

    public void ImportOnlineVideoFile()
    {
        CheckDeactivateLoadFile();
        RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);

        GameObject urlInputField = GameObject.FindWithTag("VideoURLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        //Debug.Log("Your URL: " + url);
        string ext = Path.GetExtension(url);

        if (template0Selected || template1Selected || template2Selected || template3Selected)
        {
            //Delete existing object
            if (SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject != null)
            {
                //Debug.Log("Deleting..");
                RectTransform rectt = (RectTransform)SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString());
                previousTemplateSize = rectt.rect.width;
                DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject);
            }
            PlaceOnlineVideoFile(rt, editTemplate, url, ext);
            UpdateTemplateScrollView(url, editTemplate);
            ResetSelectedTemplateFlags();
        }
        else
        {
            PlaceOnlineVideoFile(rt, noOftemplates, url, ext);
            UpdateScrollView(url);
        }

    }

    public void PlaceOnlineVideoFile(RenderTexture rt, int index, string url, string ext)
    {
        if (ext == "") { ext = ".mov"; }  // if https link does not have extension => add dummy record to be treated as video
        GameObject videoPrefab;

        if (circleTemplate)
        {
            videoPrefab = AddTemplate(index, TagManager.V_CIRCLE_PREFAB_INDEX);
            ResetShapeSelectFlags();
        }
        else if (triangleTemplate)
        {
            videoPrefab = AddTemplate(index, TagManager.V_TRIANGLE_PREFAB_INDEX);
            ResetShapeSelectFlags();
        }
        else
        {
            videoPrefab = AddTemplate(index, TagManager.VIDEO_PREFAB_INDEX);
        }

        //When user selects to divide screen by 2 or 1
        if (previousTemplateSize > 240)
        {
            int i = -1;
            Set_Size(videoPrefab, previousTemplateSize, 540);
            if (editTemplate == 1) { i = 1; }
            if (previousTemplateSize > 480)
            {
                videoPrefab.transform.position = new Vector3(0, 0, 0);
            }
            else
            {
                videoPrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
            }

        }

        if (SingletonController.instance.LoadAFile)
        {
            string[] lines = System.IO.File.ReadAllLines(SingletonController.instance.LoadAFilePath);
            string[] parts = lines[editTemplate + 1].Split(',');
            //mediaType = parts[1];

            float savedwidth = float.Parse(parts[2]);
            float savedheight = float.Parse(parts[3]);
            float savedPosX = float.Parse(parts[4]);
            float savedPosY = float.Parse(parts[5]);

            //Set_Size(imagePrefab, savedwidth, savedheight);
            Set_Size(videoPrefab, savedwidth - (savedwidth / 2.0f), savedheight - (savedheight / 2.0f));
            //mediaTemplate.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
            videoPrefab.transform.position = new Vector3(savedPosX, savedPosY, 0);
            RefreshTemplateType(parts[1]);

        }

        videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
        videoPrefab.GetComponent<RawImage>().texture = rt;
        SingletonController.instance.setURL(url, index);
        SingletonController.instance.setFileExtension(ext, index);
        //if(url ==) - check if URL is valid
        //StartCoroutine(GetText(url, videoPrefab, SingletonController.instance.OutputPanel));
        videoPrefab.GetComponent<VideoPlayer>().url = url;
        videoPrefab.GetComponent<VideoPlayer>().Pause();
        videoPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        CheckCurrentShape(index);
    }

    public void GetRSSFeedInfo()
    {
        CheckDeactivateLoadFile();
        //UnityEngine.Object[] sprites;
        //sprites = Resources.LoadAll("RuntimeSprites/2");
        string time = DateTime.Now.ToString("hh:mm:ss tt");
        //Debug.Log("Your time: " + time);
        int hour = int.Parse(time.Split(':')[0]);
        string dayOrnight = time.Split(' ')[1];

        GameObject urlInputField = GameObject.FindWithTag("RssURLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        //Debug.Log("Your URL: " + url);

        //Get RSS feed in xml format

        //Debug.Log("Your day or night: " + dayOrnight);
        Debug.Log("Your hour: " + hour);
        SingletonController.instance.setURL(url, noOftemplates);
        string ext = ".xml";

        if (template0Selected || template1Selected || template2Selected || template3Selected)
        {
            //Delete existing object
            if (SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject != null)
            {
                //Debug.Log("Deleting..");
                RectTransform rt = (RectTransform)SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString());
                previousTemplateSize = rt.rect.width;
                DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(editTemplate.ToString()).gameObject);
            }

            PlaceWeatherWidget(editTemplate, url, ext, dayOrnight, hour);
            UpdateTemplateScrollView(url, editTemplate);
            ResetSelectedTemplateFlags();
        }
        else
        {
            PlaceWeatherWidget(noOftemplates, url, ext, dayOrnight, hour);
            UpdateScrollView(url);
        }

    }

    public void PlaceWeatherWidget(int index, string url, string ext, string dayOrnight, int hour)
    {
        SingletonController.instance.setFileExtension(ext, noOftemplates);

        if (dayOrnight.Equals("pm") && hour >= TagManager.NIGHT_TIME && hour != 12)
        {
            //Night widget
            GameObject weatherPrefab = AddTemplate(index, TagManager.W_NIGHT_PREFAB_INDEX);
            //When user selects to divide screen by 2 or 1
            if (previousTemplateSize > 240)
            {
                int i = -1;
                Set_Size(weatherPrefab, previousTemplateSize, 540);
                if (editTemplate == 1) { i = 1; }
                if (previousTemplateSize > 480)
                {
                    weatherPrefab.transform.position = new Vector3(0, 0, 0);
                }
                else
                {
                    weatherPrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
                }

            }
            //weatherPrefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[16];
            SingletonController.instance.FillInDummyData(weatherPrefab);
            weatherPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);

            SingletonController.instance.GetRSSdata(weatherPrefab, url);
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

        }
        else
        {
            //Day widget
            GameObject weatherPrefab = AddTemplate(noOftemplates, TagManager.W_DAY_PREFAB_INDEX);
            //weatherPrefab.transform.Find("Image").GetComponent<Image>().sprite = (Sprite)sprites[3];
            SingletonController.instance.FillInDummyData(weatherPrefab);
            //When user selects to divide screen by 2 or 1
            if (previousTemplateSize > 240)
            {
                int i = -1;
                Set_Size(weatherPrefab, previousTemplateSize, 540);
                if (editTemplate == 1) { i = 1; }
                if (previousTemplateSize > 480)
                {
                    weatherPrefab.transform.position = new Vector3(0, 0, 0);
                }
                else
                {
                    weatherPrefab.transform.position = new Vector3(((i) * TagManager.NEXT_POS_PANEL), 0, 0);
                }

            }
            weatherPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);

            SingletonController.instance.GetRSSdata(weatherPrefab, url);
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

        }
    }


    IEnumerator GetText(int index, string path, GameObject uiMediaTemplate, GameObject uiPanel)
    {
        Texture2D tex = new Texture2D(2, 2);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Get downloaded asset bundle
            tex = DownloadHandlerTexture.GetContent(www);
            SingletonController.instance.setImageTexture(tex, index);
            if (circleTemplate)
            {
                uiMediaTemplate.transform.Find("Image").GetComponent<RawImage>().texture = tex;
            }
            else if (triangleTemplate)
            {
                uiMediaTemplate.transform.Find("Image").GetComponent<RawImage>().texture = tex;
            }
            else
            {
                uiMediaTemplate.GetComponent<RawImage>().texture = tex;
            }
            //uiMediaTemplate.GetComponent<RawImage>().texture = tex;
            uiMediaTemplate.transform.SetParent(uiPanel.transform, false);
            
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
            ResetShapeSelectFlags();
            CheckCurrentShape(index);
        }

    }

    GameObject AddTemplate(int noOftemplates, int mediaObject)
    {
        GameObject[] prefabs = SingletonController.instance.getMediaObjects;
        GameObject imagePrefab = new GameObject();

        switch (noOftemplates)
        {
            case 0:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION, 0, 0), Quaternion.identity);
                imagePrefab.name = "0";
                break;

            case 1:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                imagePrefab.name = "1";
                break;

            case 2:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                imagePrefab.name = "2";
                break;

            case 3:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                imagePrefab.name = "3";
                break;

            default:
                Debug.Log("Not more than 4 templates can be added to ouput");
                break;
        }
        return imagePrefab;
    }

    void EmptyScrollView()
    {
        string name = " ";

        for (int i = 0; i < 4; i++)
        {
            GameObject btnTemplateEdit = GameObject.Find(i.ToString());

            if (btnTemplateEdit != null)
            {
                GameObject textGO = btnTemplateEdit.transform.Find("Text").gameObject;
                Text btntext = textGO.GetComponent<Text>();
                btntext.text = name;
                //btnTemplateEdit.SetActive(false);
                btnTemplateEdit.GetComponent<Button>().interactable = false;

            }
        }

    }

    void UpdateScrollView(string fname)
    {
        string name;
        //Debug.Log("noOfTemplates= " + noOftemplates);
        GameObject btnTemplateEdit = null;
        //GameObject btnTemplateEdit = GameObject.Find(noOftemplates.ToString());
        if (fname.StartsWith("https"))
        {
            name = "Online Media " + noOftemplates + Path.GetExtension(fname);
            //GameObject btnTemplateEdit = GameObject.Find(noOftemplates.ToString());
            btnTemplateEdit = GameObject.Find(noOftemplates.ToString());
        }
        else if (fname.Equals(""))
        {
            int i = SingletonController.instance.NoOfElements - 1;
            //Debug.Log("noOfTemplates= " + i);
            //int i = noOftemplates-1;
            name = "Division " + i;
            btnTemplateEdit = GameObject.Find(i.ToString());
        }
        else
        {
            name = Path.GetFileName(fname);
            //GameObject btnTemplateEdit = GameObject.Find(noOftemplates.ToString());
            btnTemplateEdit = GameObject.Find(noOftemplates.ToString());
        }

        if (btnTemplateEdit != null)
        {
            GameObject textGO = btnTemplateEdit.transform.Find("Text").gameObject;
            Text btntext = textGO.GetComponent<Text>();
            btntext.text = name;
            btnTemplateEdit.SetActive(true);
            btnTemplateEdit.GetComponent<Button>().interactable = true;

        }
    }

    public void UpdateTemplateScrollView(string fname, int index)
    {
        string name;
        name = Path.GetFileName(fname);

        GameObject btnTemplateEdit = GameObject.Find(index.ToString());

        if (btnTemplateEdit != null)
        {
            GameObject textGO = btnTemplateEdit.transform.Find("Text").gameObject;
            Text btntext = textGO.GetComponent<Text>();
            btntext.text = name;
            btnTemplateEdit.SetActive(true);
            btnTemplateEdit.GetComponent<Button>().interactable = true;

        }
    }

    public void OnTemplateShapeSelect(GameObject buttonGO)
    {
        if(buttonGO.name == "btnCircle")
        {
            //SingletonController.instance.TriangleTemplate = false;
            triangleTemplate = false; 
            //SingletonController.instance.CircleTemplate = true; 
            circleTemplate = true;
            SingletonController.instance.setSpecialTemplate("C", noOftemplates);
        }
        if(buttonGO.name == "btnTriangle")
        {
            //SingletonController.instance.CircleTemplate = false;
            //SingletonController.instance.TriangleTemplate = true;
            circleTemplate = false;
            triangleTemplate = true;
            SingletonController.instance.setSpecialTemplate("T", noOftemplates);
        }
    }

    public void ResetShapeSelectFlags()
    {
        circleTemplate = false;
        triangleTemplate = false;
        //SingletonController.instance.ResetShapeSelectFlags();
    }

    void CheckCurrentShape(int mtemplate)
    {
        //if (SingletonController.instance.LoadAFile)
        //{
        //Debug.Log(mtemplate);
        if (SingletonController.instance.OutputPanel.transform.Find(mtemplate.ToString()).GetComponent<Image>() != null)
        {
            
            if (SingletonController.instance.OutputPanel.transform.Find(mtemplate.ToString()).GetComponent<Image>().sprite.name.Equals("OvalMask 1"))
            {
                //Debug.Log("Circle");
                //triangleTemplate = false;
                //circleTemplate = true;
                SingletonController.instance.setSpecialTemplate("C", mtemplate);
            }
            if (SingletonController.instance.OutputPanel.transform.Find(mtemplate.ToString()).GetComponent<Image>().sprite.name.Equals("TriangleMask"))
            {
                //Debug.Log("Triangle");
                //circleTemplate = false;
                //triangleTemplate = true;
                SingletonController.instance.setSpecialTemplate("T", mtemplate);
            }
        }
        else
        {
            //Debug.Log("Rectangle");
            SingletonController.instance.setSpecialTemplate("", mtemplate);
        }
        //}

    }

    public void OnbtnTemplateSelect(GameObject buttonGO)
    {
        if(buttonGO.name == "0")
        {
            template0Selected = true;
            editTemplate = 0;
            //CheckCurrentShape(editTemplate);
            ActivateMediaButtons();
        }
        if (buttonGO.name == "1")
        {
            template1Selected = true;
            editTemplate = 1;
            //CheckCurrentShape(editTemplate);
            ActivateMediaButtons();
        }
        if (buttonGO.name == "2")
        {
            template2Selected = true;
            editTemplate = 2;
            //CheckCurrentShape(editTemplate);
            ActivateMediaButtons();
        }
        if (buttonGO.name == "3")
        {
            template3Selected = true;
            editTemplate = 3;
            //CheckCurrentShape(editTemplate);
            ActivateMediaButtons();
        }
    }

    public void UpdateTemplateFlags()
    {
        if (template0Selected)
        {
            //Force all others to false
            template1Selected = false;
            template2Selected = false;
            template3Selected = false;
        }
        if (template1Selected)
        {
            template0Selected = false;
            template2Selected = false;
            template3Selected = false;
        }
        if (template2Selected)
        {
            template0Selected = false;
            template1Selected = false;
            template3Selected = false;
        }
        if (template3Selected)
        {
            template0Selected = false;
            template1Selected = false;
            template2Selected = false;
        }
    }

    public void ResetSelectedTemplateFlags()
    {
        //Force all to false to block editing unless selection is made in scroll view
        template0Selected = false;
        template1Selected = false;
        template2Selected = false;
        template3Selected = false;
    }

    public void OnSelectAutoDivide(int i)
    {
        noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
        SingletonController.instance.ClearOutputDisplay();
        GameObject dropdown = GameObject.FindWithTag("DropDownDivide");
        if (dropdown.GetComponent<Dropdown>().options[dropdown.GetComponent<Dropdown>().value].text == "x4")
        {
            SingletonController.instance.ScreenDivisionby1 = false;
            SingletonController.instance.ScreenDivisionby2 = false;
            for (int counter = 0; counter < 4; counter++)
            {
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                GameObject imagePrefab = AddTemplate(counter, TagManager.IMAGE_PREFAB_INDEX);
                Texture2D tex = new Texture2D(240, 540);
                //tex.LoadImage(bytes);
                //Debug.Log("Selected file: " + fileName);
                imagePrefab.GetComponent<RawImage>().texture = tex;
                SingletonController.instance.setImageTexture(tex, counter);
                string ext = ".jpg";
                SingletonController.instance.setFileExtension(ext, counter);
                imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                UpdateScrollView("");

            }
            //Debug.Log("Select divide by 4");
        }

        if (dropdown.GetComponent<Dropdown>().options[dropdown.GetComponent<Dropdown>().value].text == "x2")
        {
            for (int counter = 0; counter < 2; counter++)
            {
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                GameObject imagePrefab = AddTemplate(counter, TagManager.IMAGE_PREFAB_INDEX);
                if(counter > 0)
                {
                    imagePrefab.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 360) + ((noOftemplates + counter) * TagManager.NEXT_POS_PANEL), 0, 0);
                }
                else
                {
                    imagePrefab.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
                }
                Set_Size(imagePrefab, 480, 540);
                Texture2D tex = new Texture2D(480, 540);
                imagePrefab.GetComponent<RawImage>().texture = tex;
                SingletonController.instance.setImageTexture(tex, counter);
                string ext = ".jpg";
                SingletonController.instance.setFileExtension(ext, counter);
                SingletonController.instance.ScreenDivisionby1 = false;
                SingletonController.instance.ScreenDivisionby2 = true;
                imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                UpdateScrollView("");

            }
            DeactivateMediaButtons();
            //Debug.Log("Select divide by 2");
        }

        if (dropdown.GetComponent<Dropdown>().options[dropdown.GetComponent<Dropdown>().value].text == "x1")
        {
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
            GameObject imagePrefab = AddTemplate(0, TagManager.IMAGE_PREFAB_INDEX);
            imagePrefab.transform.position = new Vector3(0, 0, 0);
            Set_Size(imagePrefab, 960, 540);
            Texture2D tex = new Texture2D(960, 540);
            SingletonController.instance.setImageTexture(tex, 0);
            string ext = ".jpg";
            //PlaceImageTemplate(tex, 0, ext);
            SingletonController.instance.setFileExtension(ext, 0);
            SingletonController.instance.ScreenDivisionby2 = false;
            SingletonController.instance.ScreenDivisionby1 = true;
            imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
            UpdateScrollView("");
            DeactivateMediaButtons();

        }

    }

    public static void Set_Size(GameObject gameObject, float width, float height)
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

    public void OnLoadPlan()
    {
        string mediaType = "";
        int i = 0;
        var extensions = new[]{ new ExtensionFilter("Text Files", "csv" )};
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        //Debug.Log(path[0]);
        if (path.Length > 0)
        {
            if (File.Exists(path[0]))
            {
                noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
                if (noOftemplates > 0)
                {

                    for(int x = 0; x < noOftemplates; x++)
                    {
                        Debug.Log("Deleting..");
                        DestroyImmediate(SingletonController.instance.OutputPanel.transform.Find(x.ToString()).gameObject);
                        //SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                        //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
                    }

                }

                string[] lines = System.IO.File.ReadAllLines(path[0]);
                foreach (string line in lines)
                {
                    //string[] parts = line.Split(",");
                    string[] parts = line.Split(',');
                    mediaType = parts[1];


                    if (!(line.StartsWith("Index")))
                    {
                        GameObject mediaTemplate = null;

                        float savedwidth = float.Parse(parts[2]);
                        float savedheight = float.Parse(parts[3]);
                        float savedPosX = float.Parse(parts[4]);
                        float savedPosY = float.Parse(parts[5]);
                        mediaTemplate = InstantiateMediaTemplate(mediaType, i);
                        //Resize and position according to saved plan
                        Set_Size(mediaTemplate, savedwidth - (savedwidth / 2.0f), savedheight - (savedheight / 2.0f));
                        //mediaTemplate.transform.position = new Vector3((TagManager.PANEL_TEMPLATE_START_POSITION + 120), 0, 0);
                        mediaTemplate.transform.position = new Vector3(savedPosX, savedPosY, 0);
                        Texture2D tex = new Texture2D(2, 2);
                        SingletonController.instance.setImageTexture(tex, i);
                        string ext = ".jpg"; //Dummy extension for SingletonController
                        SingletonController.instance.setFileExtension(ext, i);
                        mediaTemplate.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                        Debug.Log(SingletonController.instance.NoOfElements);
                        //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
                        SingletonController.instance.LoadAFile = true;
                        SingletonController.instance.LoadAFilePath = path[0];
                        UpdateScrollView("");
                        i++;
                    }
                }
            }
        }
    }

    GameObject InstantiateMediaTemplate(string part, int positionIndex)
    {
        int mediaObject = 0;
        switch (part)
        {
            case "imageImportObject(Clone)":
                mediaObject = 0;
                break;

            case "videoImportObject(Clone)":
                mediaObject = 1;
                break;

            case "weatherWidgetNight(Clone)":
                mediaObject = 0; //Create image templates for saved plans with weather widget
                break;

            case "weatherWidgetDay Variant(Clone)":
                mediaObject = 0; // Create image templates for saved plans with weather widget
                 break;

            case "imageImportCircleObject(Clone)":
                mediaObject = 4;
                SingletonController.instance.setSpecialTemplate("C", positionIndex);
                break;

            case "videoImportCircleObject(Clone)":
                mediaObject = 5;
                SingletonController.instance.setSpecialTemplate("C", positionIndex);
                break;

            case "imageImportTriangleObject Variant(Clone)":
                mediaObject = 6;
                SingletonController.instance.setSpecialTemplate("T", positionIndex);
                break;

            case "videoImportCircleObject Variant(Clone)":
                mediaObject = 7;
                SingletonController.instance.setSpecialTemplate("T", positionIndex);
                break;
        }
        return AddTemplate(positionIndex, mediaObject);

    }

    public void ResetLoadAfileFlag()
    {
        if (!SingletonController.instance.LoadAFile)
        {
            ActivateLoadFileButton();
        }
        if (!SingletonController.instance.ScreenDivisionby1 || !SingletonController.instance.ScreenDivisionby2)
        {
            ActivateAutoDivideButton();
        }
        SingletonController.instance.LoadAFile = false;
        SingletonController.instance.Reset = true;
        SingletonController.instance.ResetShapeSelectFlags();
        SingletonController.instance.ClearOutputDisplay();
        EmptyScrollView();
    }

    public void ResetAutoDivideFlags()
    {
        if(!SingletonController.instance.ScreenDivisionby1 || !SingletonController.instance.ScreenDivisionby2)
        {
            ActivateAutoDivideButton();
        }
        if (!SingletonController.instance.LoadAFile)
        {
            ActivateLoadFileButton();
        }
        SingletonController.instance.ScreenDivisionby1 = false;
        SingletonController.instance.ScreenDivisionby2 = false;
        SingletonController.instance.ResetShapeSelectFlags();
        SingletonController.instance.ClearOutputDisplay();
        EmptyScrollView();
    }

    /*void ClearOutputDisplay()
    {
        //Clear display panel of elements
        Debug.Log("display output Elements: " + Singleton.ControllerOutputPanel.transform.childCount);
        if (OutputPanel.transform.childCount > 0)
        {
            //Delete existing object
            //foreach(GameObject child in SingletonController.instance.OutputPanel.gameObject)
            for (int x = 0; x < OutputPanel.transform.childCount; x++)
            {
                Debug.Log("Deleting..");
                DestroyImmediate(OutputPanel.transform.Find(x.ToString()).gameObject);
                //DestroyImmediate(child);
                //NoOfElements = displaypanel.transform.childCount;
                //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
            }

            //NoOfElements = 0;
        }
    }*/

    public void doExitGame()
    {
        Application.Quit();
        //Debug.Log("Game is exiting");
        //Just to make sure its working
    }

}
