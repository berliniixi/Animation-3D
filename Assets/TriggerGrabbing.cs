using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGrabbing : MonoBehaviour
{
     
     public GameObject rootPos; //setting a gameObject as an ANCHOR
     
     void OnTriggerEnter(Collider collider)
     { 
          collider.GetComponentInParent<ClimbUp>().GrabEdge(rootPos.transform); // We use GetComponentInParent because the sphere that is the hand of the [
                                                                                // player is child to the Y_Bot 
                                                                                //When we Trigger with the box then the grab animation is starting 
     }
}
