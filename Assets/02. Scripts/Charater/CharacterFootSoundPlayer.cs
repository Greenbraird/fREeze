using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CharacterFootSoundPlayer : MonoBehaviour
{
    int step;
    void Start()
    {
        step = 0;    
    }
    public void playStepSound()
    {
        if (step == 3) { step = 0; }

        AudioManager.Instance.footSFXPlay(gameObject, step);

        step++;
    }
}
