using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDeSpawner : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }

}
