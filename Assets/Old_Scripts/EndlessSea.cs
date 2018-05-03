using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndlessSea : MonoBehaviour {

    public GameObject Ocean;
    private GameObject Player;
    public List<GameObject> mySea = new List<GameObject>();
    public bool firstSea;

    public static GameObject CurrentSea;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Ship");
        if (firstSea)
        {
            
            //MakeSurroudingSea();
        }
    }

    // Update is called once per frame
    void Update () {

	}

    private void MakeSurroudingSea()
    {
        Vector3 parentPos = transform.parent.position;
        Vector3 spawnPos1 = new Vector3(parentPos.x, 0f, parentPos.z + 999);
        Vector3 spawnPos2 = new Vector3(parentPos.x, 0f, parentPos.z - 999);
        Vector3 spawnPos3 = new Vector3(parentPos.x + 999, 0f, parentPos.z);
        Vector3 spawnPos4 = new Vector3(parentPos.x - 999, 0f, parentPos.z);
        Vector3 spawnPos5 = new Vector3(parentPos.x - 999, 0f, parentPos.z - 999);
        Vector3 spawnPos6 = new Vector3(parentPos.x + 999, 0f, parentPos.z + 999);
        Vector3 spawnPos7 = new Vector3(parentPos.x + 999, 0f, parentPos.z - 999);
        Vector3 spawnPos8 = new Vector3(parentPos.x - 999, 0f, parentPos.z + 999);
        Vector3 spawnPos9 = new Vector3(parentPos.x, 0f, parentPos.z);

        mySea.Add(Instantiate(Ocean, spawnPos1, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos2, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos3, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos4, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos5, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos6, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos7, Quaternion.identity));
        mySea.Add(Instantiate(Ocean, spawnPos8, Quaternion.identity));
        //mySea.Add(Instantiate(Ocean, spawnPos9, Quaternion.identity));

        firstSea = false;
    }

    public void DestroyOldSea()
    {
        Debug.Log(CurrentSea.name);
        mySea.Where(a=> a != CurrentSea).ToList().ForEach(sea => Destroy(sea.gameObject));
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player"  && CurrentSea != transform.parent.gameObject)
        {
            EndlessSea oldSea = null;
            if(CurrentSea != null)
            {
                oldSea = CurrentSea.transform.GetChild(0).GetComponent<EndlessSea>();
            }
            Debug.Log(gameObject.name);
            
            CurrentSea = transform.parent.gameObject;
            
            MakeSurroudingSea();

            if (oldSea != null)
            {
                oldSea.DestroyOldSea();
            }
        }
    }    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Invoke("DestroyOldSea",1);
        }
    }
}
