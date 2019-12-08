using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Networking;
using GameAction;

public class GameActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public DataManager dataManager;
    public KinectAction gameAction;
    
    public static string URL = "http://localhost:5000/get_inference";
    public const int FRAME_PER_SECOND = 30;
    
    public bool isStreaming;
    private IEnumerator stream;

/// /////////////////////////////////

    private Dictionary<float, KinectAction> actionDic = new Dictionary<float, KinectAction>() {
        {-1, KinectAction.UNDEFINED },
        {0, KinectAction.STAND },
        {1, KinectAction.RUN_RIGHT },
        {2, KinectAction.JUMP_UP },
        {3, KinectAction.JUMP_LEFT },
        {4, KinectAction.JUMP_RIGHT },
        {5, KinectAction.STAND_ATTACK },
        {6, KinectAction.ATTACK_RIGHT },
        {7, KinectAction.ATTACK_LEFT },
        {8, KinectAction.RUN_LEFT },
    };

    void Start()
    {
        gameAction = KinectAction.STAND;
        Debug.Log(gameAction);
    }

    public void StartTakingDataPose()
    {
        stream = TakingPoseData(FRAME_PER_SECOND);
        isStreaming = !isStreaming;
        if(isStreaming)
        {
            Debug.Log("Start taking data");
            StartCoroutine(stream);
        } else
        {
            Debug.Log("Stop taking data");
            StopCoroutine(stream);
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
        //Debug.Log("call taking pose");
        while (isStreaming)
        {
            for (int i = 0; i < n; i++)
            {
                //Debug.Log("hi: " + i);
                bool readFirst = false;
                if(!readFirst)
                {
                    foreach (KeyValuePair<ulong, GameObject> entry in dataManager.bsv.getBodies())
                    {
                        //Debug.Log("Are u here");
                        GameObject body = entry.Value;
                        readFirst = true;

                        if (body == null)
                        {
                            Debug.LogError("No body found");
                        }

                        float[] data = new float[25 * 3];
                        dataManager.FillInData(data, body);
                        StartCoroutine(SendPoseData(data, ProcessResponse));

                    }
                }
                yield return new WaitForSeconds(1.0f / n);
            }
        }
    }


    private IEnumerator SendPoseData(float[] data, Action<string> response)
    {
        Debug.Log("Send data to Server");
        System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
        string url = URL;
        //Debug.Log("url: " + url);
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

        //Debug.Log("Send");
        yield return www.SendWebRequest(); // SEND REQUEST

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            response("");
        }
        else
        {
            //Debug.Log("Form upload complete!");
            //Debug.Log(www.downloadHandler.text);
            response(www.downloadHandler.text);
        }

        Debug.Log("Server send/receive: " + stopwatch.ElapsedMilliseconds);
    }

    private void ProcessResponse(string response)
    {
        //Debug.Log(response);
        Pose pose = JsonUtility.FromJson<Pose>(response);

        gameAction = actionDic[pose.pose];
        Debug.Log(pose.pose);
    }

    /// /////////////////////////


    // Update is called once per frame
    void Update()
    {
        

    }
}

[Serializable]
struct SerializedData {
    [SerializeField]
    public float[] joint_set;
}

struct Pose {
    public float pose;
}

