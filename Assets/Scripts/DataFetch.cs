using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DataFetch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool error;
    public bool pointerDown;
    private TextMeshProUGUI text;
    public GameObject bar;
    public static int gameError;
    public static int categoryError;
    private Image panel;
    private Color oriColor;

	// Use this for initialization
	void Start ()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        panel = GetComponentInChildren<Image>();
        oriColor = panel.color;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (error)
        {
            if (text.text != "Oops! Something went wrong, click restart.") text.text = "Oops! Something went wrong, click here to restart.";
            bar.SetActive(false);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (error)
        {
            panel.color = new Color(0.7f, 0.7f, 0.7f, 0.9f);
            pointerDown = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerDown)
        {
            panel.color = oriColor;
            pointerDown = false;
            error = false;
            bar.SetActive(true);
            text.text = "Fetching data from http://www.speedrun.com";

            GameObject[] move;
            move = GameObject.FindGameObjectsWithTag("Game title");
            for (int a = 0; a < move.Length; a++)
            {
                if (move[a].activeSelf) Destroy(move[a]);
            }
 

            move = GameObject.FindGameObjectsWithTag("Clickable");
            for (int a = 0; a < move.Length; a++)
            {
                if (move[a].activeSelf) Destroy(move[a]);
            }
            move = GameObject.FindGameObjectsWithTag("Mini");
            for (int a = 0; a < move.Length; a++)
            {
                if (move[a].activeSelf) Destroy(move[a]);
            }


            GameObject.FindGameObjectWithTag("Content").GetComponent<SimpleExample>().currentGame = 0;
            GameObject.FindGameObjectWithTag("Content").GetComponent<SimpleExample>().Errori();
        }
    }
}
