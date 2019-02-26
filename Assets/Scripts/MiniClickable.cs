using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniClickable : MonoBehaviour {
    public TextMeshProUGUI place;
    public TextMeshProUGUI runner;
    public string game;
    public string category;
    public TextMeshProUGUI IGT;
    public TextMeshProUGUI RTA;
    public string flagAddress;
    public string videoAddress;
    public int gameType;
    public Image flag;

    // Use this for initialization
    void Start ()
    {
        if (flagAddress != "") StartCoroutine(LoadImage());
    }
	
	// Update is called once per frame
	void Update ()
    {
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
}
