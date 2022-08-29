using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RecordingState { Record, Run};
public class HorseController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D target;

    private Vector2 prevPos;

    private List<Vector2> positions;
    private List<float> rotation;
    public RecordingState recordingState;
    private int curIndex = 0;
    private int id = 0;
    private bool callbackMade = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
        prevPos = target.transform.position;
        id = PlayerPrefs.GetInt("LastestId");
        if(id == 0)
        {
            PlayerPrefs.SetInt("LatestId", Random.Range(10000, 01000000));
        }
        PlayerPrefs.SetInt("LatestId", id + 1);
        positions = new List<Vector2>();
        rotation = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = target.transform.position;
        
        if(recordingState == RecordingState.Run&&Vector2.Distance(transform.position,positions[curIndex])<0.3f)
        {
            curIndex = (curIndex + 1) % positions.Count;
        }
        if(recordingState == RecordingState.Run)
        {
            transform.position = Vector2.Lerp((Vector2)transform.position, positions[curIndex],0.5f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,rotation[curIndex]);
        }
    }
    private void FixedUpdate()
    {

        Vector2 delta = (Vector2)target.transform.position - prevPos;
        float angle = (Mathf.Atan2(delta.y, delta.x)/(Mathf.PI*2))*360;
        //print(angle);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            angle);
        prevPos = target.transform.position;


        if(recordingState == RecordingState.Record)
        {
            positions.Add(transform.position);
            rotation.Add(transform.eulerAngles.z);
        }
        print(positions.Count);
        
    }
    public void LoadTransforms()
    {
        string rawPlayersData = PlayerPrefs.GetString("PlayersData");
        SaveData playersData = JsonUtility.FromJson<SaveData>(rawPlayersData);
        PlayerSaveData myData = playersData.getFromId(this.id);
        this.positions.AddRange(myData.positions);
        this.rotation.AddRange(myData.rotations);

    }
    public void SaveTransforms(bool isTemp=false)
    {
        string reference = "PlayersData";
        if (isTemp)
        {
            reference = "TemporaryRunData";
        }
        //PlayersData: "{name:id:"
        string rawPlayersData = PlayerPrefs.GetString(reference);
        SaveData playersData = JsonUtility.FromJson<SaveData>(rawPlayersData);
        PlayerSaveData myData = new PlayerSaveData();
        myData.positions = positions.ToArray();
        myData.rotations = rotation.ToArray();
        myData.id = id;
        myData.name = name;
        PlayerSaveData[] newData = new PlayerSaveData[playersData.data.Length + 1];
        for(int i = 0; i < playersData.data.Length; i++)
        {
            newData[i] = playersData.data[i];
        }
        newData[newData.Length - 1] = myData;
        playersData.data = newData;
        string updated = JsonUtility.ToJson(playersData);

        PlayerPrefs.SetString(reference, updated);
    }

    public void CallbackSave()
    {
        string rawPlayersData = PlayerPrefs.GetString("TemporaryRunData");
        SaveData playersData = JsonUtility.FromJson<SaveData>(rawPlayersData);
        PlayerSaveData player = playersData.getFromId(id);
        if (player == null)
        {
            callbackMade = true;

            
        }
        else
        {
            

        }
    }
}
/// <summary>
/// Structure representation of ghost runs.
/// </summary>
[System.Serializable]
public class SaveData
{
    [SerializeField]
    public PlayerSaveData[] data;

    public PlayerSaveData getFromId(int id)
    {
        foreach(PlayerSaveData psd in this.data)
        {
            if(psd.id == id)
            {
                return psd;
            }
        }
        return null;
    }
}
/// <summary>
/// Structure representation of one ghost run.
/// </summary>
[System.Serializable]
public class PlayerSaveData {
    public Vector2[] positions;
    public float[] rotations;
    public int id;
    public string name;
}

