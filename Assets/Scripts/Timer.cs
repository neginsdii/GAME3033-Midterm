using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    public Image Fill;
    public float gameTime;
    private float timer;



    void Start()
    {

        timer = gameTime;
        Fill.fillAmount = Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        
            timer -= Time.deltaTime;
            if (timer <= 0)
            {

                timer = 0;
            SceneManager.LoadScene("GameOver");
              
            }
            else
            {
                Fill.fillAmount = Normalize();
            }
      
    }

    private float Normalize()
    {
        return (float)timer / gameTime;
    }
}
