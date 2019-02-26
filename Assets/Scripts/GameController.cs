using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public Canvas canvas; 
    public float zoomSpeed;     
    public float panSpeed = 10f;
    public float oriCanvasSize;
    public static float cSize;
    private void Start()
    { 
        oriCanvasSize = canvas.scaleFactor;
    }
    void Update()
    {
        cSize = canvas.scaleFactor;
        if (Input.touchCount == 2 && !SimpleExample.isLoading && !SimpleExample.changeParents)
        {
            SimpleExample.zooming = true;
            Touch touchZero = Input.GetTouch(0); 
            Touch touchOne = Input.GetTouch(1);


            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;


            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;


            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            canvas.scaleFactor -= deltaMagnitudeDiff * zoomSpeed;

            canvas.scaleFactor = Mathf.Clamp(canvas.scaleFactor, oriCanvasSize, oriCanvasSize * 1.05f);

        }
        if (Input.touchCount < 2 && !SimpleExample.isLoading && !SimpleExample.changeParents)
        {
            if (SimpleExample.zooming) SimpleExample.zooming = false;
        }

    }



}
