using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    // ���� ���������Ʈ�� URL, TSV �������� �����ɴϴ�.
    const string URL = "https://docs.google.com/spreadsheets/d/1W_Jd2Lk4EETKxpwX5zKmS4Ip2JBg_UeV7QGebEMfKwM/export?format=tsv";

    // Unity���� ���۵� �� �ڷ�ƾ���� ���� ���������Ʈ�� �����ɴϴ�.
    IEnumerator Start()
    {
        // ���� ���������Ʈ �����͸� �������� ���� ��û�� �����մϴ�.
        UnityWebRequest www = UnityWebRequest.Get(URL);
        // ��û�� ������ ������ ��ٸ��ϴ�.
        yield return www.SendWebRequest();

        // ��û�� �����ϸ� ������ ����ϰ� �����մϴ�.
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }

        // TSV �����͸� ���ڿ��� �����ɴϴ�.
        string tsvData = www.downloadHandler.text;
        // TSV �����͸� �Ľ��Ͽ� DialogueEntry ����Ʈ�� ��ȯ�մϴ�.
        List<DialogueEntry> dialogueEntries = ParseTSV(tsvData);
        // DialogueEntry ����Ʈ�� JSON �������� ��ȯ�մϴ�.
        string json = JsonUtility.ToJson(new DialogueData { dialogues = dialogueEntries }, true);
        // JSON �����͸� ���Ϸ� �����մϴ�.
        SaveJsonToFile(json);
    }

    /// <summary>
    /// TSV �����͸� �Ľ��Ͽ� DialogueEntry ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="tsvData">TSV ������ ���ڿ� ������</param>
    /// <returns>DialogueEntry ��ü�� ����Ʈ</returns>
    List<DialogueEntry> ParseTSV(string tsvData)
    {
        // DialogueEntry ��ü�� ����Ʈ�� �����մϴ�.
        List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
        // TSV �����͸� �� ������ �и��մϴ�.
        string[] lines = tsvData.Split('\n');

        // ù ��° ���� ����̹Ƿ� �ǳʶݴϴ�.
        for (int i = 1; i < lines.Length; i++) // Start from 1 to skip header
        {
            // �� ���� ������ �и��Ͽ� �� �����͸� �����ɴϴ�.
            string[] columns = lines[i].Split('\t');
            // ���� ������ �����ϸ� �ǳʶݴϴ�.
            if (columns.Length < 6) continue;

            // �� �� �����͸� �Ľ��Ͽ� DialogueEntry ��ü�� �����մϴ�.
            if (int.TryParse(columns[0], out int order))
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                DialogueEntry entry = new DialogueEntry
                {
                    Order = int.Parse(columns[0]),
                    Format = columns[1],
                    Speaker = columns[2],
                    Contents = columns[3],
                    Next = columns[4] == "" ? "" : columns[4],
                    AddEvent = columns[5] == "\r" ? "" : columns[5]
                };
                // ������ DialogueEntry ��ü�� ����Ʈ�� �߰��մϴ�.
                dialogueEntries.Add(entry);
            }
            else
            {
                // ���� ���� ������ �߸��� ��� ��� �޽����� ����մϴ�.
                Debug.LogWarning($"Invalid format for ���� at line {i + 1}: {columns[0]}");
                continue;
            }
        }
        return dialogueEntries;
    }

    /// <summary>
    /// JSON �����͸� ���Ϸ� �����մϴ�.
    /// </summary>
    /// <param name="json">������ JSON ������</param>
    void SaveJsonToFile(string json)
    {
        // JSON �����͸� ������ ��θ� �����մϴ�.
        string path = Path.Combine(Application.dataPath, "DialogueData.json");
        // JSON �����͸� ���Ϸ� ���ϴ�.
        File.WriteAllText(path, json);
        // ���� ���� �Ϸ� �޽����� ����մϴ�.
        Debug.Log($"JSON data saved to {path}");
    }


    // DialogueEntry Ŭ���� ����, JSON���� ����ȭ �����մϴ�.
    [System.Serializable]
    public struct DialogueEntry
    {
        public int Order;
        public string Format;
        public string Speaker;
        public string Contents;
        public string Next;
        public string AddEvent;
    }

    // DialogueData Ŭ���� ����, JSON���� ����ȭ �����մϴ�.
    [System.Serializable]
    public class DialogueData
    {
        public List<DialogueEntry> dialogues;
    }

}
