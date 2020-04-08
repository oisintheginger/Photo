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

    // Start is called before the first frame update
    void Start()
    {
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
            myAgent.speed = 3f;
        } else
        {
            myAgent.stoppingDistance = 6f;
            myAgent.speed = 5f;
        }

        if(Vector3.Distance(playerObject.transform.position, this.transform.position)<detectDistance)
        {
            passive = false;
            myAgent.SetDestination(playerObject.transform.position);
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
                if (currentPathPoint < passivePath.pathPoints.Count - 1)
                {
                    currentPathPoint++;
                }
                else
                {
                    currentPathPoint = 0;
                }
            }
        }
    }


}
