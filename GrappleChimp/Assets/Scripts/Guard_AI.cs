using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Guard_AI : MonoBehaviour {

    public bool randomPatrol;
    public bool strictPatrol;
    public bool randPatrolTime;

    public Transform[] patrolPoints;
    private NavMeshAgent enemyAgent;
    private GameObject playerLoc;
    private PlayerController playerController;
    private float followDist;
    public float regularDist;
    public float sneakDist;

    private int currentPatrolPoint = 0;
    public float patrolRadius = 30f;
    public float patrolTimerStart;
    private float patrolTimer;
    private float timerCount;
    public float attackTimer;

    public bool follow;
    public bool patroling;
    public bool attack;
    //public bool retreat;

    //public GameObject currentTarget;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();

        playerLoc = GameObject.FindGameObjectWithTag("Player");

        if (!randomPatrol && strictPatrol && patroling)
            SetPatrolPoint();
        else if (!randomPatrol && !strictPatrol && patroling)
            SetRandomPatrolPoint();

    }

    // Use this for initialization
    void Start ()
    {
        playerController = playerLoc.GetComponent<PlayerController>();
        patrolTimer = patrolTimerStart;
        timerCount = patrolTimer;
        patroling = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        attackTimer -= 0.1f;
        float playerDist = Vector3.Distance(playerLoc.transform.position, this.transform.position);

        if (!follow)
            enemyAgent.stoppingDistance = 0;

        if(playerController.sneaking)
        {
            followDist = sneakDist;
        }
        else
        {
            followDist = regularDist;
        }

        if (playerDist < followDist)
        {
            PursuePlayer();
            patroling = false;
            follow = true;
            //Debug.Log("Chasing!");
        }
        else if (playerDist > followDist)
        {
            patroling = true;
            follow = false;
        }

        //enemyAgent.SetDestination(currentTarget.transform.position);
        if(patroling)
        timerCount += Time.deltaTime;

        if (timerCount > patrolTimer)
        {
            if (randomPatrol && patroling)
            {
                NextPatrolPoint();
            }
            else if(!randomPatrol && strictPatrol && patroling)
            {
                if(!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.5f)
                SetPatrolPoint();
            }
            else if(!randomPatrol && !strictPatrol && patroling)
            {
                if (!enemyAgent.pathPending && enemyAgent.remainingDistance < 0.5f)
                    SetRandomPatrolPoint();
            }

            if (randPatrolTime)
            {
                patrolTimer = Random.Range(patrolTimerStart / 2, patrolTimerStart);
            }
            else
            {
                patrolTimer = patrolTimerStart;
            }
            timerCount = 0f;
        }

	}

    void NextPatrolPoint()
    {
        Vector3 newPatrolPoint;

        newPatrolPoint = RandomNavSphere(transform.position, patrolRadius, -1);
        enemyAgent.SetDestination(newPatrolPoint);
        
        //else if(!randomPatrol)
        //{
        //    newPatrolPoint = SetPatrolPoint();
        //    enemyAgent.SetDestination(newPatrolPoint);
        //}
    }

    Vector3 RandomNavSphere(Vector3 originPos, float radiusMult, int layerMask)
    {
        Vector3 randDir = Random.insideUnitSphere * radiusMult;
        randDir += originPos;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, radiusMult, layerMask);

        return navHit.position;
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Touching!");
        if(other.CompareTag("Player"))
        {
            if(attackTimer <= 0)
            {
                attack = true;
                attackTimer = 10.0f;
            }
            else
            {
                attack = false;
            }
            if(attack)
            {
                playerController.health -= 1;
            }
        }
    }


    void SetPatrolPoint()
    {
        if(patrolPoints.Length > 0)
        {
            enemyAgent.destination = patrolPoints[currentPatrolPoint].position;
            currentPatrolPoint++;
            currentPatrolPoint %= patrolPoints.Length;

            if (randPatrolTime)
            {
                patrolTimer = Random.Range(patrolTimerStart / 2, patrolTimerStart);
            }
            else
            {
                patrolTimer = patrolTimerStart;
            }
            timerCount = 0f;
            //Debug.Log("Move Position!");
        }
    }

    void SetRandomPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            enemyAgent.destination = patrolPoints[currentPatrolPoint].position;
            currentPatrolPoint = Random.Range(0, patrolPoints.Length);
            currentPatrolPoint %= patrolPoints.Length;

            if (randPatrolTime)
            {
                patrolTimer = Random.Range(patrolTimerStart / 2, patrolTimerStart);
            }
            else
            {
                patrolTimer = patrolTimerStart;
            }
            timerCount = 0f;
            //Debug.Log("Move Position!");
        }
    }

    void PursuePlayer()
    {
        if(follow)
        {
            enemyAgent.SetDestination(playerLoc.transform.position);
            enemyAgent.stoppingDistance = 2.0f;
        }
        


        else if (!randomPatrol && strictPatrol && patroling)
        {
                SetPatrolPoint();
        }
        else if (!randomPatrol && !strictPatrol && patroling)
        {
                SetRandomPatrolPoint();
        }
    }
}
