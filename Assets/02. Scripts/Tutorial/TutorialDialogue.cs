using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    // DialogueData 클래스와 동일한 구조로 정의합니다.
    [System.Serializable]
    public class DialogueEntry
    {
        public int Order;
        public string Format;
        public string Speaker;
        public string Contents;
        public string Next;
        public string AddEvent;
    }

    // DialogueData 클래스와 동일한 구조로 정의합니다.
    [System.Serializable]
    public class DialogueData
    {
        public List<DialogueEntry> dialogues;
    }

    public TMP_Text speakerText;
    public TMP_Text contentsText;

    int order = 0;

    DialogueData dialogueData;
 

    // 시작 시 호출되는 함수입니다.
    void Start()
    {
        // JSON 파일에서 데이터를 읽어옵니다.
        string jsonFilePath = Path.Combine(Application.dataPath, "DialogueData.json");
        string jsonData = File.ReadAllText(jsonFilePath);

        // JSON 데이터를 DialogueData 객체로 역직렬화합니다.
        dialogueData = JsonUtility.FromJson<DialogueData>(jsonData);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        DialogueEntry Dialogue = dialogueData.dialogues[order];

        if (Dialogue.Order == order)
        {
            speakerText.text = Dialogue.Speaker;
            contentsText.text = Dialogue.Contents;
        }
        else
        {
            print("버튼 형식의 Dialogue임");
        }
        order++;
        
        if(Dialogue.AddEvent == "END\r")
        {
            order = 0;
        }
    }
}
