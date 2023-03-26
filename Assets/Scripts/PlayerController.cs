using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// public variables with tooltips
	[Header("Jump variables")]
	[Tooltip("Force with which the player jumps")] public float jumpForce = 700;                  // jump force
	[Tooltip("Force with which the player jumps")] public float downForce = 1000;           // double jump force
	[Tooltip("Gravity multiplier")] public float gravityModifier = 1;                             // gravity modifier
	[Tooltip("Super Jump Force")] public float superJumpForce = 1000;                             // super jump force

	[Header("Particle Systems")]
	[Tooltip("Explosion Particle System for Death")] public ParticleSystem explosionParticle;     // explosion particle system
	[Tooltip("Dirt Particle System for trail")] public ParticleSystem dirtParticle;               // dirt particle system

	[Header("Audio components")]
	[Tooltip("Audio clip for jump")] public AudioClip jumpAudio;                                  // Audio clip for jump
	[Tooltip("Audio clip for crash")] public AudioClip crashAudio;                                // Audio clip for crash

	[HideInInspector]
	public bool gameOver = false;
	[HideInInspector]
	public bool isOnGround = true; 
	[HideInInspector]
	public bool isDashing;
	[HideInInspector]
	public bool gameStarted = false;

	// private variables
	private Animator playerAnim;                                                                  // animator on player
	private Rigidbody rb;                                                                         // rigidbody component
	private AudioSource playerAudio;                                                              // Player Audio Source
	private bool doubleJumpUsed;                                                                  // double jump used
	
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();                                                           // get rigidbody component
		Physics.gravity *= gravityModifier;                                                       // set gravity
		playerAnim = GetComponent<Animator>();                                                    // get animator component on player
		playerAudio = GetComponent<AudioSource>();                                                // get audio source component on player
	}

	// Update is called once per frame
	void Update()
	{
		VerticalMovement();                                                                       // call vertical movement function
		Dash();                                                                                   // call dash function
	}

	private void Dash()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))                                                  // if left shift is pressed
		{
			isDashing = true;                                                                     // set is dashing to true
			playerAnim.SetFloat("Speed_Mul", 2.0f);                                               // set speed multiplier to 2
			// Debug.Log("Dash");
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))                                                    // if left shift is released
		{
			isDashing = false;                                                                    // set is dashing to false
			playerAnim.SetFloat("Speed_Mul", 1.0f);                                               // set speed multiplier to 1
			// Debug.Log("No Dash");
		}
	}

	void VerticalMovement()
	{
		// if space or up arrow is pressed
		if (ActivateJump() && isOnGround && !gameOver)
		{
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);                               // add up force to rigidbody
			isOnGround = false;                                                                   // set is on ground to false
			playerAnim.SetTrigger("Jump_trig");                                                   // set jump trigger
			playerAudio.PlayOneShot(jumpAudio, 1.0f);                                             // play jump audio once
			dirtParticle.Stop();                                                                  // stop dirt particle system
		}
		// double jump condition
		else if (ActivateJump() && !isOnGround && !doubleJumpUsed)
		{
			rb.AddForce(Vector3.up * superJumpForce, ForceMode.Impulse);                          // add up force to rigidbody
			doubleJumpUsed = true;                                                                // set double jump used to true
			playerAnim.Play("Running_Jump", 3, 0f);                                               // play running jump animation
			playerAudio.PlayOneShot(jumpAudio, 1.0f);                                             // play jump audio once
		}

		if (Input.GetKeyDown(KeyCode.DownArrow) && !isOnGround)
		{
			rb.AddForce(Vector3.down * downForce, ForceMode.Impulse);                             // add down force to rigidbody
		}
	}

	private bool ActivateJump()
	{
		return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow));
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))                                            // if collision with ground
		{
			isOnGround = true;                                                                    // set is on ground to true
			doubleJumpUsed = false;                                                               // set double jump used to false
			dirtParticle.Play();                                                                  // play dirt particle system
		}
		else if (collision.gameObject.CompareTag("Obstacle"))                                     // if collision with obstacle
		{
			gameOver = true;                                                                      // set game over to true
			// Debug.Log("Game Over");                                                            // log game over
			playerAnim.SetBool("Death_b", true);                                                  // set death animation
			playerAnim.SetInteger("DeathType_int", 1);                                            // set death type
			explosionParticle.Play();                                                             // play the particle system
			playerAudio.PlayOneShot(crashAudio, 1.0f);                                            // play crash audio once
			dirtParticle.Stop();                                                                  // stop dirt particle system
			playerAudio.PlayOneShot(crashAudio, 1.0f);                                            // play crash audio once
		}
	}
}
