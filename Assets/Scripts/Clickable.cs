using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string game;
    public TextMeshProUGUI category;
    public TextMeshProUGUI runner;
    public TextMeshProUGUI IGT;
    public RectTransform content;
    public TextMeshProUGUI RTA;
    private Vector2 contentPos;
    public GameObject m_mClickable;
    public GameObject _copyright;
    private Image highlight;
    private Color oriColor;
    public Image flag;
    public string videoAddress;
    public string flagAddress;
    private GameObject dataFetch;
    public string lbAddress;
    private bool pointerDown;
    private bool listActive;
    private int howLong;
    private LeaderBoard.Info lb;
    private LeaderBoard.UInfo uinfo;
    private UnityWebRequest www;
    private ScrollRect scroll;
    public class lbL
    {
        public string place;
        public string name;
        public string timeIGT;
        public string timeRTA;
        public string videolink;
    }
    // Use this for initialization

    public lbL lbList;

    void Start()
    {
        scroll = GameObject.FindGameObjectWithTag("Viewport").GetComponent<ScrollRect>();
        lbList = new lbL();
        dataFetch = GameObject.FindGameObjectWithTag("Datafetch");
        lb = new LeaderBoard.Info();
        highlight = GetComponent<Image>();
        oriColor = highlight.color;
        if (flagAddress != "") StartCoroutine(LoadImage());


    }

    // Update is called once per frame
    void Update()
    { 
        if (listActive && !pointerDown && highlight.color.b != 0.9f) highlight.color = new Color(0.2f, 0.4f, 0.9f, 0.8f);
        if (!listActive && !pointerDown && highlight.color != oriColor) highlight.color = oriColor;
        if (howLong != 0 && !pointerDown) howLong = 0;
        if (pointerDown) howLong++;
        if (transform.parent.tag != "Content" && !SimpleExample.isLoading && !SimpleExample.changeParents) transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
    }
    private IEnumerator LoadImage()
    {
        WWW wwwLoader = new WWW(flagAddress);
        yield return wwwLoader;

        flag.sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));
        flag.color = new Color(1f, 1f, 1f, 1f);
        flag.preserveAspect = true;
        yield return null;
}

    public void OnPointerDown(PointerEventData eventData)
    {
        highlight.color = new Color(0.8f, 0.8f, 0.8f, 0.6f);
        pointerDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerDown)
        {
            highlight.color = oriColor;
            pointerDown = false;
            if (!SimpleExample.isLoading && listActive && howLong > 4 && Input.touchCount == 1)
            {
                listActive = false;
                DestroyList();
            }
            else if (!SimpleExample.isLoading && !listActive && howLong > 4 && Input.touchCount == 1)
            {
                listActive = true;
                StartCoroutine(UpdateList());
                SimpleExample.isLoading = true;
            }
        }
    }

    void DestroyList()
    {
        
        scroll.horizontal = false;
        scroll.vertical = false;
        SimpleExample.changeParents = true;
        contentPos = content.anchoredPosition;
        ChangeParents();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Mini");
        GameObject[] move;
        foreach (GameObject dstroy in objects)
        {
            if (category.text == dstroy.GetComponent<MiniClickable>().category && game == dstroy.GetComponent<MiniClickable>().game)
            {
                dstroy.SetActive(true);
                Destroy(dstroy);
                
                float multiplier = content.localScale.x;
                move = GameObject.FindGameObjectsWithTag("Game title");
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].GetComponent<RectTransform>().anchoredPosition.y < dstroy.GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        if (move[a].transform.parent.name == "UI" && move[a].activeSelf) move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y + 60f * multiplier);
                       
                    }
                }
                move = GameObject.FindGameObjectsWithTag("Clickable");
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].GetComponent<RectTransform>().anchoredPosition.y < dstroy.GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        if (move[a].transform.parent.name == "UI" && move[a].activeSelf) move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y + 60f * multiplier);
                      

                    }
                }
                move = GameObject.FindGameObjectsWithTag("Mini");
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].GetComponent<RectTransform>().anchoredPosition.y < dstroy.GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        if (move[a].transform.parent.name == "UI" && move[a].activeSelf) move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y + 60f * multiplier);
                       
                        
                    }
                }

                _copyright.GetComponent<RectTransform>().anchoredPosition = new Vector2(_copyright.GetComponent<RectTransform>().anchoredPosition.x, _copyright.GetComponent<RectTransform>().anchoredPosition.y + 60f * multiplier);
                SimpleExample.contentY -= 60f;
            }
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, SimpleExample.contentY);
        _copyright.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
        ReturnParents();
        SimpleExample.changeParents = false;
        SimpleExample.isLoading = false;
        content.anchoredPosition = contentPos;
        scroll.horizontal = true;
        scroll.vertical = true;

    }


    IEnumerator UpdateList()
    {
        HeaderImage.showBanner = true;
        yield return null;
        scroll.horizontal = false;
        scroll.vertical = false;
        contentPos = content.anchoredPosition;
        if (!dataFetch.activeSelf) dataFetch.SetActive(true);
        dataFetch.GetComponentInChildren<Slider>().value = 0;
        dataFetch.GetComponentInChildren<Slider>().maxValue = 10;
        SimpleExample.changeParents = true;
        ChangeParents();

        yield return null;
        www = UnityWebRequest.Get(lbAddress);
        
        www.downloadHandler = new DownloadHandlerBuffer();
        www.useHttpContinue = false;
        www.timeout = 10;
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            DataFetch.error = true;
        }

        lb = JsonUtility.FromJson<LeaderBoard.Info>(www.downloadHandler.text);
        int i = 0;
        yield return null;

        foreach (LeaderBoard.Run run in lb.data.runs)
        {
            if (content.anchoredPosition != contentPos) content.anchoredPosition = contentPos;
            dataFetch.GetComponentInChildren<Slider>().value = i;

            if (i > 0)
            {
                GameObject mclickable = Instantiate(m_mClickable) as GameObject;
                mclickable.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform, false);
                mclickable.GetComponent<RectTransform>().localScale = content.localScale;
                mclickable.GetComponent<MiniClickable>().place.text = i + 1 + ".";
                mclickable.GetComponent<MiniClickable>().game = game;
                mclickable.GetComponent<MiniClickable>().category = category.text;
                if (run.run.videos.links.Count > 0) mclickable.GetComponent<MiniClickable>().videoAddress = run.run.videos.links[0].uri;
                if (run.run.players[0].name != null) mclickable.GetComponent<MiniClickable>().runner.text = run.run.players[0].name;
                else
                {
                    UnityWebRequest www2 = UnityWebRequest.Get("https://www.speedrun.com/api/v1/users/" + run.run.players[0].id);
                    www2.timeout = 10;
                    yield return www2.SendWebRequest();
                    if (www2.isNetworkError || www2.isHttpError)
                    {
                        DataFetch.error = true;
                    }
                    uinfo = JsonUtility.FromJson<LeaderBoard.UInfo>(www2.downloadHandler.text);
                    mclickable.GetComponent<MiniClickable>().runner.text = uinfo.data.names.international;
                    mclickable.GetComponent<MiniClickable>().flagAddress = "https://www.countryflags.io/" + uinfo.data.location.country.code + "/flat/64.png";
                }
                TimeSpan timeIGT = TimeSpan.FromSeconds(run.run.times.ingame_t);
                TimeSpan timeRTA = TimeSpan.FromSeconds(run.run.times.realtime_t);
               
                if (game == "Dark Souls II" || game == "Dark Souls II SoTFS")
                {
                    if (run.run.times.realtime_noloads_t > 0) timeIGT = TimeSpan.FromSeconds(run.run.times.realtime_noloads_t);
                    else timeIGT = TimeSpan.FromSeconds(run.run.times.realtime_t);
                }

                if (timeIGT.Hours > 0) mclickable.GetComponent<MiniClickable>().IGT.text = string.Format("{0}:{1:00}:{2:00}", timeIGT.Hours, timeIGT.Minutes, timeIGT.Seconds);
                else mclickable.GetComponent<MiniClickable>().IGT.text = string.Format("{0}:{1:00}", timeIGT.Minutes, timeIGT.Seconds);
                if (timeRTA.Hours > 0) mclickable.GetComponent<MiniClickable>().RTA.text = string.Format("{0}:{1:00}:{2:00}", timeRTA.Hours, timeRTA.Minutes, timeRTA.Seconds);
                else mclickable.GetComponent<MiniClickable>().RTA.text = string.Format("{0}:{1:00}", timeRTA.Minutes, timeRTA.Seconds);
                if (run.run.times.realtime_t == 0 || game == "Dark Souls II"  || game == "Dark Souls II SoTFS") mclickable.GetComponent<MiniClickable>().RTA.text = "";
                if (timeIGT.TotalSeconds == 0) mclickable.GetComponent<MiniClickable>().IGT.text = "";

                float multiplier = content.localScale.x;
                mclickable.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y - (float) i * 60f * multiplier);

                if (!mclickable.activeSelf) mclickable.SetActive(true);
                GameObject[] move = GameObject.FindGameObjectsWithTag("Game title");
                int b = move.Length;
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].transform.parent.name == "UI" && move[a].activeSelf) 
                    {
                        if (move[a].GetComponent<RectTransform>().anchoredPosition.y < mclickable.GetComponent<RectTransform>().anchoredPosition.y)
                        {
                           move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y - 60f * multiplier);
                        }
                    }
                }
                move = GameObject.FindGameObjectsWithTag("Clickable");
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].transform.parent.name == "UI" && move[a].activeSelf)
                    {
                        if (move[a].GetComponent<RectTransform>().anchoredPosition.y < mclickable.GetComponent<RectTransform>().anchoredPosition.y)
                        {
                            move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y - 60f * multiplier);
                        }
                    }
                }
                move = GameObject.FindGameObjectsWithTag("Mini");
                for (int a = 0; a < move.Length; a++)
                {
                    if (move[a].transform.parent.name == "UI" && move[a].activeSelf)
                    {
                        if (move[a].GetComponent<RectTransform>().anchoredPosition.y < mclickable.GetComponent<RectTransform>().anchoredPosition.y)
                        {
                            move[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(move[a].GetComponent<RectTransform>().anchoredPosition.x, move[a].GetComponent<RectTransform>().anchoredPosition.y - 60f * multiplier);
                        }
                    }
                }
                _copyright.GetComponent<RectTransform>().anchoredPosition = new Vector2(_copyright.GetComponent<RectTransform>().anchoredPosition.x, _copyright.GetComponent<RectTransform>().anchoredPosition.y - 60f * multiplier);
                SimpleExample.contentY += 60f;
            }
               
            i++;
            

        }
        if (content.anchoredPosition != contentPos) content.anchoredPosition = contentPos;
        content.sizeDelta = new Vector2(content.sizeDelta.x, SimpleExample.contentY);
        ReturnParents();
        SimpleExample.changeParents = false;
        SimpleExample.isLoading = false;
        dataFetch.SetActive(false);
        if (content.anchoredPosition != contentPos) content.anchoredPosition = contentPos;
        scroll.horizontal = true;
        scroll.vertical = true;
        yield return null;


    }
    void ChangeParents()
    {
        GameObject[] amove = GameObject.FindGameObjectsWithTag("Game title");
        for (int a = 0; a < amove.Length; a++)
        {
            if (amove[a].transform.parent.name == "Content" && amove[a].activeSelf) amove[a].transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        }

        GameObject[] bmove = GameObject.FindGameObjectsWithTag("Clickable");
        for (int a = 0; a < bmove.Length; a++)
        {
            if (bmove[a].transform.parent.name == "Content" && bmove[a].activeSelf) bmove[a].transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        }

        GameObject[] cmove = GameObject.FindGameObjectsWithTag("Mini");
        for (int a = 0; a < cmove.Length; a++)
        {
            if (cmove[a].transform.parent.name == "Content" && cmove[a].activeSelf) cmove[a].transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        }
        _copyright.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
    }

    void ReturnParents()
    {
        GameObject[] amove = GameObject.FindGameObjectsWithTag("Game title");
        for (int a = 0; a < amove.Length; a++)
        {
            if (amove[a].transform.parent.name == "UI" && amove[a].activeSelf)  amove[a].transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
        }

        GameObject[] bmove = GameObject.FindGameObjectsWithTag("Clickable");
        for (int a = 0; a < bmove.Length; a++)
        {
            if (bmove[a].transform.parent.name == "UI" && bmove[a].activeSelf) bmove[a].transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
        }

        GameObject[] cmove = GameObject.FindGameObjectsWithTag("Mini");
        for (int a = 0; a < cmove.Length; a++)
        {
            if (cmove[a].transform.parent.name == "UI" && cmove[a].activeSelf) cmove[a].transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
        }
        _copyright.transform.SetParent(GameObject.FindGameObjectWithTag("Content").transform);
    }

}