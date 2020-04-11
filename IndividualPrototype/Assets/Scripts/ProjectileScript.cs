using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectileScript : MonoBehaviour
{
    public GameObject parent, playerObject;
    AudioSource mysource;
    [SerializeField] AudioClip shot, collisionSound, playerHurt;
    private void Start()
    {
        StartCoroutine(deathTime());
        mysource = this.gameObject.GetComponent<AudioSource>();
        this.GetComponent<Rigidbody>().AddForce((new Vector3(playerObject.transform.position.x, playerObject.transform.position.y-1, playerObject.transform.position.z)-parent.transform.position).normalized * 30f, ForceMode.VelocityChange);
        mysource.PlayOneShot(shot);
    }
    IEnumerator deathTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        Destroy(this.gameObject);
    }
    //source for audio https://freesound.org/people/tmokonen/sounds/95951/ , https://freesound.org/people/morganpurkis/sounds/394128/, https://freesound.org/people/komanderkyle/sounds/482217/ , https://freesound.org/people/DeadlyMustard/sounds/79643/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementScript>().health -= 5;
            if(collision.gameObject.GetComponent<PlayerMovementScript>().health<=0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            mysource.PlayOneShot(playerHurt);
        }
        else
        {

            mysource.PlayOneShot(collisionSound);
        }
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        for(int i =0; i< this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        
    }
}
