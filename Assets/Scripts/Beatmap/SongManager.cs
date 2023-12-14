using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SongManager : MonoBehaviour
{

    //private GameObject[] sourceObjects;
    public static SongManager Instance;
    protected Dictionary<string, AudioSource> songDict;
    protected Dictionary<string, float> songdDictPitch;
    protected Dictionary<string, float> songVoldict;
    protected Dictionary<string, float> songFadingDict;
    float oldTimeScale;

    private void Awake() {
        if (Instance==null) {
            Instance = this;
        }
    }

    protected void initialize() {
        oldTimeScale = 1f;
        //DontDestroyOnLoad(gameObject);
        songDict = new Dictionary<string, AudioSource>();
        songdDictPitch = new Dictionary<string, float>();
        songVoldict = new Dictionary<string, float>();
        songFadingDict = new Dictionary<string, float>();
        foreach (Transform g in transform) {
            foreach (AudioSource source in g.gameObject.GetComponents<AudioSource>()) {
                if (source.clip!=null) {
                    //Debug.Log(source.clip.name);
                    songDict.Add(source.clip.name, source);
                    songdDictPitch.Add(source.clip.name, source.pitch);
                    songVoldict.Add(source.clip.name, source.volume);
                    songFadingDict.Add(source.clip.name, 0f);
                }
            }
            
        }
    }

    void Start()
    {
        initialize();
    }

    void Update()
    {
        if (oldTimeScale != Time.timeScale && Time.timeScale != 0) {
            oldTimeScale = Time.timeScale;
            foreach (KeyValuePair<string,AudioSource> pair in songDict) {
                pair.Value.pitch = songdDictPitch[pair.Key] * Time.timeScale;
            }
            
        }
    }

    public bool isSongFading(string s) {
        return !songFadingDict.ContainsKey(s) || songFadingDict[s]!=0f;
    }

    public void resetSongVolumes() {
        foreach (KeyValuePair<string,AudioSource> pair in songDict) {
            pair.Value.volume = songVoldict[pair.Key];
        }
    }


    public AudioSource getSong(string soundName) {
        return songDict[soundName];
    }

    void stopAllOthers(string[] sounds){

        foreach(KeyValuePair<string, AudioSource> entry in songDict) {
            if (!contains(sounds,entry.Key)) {
                entry.Value.Stop();
            }
        }

    }

    bool contains(string[] sounds,string s) {
        foreach(string soundName in sounds){
            if (soundName == s) {
                return true;
            }
        }
        return false;
    }
    public void playSong(string soundName, bool fade = false, float fadeTime = 1f) {
        Debug.Log(soundName);
        if (songDict.ContainsKey(soundName)) {
            if (fade) {
                Timing.RunCoroutine(FadeIn(soundName, songDict[soundName],fadeTime).CancelWith(this.gameObject),this.gameObject.GetInstanceID());
            } else {
                songDict[soundName].Play();
            }
        }
    }

    public bool isSongPlaying(string s) {
        return !songDict.ContainsKey(s) || songDict[s].isPlaying;
    }
    public List<string> getCurrPlayingSongs() {
        List<string> listy = new List<string>();
        foreach(KeyValuePair<string, AudioSource> entry in songDict) {
            if (entry.Value.isPlaying) {
                listy.Add(entry.Key);
            }
        }
        return listy;
    }

    public bool isAnySongPlaying() {
        foreach(KeyValuePair<string, AudioSource> entry in songDict) {
            if (entry.Value.isPlaying) {
                return true;
            }
        }
        return false;
    }

    public void playSongs(List<string> sounds, bool fade = false, float fadeTime = 1f) {
        string[] newArray = sounds.ToArray();
        playSongs(newArray,fade,fadeTime );
    }

    public void playSongs(string[] sounds, bool fade = false, float fadeTime = 1f) {
        bool aSongPlaying = false;
        foreach(string soundName in sounds){
            if (songDict.ContainsKey(soundName) && songDict[soundName].isPlaying) {
                aSongPlaying = true;
            }
        }
        if (aSongPlaying) {
            return;
        }
        stopAllOthers(sounds);
        int cnt = 0;
        foreach(string soundName in sounds){
           
            if (songDict.ContainsKey(soundName) && !songDict[soundName].isPlaying) {
                if (fade) {
                    Timing.RunCoroutine(FadeIn(soundName, songDict[soundName],fadeTime).CancelWith(this.gameObject),this.gameObject.GetInstanceID());
                } else {
                    
                    songDict[soundName].Play();
                    
                }

            }
            cnt++;
        }
    }

    public void stopSong(string soundName, bool fade = false, float fadeTime = 1f) {
        Debug.Log(soundName);
        if (songDict.ContainsKey(soundName) && songDict[soundName].isPlaying) {
             Debug.Log("has sound");
            if (fade) {
                Timing.RunCoroutine(FadeOut(soundName, songDict[soundName],fadeTime).CancelWith(this.gameObject),this.gameObject.GetInstanceID());
            } else {
                songDict[soundName].Stop();
            }
        }
    }

    public void continueSongFade(string soundName,bool fadeOut,float fadeTime) {
        if (fadeOut) {
            Timing.RunCoroutine(ContinueFadeOut(soundName, songDict[soundName],fadeTime).CancelWith(this.gameObject),this.gameObject.GetInstanceID());
        } else {
            Timing.RunCoroutine(ContinueFadeIn(soundName, songDict[soundName],fadeTime).CancelWith(this.gameObject),this.gameObject.GetInstanceID());
        }

    }

    public void killAllCo() {
        Timing.KillCoroutines(this.gameObject.GetInstanceID());
    }
        
    public IEnumerator<float> FadeOut(string soundName, AudioSource audioSource, float FadeTime) {
        
        float startVolume = songVoldict[soundName];
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            songFadingDict[soundName] =  audioSource.volume;
            yield return Timing.WaitForOneFrame;;
        }
        audioSource.Stop ();
        songFadingDict[soundName] = 0f;
        audioSource.volume = songVoldict[soundName];
    }


    public static IEnumerator<float> TapeStop(AudioSource audioSource, float FadeTime) {
        
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return Timing.WaitForOneFrame;
        }
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }

    public void stopAll(){
        foreach(KeyValuePair<string, AudioSource> entry in songDict) {
            entry.Value.Stop();
        }
        Timing.KillCoroutines(this.gameObject.GetInstanceID());
        resetSongVolumes();
    }

    public IEnumerator<float> FadeIn(string soundName, AudioSource audioSource, float FadeTime) {
        
        yield return Timing.WaitForSeconds(1);
        float startVolume = songVoldict[soundName];
        audioSource.volume=0;
        audioSource.Play(0);
        while (audioSource.volume < startVolume) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            songFadingDict[soundName] = audioSource.volume;
            yield return Timing.WaitForOneFrame;
        }
        //stopAllOthers(songName);
        songFadingDict[soundName] = 0f;
        audioSource.volume = songVoldict[soundName];
    }

    public IEnumerator<float> ContinueFadeOut(string soundName, AudioSource audioSource, float FadeTime) {
        
        float startVolume =songFadingDict[soundName];
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            songFadingDict[soundName] =  audioSource.volume;
            yield return Timing.WaitForOneFrame;
        }
        audioSource.Stop ();
        songFadingDict[soundName] = 0f;
        audioSource.volume = songVoldict[soundName];
    }

    public IEnumerator<float> ContinueFadeIn(string soundName, AudioSource audioSource, float FadeTime) {
        
        yield return Timing.WaitForSeconds(1);
        float startVolume = songVoldict[soundName];
        audioSource.volume=songFadingDict[soundName];
        if (!audioSource.isPlaying) {
            audioSource.Play(0);
        }
        while (audioSource.volume < startVolume) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            songFadingDict[soundName] = audioSource.volume;
            yield return Timing.WaitForOneFrame;
        }
        //stopAllOthers(songName);
        songFadingDict[soundName] = 0f;
        audioSource.volume = songVoldict[soundName];
    }
}
