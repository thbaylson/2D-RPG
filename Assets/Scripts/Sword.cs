using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;

    private void Awake()
    {
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
