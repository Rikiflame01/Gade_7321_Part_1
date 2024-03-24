using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagResetAI : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlueFlag")
        {
            GameEventSystem.ResetFlag(collision.gameObject);
        }
    }
}
