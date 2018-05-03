using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeForce : MonoBehaviour {

    public GameObject OrangeParticle;
    private HealthSystem healthSys;
    private bool frozeVelocity = false;
	// Use this for initialization
	void Start () {
        healthSys = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        float rndX = UnityEngine.Random.Range(2.0f, 10.0f);
        float rndZ = UnityEngine.Random.Range(2.0f, 10.0f);

        //GetComponent<Rigidbody>().AddExplosionForce(1.0f, transform.position - new Vector3(rndX, 20.0f, rndZ), 40.0f, 10.0f);
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10.0f, rndZ));
        //for (int i = 0; i < 3; i++)
        //{            

        //}
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y <= 0)
        {
            if (!frozeVelocity)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                frozeVelocity = true;
            }
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().AddForce(0f, 40f, 0f, ForceMode.Impulse);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            healthSys.currentScurvy -= 6f;
            healthSys.currentHP += 5f;
            GetComponent<AudioSource>().Play();
            GetComponent<SphereCollider>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
            Instantiate(OrangeParticle, transform.position, Quaternion.Euler(Vector3.zero));
            Invoke("DestroyObject", 2f);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
