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

    void Start()
    {
        //noOftemplates = 0;
        SingletonController.instance.OutputPanel = GameObject.FindWithTag("OutputPanel");
    }

    // Update is called once per frame
    void Update()
    {
        //SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        if(noOftemplates > 3)
        {
            noOftemplates = 0;
        }
        else
        {
            noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
        }
        //noOftemplates = SingletonController.instance.OutputPanel.transform.childCount;
    }

    public void PlayShowroom()
    {
        SceneManager.LoadScene(TagManager.SHOWROOM_SCENE_NAME);
    }

    //Images
    public void ImportLocalFile()
    {
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

        GameObject imagePrefab = AddTemplate(noOftemplates, TagManager.IMAGE_PREFAB_INDEX);
        if (SingletonController.instance.IsResizablePanel)
        {
            //imagePrefab = AddTemplate(noOftemplates, (TagManager.IMAGE_PREFAB_INDEX + TagManager.JUMP_PREFAB_PICKER));
            SingletonController.instance.setIsResizable(true, noOftemplates);
            SingletonController.instance.IsResizablePanel = false; //reset
        }

        //SingletonController.instance.setMediaObject(imagePrefab, 0);

        //string fileName = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        var fileName = StandaloneFileBrowser.OpenFilePanel("Title", "", "", false);
        if (fileName.Length > 0)
        {
            if (File.Exists(fileName[0]))
            {
                byte[] bytes = File.ReadAllBytes(fileName[0]);
                //Texture2D tex = new Texture2D(2, 2);
                Texture2D tex = new Texture2D(240, 540);
                tex.LoadImage(bytes);
                //Debug.Log("Selected file: " + fileName);
                imagePrefab.GetComponent<RawImage>().texture = tex;
                SingletonController.instance.setImageTexture(tex, noOftemplates);
                string ext = Path.GetExtension(fileName[0]);
                SingletonController.instance.setFileExtension(ext, noOftemplates);
                imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
            }
        }
        /*else
        {
            Debug.Log("No file selected");
        }*/

    }

    //Videos
    public void ImportLocalVideoFile()
    {
        //Get output panel to view imported file
        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
        //GameObject[] prefabs = SingletonController.instance.getMediaObjects;
        //GameObject videoPrefab = (GameObject)Instantiate(prefabs[1], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION, 0, 0), Quaternion.identity);
        GameObject videoPrefab = AddTemplate(noOftemplates, TagManager.VIDEO_PREFAB_INDEX);
        videoPrefab.GetComponent<VideoPlayer>().source = VideoSource.Url;
        if (SingletonController.instance.IsResizablePanel)
        {
            SingletonController.instance.setIsResizable(true, noOftemplates);
            SingletonController.instance.IsResizablePanel = false; //reset
        }


        //string url = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        var url = StandaloneFileBrowser.OpenFilePanel("Title", "", "", true);
        if (url.Length > 0)
        {
            string path = "file:///" + url[0];
            if (File.Exists(url[0]))
            {
                RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);
                SingletonController.instance.setURL(path, noOftemplates);
                string ext = Path.GetExtension(url[0]);
                SingletonController.instance.setFileExtension(ext, noOftemplates);
                videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
                videoPrefab.GetComponent<RawImage>().texture = rt;
                videoPrefab.GetComponent<VideoPlayer>().url = path;
                videoPrefab.GetComponent<VideoPlayer>().Pause();
                videoPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
            }
        }
        /*else
        {
            Debug.Log("No file selected");
        }*/      
    }

    public void ImportOnlineFile()
    {
        GameObject imagePrefab = AddTemplate(noOftemplates, TagManager.IMAGE_PREFAB_INDEX);
        if (SingletonController.instance.IsResizablePanel)
        {
            SingletonController.instance.setIsResizable(true, noOftemplates);
            SingletonController.instance.IsResizablePanel = false; //reset
        }
        GameObject urlInputField = GameObject.FindWithTag("URLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        Debug.Log("Your URL: " + url);
        string ext = Path.GetExtension(url);
        if(ext == "") { ext = ".png"; }  // if https link does not have extension => add dummy record to be treated as image
        SingletonController.instance.setFileExtension(ext, noOftemplates);
        StartCoroutine(GetText(url, imagePrefab, SingletonController.instance.OutputPanel));

    }

    public void ImportOnlineVideoFile()
    {
        RenderTexture rt = new RenderTexture(240, 540, 16, RenderTextureFormat.ARGB32);
        GameObject videoPrefab = AddTemplate(noOftemplates, TagManager.VIDEO_PREFAB_INDEX);
        {
            SingletonController.instance.setIsResizable(true, noOftemplates);
            SingletonController.instance.IsResizablePanel = false; //reset
        }
        videoPrefab.GetComponent<VideoPlayer>().targetTexture = rt;
        videoPrefab.GetComponent<RawImage>().texture = rt;
        GameObject urlInputField = GameObject.FindWithTag("VideoURLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        //Debug.Log("Your URL: " + url);
        SingletonController.instance.setURL(url, noOftemplates);
        string ext = Path.GetExtension(url);
        if (ext == "") { ext = ".mov"; }  // if https link does not have extension => add dummy record to be treated as video
        SingletonController.instance.setFileExtension(ext, noOftemplates);
        //if(url ==) - check if URL is valid
        //StartCoroutine(GetText(url, videoPrefab, SingletonController.instance.OutputPanel));
        videoPrefab.GetComponent<VideoPlayer>().url = url;
        videoPrefab.GetComponent<VideoPlayer>().Pause();
        videoPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);

        SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

    }

    public void GetRSSFeedInfo()
    {
        //UnityEngine.Object[] sprites;
        //sprites = Resources.LoadAll("RuntimeSprites/2");
        string time = DateTime.Now.ToString("h:mm:ss tt");
        Debug.Log("Your time: " + time);
        int hour = int.Parse(time.Split(':')[0]);
        string dayOrnight = time.Split(' ')[1];

        GameObject urlInputField = GameObject.FindWithTag("RssURLInput");
        string url = urlInputField.GetComponent<InputField>().text;
        Debug.Log("Your URL: " + url);

        //Get RSS feed in xml format

        Debug.Log("Your day or night: " + dayOrnight);
        Debug.Log("Your hour: " + hour);
        SingletonController.instance.setURL(url, noOftemplates);
        string ext = ".xml";
        SingletonController.instance.setFileExtension(ext, noOftemplates);

        if (dayOrnight.Equals("pm") &&  hour  >= TagManager.NIGHT_TIME)
        {
            //Night widget
            GameObject weatherPrefab = AddTemplate(noOftemplates, TagManager.W_NIGHT_PREFAB_INDEX);
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
            weatherPrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);

            SingletonController.instance.GetRSSdata(weatherPrefab, url);
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

        }

    }


    IEnumerator GetText(string path, GameObject uiMediaTemplate, GameObject uiPanel)
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
            uiMediaTemplate.GetComponent<RawImage>().texture = tex;
            uiMediaTemplate.transform.SetParent(uiPanel.transform, false);
            SingletonController.instance.setImageTexture(tex, noOftemplates);
            SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
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
                break;

            case 1:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                break;

            case 2:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                break;

            case 3:
                imagePrefab = (GameObject)Instantiate(prefabs[mediaObject], new Vector3(TagManager.PANEL_TEMPLATE_START_POSITION + (noOftemplates * TagManager.NEXT_POS_PANEL), 0, 0), Quaternion.identity);
                break;

            default:
                Debug.Log("Not more than 4 templates can be added to ouput");
                break;
        }
        return imagePrefab;
    }

    public void OnSelectAutoDivide(int i)
    {
        GameObject dropdown = GameObject.FindWithTag("DropDownDivide");
        if (dropdown.GetComponent<Dropdown>().options[dropdown.GetComponent<Dropdown>().value].text == "x4")
        {
            for(int counter = 0; counter < 4; counter++)
            {
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;
                GameObject imagePrefab = AddTemplate(counter, TagManager.IMAGE_PREFAB_INDEX);
                imagePrefab.transform.SetParent(SingletonController.instance.OutputPanel.transform, false);
                SingletonController.instance.NoOfElements = SingletonController.instance.OutputPanel.transform.childCount;

            }
            Debug.Log("Select divide by 4");
        }

    }

    public void OnSelectTemplateType(bool isResizable)
    {
        GameObject toggle = GameObject.FindWithTag("ToggleTemplate");
        if (isResizable)
        {
            SingletonController.instance.IsResizablePanel = toggle.GetComponent<Toggle>().isOn;
        }
    }

    public void doExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
        //Just to make sure its working
    }

}
