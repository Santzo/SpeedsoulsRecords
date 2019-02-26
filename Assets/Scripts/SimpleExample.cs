using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;


public class SimpleExample : MonoBehaviour
{
    [System.Serializable]
    public class Link
    {
        public string uri;
    }

    [System.Serializable]
    public class Videos
    {
        public List<Link> links;
    }

    [System.Serializable]
    public class Status
    {
        public string status;
        public string examiner;
    }

    [System.Serializable]
    public class Player
    {
        public string rel;
        public string name;
        public string id;
        public string uri;
    }

    [System.Serializable]
    public class Times
    {
        public string primary;
        public int primary_t;
        public object realtime;
        public int realtime_t;
        public object realtime_noloads;
        public int realtime_noloads_t;
        public string ingame;
        public int ingame_t;
    }

    [System.Serializable]
    public class Systema
    {
        public string platform;
        public bool emulated;
        public object region;
    }
    [System.Serializable]
    public class Values2
    {
        public string ql6g4jw8;
        public string p85xdvng;
        public string jlz7r7n2;
        public string rn1rpv8j;
    }

    [System.Serializable]
    public class Run2
    {
        public string id;
        public string weblink;
        public string game;
        public object level;
        public string category;
        public Videos videos;
        public string comment;
        public Status status;
        public List<Player> players;
        public string date;
        public Times times;
        public Systema system;
        public object splits;
        public Values2 values;
    }
    [System.Serializable]
    public class Run
    {
        public int place;
        public Run2 run;
    }
    [System.Serializable]
    public class Link2
    {
        public string rel;
        public string uri;
    }
    [System.Serializable]
    public class Datum
    {
        public string weblink;
        public string game;
        public string category;
        public object level;
        public object platform;
        public object region;
        public object emulators;
        public object timing;
        public List<Run> runs;
        public List<Link2> links;

    }
    [System.Serializable]
    public class Record
    {
        public List<Datum> data;
    }

    [System.Serializable]
    public class Names
    {
        public string international;
    }
    [System.Serializable]
    public class Names2
    {
        public string international;
    }
    [System.Serializable]
    public class Country
    {
        public string code;
        public Names2 names;
    }
    [System.Serializable]
    public class Location
    {
        public Country country;
    }
    [System.Serializable]
    public class Data2
    {

        public Names names;

        public Location location;

    }
    [System.Serializable]
    public class UserInfo
    {
        public Data2 data;
    }

    public class SiteInfo
    {
        public string game;
        public string category;
        public string categoryID;
        public string igt;
        public string rta;
        public string runner;
        public string videoAddress;
        public string country;
        public string countryCode;
        public string countryFlag;
        public string lbAddress;

    }

    private WWW request;
    private string stream;
    public static Vector2 contentPos;
    public static bool zooming;
    public Record info;
    public UserInfo uinfo;
    public GameObject dataFetch;
    public GameObject[] aclickable;
    public GameObject[] gameTitle;
    public Canvas canvas;
    private GameObject lastAdd;
    public GameObject m_clickable;
    public GameObject m_gameTitle;
    public RectTransform content;
    private Slider loadSlider;
    public float posX;
    public static float dividerY;
    public static float dividerX;
    public float posY;
    public int currentGame;
    public int cr;
    public int loadData;
    public float dataCounter;
    public static float multiplier;
    public static float cMultiplier;
    public static float contentRef;
    public GameObject copyright;
    public static float contentY;
    private TimeSpan timeIGT;
    private TimeSpan timeRTA;
    private float bonus;
    public UnityWebRequest www;
    public List<SiteInfo> lboard = new List<SiteInfo>();
    public SiteInfo lb = new SiteInfo();
    public static bool changeParents;
    private ScrollRect scroll;
    public static bool isLoading;


    private void Start()
    {

        isLoading = true;
        loadData = 0;
        scroll = GetComponentInParent<ScrollRect>();

        scroll.horizontal = false;
        scroll.vertical = false;
        dataCounter = 0;
        currentGame = 0;
        // 3:2 = 1.5 - 16:10 = 1.6 - 4:3 = 1.33 - 16:9 = 1.78 

        multiplier = (float) Screen.height / 1000;
        dividerX = Screen.height / 1280f;
        dividerY = Screen.width / 720f;
        cMultiplier = 1f;
        if (Screen.height <= 1280) cMultiplier = 1.1f;
        if ((float) Screen.height / (float) Screen.width <= 1.51f) cMultiplier = 1.3f;
        if (cMultiplier < 0.1f) cMultiplier = 0.1f;
        cr = 0;
        info = new Record();
        aclickable = new GameObject[50];
        gameTitle = new GameObject[7];
        posX = m_gameTitle.transform.position.x;
        posY = m_gameTitle.transform.position.y;
        scroll.horizontalNormalizedPosition = 0;
        scroll.verticalNormalizedPosition = 0;
        loadSlider = dataFetch.GetComponentInChildren<Slider>();
        StartCoroutine(UpdateInfo());
      

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
        if (zooming)
        {
            if (scroll.horizontal) scroll.horizontal = false;
            if (scroll.vertical) scroll.vertical = false;
        }

        if (!zooming)
        {
            if (!scroll.horizontal) scroll.horizontal = true;
            if (!scroll.vertical) scroll.vertical = true;
        }

        contentRef = Mathf.Pow(canvas.scaleFactor / dividerY, 10f);
        if (bonus == 0) bonus = contentRef;
        if (!isLoading)
        {
            if (contentRef != content.localScale.y)
            {
                bonus -= contentRef;
                content.localScale = new Vector3(contentRef, contentRef, content.localScale.z);
                content.sizeDelta = new Vector2(content.sizeDelta.x - (bonus * 40), content.sizeDelta.y);
                bonus = contentRef;
            }
     
        }
  
     
        
       

    }
    private void LateUpdate()
    {
       
    }

    private IEnumerator UpdateInfo()
    {
        yield return null;
        if (currentGame == 0) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/m1mn8kd2/records?top=1&miscellaneous=no");
        if (currentGame == 1) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/w6jve26j/records?top=1&miscellaneous=no");
        if (currentGame == 2) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/m1zky010/records?top=1&miscellaneous=no");
        if (currentGame == 3) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/y65lw01e/records?top=1&miscellaneous=no");
        if (currentGame == 4) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/9d3kqg1l/records?top=1&miscellaneous=no");
        if (currentGame == 5) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/k6qg0xdg/records?top=1&miscellaneous=no");
        if (currentGame == 6) www = UnityWebRequest.Get("https://www.speedrun.com/api/v1/games/lde3woe6/records?top=1&miscellaneous=no");

        www.downloadHandler = new DownloadHandlerBuffer();
        www.useHttpContinue = false;
        www.timeout = 10;
        loadSlider.value = loadData;

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            DataFetch.error = true;
        }
        
        lboard.Clear();
        info = JsonUtility.FromJson<Record>(www.downloadHandler.text);
        int i = 0;
        if (DataFetch.categoryError > 0)
        {
            i = DataFetch.categoryError;
            DataFetch.categoryError = 0;
        }
        foreach (Datum run in info.data)
        {
            loadSlider.value = loadData;

            loadData++;
            yield return null;
           
            if (currentGame == 0 && info.data[i].category == "zd3x6ndn") lb.category = "Any%";


            if (currentGame == 0 && info.data[i].category == "ndxpx5o2") lb.category = "Any% Glitchless";
            if (currentGame == 0 && info.data[i].category == "w20zlmjd") lb.category = "All Trophies";

            if (currentGame == 1 && info.data[i].category == "9d86vww2") lb.category = "Any%";
            if (currentGame == 1 && info.data[i].category == "7dg75ld4") lb.category = "Any% No Wrong Warp";
            if (currentGame == 1 && info.data[i].category == "xk9zng20") lb.category = "Any% Force Quit";
            if (currentGame == 1 && info.data[i].category == "mke7jn26") lb.category = "All Bosses";

            if (currentGame == 2 && info.data[i].category == "vdo0q8yd") lb.category = "Any%";
            if (currentGame == 2 && info.data[i].category == "jdzv9gkv") lb.category = "Any% Current Patch";
            if (currentGame == 2 && info.data[i].category == "xk9nr620") lb.category = "All Bosses";

            if (currentGame == 3 && info.data[i].category == "xd1pq8d8") lb.category = "Any%";
            if (currentGame == 3 && info.data[i].category == "zdnwmlqd") lb.category = "Any% Old Souls";
            if (currentGame == 3 && info.data[i].category == "zd3ernkn") lb.category = "All Bosses";

            if (currentGame == 4 && info.data[i].category == "9kvzy8dg") lb.category = "Any%";
            if (currentGame == 4 && info.data[i].category == "wk6l0pk1") lb.category = "Any% Current Patch";
            if (currentGame == 4 && info.data[i].category == "xd1rqzrk") lb.category = "All Bosses";

            if (currentGame == 5 && info.data[i].category == "n2y143z2") lb.category = "Any%";
            if (currentGame == 5 && info.data[i].category == "xk9lx0gk") lb.category = "Any% No TearDrop";
            if (currentGame == 5 && info.data[i].category == "7kjz1ond") lb.category = "All Bosses";

            if (currentGame == 6 && info.data[i].category == "ndx1pm52") lb.category = "Any%";
            if (currentGame == 6 && info.data[i].category == "wdm84w52") lb.category = "Any% No Wrong Warp";
            if (currentGame == 6 && info.data[i].category == "xd173pzd") lb.category = "Any% Force Quit";
            if (currentGame == 6 && info.data[i].category == "vdo3qoyd") lb.category = "All Bosses";
            lb.categoryID = info.data[i].category;
            

            if (currentGame == 0) lb.game = "Demon's Souls";
            if (currentGame == 1) lb.game = "Dark Souls PTDE";
            if (currentGame == 2) lb.game = "Dark Souls II";
            if (currentGame == 3) lb.game = "Dark Souls II SoTFS";
            if (currentGame == 4) lb.game = "Bloodborne";
            if (currentGame == 5) lb.game = "Dark Souls III";
            if (currentGame == 6) lb.game = "Dark Souls Remastered";

            if (lb.category == null) lb.category = "";
            lb.lbAddress = "https://www.speedrun.com/api/v1/leaderboards/" + info.data[i].game + "/category/" + info.data[i].category + "?top=10";
            lb.videoAddress = info.data[i].runs[0].run.videos.links[0].uri;
            if (info.data[i].runs[0].run.players[0].id == null)
            {
                lb.runner = info.data[i].runs[0].run.players[0].name;
                lb.country = "";
                lb.countryCode = "";
                lb.countryFlag = "";
            }
            else
            {
                UnityWebRequest www2 = UnityWebRequest.Get("https://www.speedrun.com/api/v1/users/" + info.data[i].runs[0].run.players[0].id);
                www2.timeout = 10;
                yield return www2.SendWebRequest();
                if (www2.isNetworkError || www2.isHttpError)
                {
                    DataFetch.error = true;
                }
                uinfo = JsonUtility.FromJson<UserInfo>(www2.downloadHandler.text);
                lb.runner = uinfo.data.names.international;
                lb.country = uinfo.data.location.country.names.international;
                lb.countryCode = uinfo.data.location.country.code;
                lb.countryFlag = "https://www.countryflags.io/" + lb.countryCode + "/flat/64.png";
            }
            timeIGT = TimeSpan.FromSeconds(info.data[i].runs[0].run.times.ingame_t);
            timeRTA = TimeSpan.FromSeconds(info.data[i].runs[0].run.times.realtime_t);
            if (currentGame == 2 || currentGame == 3)
            {
                if (info.data[i].runs[0].run.times.realtime_noloads_t > 0) timeIGT = TimeSpan.FromSeconds(info.data[i].runs[0].run.times.realtime_noloads_t);
                else timeIGT = TimeSpan.FromSeconds(info.data[i].runs[0].run.times.realtime_t);
            }

            if (timeIGT.Hours > 0) lb.igt = string.Format("{0}:{1:00}:{2:00}", timeIGT.Hours, timeIGT.Minutes, timeIGT.Seconds);
            else lb.igt = string.Format("{0}:{1:00}", timeIGT.Minutes, timeIGT.Seconds);
            if (timeRTA.Hours > 0) lb.rta = string.Format("{0}:{1:00}:{2:00}", timeRTA.Hours, timeRTA.Minutes, timeRTA.Seconds);
            else lb.rta = string.Format("{0}:{1:00}", timeRTA.Minutes, timeRTA.Seconds);
            if (info.data[i].runs[0].run.times.realtime_t == 0 || currentGame == 2 || currentGame == 3) lb.rta = "";
            if (timeIGT.TotalSeconds == 0) lb.igt = "";



            lboard.Add(lb);
            if (!gameTitle[currentGame])
            {
                gameTitle[currentGame] = Instantiate(m_gameTitle) as GameObject;
                if (!gameTitle[currentGame].activeSelf) gameTitle[currentGame].SetActive(true);
                gameTitle[currentGame].GetComponentInChildren<TextMeshProUGUI>().text = lb.game;
                gameTitle[currentGame].transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
                gameTitle[currentGame].transform.position = new Vector2(m_gameTitle.transform.position.x, posY);

                posY -= 40f * multiplier * cMultiplier;


                if (currentGame == 2 || currentGame == 3)
                {
                    gameTitle[currentGame].GetComponent<Gametitle>().IGT.text = "RTA NoLoad";
                    gameTitle[currentGame].GetComponent<Gametitle>().RTA.enabled = false;
                }
            }
            aclickable[cr] = Instantiate(m_clickable) as GameObject;
            aclickable[cr].GetComponent<Clickable>().game = lboard[i].game;
            aclickable[cr].GetComponent<Clickable>().lbAddress = lb.lbAddress;
            aclickable[cr].GetComponent<Clickable>().category.text = lboard[i].category;
            aclickable[cr].GetComponent<Clickable>().runner.text = lboard[i].runner;
            aclickable[cr].GetComponent<Clickable>().IGT.text = lboard[i].igt;
            aclickable[cr].GetComponent<Clickable>().RTA.text = lboard[i].rta;
            aclickable[cr].GetComponent<Clickable>().videoAddress = lboard[i].videoAddress;
            aclickable[cr].GetComponent<Clickable>().flagAddress = lboard[i].countryFlag;
            aclickable[cr].transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
            aclickable[cr].transform.position = new Vector2(m_clickable.transform.position.x, posY);
            if (!aclickable[cr].activeSelf) aclickable[cr].SetActive(true);
            posY -= 50f * multiplier * cMultiplier  ;
            if (currentGame == 6 && lb.category == "All Bosses") lastAdd = aclickable[cr];

            i++;

                
           
            cr++;
            yield return null;

        }
        currentGame++;
        posY -= 10f * multiplier * cMultiplier;

        UpdateGame();
        yield return null;

    }
    public void UpdateGame()
    {
        if (currentGame < 7) StartCoroutine(UpdateInfo());
        if (currentGame >= 7)
        {
            lastAdd.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
            if (!copyright.activeSelf) copyright.SetActive(true);
            copyright.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
            copyright.GetComponent<RectTransform>().anchoredPosition = new Vector2(copyright.GetComponent<RectTransform>().anchoredPosition.x, lastAdd.GetComponent<RectTransform>().anchoredPosition.y - 50f);
            contentY = copyright.GetComponent<RectTransform>().anchoredPosition.y - 50f;
            lastAdd.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
            copyright.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
            contentY = Mathf.Abs(contentY);

            if (content.sizeDelta.y != contentY) content.sizeDelta = new Vector2(content.sizeDelta.x, contentY);
            copyright.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);

            dataFetch.SetActive(false);
            isLoading = false;
            scroll.horizontal = true;
            scroll.vertical = true;
            HeaderImage.showBanner = true;

 

        }
       

    }


    public void Errori()
    {
        isLoading = true;
        loadData = 0;
        loadSlider.maxValue = 23;
        loadSlider.value = 0;
        scroll.horizontal = false;
        scroll.vertical = false;
        dataCounter = 0;
        currentGame = 0;
        // 3:2 = 1.5 - 16:10 = 1.6 - 4:3 = 1.33 - 16:9 = 1.78 

        multiplier = (float)Screen.height / 1000;
        dividerX = Screen.height / 1280f;
        dividerY = Screen.width / 720f;
        cMultiplier = 1f;
        if (Screen.height <= 1280) cMultiplier = 1.1f;
        if ((float)Screen.height / (float)Screen.width <= 1.51f) cMultiplier = 1.3f;
        if (cMultiplier < 0.1f) cMultiplier = 0.1f;
        cr = 0;
        posX = m_gameTitle.transform.position.x;
        posY = m_gameTitle.transform.position.y;
        scroll.horizontalNormalizedPosition = 0;
        scroll.verticalNormalizedPosition = 0;
        content.anchoredPosition = new Vector2(0, 0);
        content.sizeDelta = new Vector2(0, 0);
        loadSlider = dataFetch.GetComponentInChildren<Slider>();
        StartCoroutine(UpdateInfo());
    }
}


