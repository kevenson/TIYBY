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
    private Animator[] anim;
    public AudioClip[] antAudioClips;     // generic ant noises
    private AudioSource antAudio;
    public float antSoundVolume = 0.5f;
    [Range(0.0f, 20.0f)]
    public float antSoundDelayLow = 5f;
    [Range(10f, 60.0f)]
    public float antSoundDelayHigh = 30f;
    //private Animator[] warriorAnim;
    [Header("Worker Ant")]
    public Transform goal;
    public Transform nest;
    private bool hasFood = false;
    static bool firstAntTrail = false;  //only one ant will lay trail
    public GameObject foodTrail;
    public float foodTrailDelay = 2.0f;
    [Tooltip("gameobject attached to worker ant that will be activated when returning with food")]
    public GameObject food;
    public int nestRespawnDelay = 3;
    [Header("Warrior Ant")]
    public Transform[] Patrolpoints;
    private int destPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        anim = GetComponentsInChildren<Animator>();
        agent.enabled = true;
        antAudio = GetComponent<AudioSource>();

        // if worker, check to ensure food is inactive and run collection function
        if (isWorker == true)
        {
            if (food != null)
            {
                food.SetActive(false);
            }
            CollectFood();
        }
        // if warrior, run patrol
        if (isWarrior == true)
        {
            RunPatrol();
        }

        // start playing ant noises
        StartAntAudio();
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

    private void StartAntAudio()
    {
        // set up Audio Source
        antAudio.spatialBlend = 1.0f;
        antAudio.volume = antSoundVolume;
        antAudio.loop = false;
        antAudio.playOnAwake = false;
        StartCoroutine("AntSounds");
    }
    // generate random ambient ant noise every x seconds
    IEnumerator AntSounds()
    {
        for (; ; )
        {
            var clipToPlay = antAudioClips[Random.Range(0, antAudioClips.Length - 1)];
            antAudio.clip = clipToPlay;
            antAudio.Play();
            var randomNum = Random.Range(antSoundDelayLow, antSoundDelayHigh);
            yield return new WaitForSeconds(randomNum);
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
        foreach (Animator i in anim)
            i.SetBool("Walk", true);
        //anim.SetBool("Walk", true);
        agent.autoBraking = true;
        agent.destination = goal.position;
    }

    // To handle animation transition and waypoint control for workers picking up food
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food" && hasFood == false)
        {
            // if entered food zone, start coroutine to pick up and return to nest
            foreach (Animator i in anim)
                i.SetBool("Walk", false);
            //anim.SetBool("Walk", false);
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
            foreach (Animator i in anim)
                i.SetBool("CarryWalk", false);
            //anim.SetBool("CarryWalk", false);
            agent.enabled = false;
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            StartCoroutine("ReturnToSurface");
        }
    }

    // activate food object, start pick up animation, start navmesh agent after pause and return
    IEnumerator PickUpFood()
    {
        hasFood = true;
        foreach (Animator i in anim)
            i.SetBool("PickUp", true);
        //anim.SetBool("PickUp", true);
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
        foreach (Animator i in anim)
            i.SetBool("PickUp", false);
        //anim.SetBool("PickUp", false);
        foreach (Animator i in anim)
            i.SetBool("CarryWalk", true);
        //anim.SetBool("CarryWalk", true);
        agent.destination = nest.position;
        StartCoroutine("LayPheromoneTrail");

    }

    IEnumerator LayPheromoneTrail()
    {
        if(firstAntTrail == false)
        {
            firstAntTrail = true;
            while (hasFood == true) {
                var pos = gameObject.transform.position;
                var rot = gameObject.transform.rotation;
                Instantiate(foodTrail, pos, rot);
                yield return new WaitForSeconds(foodTrailDelay);
            }
            //transform.position;
        }
    }

    // Have collector ants reactivate (return to surface) to collect more food after pause
    IEnumerator ReturnToSurface()
    {
        foreach (Animator i in anim)
            i.SetBool("Walk", true);
        yield return new WaitForSeconds(nestRespawnDelay);
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        agent.enabled = true;
        CollectFood();

    }

    /// <summary>
    /// WARRIORS
    /// </summary>

    // Patrol function for warriors
    public void RunPatrol()
    {
        //anim.SetBool("Walk", true);
        foreach (Animator i in anim)
            i.SetBool("Walk", true);
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