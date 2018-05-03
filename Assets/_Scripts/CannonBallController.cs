using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * 30000);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            collider.gameObject.GetComponent<EnemyHealth>().GotHit(10);
        }

        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player hit");
            collider.gameObject.GetComponent<HealthSystem>().GotHit(10);
        }
        Destroy(gameObject);
    }
}
