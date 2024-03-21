using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehavior : MonoBehaviour
{
    private BTNode behaviorTree;
    public NavMeshAgent agent;
    public Transform playerTransform;
    public Transform ownFlagTransform; // AI's flag
    public Transform aiBaseTransform; // AI's base transform, blue flag spawn point.
    public Transform aiFlagSlot; // Slot where the AI's flag is attached to

    // Configuration parameters
    [SerializeField]
    private float checkRate = 2.0f;
    private float nextCheckTime = 0f;
    [SerializeField]
    private float captureDistance = 1.0f; // Externalize as a configurable parameter

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeBehaviorTree();
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            behaviorTree.Evaluate(); // Changed from Invoke to Evaluate for clarity
            nextCheckTime = Time.time + checkRate;
        }
    }

    void InitializeBehaviorTree()
    {
        // Dynamically build the behavior tree based on current game state
        behaviorTree = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new ConditionNode(HasFlag),
                new ActionNode(ReturnFlag)
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(() => !HasFlag()),
                new ActionNode(TryCaptureOwnFlag)
            }),
        });
    }

    private bool HasFlag()
    {
        return aiFlagSlot.childCount > 0;
    }

    private bool ReturnFlag()
    {
        if (IsCloseTo(aiBaseTransform.position, captureDistance) && HasFlag())
        {
            Debug.Log("Flag returned to base!");
            GameEventSystem.FlagCaptured(gameObject, "captured the flag");
            ResetFlag();
            return true;
        }
        MoveToTarget(aiBaseTransform.position);
        return false;
    }

    private bool TryCaptureOwnFlag()
    {
        if (IsCloseTo(ownFlagTransform.position, captureDistance))
        {
            FlagInteraction(ownFlagTransform.gameObject);
            Debug.Log("Flag captured!");
            return HasFlag();
        }
        MoveToTarget(ownFlagTransform.position);
        return false;
    }

    private void MoveToTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void FlagInteraction(GameObject flag)
    {
        flag.transform.SetParent(aiFlagSlot);
        flag.transform.localPosition = Vector3.zero;
        flag.transform.localRotation = Quaternion.identity;
    }

    private bool IsCloseTo(Vector3 position, float distance)
    {
        return Vector3.Distance(transform.position, position) < distance;
    }

    private void ResetFlag()
    {
        foreach (Transform child in aiFlagSlot)
        {
            child.SetParent(null); // Consider resetting position/rotation here if necessary
        }
    }
}
