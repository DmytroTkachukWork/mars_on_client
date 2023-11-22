using System.Collections;
using UnityEngine;
using TMPro;

public class ScreenDebugUI : MonoBehaviourBase
{
    [SerializeField] private TMP_Text fps_text = null;
    private IEnumerator fps_counter_cor = null;
    private int frames_delay = 60;
    private float curent_fps = 0;
    void Start()
    {
      fps_counter_cor = impl();
      fps_counter_cor.start();

      IEnumerator impl()
      {
        while ( true )
        {
          for( int i = 0; i < frames_delay; i++ )
            yield return null;

          curent_fps = 1 / Time.deltaTime;
          fps_text.text = "FPS: " + Mathf.RoundToInt( curent_fps );
        }
      }
    }

    
}
