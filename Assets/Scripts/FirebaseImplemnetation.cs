using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseImplemnetation : MonoBehaviour
{
    public FirebaseLoader loader;
    [SerializeField]
    public PlayerPush[] players;
    // Start is called before the first frame update
    void Start()
    {
        string finalString = "[";
        int count = 0;
        foreach (PlayerPush player in players)
        {
            string item = "{\"name\":\"" + player.name + "\",\"time\":\"" + player.time + "\",\"position\":\"" + player.pos + "\"}";
            // print(item);
            loader.send("/racing-game/"+count,item);
            count++;
            finalString += item;
        }
        //finalString=finalString.Substring(0, finalString.Length - 1);
        finalString += "]";
        //loader.send("/racing-game/", finalString);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct PlayerPush
{
    public string name;
    public float time;
    public int pos;
}