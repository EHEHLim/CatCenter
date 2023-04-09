using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CurrentScore : MonoBehaviour
{
    private float score = 1;
    private int scorespeed = 10;
    public Text text_scoreUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        score += Time.deltaTime * scorespeed;
        text_scoreUI.text = "Score : " + (int)score;

    }
}
