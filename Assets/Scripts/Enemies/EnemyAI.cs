using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float roamChangeDirectionCooldown = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private bool canAttack = true;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    // Start is called before the first frame update
    void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    // TODO: This is changing more than just movement state. Rename to something like StateControl.
    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            //default:
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange)
        {
            state = State.Attacking;
        }

        if(timeRoaming > roamChangeDirectionCooldown)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        // TODO: Repeating code from Roaming
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;

            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                // TODO: Why are we calling this here and in Roaming()? Can't we just set the state back to State.Roaming?
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        // TODO: This make me want to puke because the method is officially doing too much.
            // Put the timer reset functionality into its own method.
        timeRoaming = 0f;

        // Normalized to make sure the slime isn't moving faster on diagonals
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
