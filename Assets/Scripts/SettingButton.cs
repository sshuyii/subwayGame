using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public Sprite music;

    public Sprite sound;

    public Sprite realtime;

    public Sprite music_no;
    public Sprite sound_no;
    public Sprite realtime_no;

    public Image musicImage;
    public Image soundImage;
    public Image realtimeImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickMusic()
    {
        if(musicImage.sprite != music)
        {
            musicImage.sprite = music;
        }
        else
        {
            musicImage.sprite = music_no;
        }
    }
    
    public void clickSound()
    {
        if(soundImage.sprite != sound)
        {
            soundImage.sprite = sound;
        }
        else
        {
            soundImage.sprite = sound_no;
        }
    }
    
    public void clickRealtime()
    {
        if(realtimeImage.sprite != realtime)
        {
            realtimeImage.sprite = realtime;
        }
        else
        {
            realtimeImage.sprite = realtime_no;
        }
    }
}
