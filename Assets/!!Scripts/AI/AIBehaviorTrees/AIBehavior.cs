using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    [Header("Configuration Parameters")]
    public Transform playerTransform;
    public Transform ownFlagTransform;
    public Transform redFlagSpawnTransform;
    public Transform aiBaseTransform;
    public Transform aiFlagSlot;
    public Transform eyeTransform; 

    [SerializeField] private float checkRate = 2.0f;
    [SerializeField] private float captureDistance = 1.0f;
    [SerializeField] private float engageDistance = 10f; 

    [Header("Shooting Parameters")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootingDistance = 10f;
    [SerializeField] private float shootingCooldown = 2f;

    [Header("Strategy Parameters")]
    [SerializeField] private float evadeCooldown = 10f;

    private NavMeshAgent agent;
    private BTNode behaviorTree;
    private float nextCheckTime = 0f;
    private float nextShotTime = 0f;
    private float nextEvadeTime = 0f;
    private bool isHostile = false;

    private GameObject detectedProjectile;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeBehaviorTree();
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            behaviorTree.Evaluate();
            nextCheckTime = Time.time + checkRate;
            FacePlayer();
        }
    }

    private void OnEnable()
    {
        GameEventSystem.OnFlagReset += ResetFlag;
    }
    private void OnDisable()
    {
        GameEventSystem.OnFlagReset -= ResetFlag;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            detectedProjectile = other.gameObject; // Assign the detected projectile
                                                   // Trigger evade behavior if within evade cooldown
            if (CanEvade())
            {
                StartCoroutine(EvadeAttack(detectedProjectile));
            }
        }
    }

    void InitializeBehaviorTree()
    {
        behaviorTree = new Selector(new List<BTNode>
    {
        // Handle behavior when AI has the flag
        new Sequence(new List<BTNode>
        {
            new DecoratorNode(HasFlag),
            new Selector(new List<BTNode>
            {
                // Check if path is blocked
                new Sequence(new List<BTNode>
                {
                    new DecoratorNode(() => IsPathBlocked(aiBaseTransform.position)),
                    new ActionNode(EngagePlayerOrReevaluate)
                }),
                // Attempt to return the flag if the path is clear
                new Sequence(new List<BTNode>
                {
                    new DecoratorNode(() => !IsPathBlocked(aiBaseTransform.position)),
                    new ActionNode(ReturnFlag)
                })
            })
        }),
        
        // Handle behavior when AI does not have the flag
        new Sequence(new List<BTNode>
        {
            new DecoratorNode(() => !HasFlag()),
            new Selector(new List<BTNode>
            {
                // Attempt to capture the flag if the path is clear
                new Sequence(new List<BTNode>
                {
                    new DecoratorNode(() => !IsPathBlocked(ownFlagTransform.position)),
                    new ActionNode(TryCaptureOwnFlag)
                }),
                // Engage the player if the path is blocked or the player is within engagement distance
                new Sequence(new List<BTNode>
                {
                    new DecoratorNode(() => IsPathBlocked(ownFlagTransform.position) || IsPlayerWithinEngagementDistance()),
                    new ActionNode(BecomeHostile)
                })
            })
        })
    });
    }


    private bool HasFlag() => aiFlagSlot.childCount > 0;

    private bool ReturnFlag()
    {
        if (!HasFlag()) return false; // Ensure the AI has the flag before attempting to return it

        // Move towards the AI's base
        MoveToTarget(aiBaseTransform.position);

        // Check if the AI is close enough to its base to return the flag
        if (IsCloseTo(aiBaseTransform.position, captureDistance))
        {
            ResetFlag();
            GameEventSystem.FlagCaptured(gameObject, "AI");
            return true; // Flag has been successfully returned
        }
        return false; // Still in the process of returning the flag
    }

    private bool TryCaptureOwnFlag()
    {
        // Directly move towards the flag's position
        MoveToTarget(ownFlagTransform.position);

        // Check if the AI is close enough to interact with the flag
        if (IsCloseTo(ownFlagTransform.position, captureDistance))
        {

            GameObject flag = GameObject.FindGameObjectWithTag("RedFlag");
            FlagInteraction(flag);

            return true; // Flag interaction attempted/succeeded
        }
        return false; // Still trying to capture flag
    }

    private void MoveToTarget(Vector3 target) => agent.SetDestination(target);

    private void FlagInteraction(GameObject flag)
    {
        flag.transform.SetParent(aiFlagSlot);
        flag.transform.localPosition = Vector3.zero;
        flag.transform.localRotation = Quaternion.identity;
    }

    private bool IsCloseTo(Vector3 position, float distance) => Vector3.Distance(transform.position, position) < distance;

    private void ResetFlag()
    {
        foreach (Transform child in aiFlagSlot)
        {
            child.SetParent(null); // Reset flag
        }
    }

    private bool CanShootPlayer()
    {
        Debug.Log($"Checking CanShootPlayer at {Time.time}, next shot time: {nextShotTime}");
        return Vector3.Distance(transform.position, playerTransform.position) <= shootingDistance && Time.time >= nextShotTime;
    }
    private bool ShootAtPlayer()
    {
        if (CanShootPlayer())
        {
            nextShotTime = Time.time + shootingCooldown;
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.LookRotation(playerTransform.position - shootingPoint.position));
            projectile.GetComponent<Rigidbody>().velocity = (playerTransform.position - shootingPoint.position).normalized * 20f;
            return true; // Shooting succeeded
        }
        return false; // Shooting not possible
    }

    private bool IsPathBlocked(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        return path.status != NavMeshPathStatus.PathComplete;
    }

    private bool IsPlayerWithinEngagementDistance()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= engageDistance;
    }

    private bool BecomeHostile()
    {
        isHostile = true;

        // Continuously check to shoot at player when hostile and within shooting range
        if (Time.time >= nextShotTime && Vector3.Distance(transform.position, playerTransform.position) <= shootingDistance)
        {
            StopAndShoot();
        }
        else
        {
            MoveSideToSide();
        }
        MaintainDistanceFromPlayer();
        return true; // Becoming hostile always succeeds
    }

    private bool EngagePlayerOrReevaluate()
    {
        isHostile = true;
        if (CanEvade() && detectedProjectile != null)
        {
            EvadeAttack(detectedProjectile);
        }
        else if (Time.time >= nextShotTime)
        {
            StopAndShoot();
        }
        else
        {
            MoveSideToSide();
        }
        MaintainDistanceFromPlayer();

        // Re-evaluate if the AI still has the flag
        if (!HasFlag())
        {
            // Flag has been taken, possibly reset or change behavior
            isHostile = false; // Consider changing state or behavior
            return false; // Indicate that action has finished due to flag loss
        }

        // Re-check if the path has become clear to return the flag
        if (!IsPathBlocked(aiBaseTransform.position))
        {
            isHostile = false; // Reset hostile state
            return ReturnFlag(); // Attempt to return the flag again
        }

        return true; // Continue engagement
    }

    private bool CanEvade() => Time.time >= nextEvadeTime;

    private IEnumerator EvadeAttack(GameObject incomingProjectile)
    {
        nextEvadeTime = Time.time + evadeCooldown;

        Vector3 evadeDirection = GetEvadeDirection(incomingProjectile.transform.forward);
        Vector3 evadeTarget = transform.position + evadeDirection * 5f; // Adjust evade distance as needed

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(evadeTarget, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(evadeTarget);
            yield return new WaitForSeconds(1f); // Wait time after evading before next action
        }
    }

    private Vector3 GetEvadeDirection(Vector3 projectileDirection)
    {
        Vector3 perpendicular = Vector3.Cross(projectileDirection, Vector3.up);
        return Random.value < 0.5f ? perpendicular : -perpendicular;
    }

    private void StopAndShoot()
    {
        if (CanShootPlayer())
        {
            // Explicitly stop the agent
            agent.isStopped = true;

            // Shoot at the player
            if (ShootAtPlayer())
            {
                Debug.Log("Shooting at player.");
            }

            // Resume movement after a delay to simulate shooting action duration
            StartCoroutine(ResumeMovementAfterDelay(0.2f));
        }
    }

    IEnumerator ResumeMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        agent.isStopped = false;
    }

    private void MoveSideToSide()
    {
        if (!isHostile) return; // Only move side to side if in hostile mode
        StartCoroutine(SideToSideMovement());
    }

    IEnumerator SideToSideMovement()
    {
        while (Time.time < nextShotTime)
        {
            Vector3 sideStepDirection = Random.value < 0.5f ? transform.right : -transform.right;
            Vector3 targetPosition = transform.position + sideStepDirection * 2f; 
            agent.SetDestination(targetPosition);
            yield return new WaitForSeconds(Random.Range(1f, 3f)); // Random interval between moves
        }
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Keep rotation in the horizontal plane
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void MaintainDistanceFromPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer > engageDistance)
        {
            Vector3 closerPosition = Vector3.MoveTowards(transform.position, playerTransform.position, 2f);
            agent.SetDestination(closerPosition);
        }
        else if (distanceToPlayer < engageDistance)
        {
            Vector3 backAwayPosition = Vector3.MoveTowards(transform.position, playerTransform.position, -2f);
            agent.SetDestination(backAwayPosition);
        }
    }

    private void ResetFlag(GameObject flag)
    {
        if (flag.tag == "RedFlag")
        {
            flag.transform.SetParent(null);
            flag.transform.position = redFlagSpawnTransform.position;
            Debug.Log("AI Flag reset.");
        }

    }
}
