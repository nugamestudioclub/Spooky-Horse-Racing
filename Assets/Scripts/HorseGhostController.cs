using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HorseGhostController : MonoBehaviour
{
    public HorseRecordingState state;
    public static string raw_filepath = @"D:\UnityGames\Game Studio Club\spooky-horse-racing\Assets\Resources\";
    private StreamReader reader;
    private StreamWriter writer;
    public int id = 1;

    private int curIndex = 0;
    string path;
    SerializableTransformData curPos;

    private bool reachedEnd = false;


    // Start is called before the first frame update
    void Awake()
    {
        //print("STARTInG!");
        path = raw_filepath + "horses" + id.ToString() + ".txt";
        if (!File.Exists(path))
        {
            //print("Creating file!");
            File.Create(path);
           // print("CREATED FILE!");
        }
        //text = (TextAsset)Resources.Load(path);
        print("Loaded resource:");
        if(state == HorseRecordingState.Playback)
            reader = new StreamReader(path);
        else
            writer = new StreamWriter(path);
        
        print(writer);
        print(reader);
            
    }
    
    public void WriteData(string data)
    {
        writer.WriteLine(data);
    }
    public string ReadNext()
    {
        string data = reader.ReadLine();
        if(data == null)
        {
            reader.Close();
            this.reachedEnd = true;
            reader = new StreamReader(this.path);
        }
        return reader.ReadLine();
    }
   

    public SerializableTransformData getNextPosition()
    {
        return JsonUtility.FromJson<SerializableTransformData>(ReadNext());
    }

    public void writePosition(SerializableTransformData dat)
    {
        string jDat = JsonUtility.ToJson(dat);
        this.WriteData(jDat);
    }


    private void FixedUpdate()
    {
        if(this.state == HorseRecordingState.Playback)
        {
            if (!reachedEnd)
            {
                //CHANGE 0.5 if you want faster or slower
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(curPos.pX, curPos.pY, curPos.pZ), 0.5f);
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, curPos.rot);

                if (Vector2.Distance(new Vector2(curPos.pX, curPos.pY), transform.position) < 0.1f)
                {
                    curPos = this.getNextPosition();
                }
            }
            

        }
        else if(this.state==HorseRecordingState.Record)
        {
            SerializableTransformData dat = new SerializableTransformData();
            dat.pX = transform.position.x;
            dat.pY = transform.position.y;
            dat.pZ = transform.position.z;
            dat.rot = transform.eulerAngles.z;
            this.writePosition(dat);
        }
        curIndex += 1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum HorseRecordingState { Record, Playback};

[System.Serializable]
public struct SerializableTransformData
{
    public float pX;
    public float pY;
    public float pZ;
    public float rot;
}