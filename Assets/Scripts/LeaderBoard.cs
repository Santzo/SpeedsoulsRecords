using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class LeaderBoard
{
    [System.Serializable]

    public class Link
    {
        public string uri ;
    }
    [System.Serializable]
    public class Videos
    {
        public List<Link> links ;
    }
    [System.Serializable]
    public class Status
    {
        public string status ;
        public string examiner ;
    }
    [System.Serializable]
    public class Player
    {
        public string rel ;
        public string id ;
        public string uri ;
        public string name ;
    }
    [System.Serializable]
    public class Times
    {
        public string primary ;
        public int primary_t ;
        public string realtime ;
        public int realtime_t ;
        public object realtime_noloads ;
        public int realtime_noloads_t ;
        public string ingame ;
        public int ingame_t ;
    }


    [System.Serializable]
    public class Run2
    {
        public string id ;
        public string weblink ;
        public string game ;
        public object level ;
        public string category ;
        public Videos videos ;
        public string comment ;
        public Status status ;
        public List<Player> players ;
        public string date ;

        public Times times ;
        public object splits ;

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
    public class Data
    {
        public string weblink;
        public string game ;
        public string category ;
        public object level ;
        public object platform ;
        public object region ;
        public object emulators ;
        public string timing ;
        public List<Run> runs ;
        public List<Link2> links ;
    }

    public class Info
    {
        public Data data ;
    }
    [System.Serializable]
    public class Names
    {
        public string international ;
        public object japanese ;
    }
    [System.Serializable]
    public class Country
    {
        public string code ;
    }
    [System.Serializable]
    public class Location
    {
        public Country country ;
    }



    [System.Serializable]
    public class Data2
    {
        public string id ;
        public Names names ;
        public string weblink ;
        public Location location;



        public string role ;


    }

    public class UInfo
{
    public Data2 data ;
}
}
