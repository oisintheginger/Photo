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
    private void Awake()
    {
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
        followerRB.MovePosition(this.transform.position + (myPath.pathPoints[currentPathPoint].position - this.transform.position).normalized *Time.deltaTime*speed);
        if(Vector3.Distance(this.transform.position, myPath.pathPoints[currentPathPoint].position)<1f)
        {
            if(currentPathPoint<myPath.pathPoints.Count-1)
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
