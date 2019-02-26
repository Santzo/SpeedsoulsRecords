using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Playbutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image img;
    public Color oriColor;
    private bool pointerDown;
    private string url;
    private int howLong;

	// Use this for initialization
	void Start ()
    {
        if (transform.parent.tag == "Clickable") url = GetComponentInParent<Clickable>().videoAddress;
        if (transform.parent.tag == "Mini") url = GetComponentInParent<MiniClickable>().videoAddress;
        img = GetComponent<Image>();
        oriColor = img.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (!pointerDown && img.color.b != oriColor.b) img.color = oriColor;
        if (!pointerDown && howLong > 0) howLong = 0;
        if (pointerDown) howLong++;
	}

    public void OnPointerDown (PointerEventData eventData)
    {
        if (url != "" && Input.touchCount == 1)
        {
            img.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
            pointerDown = true;
        }
        if (url == "" && Input.touchCount == 1)
        {
            img.color = new Color(0.9f, 0.3f, 0.2f, 0.9f);
            pointerDown = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerDown && url != "" && howLong > 4 && Input.touchCount == 1)
        {
            img.color = oriColor;
            pointerDown = false;
            Application.OpenURL(url);
        }
        else pointerDown = false;
    }
}
