using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;
    public float leftBound = -8f;

    private PlayerController playerControllerScript;
    private float currentSpeed;
    private float superSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // get player controller script
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        superSpeed = speed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.isDashing)
        {
            currentSpeed = superSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
        if (!playerControllerScript.gameOver && playerControllerScript.gameStarted)
        {
            transform.Translate(Time.deltaTime * currentSpeed * Vector3.left);
        }
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
