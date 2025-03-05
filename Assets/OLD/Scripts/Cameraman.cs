using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CamUpdate(Vector3 speed, bool isRunning, bool isJumping)
    {
        animator.SetFloat("Velocity", Mathf.Abs(speed.x) + Mathf.Abs(speed.y) + Mathf.Abs(speed.z));
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }
}
