using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float movespeed = -0.5f;
    private GameObject currentFloor;
    [SerializeField] int hp = 10;
    [SerializeField] GameObject HpBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject replayButton;

    private int score;
    private float scoreTime;
    private Animator anim;
    private SpriteRenderer render;
    private AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-movespeed * Time.deltaTime, 0, 0);
            render.flipX = true;
            anim.SetBool("run",true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(movespeed * Time.deltaTime, 0, 0);
            render.flipX = false;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, movespeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -movespeed * Time.deltaTime, 0);
        }
        else
        {
            anim.SetBool("run",false);
        }
        updateScore();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Normal_F")
        {
            if (col.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("撞到了第一个阶梯");
                currentFloor = col.gameObject;
                ModifyHp(1);
                col.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (col.gameObject.tag == "Naills_F")
        {
            if (col.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("第二个阶梯被撞到了");
                currentFloor = col.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                col.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (col.gameObject.tag == "TopFloor")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            anim.SetTrigger("hurt");
            col.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeadLine")
        {
            Debug.Log("游戏结束");
            die();
        }
    }

    void ModifyHp(int num)
    {
        hp += num;
        if (hp > 10)
        {
            hp = 10;
        }
        else if (hp < 0)
        {
            hp = 0;
            die();
        }
        updateHpBar();
    }

    void updateHpBar()
    {
        for (int i = 0; i < HpBar.transform.childCount; i++)
        {
            if (hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);

            }
        }
    }

    void updateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "层";
        }
    }

    void die()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}