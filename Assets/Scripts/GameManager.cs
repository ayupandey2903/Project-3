using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	// public variables
	[Header("Starting Lerp")]
	[Tooltip("Position where player starts running")] public float startingPointX = 0;
	[Tooltip("Speed at which player moves in intro")] public float lerpSpeed = 5.0f;

	[HideInInspector]
	public float score;
	private PlayerController playerControllerScript;
	private TextMeshProUGUI scoreText;
	private TextMeshProUGUI gameOverText;

	private Rigidbody playerRB; 

	// test lerp
	[Header(" ")]
	private Vector3 startPos;
	[SerializeField] private Transform endPositionTransform;
	private Vector3 endPos;
	[SerializeField] private float desiredDuration = 3.0f;
	private float elapsedTime = 0.0f;
	private bool isDoneOnce = false;


    private void Awake()
	{
		scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

		gameOverText = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
		gameOverText.gameObject.SetActive(false);

		playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

		playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
	{
		playerControllerScript.gameStarted = false;
		score = 0f;

		// StartCoroutine(PlayIntro());

		startPos = playerControllerScript.transform.position;						// starting position of lerp
		endPos = endPositionTransform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if(!playerControllerScript.gameOver && playerControllerScript.gameStarted)
		{
			if (playerControllerScript.isDashing)
			{
				score += 0.2f;
			}
			else
			{
				score += 0.1f;
			}
			scoreText.text = "Score: " + Mathf.Round(score);
		}
		else if (playerControllerScript.gameOver && playerControllerScript.gameStarted)
		{
			// deactivate score text
			scoreText.gameObject.SetActive(false);
			gameOverText.gameObject.SetActive(true);
			gameOverText.text = "Game Over!\nFinal Score: " + Mathf.Round(score);
			// Debug.Log("Game Over, Final Score: " + score);
		}

		if (!isDoneOnce)
		{
			playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Mul", 0.5f);        // set player animation speed to half
			isDoneOnce = true;
		}

		// test lerp
		if (!playerControllerScript.gameStarted)
			PlayIntro();

	}

	void PlayIntro()
	{
		elapsedTime += Time.deltaTime;
		float percentageComplete = elapsedTime / desiredDuration;

		playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos, percentageComplete);

        if (percentageComplete >= 1.0f)
        {
            playerControllerScript.gameStarted = true;
			playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Mul", 1.0f);

			playerRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

	/* private IEnumerator PlayIntro()
	{
		Vector3 startPos = playerControllerScript.transform.position;						// starting position of lerp
		Vector3 endPos = new(startingPointX, startPos.y, startPos.z);						// ending position of lerp

		float journeyDistance = Vector3.Distance(startPos, endPos);							// distance between starting and ending position
		float startTime = Time.time;                                                        // time at start of lerp

		float distanceCovered = (Time.time - startTime) * lerpSpeed;                        // distance covered by lerp
		float fractionOfJourney = distanceCovered / journeyDistance;                        // fraction of journey covered

		playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Mul", 0.5f);		// set player animation speed to half

		while (fractionOfJourney > 0)
		{
			distanceCovered = (Time.time - startTime ) * lerpSpeed;                          // distance covered by lerp
			fractionOfJourney = distanceCovered / journeyDistance;                           // fraction of journey covered

			playerControllerScript.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney); // lerp player position
			
			yield return null;
		}

		playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Mul", 1.0f);

		playerControllerScript.gameStarted = true;
	}*/
}
