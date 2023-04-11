using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score=1;
    public int scorespeed = 10;
    public Text text_scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime*scorespeed;
        text_scoreUI.text = "현재점수 : " + (int)score;
    }
}
