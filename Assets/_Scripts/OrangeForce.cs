using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeForce : MonoBehaviour
{
    public GameObject OrangeParticle;
    private bool FrozeVelocity;
	// Use this for initialization
	void Start ()
	{
        //HealthSys = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        float rndX = Random.Range(2.0f, 10.0f);
        float rndZ = Random.Range(2.0f, 10.0f);

        GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10.0f, rndZ));
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (transform.position.y <= 0)
        {
            if (!FrozeVelocity)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                FrozeVelocity = true;
            }
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().AddForce(0f, 40f, 0f, ForceMode.Impulse);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<HealthSystem>().ChangeHp(5);
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
