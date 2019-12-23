using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject {

    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 0.5f;

    [Range(0, 2f)]
    public float pitch = 1f;
}
