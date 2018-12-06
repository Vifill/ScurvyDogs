using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CannonBallController : NetworkBehaviour {

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 100), ForceMode.VelocityChange);
        StartCoroutine(DestroyCoroutine());
	}

    // replaced Update
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    //void Update ()
    //   {
    //	if (transform.position.y < 0)
    //       {
    //           Destroy(gameObject);
    //       }
    //}

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

        if (collider.gameObject.tag == "Ocean")
        {
            return;
        }
        Destroy(gameObject);
    }
}
