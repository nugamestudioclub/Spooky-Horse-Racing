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
            string item = JsonUtility.ToJson(player);// string item = "{\"name\":\"" + player.name + "\",\"time\":\"" + player.time + "\",\"position\":\"" + player.pos + "\"}";
                
            // print(item);
            loader.send("/racing-game/"+count,item);
            count++;
            finalString += item;
        }
        //finalString=finalString.Substring(0, finalString.Length - 1);
        finalString += "]";
        //loader.send("/racing-game/", finalString);
        loader.path = "racing-game/";
        loader.read();

        //print(loader.Buffer);

    }

   
    // Update is called once per frame
    void Update()
    {
       

        List<string> data = loader.Buffer;
        if(data.Count != 0)
        {
           // print("LOADING SCRIPT!");
            string dat = loader.ReadFirstFromBuf();
           // print(dat);
            if (dat.Contains("racing-game"))
            {
                DBDesign design = JsonUtility.FromJson<DBDesign>(dat.Replace("racing-game","racing_game"));
              //  print("LOADED DES:" + design);
              //  print("LOADE DES:" + design.racing_game);
              //  print("LOADED DES:" + design.racing_game.Length);
                for(int i = 0; i < design.racing_game.Length; i++)
                {
              //      print("LOADED PLAYER DATA:" + design.racing_game[i].name);
               }
            }
            
           
            
        }
        
    }
}

