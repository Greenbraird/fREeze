using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CharacterFootSoundPlayer : MonoBehaviour
{
    int step;

    private void Start()
    {
        if (gameObject.name == "Foot_02.L_end") { step = 0; }
        else { step = 1; }
    }

    private void OnTriggerEnter(Collider other)
    {

        AudioManager.Instance.footSFXPlay(gameObject, step);
    }
}
