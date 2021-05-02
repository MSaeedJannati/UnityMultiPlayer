using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 3.0f);
    }
}
