using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour 
{
    public List<AudioClip> SongList;
    private AudioSource Asource;
    [SerializeField]
    private Queue<AudioClip> SongQueue;
	// Use this for initialization
	private void Start() 
	{
        Asource = GetComponent<AudioSource>();
        SongList.Shuffle();
        SongQueue = new Queue<AudioClip>(SongList);
	}

    // Update is called once per frame
    private void Update() 
	{
        if (!Asource.isPlaying)
        {
            Asource.clip = SongQueue.Dequeue();
            Asource.Play();
        }
	}
}
