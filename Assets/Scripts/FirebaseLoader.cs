using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

using System.Net.Http.Headers;


public class FirebaseLoader : MonoBehaviour
{

    HttpClient httpClient = new HttpClient();

    [SerializeField]
    private bool testing = false;
    [SerializeField]
    private bool write;

    private List<string> buf = new List<string>();
    [HideInInspector]
    public List<string> Buffer { get {return this.buf; } }
    [SerializeField]
    private string net;

    [SerializeField]
    public string path;
    [SerializeField]
    private string data;

    private Player[] db;

    public async void read()
    {
        using(httpClient = new HttpClient())
        {
            using(var request = new HttpRequestMessage(new HttpMethod("GET"), net+".json"))
            {
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                this.buf.Add(content);
                print("Added " + content);
            }
        }
    }

    public string ReadFirstFromBuf()
    {
        if(buf.Count > 0)
        {
            string data = buf[0];
            buf.RemoveAt(0);
            return data;
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// Send to firebase using the given path and sending a json message as a string.
    /// </summary>
    /// <param name="path">The Path from root</param>
    /// <param name="message">The JSON message as a string being sent.</param>
    public async void send(string path,string message)
    {
        using(httpClient = new HttpClient())
        {
            using(var request = new HttpRequestMessage(new HttpMethod("PUT"), net+path+".json"))
            {
                request.Content = new StringContent(message);
               
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                this.buf.Add(content);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="upload">Upload the HTTP content object.</param>
    public async void send(string path, HttpContent upload)
    {
        using (httpClient = new HttpClient())
        {
            using (var request = new HttpRequestMessage(new HttpMethod("PUT"), net + path + ".json"))
            {
                request.Content = upload;

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                //this.buf.Add(content);
            }
        }
    }

    /// <summary>
    /// Send scores to the firebase db. Player username, email, absolute time, time removed from other players, their end positions in the race.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="time"></param>
    /// <param name="timeRemoved"></param>
    /// <param name="pos"></param>
    public void PushScore(string username, string email, float time,float timeRemoved,int pos,int stuns, int coins,int id)
    {
        Player newPlayer = new Player() {
            name = username,
            email = email,
            time = time,
            timeRemoved = timeRemoved,
            position = pos,
            stuns = stuns,
            coins = coins,
            id = id
        };
        Player[] newDB = new Player[db.Length+1];
        for(int i = 0; i < db.Length; i++)
        {
            newDB[i] = db[i];
        }
        newDB[newDB.Length - 1] = newPlayer;
        db = newDB;
        PushDB();

    }

    private void PushDB()
    {
        var stringPayload = JsonUtility.ToJson(db);
        send("/users/", stringPayload);
    }

    public class DatabaseUpload
    {
        public Player[] players;

    }

    public class Player
    {
        public string name;
        public string email;
        public float time;
        public float timeRemoved;
        public int position;
        public int stuns;
        public int coins;
        public int id;

    }
    // Start is called before the first frame update
    void Start()
    {
        if (testing)
        {
            if (write)
            {
                send(path, data);
            }
            else
            {
                read();
            }
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {

        //if (buf.Count > 0)
        {
            // print(buf[0]);
          //  buf.RemoveAt(0);
        }

    }

    public DBDesign GetDBData()
    {
        FirebaseLoader loader = this;
        List<string> data = loader.Buffer;
        if (data.Count != 0)
        {
            string dat = loader.ReadFirstFromBuf();
            if (dat.Contains("racing-game"))
            {
                DBDesign design = JsonUtility.FromJson<DBDesign>(dat.Replace("racing-game", "racing_game"));
                return design;
            }
            else
            {
                DBDesign newDes = new DBDesign();
                PlayerPush push = JsonUtility.FromJson<PlayerPush>(dat);
                newDes.racing_game = new PlayerPush[] { push };
                return newDes;
            }
        }
        else
        {
            return null;
        }
    }

    public void SendDBData(DBDesign design)
    { 
        this.send("/", JsonUtility.ToJson(design));
    }
}
[System.Serializable]
public struct PlayerPush
{
    public string name;
    public float time;
    public int position;
    public int stuns;
    public int coins;
    public int id;
}



[System.Serializable]
public class DBDesign
{
    public PlayerPush[] racing_game;
}