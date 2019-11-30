using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private Dictionary<string, List<float[]>> dicData;
    public InputField inputField;
    public Button CollectSinglebutton;
    //public GameObject body;
    public BodySourceView bsv;

    public Text display;
    public const string FILE_NAME = "bodydata.csv";

    void Start()
    {
        bsv = GameObject.Find("BodyView").GetComponent<BodySourceView>();
        dicData = new Dictionary<string, List<float[]>>();
        CollectSinglebutton.onClick.AddListener(() =>
        {
            OnClickCollectSingleData();
        });
    }

    // Update is called once per frame
    void Update()
    {
        string val = "";
        foreach (KeyValuePair<string, List<float[]>> entry in dicData)
        {
            // do something with entry.Value or entry.Key
            val += string.Format("label {0}, data size {1}", entry.Key, entry.Value.Count);
        }

        display.text = val;
    }

    public void OnClickCollectSingleData()
    {
        foreach (KeyValuePair<ulong, GameObject> entry in bsv.getBodies())
        {
            GameObject body = entry.Value;

            if (body == null)
            {
                Debug.LogError("No body found");
                return;
            }

            float[] data = new float[25 * 3];
            string label = inputField.text;

            if (label.Equals(string.Empty))
            {
                Debug.LogError("label cant be empty");
                return;
            }
            List<float[]> list = null;
            if (!dicData.ContainsKey(label))
            {
                list = new List<float[]>();
                dicData.Add(inputField.text, list);
            }
            else
            {
                list = dicData[label];
            }

            FillInData(data, body);

            list.Add(data);


            StringBuilder val = new StringBuilder();
            val.Append(label + ",");
            val.Append(data[0]);
            for (int i = 0; i < data.Length; i++)
            {
                val.Append("," + data[i]);
            }
            val.Append("\n");
            string result = val.ToString();

            Debug.Log(string.Format("Data collected: {0}", result));
            WriteDataToCSVFile(Application.streamingAssetsPath + "/" + FILE_NAME, result);
        }
        
    }

    private void FillInData(float[] data, GameObject body)
    {
        int index = 0;
        foreach(Transform transform in body.transform)
        {
            Vector3 position = transform.localPosition;
            data[index] = position.x;
            data[index+1] = position.y;
            data[index+2] = position.z;
            index += 3;
        }
    }

    public void WriteDataToCSVFile(string filePath, string data)
    {
        if(!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        File.AppendAllText(filePath, data);
    }
}
