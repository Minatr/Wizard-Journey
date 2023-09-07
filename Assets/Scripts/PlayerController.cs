using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("REFERENCES")]

    [SerializeField]
    private LayerMask terrainLayer;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private FloatingJoystick joystick;

    [SerializeField]
    private Animator animator;


    [Header("STATS")]

    [SerializeField]
    private float speed;

    [SerializeField]
    private float groundDist;


    void Update()
    {
        // On positionne le joueur au dessus du sol
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos= transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        // Déplacement du joueur
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(x * speed), Mathf.Abs(z * speed)));

        rb.velocity = new Vector3(x * speed, rb.velocity.y, z * speed);

        if (x != 0 && x < 0)
        {
            sr.flipX = true;
            animator.SetBool("speedRight", false);
            animator.SetBool("speedLeft", true);
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
            animator.SetBool("speedRight", true);
            animator.SetBool("speedLeft", false);
        }
        else if (z != 0)
        {
            if (!sr.flipX)
            {
                animator.SetBool("speedRight", true);
                animator.SetBool("speedLeft", false);
            } else
            {
                animator.SetBool("speedRight", false);
                animator.SetBool("speedLeft", true);
            }
        }
        else
        {
            animator.SetBool("speedRight", false);
            animator.SetBool("speedLeft", false);
        }
    }
}
