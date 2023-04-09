using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform target; //현재 배경과 이어지는 배경
    public float scrollAmount; //이어지는 두 배경 사이의 거리
    public float moveSpeed; // 이동속도
    public Vector3 moveDirection; // 이동 방향

    private bool alive = true; // 스페이스바 재시작

    private GameObject Cat;
    public float Power = 0.5f;// 플레이어 움직임(AddForce)

    public bool start = false; // 스페이스바 누르면 시작

    private bool delay = false; //스페이스바를 눌렀을 때 처음 시작과 재시작을 나눠주는 변수(처음 씬에서 같이 인식되면 페이드아웃 화면없이 바로 넘어감)

    public Image Panel;        // 페이드아웃
    private float time = 0f;
    private float F_time = 1f;

    IEnumerator FadeOut() //페이드아웃 코루틴
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
        yield return null;
        Panel.gameObject.SetActive(false);
    }

    void Start()
    {
        Cat = GameObject.FindGameObjectWithTag("Cat"); //플레이어

    }

    void OnCollisionEnter2D(Collision2D collision) //충돌 시 게임 종료
    {
        if(gameObject.CompareTag("Cat"))
            if (collision.gameObject.name == "Ground")      
            {
               alive = false;
               Time.timeScale = 0;
            }

    }

    // Update is called once per frame
    void Update()
    {
        

        if (gameObject.CompareTag("BackGround"))  //배경 움직임
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            
            if (transform.position.x <= -scrollAmount)  //배경이 설정된 범위를 벗어나면 위치 재설정
            {
                transform.position = target.position - moveDirection * scrollAmount;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && delay == false) //페이드아웃 코루틴
        {
            StartCoroutine(FadeOut());
            delay = true;
        }

            if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 0)    //스페이스바로 게임 시작
        {
            Invoke("GameStart", 1f); //씬전환 딜레이
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

        if (gameObject.CompareTag("Cat")) //방향키로 움직이기
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

    
}
