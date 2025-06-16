using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;
    public float downForse = 1;
    public float moveSpeed = 5;
    public float runSpeed = 7;
    public float jumpForce = 10;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isFacingRight = true;
    public Animator animator;
    public LayerMask platformsLayer;
    public TilemapCollider2D tc;
    public Transform platformCheck;
    private bool jumped;
    public TextMeshProUGUI livesText;
    public int maxItems = 3;
    private int collectedItems = 0;
    public float dropDownDuration = 0.5f;
    public TextMeshProUGUI itemsText;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentLives = maxLives;
        UpdateLivesText();
    }

    [System.Obsolete]
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");


        animator.SetFloat("Move", Mathf.Abs(moveInput));




        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = new Vector2(moveInput * runSpeed, rb.velocity.y);


        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        }



        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || IsPlatform()))
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("jump", true);
            jumped = true;
            tc.enabled = false;


        }
        else
        {
            animator.SetBool("jump", false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (IsPlatform())
            {
                StartCoroutine(DropDown());
            }
        }





        if (jumped)
        {
            if (rb.velocity.y <= 0f)
            {
                tc.enabled = true;
                jumped = false;
                animator.SetBool("jump", false);
            }
        }



        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }

    }



    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("lives: " + currentLives);
        UpdateLivesText();
        if (currentLives <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("game_over");

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
    void UpdateLivesText()
    {
        string hearts = "";
        for (int i = 0; i < currentLives; i++)
        {
            hearts += "♥ ";
        }
        for (int i = currentLives; i < maxLives; i++)
        {
            hearts += "♡ ";
        }

        livesText.text = hearts;
    }

    





    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("items"))
        {
            collectedItems++;
            Debug.Log("items: " + collectedItems);
            Destroy(collision.gameObject);
            itemsText.text = "items colect:" + collectedItems + "/" + maxItems;

            if (collectedItems >= maxItems)
            {
                Debug.Log("win");
                SceneManager.LoadScene("game_over");
            }
        }
    }
    private IEnumerator DropDown()
    {
        
        tc.enabled = false;

       
        yield return new WaitForSeconds(dropDownDuration);

        
        tc.enabled = true;
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    }

    private bool IsPlatform()
    {
        return Physics2D.OverlapCircle(platformCheck.position, 0.2f, platformsLayer);

    }
}

