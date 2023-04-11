using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform target; //현재 배경과 이어지는 배경
    public float scrollAmount; //이어지는 두 배경 사이의 거리
    public float moveSpeed; // 이동속도
    public Vector3 moveDirection; //이동 방향

    private bool alive = true; //스페이스바 재시작

    private GameObject Cat;
    public float Power = 0.5f;//플레이어 움직임

    public bool start = false; //스페이스바 시작

    public bool delay = false;

    public Image Panel;        //화면전환
    private float time = 0f;
    private float F_time = 1f;


    void Start()
    {
        Cat = GameObject.FindGameObjectWithTag("cat");

    }

    void OnCollisionEnter2D(Collision2D collision) //충돌 시 게임 종료
    {
        if (gameObject.CompareTag("cat"))
            if (collision.gameObject.name == "ground")
            {
                alive = false;
                delay = true;
                Time.timeScale = 0;
            }

    }
    void Update()
    {


        if (gameObject.CompareTag("background"))  //배경 움직임
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //배경이 설정된 범위를 벗어나면 위치 재설정
            if (transform.position.x <= -scrollAmount)
            {
                transform.position = target.position - moveDirection * scrollAmount;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().buildIndex == 0)    //스페이스바로 게임 시작
        {
            Invoke("GameStart", 1.25f); //씬전환 디레이
        }

        if (Input.GetKeyDown(KeyCode.Return) && delay == false) //페이드인
        {
            StartCoroutine(FadeIn());
            delay = true;
        }

        if (delay == true && alive == false) //스페이스바로 게임 재시작
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
                Time.timeScale = 1;
                alive = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))   //esc로 시작화면으로 복귀
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
            delay = false;
        }

        if (gameObject.CompareTag("cat")) //방향키로 움직이기
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Cat.GetComponent<Rigidbody2D>().AddForce(new Vector3(-Power, 0, 0), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Cat.GetComponent<Rigidbody2D>().AddForce(new Vector3(Power, 0, 0), ForceMode2D.Impulse);
            }
        }
    }
    void GameStart()
    {
        SceneManager.LoadScene(1);      //시작씬 = 0, 메인씬 = 1
        start = true;
    }
    IEnumerator FadeIn() //페이드인
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        UnityEngine.Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;


        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        yield return null;
    }
}
