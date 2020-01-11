using UnityEngine;

public class VolumeValueChange : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicVolume = 1.0f;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSrc.volume = musicVolume;
    }
    public void SetVolume(float vol){
        musicVolume = vol;
        DataHolder.Set(vol);
        Debug.Log("SetVolume" + vol);
    }
}
