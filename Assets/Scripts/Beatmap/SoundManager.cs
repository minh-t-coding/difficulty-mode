using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;
    protected Dictionary<string, AudioSource> soundDict;
    protected Dictionary<string, float> soundDictPicth;
    float oldTimeScale;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        oldTimeScale = 1f;
        soundDict = new Dictionary<string, AudioSource>();
        soundDictPicth = new Dictionary<string, float>();
        foreach (Transform g in transform) {
            foreach (AudioSource source in g.GetComponents<AudioSource>()) {
                if (source.clip != null) {
                    //Debug.Log(source.clip.name);
                    soundDict.Add(source.clip.name, source);
                    soundDictPicth.Add(source.clip.name, source.pitch);
                }
            }

        }
    }

    // Update is called once per frame
    void Update() {
        if (oldTimeScale != Time.timeScale && Time.timeScale != 0) {
            oldTimeScale = Time.timeScale;
            foreach (KeyValuePair<string, AudioSource> pair in soundDict) {
                pair.Value.pitch = soundDictPicth[pair.Key] * Time.timeScale;
            }

        }
    }

    public AudioSource getSound(string soundName) {
        return soundDict[soundName];
    }

    // Play sound by calling SoundManager.Instance.playSound(clipName);
    public void playSound(string soundName) {
        if (soundDict.ContainsKey(soundName)) {
            soundDict[soundName].PlayOneShot(soundDict[soundName].clip, 1f);

            //soundDict[soundName].Play();
        }
    }

    public void playSoundNotOneShot(string soundName) {


        if (soundDict.ContainsKey(soundName)) {
            soundDict[soundName].Play();
            //soundDict[soundName].Play();
        }
    }

}
