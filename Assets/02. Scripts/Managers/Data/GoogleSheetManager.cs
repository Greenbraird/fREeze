using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    //현재 B:B 영역만 참조중
    const string URL = "https://docs.google.com/spreadsheets/d/1W_Jd2Lk4EETKxpwX5zKmS4Ip2JBg_UeV7QGebEMfKwM/export?format=tsv&range=B:B";

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;


        string[] lines = data.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split('\t');
            for (int j = 0; j < columns.Length; j++)
            {
                Debug.Log($"  Column {j + 1}: {columns[j]}");
            }
        }
    }
}
