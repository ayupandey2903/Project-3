using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    [Tooltip("Additional gravity multiplier for snappy falls")] public float fallMultiplier = 2.5f;
    // [Tooltip("Additional gravity multiplier for short jumps")] public float lowJumpMultiplier = 2f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += (fallMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
        // else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        // {
        //     rb.velocity += (lowJumpMultiplier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        // }
    }
}
