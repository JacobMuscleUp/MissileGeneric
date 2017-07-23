using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// a class used to manage all audio events taking place in the game
public class ManageAudio : MonoBehaviour {
    public static ManageAudio Ins { get; private set; }

    [System.Serializable]
    public class AudioClipsBundle
    {
        [Tooltip("the name of the set of audio clips")]
        public string name;
        [Tooltip("the set of audio clips")]
        public AudioClip[] clips;
    }

    [Tooltip("An array of variable size, every element of which is a combination of a set of audio clips and its name")]
    public AudioClipsBundle[] audioClipsBundles;

    AudioSource SFXChannel;
    AudioSource BGMChannel;

    Dictionary<string, AudioClip[]> audioClips = new Dictionary<string, AudioClip[]>();

    void Awake()
    {
        Ins = this;
        //if (!InitGlobalIns()) return;
        RegisterAllAudioClipBundles();

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEventSignals.OnMissileExplode += OnMissileExplode;
        GameEventSignals.OnCannonFire += OnCannonFire;
        GameEventSignals.OnCannonFailToFire += OnCannonFailToFire;
    }

    void RegisterAudioClipBundle(string _clipName)
    {
        foreach (AudioClipsBundle elem in audioClipsBundles)
            if (elem.name == _clipName) {
                audioClips[_clipName] = elem.clips;
                return;
            }
        Debug.LogError("A clip named " + _clipName + " is not found");
    }

    void RegisterAllAudioClipBundles()
    {
        foreach (AudioClipsBundle elem in audioClipsBundles)
            audioClips[elem.name] = elem.clips;
    }

    void PlayAudioClip(AudioSource _source, string _clipName, bool _loop = false)
    {
        if (!audioClips.ContainsKey(_clipName))
            RegisterAudioClipBundle(_clipName);
        else {
            _source.clip = audioClips[_clipName][Random.Range(0, audioClips[_clipName].Length)];
            _source.loop = _loop;
            _source.Play();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEventSignals.OnMissileExplode -= OnMissileExplode;
        GameEventSignals.OnCannonFire -= OnCannonFire;
        GameEventSignals.OnCannonFailToFire -= OnCannonFailToFire;
    }

    

    void OnMissileExplode(Missile _missile, Explosion _explosion)
    {
        PlayAudioClip(SFXChannel, "Missile Explode Clip");
    }

    void OnCannonFire(Vector3 _target)
    {
        PlayAudioClip(SFXChannel, "Cannon Fire Clip");
    }

    void OnCannonFailToFire()
    {
        PlayAudioClip(SFXChannel, "Cannon Fail To Fire Clip");
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.buildIndex != 0) {
            SFXChannel = Query.FindInGameObject(gameObject, "SFX Channel").GetComponent<AudioSource>();
            BGMChannel = Query.FindInGameObject(gameObject, "BGM Channel").GetComponent<AudioSource>();
        }
    }

    bool InitGlobalIns()
    {
        if (Ins == null)
            Ins = this;
        else if (Ins != this) {
            DestroyImmediate(gameObject);
            return false;
        }
        DontDestroyOnLoad(gameObject);
        return true;
    }
}
