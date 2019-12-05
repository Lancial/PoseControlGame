using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Networking;
using GameAction;

public class GameActionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public DataManager dataManager;
    public KinectAction gameAction;
    
    public static string URL = "http://localhost:5000/get_inference";
    public const int FRAME_PER_SECOND = 10;
    
    private Rigidbody playerRB;
    private bool isStreaming;
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
        playerRB = player.GetComponent<Rigidbody>();
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


    bool isJump;
    float jumpElapsed;
    float jumpReset = 1f;

    public float force = 200;
    public float forceJump = 200;
    public float runSpeed = 2f;
    // Update is called once per frame
    void Update()
    {
        if(gameAction == KinectAction.STAND)
        {
            //playerRB.velocity = Vector3.zero;
            //Debug.Log("standing");
        } else if(gameAction == KinectAction.RUN_RIGHT)
        {
            Debug.Log("run right");
            playerRB.velocity = new Vector3(1, 0, 0) * runSpeed;
        }
        else if (gameAction == KinectAction.RUN_LEFT)
        {
            Debug.Log("run right");
            playerRB.velocity = new Vector3(-1, 0, 0) * runSpeed;
        }
        else if (gameAction == KinectAction.JUMP_UP && !isJump)
        {
            Debug.Log("jump up");
            playerRB.AddForce(Vector3.up * force);
            isJump = true;
        }
        else if (gameAction == KinectAction.JUMP_LEFT && !isJump)
        {
            Debug.Log("jump left");
            playerRB.AddForce(new Vector3(-1,1,0) * forceJump);
            isJump = true;
        }
        else if (gameAction == KinectAction.JUMP_RIGHT && !isJump)
        {
            Debug.Log("jump right");
            playerRB.AddForce(new Vector3(1, 1, 0) * forceJump);
            isJump = true;
        }
        else if (gameAction == KinectAction.STAND_ATTACK)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("stand attack");
        }
        else if (gameAction == KinectAction.ATTACK_RIGHT)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack right");
        }
        else if (gameAction == KinectAction.ATTACK_LEFT)
        {
            //playerRB.velocity = Vector3.zero;
            Debug.Log("attack left");
        }
        else { 

            //playerRB.velocity = Vector3.zero;
            Debug.LogError("undefined action");
        }

        if(isJump)
        {
            jumpElapsed += Time.deltaTime;
        }

        if(jumpElapsed >= jumpReset)
        {
            jumpElapsed = 0;
            isJump = false;
        }
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

