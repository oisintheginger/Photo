using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] PathObject myPath;
    [SerializeField] float speed;
    public bool Active=false;
    Rigidbody followerRB;
    int currentPathPoint = 0;

    [SerializeField] float waitTime=0.5f;

    float timer =0.5f;
    private void Awake()
    {
        timer = waitTime;
        followerRB = this.gameObject.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (Active)
        {
            FollowPathObject();
        }
    }
    public void FollowPathObject()
    {
        //followerRB.velocity = (myPath.pathPoints[currentPathPoint].position - this.transform.position).normalized * 2f;
        //followerRB.AddForce((myPath.pathPoints[currentPathPoint].position - this.transform.position).normalized*100f);
        if (timer >= waitTime)
        {
            followerRB.MovePosition(this.transform.position + (myPath.pathPoints[currentPathPoint].position - this.transform.position).normalized * Time.deltaTime * speed);
        }
        if(Vector3.Distance(this.transform.position, myPath.pathPoints[currentPathPoint].position)<0.3f)
        {
            if (timer <= 0)
            {
                if (currentPathPoint < myPath.pathPoints.Count - 1)
                {
                    currentPathPoint++;
                }
                else
                {
                    currentPathPoint = 0;
                }
                timer = waitTime;
            }
            else
            {
                timer -= Time.deltaTime/2f;
            }
        }
    }

    
}
