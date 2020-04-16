using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    bool passive = true;
    NavMeshAgent myAgent;
    [SerializeField] GameObject playerObject;
    [SerializeField] PathObject passivePath;
    [SerializeField] int currentPathPoint;
    [SerializeField] float detectDistance;
    Animator childAnimator;
    float timer;
    public float ShootingTimer, passiveSpeed, aggressiveSpeed;
    [SerializeField] Transform shootingPos;
    [SerializeField] GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        timer = ShootingTimer;
        myAgent = this.gameObject.GetComponent<NavMeshAgent>();
        playerObject = FindObjectOfType<PlayerMovementScript>().gameObject;
        childAnimator = this.gameObject.GetComponentInChildren<Animator>();
        myAgent.SetDestination(playerObject.transform.position);
    }

    private void Update()
    {
        if(passive == true)
        {
            myAgent.stoppingDistance = 0f;
            myAgent.speed = passiveSpeed;
        } else
        {
            myAgent.stoppingDistance = 6f;
            myAgent.speed = aggressiveSpeed;
        }

        if(Vector3.Distance(playerObject.transform.position, this.transform.position)<detectDistance)
        {
            passive = false;
            myAgent.SetDestination(playerObject.transform.position);
            RaycastHit rh;
            if (Physics.Raycast(this.transform.position, playerObject.transform.position - this.transform.position, out rh, 30f))
            {
                if(rh.collider.gameObject.tag== "Player")
                {
                    Shooting();
                }
            }
            
        }
        else
        {
            passive = true;
        }
        
        passivePathFollow(passivePath, passive);
        childAnimator.SetBool("Attacking", !passive);
    }


    void passivePathFollow(PathObject passivePath,bool passive)
    {

        if (passive)
        {
            myAgent.SetDestination(passivePath.pathPoints[currentPathPoint].transform.position);

            if (Vector3.Distance(this.transform.position, passivePath.pathPoints[currentPathPoint].transform.position) < 1f)
            {
                currentPathPoint = Random.Range(0, passivePath.pathPoints.Count);
                /*
                if (currentPathPoint < passivePath.pathPoints.Count - 1)
                {
                    currentPathPoint++;
                }
                else
                {
                    currentPathPoint = 0;
                }
                */
            }
        }
    }
    
    void Shooting()
    {
        
        if(timer<=0)
        {
            if(projectile!=null)
            {
                GameObject newProjectile= Instantiate(projectile, shootingPos.position, shootingPos.transform.rotation);
                newProjectile.GetComponent<ProjectileScript>().parent = this.gameObject;
                newProjectile.GetComponent<ProjectileScript>().playerObject = playerObject;
            }
            timer = ShootingTimer;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


}
