using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "PacMan") {
            GameEventsController.current.getCoin();
            Destroy(gameObject);
        }
    }
}
