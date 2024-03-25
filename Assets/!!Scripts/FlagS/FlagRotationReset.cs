using UnityEngine;

// This class is used to reset the flag's rotation.

public class FlagRotationReset : MonoBehaviour
{
    private Quaternion defaultRotation;

    void Start()
    {
        defaultRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = defaultRotation;
    }
}
