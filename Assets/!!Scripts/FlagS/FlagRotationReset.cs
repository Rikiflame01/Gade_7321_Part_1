using UnityEngine;

public class FlagRotationReset : MonoBehaviour
{
    private Quaternion defaultRotation;

    void Start()
    {
        // Store the default rotation at the start.
        defaultRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = defaultRotation;
    }
}
