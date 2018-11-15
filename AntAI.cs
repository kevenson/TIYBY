// AntAI.cs
// Basic ant behavior script for worker & warrior ants in general anthill scene with warriors patroling nest 
//  and workers going for food
// Kai Evenson

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AntAI : MonoBehaviour
{
    [Header("Shared")]
    private NavMeshAgent agent;
    public bool isWorker = true;
    public bool isWarrior = false;
    private Animator anim;
    [Header("Worker Ant")]
    public Transform goal;
    public Transform nest;
    private bool hasFood = false;
    [Tooltip("gameobject attached to worker ant that will be activated when returning with food")]
    public GameObject food;
    public int nestRespawnDelay = 3;
    [Header("Warrior Ant")]
    public Transform[] Patrolpoints;
    private int destPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.enabled = true;

        if (isWorker == true)
        {
            if (food != null)
            {
                food.SetActive(false);
                Debug.Log("food.activeSelf" + agent);
                Debug.Log(food.activeSelf);
            }

            //hasFood = false;
            Debug.Log("food.activeSelf2"  + agent);
            Debug.Log(food.activeSelf);
            CollectFood();
        }
        if (isWarrior == true)
        {
            RunPatrol();
        }
        
    }

    void Update()
    {
        if (isWarrior == true)
        {
            // Choose the next destination point for patrolling agents when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }

    }

    /// <summary>
    /// WORKERS
    /// </summary>

    // Go to find food after animation state change
    public void CollectFood()
    {
        //agent.enabled = true;
        hasFood = false;
        anim.SetBool("Walk", true);
        agent.autoBraking = true;
        agent.destination = goal.position;
    }

    // To handle animation transition and waypoint control for workers picking up food

    // NOTE: ADD NEW COROUTINE HERE TO HAVE ANTS PAUSE TO PICK UP FOOD (DELAY AFTER ANIMATION) AND THEN
    //  CONTINUE BACK TO NEST AFTER NEW STATE CHANGE

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food" && hasFood == false)
        {
            // think this is causing the issues
            
            // if entered food zone, pick up and return to nest
            anim.SetBool("Walk", false);
            agent.isStopped = true;
            StartCoroutine("PickUpFood");
        }
        if(other.tag == "Nest" && hasFood == true)
        {
            // if returned to nest w/ food, make invisible and stop movement; start coroutine to restart
            hasFood = false;
            if (food != null)
            {
                food.SetActive(false);
            }
            anim.SetBool("CarryWalk", false);
            agent.enabled = false;
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            StartCoroutine("ReturnToSurface");
        }
    }

    IEnumerator PickUpFood()
    {
        //anim.SetTrigger("PickUp");
        anim.SetBool("PickUp", true);
        hasFood = true;
        yield return new WaitForSeconds(1);
        agent.isStopped = false;
        ReturnFood();
    }

    // Return to Nest with visible food after animation state change
    public void ReturnFood()
    {
        if (food != null)
        {
            food.SetActive(true);
        }
        anim.SetBool("CarryWalk", true);
        agent.destination = nest.position;
        //Debug.Log("Agent returning to nest");
        //anim.SetBool("CarryWalk", false);
    }

    // Have collector ants reactivate (return to surface) to collect more food after pause
    IEnumerator ReturnToSurface()
    {
        yield return new WaitForSeconds(nestRespawnDelay);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        agent.enabled = true;
        anim.SetBool("Walk", true);
        //Debug.Log(gameObject + "GameObject Active : " + gameObject.activeInHierarchy);
        CollectFood();

    }

    /// <summary>
    /// WARRIORS
    /// </summary>

    // Patrol function for warriors
    public void RunPatrol()
    {
        anim.SetBool("Walk", true);
        agent.autoBraking = false;
        GotoNextPoint();
    }

    // Patrol points control for warriors
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (Patrolpoints.Length == 0)
            return;

        //Choose a random start point
        int startPoint = Random.Range(0, Patrolpoints.Length);

        // Set the agent to go to the currently selected destination.
        agent.destination = Patrolpoints[startPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        startPoint = (startPoint + 1) % Patrolpoints.Length;
    }
}