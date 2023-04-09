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
    public Transform target; //���� ���� �̾����� ���
    public float scrollAmount; //�̾����� �� ��� ������ �Ÿ�
    public float moveSpeed; // �̵��ӵ�
    public Vector3 moveDirection; // �̵� ����

    private bool alive = true; // �����̽��� �����

    private GameObject Cat;
    public float Power = 0.5f;// �÷��̾� ������(AddForce)

    public bool start = false; // �����̽��� ������ ����

    private bool delay = false; //�����̽��ٸ� ������ �� ó�� ���۰� ������� �����ִ� ����(ó�� ������ ���� �νĵǸ� ���̵�ƿ� ȭ����� �ٷ� �Ѿ)

    public Image Panel;        // ���̵�ƿ�
    private float time = 0f;
    private float F_time = 1f;



    IEnumerator FadeOut() //���̵�ƿ� �ڷ�ƾ
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
        Cat = GameObject.FindGameObjectWithTag("Cat"); //�÷��̾�

    }

    void OnCollisionEnter2D(Collision2D collision) //�浹 �� ���� ����
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
        

        if (gameObject.CompareTag("BackGround"))  //��� ������
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            
            if (transform.position.x <= -scrollAmount)  //����� ������ ������ ����� ��ġ �缳��
            {
                transform.position = target.position - moveDirection * scrollAmount;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && delay == false) //���̵�ƿ� �ڷ�ƾ
        {
            StartCoroutine(FadeOut());
            delay = true;
        }

            if (Input.GetKeyDown(KeyCode.Space) && SceneManager.GetActiveScene().buildIndex == 0)    //�����̽��ٷ� ���� ����
        {
            Invoke("GameStart", 1f); //����ȯ ������
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

        if (gameObject.CompareTag("Cat")) //����Ű�� �����̱�
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

    
}
