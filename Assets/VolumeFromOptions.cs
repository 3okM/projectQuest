using UnityEngine;

public class VolumeFromOptions : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicVolume;
    public static float VolumeData;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    //}

    //void Update()
    //{
        audioSrc.volume = DataHolder.Get();
    }
    //public void SetVolumeFromOptions()
    //{
    //    musicVolume = DataHolder.Get();
    //}
}
