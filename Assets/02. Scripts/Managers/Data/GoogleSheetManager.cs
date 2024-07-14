using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    // 구글 스프레드시트의 URL, TSV 형식으로 가져옵니다.
    const string URL = "https://docs.google.com/spreadsheets/d/1W_Jd2Lk4EETKxpwX5zKmS4Ip2JBg_UeV7QGebEMfKwM/export?format=tsv";

    // Unity에서 시작될 때 코루틴으로 구글 스프레드시트를 가져옵니다.
    IEnumerator Start()
    {
        // 구글 스프레드시트 데이터를 가져오기 위한 요청을 생성합니다.
        UnityWebRequest www = UnityWebRequest.Get(URL);
        // 요청을 보내고 응답을 기다립니다.
        yield return www.SendWebRequest();

        // 요청이 실패하면 에러를 출력하고 종료합니다.
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }

        // TSV 데이터를 문자열로 가져옵니다.
        string tsvData = www.downloadHandler.text;
        // TSV 데이터를 파싱하여 DialogueEntry 리스트로 변환합니다.
        List<DialogueEntry> dialogueEntries = ParseTSV(tsvData);
        // DialogueEntry 리스트를 JSON 형식으로 변환합니다.
        string json = JsonUtility.ToJson(new DialogueData { dialogues = dialogueEntries }, true);
        // JSON 데이터를 파일로 저장합니다.
        SaveJsonToFile(json);
    }

    /// <summary>
    /// TSV 데이터를 파싱하여 DialogueEntry 리스트로 변환합니다.
    /// </summary>
    /// <param name="tsvData">TSV 형식의 문자열 데이터</param>
    /// <returns>DialogueEntry 객체의 리스트</returns>
    List<DialogueEntry> ParseTSV(string tsvData)
    {
        // DialogueEntry 객체의 리스트를 생성합니다.
        List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();
        // TSV 데이터를 줄 단위로 분리합니다.
        string[] lines = tsvData.Split('\n');

        // 첫 번째 줄은 헤더이므로 건너뜁니다.
        for (int i = 1; i < lines.Length; i++) // Start from 1 to skip header
        {
            // 각 줄을 탭으로 분리하여 열 데이터를 가져옵니다.
            string[] columns = lines[i].Split('\t');
            // 열의 개수가 부족하면 건너뜁니다.
            if (columns.Length < 6) continue;

            // 각 열 데이터를 파싱하여 DialogueEntry 객체를 생성합니다.
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
                // 생성된 DialogueEntry 객체를 리스트에 추가합니다.
                dialogueEntries.Add(entry);
            }
            else
            {
                // 순서 열의 형식이 잘못된 경우 경고 메시지를 출력합니다.
                Debug.LogWarning($"Invalid format for 순서 at line {i + 1}: {columns[0]}");
                continue;
            }
        }
        return dialogueEntries;
    }

    /// <summary>
    /// JSON 데이터를 파일로 저장합니다.
    /// </summary>
    /// <param name="json">저장할 JSON 데이터</param>
    void SaveJsonToFile(string json)
    {
        // JSON 데이터를 저장할 경로를 설정합니다.
        string path = Path.Combine(Application.dataPath, "DialogueData.json");
        // JSON 데이터를 파일로 씁니다.
        File.WriteAllText(path, json);
        // 파일 저장 완료 메시지를 출력합니다.
        Debug.Log($"JSON data saved to {path}");
    }


    // DialogueEntry 클래스 정의, JSON으로 직렬화 가능합니다.
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

    // DialogueData 클래스 정의, JSON으로 직렬화 가능합니다.
    [System.Serializable]
    public class DialogueData
    {
        public List<DialogueEntry> dialogues;
    }

}
