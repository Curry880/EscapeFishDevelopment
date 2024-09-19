using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private float normalizedTime;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        normalizedTime = Random.Range(0f, 1f);
        animator.Play("Seagull", 0, normalizedTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
