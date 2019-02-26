using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class HeaderImage : MonoBehaviour {


    bool testMode = false;
    private string bannerPlacement = "banner";
    private string gameID = "";
    public static bool showBanner;

    public bool showingBanner;


    void Start()
    {
        //Advertisement.Initialize(gameID, testMode);
    }

    // Update is called once per frame
    void Update()
    {

        if (showBanner && !showingBanner) StartCoroutine(showBanneri());
    }
    IEnumerator showBanneri()
    {

        showingBanner = true;
        //if (Advertisement.Banner.isLoaded) Advertisement.Banner.Hide(true);

        /*while(!Advertisement.IsReady("banner")) {
            yield return null;
        }*/
        //Advertisement.Banner.Show(bannerPlacement);
        showBanner = false;
        showingBanner = false;
        yield return null;
 

    }



}
