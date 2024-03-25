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

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;

    [SerializeField] private float checkRate = 2.0f;
    [SerializeField] private float captureDistance = 1.0f;
    [SerializeField] private float engageDistance = 10f;
    [SerializeField] private float shootingDistance = 10f;
    [SerializeField] private float shootingCooldown = 2f;
    [SerializeField] private float evadeCooldown = 10f;

    private NavMeshAgent agent;
    private BTNode behaviorTree;

    private float nextCheckTime;
    private float nextShotTime;
    private float nextEvadeTime;
    private bool isHostile;
    private bool shouldMaintainDistance = false;
    private bool shouldFacePlayer = false;

    private GameObject detectedProjectile;

    public Transform[] warpPoints;



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeBehaviorTree();
    }

    private void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            behaviorTree.Evaluate();
            UpdateBehaviorFlags();
            nextCheckTime = Time.time + checkRate;
        }

        if (shouldFacePlayer)
        {
            FacePlayer();
        }

        if (shouldMaintainDistance)
        {
            MaintainDistanceFromPlayer();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            detectedProjectile = other.gameObject;
            if (CanEvade()) TryWarpToSafety();
        }
    }

    private void InitializeBehaviorTree()
    {
        behaviorTree = new Selector(new List<BTNode>
        {
            //If the AI has the flag.
            new Sequence(new List<BTNode>
            {
                new DecoratorNode(HasFlag),
                new Selector(new List<BTNode>
                {
                    new Sequence(new List<BTNode>
                    {
                        new DecoratorNode(() => IsPathBlocked(aiBaseTransform.position)),
                        //new ActionNode(EngagePlayerOrReevaluate)
                        new ActionNode(BecomeHostile)
                    }),
                    new Sequence(new List<BTNode>
                    {
                        new DecoratorNode(() => !IsPathBlocked(aiBaseTransform.position)),
                        new ActionNode(ReturnFlag)
                    })
                })
            }),
            //If the AI does not have the flag.
            new Sequence(new List<BTNode>
            {
                new DecoratorNode(() => !HasFlag()),
                new Selector(new List<BTNode>
                {
                    new Sequence(new List<BTNode>
                    {
                        new DecoratorNode(() => IsPathBlocked(ownFlagTransform.position) || IsPlayerWithinEngagementDistance()),
                        new ActionNode(BecomeHostile)
                    }),
                    new Sequence(new List<BTNode>
                    {
                        new DecoratorNode(() => !IsPathBlocked(ownFlagTransform.position)),
                        new ActionNode(TryCaptureOwnFlag)
                    })
                })
            })
        });
    }

    private void UpdateBehaviorFlags()
    {
        // Example condition to update flags
        if (isHostile)
        {
            shouldFacePlayer = true;
            shouldMaintainDistance = true;
        }
        else
        {
            shouldFacePlayer = false;
            shouldMaintainDistance = false;
        }

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
        isHostile = false;
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
            FacePlayer();
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
        if (Time.time >= nextShotTime && Vector3.Distance(transform.position, playerTransform.position) <= shootingDistance)
        {
            StopAndShoot();
        }
        else
        {
            MoveSideToSide();
        }
        MaintainDistanceFromPlayer();
        return true; 
    }
    private bool CanEvade() => Time.time >= nextEvadeTime;

    private void TryWarpToSafety()
    {
        foreach (var warpPoint in warpPoints)
        {
            if (!Physics.CheckSphere(warpPoint.position, 0.01f))
            {
                WarpTo(warpPoint.position);
                nextEvadeTime = Time.time + evadeCooldown;
                return;
            }
        }
    }

    void WarpTo(Vector3 targetPosition)
    {
        agent.Warp(targetPosition); 
    }

    private void StopAndShoot()
    {
        if (CanShootPlayer())
        {
            agent.isStopped = true;

            if (ShootAtPlayer())
            {
                Debug.Log("Shooting at player.");
            }

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
        if (!isHostile) return; 
        StartCoroutine(SideToSideMovement());
    }

    IEnumerator SideToSideMovement()
    {
        while (Time.time < nextShotTime)
        {
            Vector3 sideStepDirection = Random.value < 0.5f ? transform.right : -transform.right;
            Vector3 targetPosition = transform.position + sideStepDirection * 2f;
            agent.SetDestination(targetPosition);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    private void FacePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Keep rotation in the horizontal plane
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
    }

    private void MaintainDistanceFromPlayer()
    {
        // First, confirm that the AI should be trying to maintain distance
        // For instance, only maintain distance if in a hostile state or actively engaging the player
        if (!isHostile) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float stopDistance = engageDistance * 0.8f; // Stop moving closer if within 80% of engage distance
        float backOffDistance = engageDistance * 1.2f; // Start moving away if closer than 120% of engage distance

        if (distanceToPlayer > backOffDistance)
        {
            // If too far, move closer
            MoveToTarget(playerTransform.position);
        }
        else if (distanceToPlayer < stopDistance)
        {
            // If too close, back away
            Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
            Vector3 backAwayPosition = transform.position + directionAwayFromPlayer * 2f;
            MoveToTarget(backAwayPosition);
        }
        // If within the optimal range, consider not moving to avoid jittery behavior
    }

}
