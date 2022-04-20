using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private int selfDestructCounter;

    // Start is called before the first frame update
    private void Start()
    {
        selfDestructCounter = Globals.SelfDestructAnimation;
    }

    // Update is called once per frame
    private void Update()
    {
        selfDestructCounter--;
        if (selfDestructCounter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
