using System;
using UnityEngine;
using System.Collections;

public class ClimbUp : MonoBehaviour {

    [SerializeField] float speed = 5.0F;
    [SerializeField] float rotationSpeed = 100.0F;
    [SerializeField] float lerpSpeed = 5.0F;
    Animator anim;
    [SerializeField] bool isHanging; // Doesnt Hanging up 
    [SerializeField] private bool isShimmy;
    Transform animRootTarget;
    private Rigidbody _rigidbody;
    
    public void GrabEdge(Transform rootTarget) // rootTarget is the ANCHOR (position of the object) 
    {
        if(isHanging) //false
        { return; }
        
        anim.SetTrigger("grabEdge");
        _rigidbody.isKinematic = true; // it leaves player in the physics system but doesnt have it affected by physics object 
        Debug.Log(_rigidbody.isKinematic);
        
        isHanging = true;
        animRootTarget = rootTarget;

        MoveAnchor();

    }

    void MoveAnchor()
    {
        Plane rootPlane = new Plane(animRootTarget.position,
            animRootTarget.position + animRootTarget.right,
            animRootTarget.position + animRootTarget.up);

        
        // adjustPos = it takes x,z position of the player when is in grab animation 
        Vector3 adjustedPos = new Vector3(transform.position.x, 
            animRootTarget.position.y, // keeping the height of the box (ANCHOR) 
            transform.position.z);

        
        
        Ray ray = new Ray(adjustedPos - animRootTarget.forward, animRootTarget.forward); // adjustedPos is the started position , animRootTarget is the direction  
        float rayDistance; // create a float variable to hold the... 
        if (rootPlane.Raycast(ray, out rayDistance))
        {
            animRootTarget.position = ray.GetPoint(rayDistance);
        }
    }

    public void StandUp()
    {
        isHanging = false; // the player stop hanging up
        Debug.Log("isHanging " +  isHanging);
        
        
        GetComponent<Rigidbody>().isKinematic = false; 
        Debug.Log("isKinematic " +  GetComponent<Rigidbody>().isKinematic);
        
        animRootTarget = null;
        Debug.Log("animRootTarget " +  animRootTarget);

    }

    public void EndShimmy()
    {
        isShimmy = false;

        MoveAnchor();
    }
    
     void AnimLerp()
     {
        if(!animRootTarget || isShimmy) return;
        
        if (Vector3.Distance(this.transform.position,animRootTarget.position) > 0.1f)
        {
            this.transform.rotation = Quaternion.Lerp(transform.rotation, //From
                                                 animRootTarget.rotation, //To
                                                 Time.deltaTime * lerpSpeed); // On that time
            this.transform.position = Vector3.Lerp(transform.position, //From  
                                              animRootTarget.position, //To
                                              Time.deltaTime * lerpSpeed); // On that time
        }
        else
        {
            this.transform.position = animRootTarget.position;   //Make the position of the player equal to Anchor
            this.transform.rotation = animRootTarget.rotation;   //Make the rotation of the player equal to Anchor
        }
        
    }

    void Start()
    {
    	anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        animRootTarget = null;
    }

    void FixedUpdate()
    {
        AnimLerp();
    }

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;

        if (!isHanging)
        {
            transform.Rotate(0, rotation, 0);
        }
            

        if (translation != 0)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("speed", translation * 0.5f);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetFloat("speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHanging)
            {
                anim.SetTrigger("drop");
                _rigidbody.isKinematic = false;
                animRootTarget = null;
            }
            else
            { anim.SetTrigger("isJumping");}
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isHanging)
            {
                anim.SetTrigger("isClimbing");
                animRootTarget = null; // leaving empty the ANCHOR for letting the player climb
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isHanging)
            {
                anim.SetTrigger("leftShimmy");
                isShimmy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (isHanging)
            {
                anim.SetTrigger("rightShimmy");
                isShimmy = true;
            }
        }
    }
}
