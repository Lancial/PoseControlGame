using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Action = GameAction.Action;
using UnityEngine.Networking;

public class GameActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public DataManager dataManager;
    public Action gameAction = Action.STAND;
    public static string URL = "http://localhost:5000/get_inference";

    private bool isStreaming;
    private IEnumerator stream;
    void Start()
    {
        
    }

    public void StartTakingDataPose()
    {
        stream = TakingPoseData(10);
        isStreaming = !isStreaming;

        if(!isStreaming)
        {
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="n">Frame per second</param>
    /// <returns></returns>
    public IEnumerator TakingPoseData(int n)
    {
        while (isStreaming)
        {
            for (int i = 0; i < n; i++)
            {
                foreach (KeyValuePair<ulong, GameObject> entry in dataManager.bsv.getBodies())
                {
                    GameObject body = entry.Value;

                    if (body == null)
                    {
                        Debug.LogError("No body found");
                    }

                    float[] data = new float[25 * 3];
                    dataManager.FillInData(data, body);
                    Array.ForEach(data, write);
                    StartCoroutine(SendPoseData(data, ProcessResponse));

                }

                
                yield return new WaitForSeconds(1.0f / n);
            }
        }
    }

    private void write(float val)
    {
        Debug.Log(val);
    }

    private IEnumerator SendPoseData(float[] data, Action<string> response)
    {
        Debug.Log("Send data to Server");
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        string url = URL;
        Debug.Log("url: " + url);
        SerializedData serializedData;
        serializedData.joint_set = data;

        string body = JsonUtility.ToJson(serializedData);

        // Post request
        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(body);
        www.SetRequestHeader("content-type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(jsonToSend)
        {
            contentType = "application/json"
        };
        www.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log("Send");
        yield return www.SendWebRequest(); // SEND REQUEST

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            response("");
        }
        else
        {
            Debug.Log("Form upload complete!");
            response(www.downloadHandler.text);
        }

        Debug.Log("Server send/receive: " + stopwatch.ElapsedMilliseconds);
    }

    private void ProcessResponse(string response)
    {
        Debug.Log(response);
    }



    // Update is called once per frame
    void Update()
    {
        if(gameAction == Action.STAND)
        {

        } else if(gameAction == Action.RUN)
        {

        } else if(gameAction == Action.JUMP_LEFT)
        {

        }
    }
}

[Serializable]
struct SerializedData {
    [SerializeField]
    public float[] joint_set;
}
