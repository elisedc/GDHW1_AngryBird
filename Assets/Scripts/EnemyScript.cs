using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float threshold;
    public float speedThreshold;
    public ParticleSystem killEffect;
    public AudioSource enemyKillSound;
    public AudioSource bumpSoundEffect;

    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        bumpSoundEffect.Play();
        if (col.relativeVelocity.magnitude > threshold || rigidBody.velocity.magnitude > speedThreshold)
        {
            enemyKillSound.Play();
            killEffect.transform.position = this.transform.position;
            Destroy(this.gameObject);
            killEffect.gameObject.SetActive(true);
        }   
    }
}
