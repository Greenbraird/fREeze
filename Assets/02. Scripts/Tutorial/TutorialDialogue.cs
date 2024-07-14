using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    // DialogueData Ŭ������ ������ ������ �����մϴ�.
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

    // DialogueData Ŭ������ ������ ������ �����մϴ�.
    [System.Serializable]
    public class DialogueData
    {
        public List<DialogueEntry> dialogues;
    }

    public TMP_Text speakerText;
    public TMP_Text contentsText;

    int order = 0;

    DialogueData dialogueData;
 

    // ���� �� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        // JSON ���Ͽ��� �����͸� �о�ɴϴ�.
        string jsonFilePath = Path.Combine(Application.dataPath, "DialogueData.json");
        string jsonData = File.ReadAllText(jsonFilePath);

        // JSON �����͸� DialogueData ��ü�� ������ȭ�մϴ�.
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
            print("��ư ������ Dialogue��");
        }
        order++;
        
        if(Dialogue.AddEvent == "END\r")
        {
            order = 0;
        }
    }
}
