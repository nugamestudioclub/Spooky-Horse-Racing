using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RaceGhostPlayerMovement : RacePlayerMovement
{
    public HorseRecordingState state;
    //@"D:\UnityGames\Game Studio Club\spooky-horse-racing\Assets\Resources\"
    public static string raw_filepath;
    private StreamReader reader;
    private StreamWriter writer;

    private int curIndex = 0;
    string path;
    SerializableTransformData curPos;

    private bool reachedEnd = false;

    private bool controlEnabled;
    public override bool ControlEnabled
    {
        get => controlEnabled;
        set
        {
            if (value) Begin();
            controlEnabled = value;
        }
    }


    // Start is called before the first frame update
    private void Begin()
    {
        raw_filepath = Application.dataPath + @"\Resources\";
        //print("STARTInG!");
        path = raw_filepath + "horses" + ControllerId.ToString() + ".txt";
        print(path);
        if (!File.Exists(path))
        {
            //print("Creating file!");
            File.Create(path);
            // print("CREATED FILE!");
        }
        //text = (TextAsset)Resources.Load(path);
        print("Loaded resource:");
        if (state == HorseRecordingState.Playback)
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
        print($"Reader is null: {reader == null}");
        string data = reader.ReadLine();

        if (data == null)
        {
            reader.Close();
            this.reachedEnd = true;
            reader = new StreamReader(this.path);
            data = reader.ReadLine();
        }
        return data;
    }


    public SerializableTransformData getNextPosition()
    {
        string data = ReadNext();
        print($"Data is null: {data == null}");
        return JsonUtility.FromJson<SerializableTransformData>(data);
    }

    public void writePosition(SerializableTransformData dat)
    {
        string jDat = JsonUtility.ToJson(dat);
        this.WriteData(jDat);
    }


    private void FixedUpdate()
    {
        if (!ControlEnabled)
        {
            return;
        }
        if (this.state == HorseRecordingState.Playback)
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
        else if (this.state == HorseRecordingState.Record)
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

    void OnApplicationQuit()
    {
        if (reader != null) reader.Close();
        if (writer != null) writer.Close();
    }
}

public enum HorseRecordingState { Record, Playback };

[System.Serializable]
public struct SerializableTransformData
{
    public float pX;
    public float pY;
    public float pZ;
    public float rot;
}