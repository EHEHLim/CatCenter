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
    public Transform target; //���� ���� �̾����� ���
    public float scrollAmount; //�̾����� �� ��� ������ �Ÿ�
    public float moveSpeed; // �̵��ӵ�
    public Vector3 moveDirection; //�̵� ����

    private bool alive = true; //�����̽��� �����

    private GameObject Cat;
    public float Power = 0.5f;//�÷��̾� ������

    public bool start = false; //�����̽��� ����

    public bool delay = false;

    public Image Panel;        //ȭ����ȯ
    private float time = 0f;
    private float F_time = 1f;


    void Start()
    {
        Cat = GameObject.FindGameObjectWithTag("cat");

    }

    void OnCollisionEnter2D(Collision2D collision) //�浹 �� ���� ����
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


        if (gameObject.CompareTag("background"))  //��� ������
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //����� ������ ������ ����� ��ġ �缳��
            if (transform.position.x <= -scrollAmount)
            {
                transform.position = target.position - moveDirection * scrollAmount;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().buildIndex == 0)    //�����̽��ٷ� ���� ����
        {
            Invoke("GameStart", 1.25f); //����ȯ ����
        }

        if (Input.GetKeyDown(KeyCode.Return) && delay == false) //���̵���
        {
            StartCoroutine(FadeIn());
            delay = true;
        }

        if (delay == true && alive == false) //�����̽��ٷ� ���� �����
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
                Time.timeScale = 1;
                alive = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))   //esc�� ����ȭ������ ����
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
            delay = false;
        }

        if (gameObject.CompareTag("cat")) //����Ű�� �����̱�
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
        SceneManager.LoadScene(1);      //���۾� = 0, ���ξ� = 1
        start = true;
    }
    IEnumerator FadeIn() //���̵���
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
