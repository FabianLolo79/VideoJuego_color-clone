using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //SerielizeField nos permite visualizar en el inspector a las variables privadas.
    [SerializeField] private float verticalForce = 400f;
    [SerializeField] private float restartDelay = 1f;

    [SerializeField] private ParticleSystem playerParticles;

    //color del Player
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color violetColor;
    [SerializeField] private Color cyanColor;
    [SerializeField] private Color pinkColor;
    private string currentColor;

    Rigidbody2D playerRb; 
    SpriteRenderer playerSr; // referencia al player

    // Start is called before the first frame update
    void Start()
    {
        /* Se agrega una fuerza al objeto con la función AddForce que requiere un parámetro
         de tipo Vector2 especificando las fuerzas en los ejes X e Y respectivamente. */

        playerRb = GetComponent<Rigidbody2D>();

        //Se cambia la propiedad color del SpriteRenderer.
        playerSr = GetComponent<SpriteRenderer>();

        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerRb.velocity = Vector2.zero; // también es posible playerRb.velocity = new Vector2(0, 0);
            playerRb.AddForce(new Vector2(0, verticalForce));
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión con: " + collision.gameObject.name);
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    } */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ColorChanger"))
        {
            ChangeColor();
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("FinishLine"))
        {
            gameObject.SetActive(false);
            Instantiate(playerParticles, transform.position, Quaternion.identity);
            Invoke("LoadNextScene", restartDelay);
            return;
        }

        //Debug.Log(collision.gameObject.tag);

        if (!collision.gameObject.CompareTag(currentColor))
        {
            //Debug.Log("Los colores no coinciden. Juego terminado!");
            gameObject.SetActive(false);
            Instantiate(playerParticles, transform.position, Quaternion.identity); 
            Invoke("RestartScene", restartDelay);
        }
    }

    void LoadNextScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex + 1);
    }

    void RestartScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    void ChangeColor()
    {
        int randomNumber = Random.Range(0, 4);
        Debug.Log(randomNumber);

        if (randomNumber == 0)
        {
            playerSr.color = yellowColor;
            currentColor = "Yellow";
        }
        else if (randomNumber == 1)
        {
            playerSr.color = violetColor;
            currentColor = "Violet";
        }
        else if (randomNumber == 2)
        {
            playerSr.color = cyanColor;
            currentColor = "Cyan";
        }
        else if (randomNumber == 3)
        {
            playerSr.color = pinkColor;
            currentColor = "Pink";
        }
    }
}
