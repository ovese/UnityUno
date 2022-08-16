using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // leaving use of isGrounded for transform
    // the serializefield is akin to saying public
    // this exposes the variable inside the unity editor inspector for the player
    [SerializeField] private Transform groundCheckTransform; // intialize to null if you want
    [SerializeField] private LayerMask playerMask;

    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody rigidBodyComponent; // will be initialized in the start method
    private int superJumpsRemaining;

    private int score;
    private bool destroyCoinStatus;
    

    // this next variabe will be used to control the repeated jumps of the player in the air ;like a bird
    // because such repeated jumps is not the bahavior we want
    //private bool isGrounded; // if grounded, then we can jump but ifnot grounded, means we are airborne and cant jump in air


    // Start is called before the first frame update
    // any instruction here is executed just once
    void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if space bar key is pressed down
        if(Input.GetKeyDown(KeyCode.Space)==true)
        {
            jumpKeyWasPressed = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        
    }

    // fixedUpdate is called once every physics update
    // A Physics update is 100 cycles per second i.e. 100Hz
    // This method is useful to take care of different macine specs discrepancies in speed
    // make sure to apply the physics here and not in the regular update method
    private void FixedUpdate()
    {

        //GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, 0, 0); // motion restricted to x-axis only left and right
        // hjowever the above line makes the gravity field too slow because it is set to zero right after it was set to 1 previously
        //GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, GetComponent<Rigidbody>().velocity.y, 0); // changes to the line below
        rigidBodyComponent.velocity = new Vector3(horizontalInput, rigidBodyComponent.velocity.y, 0);


        // here i will implement the collision and isgrounded functionality
        //if(!isGrounded)
        //{
        //    return;
        //}
        // abandoned this implmentation to use a better one i.e. Transform

        if (Physics.OverlapSphere(groundCheckTransform.position,0.1f, playerMask).Length==0)
        {
            return;
        }


        // check if space bar key is pressed down
        if (jumpKeyWasPressed == true)
        {
            // Debug.Log("Space key was pressed down"); // we dont really need this
            // rather make the player jump
            //GetComponent<Rigidbody>().AddForce(Vector3.up*5, ForceMode.VelocityChange);
            // instead of the above format, I could do
            Vector3 go_up = new Vector3(0, 1, 0);
            float jumpPower = 5.0f;
            if(superJumpsRemaining>0)
            {
                jumpPower *= 2;
                superJumpsRemaining--;
            }
            //GetComponent<Rigidbody>().AddForce(go_up * 5, ForceMode.VelocityChange); // changes to the line below
            rigidBodyComponent.AddForce(go_up * jumpPower, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        score = 0;
        destroyCoinStatus = false;
        if(other.gameObject.layer==7 && destroyCoinStatus==false)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
            score = score + 1;
            destroyCoinStatus = true;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    isGrounded = true;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    isGrounded = false;
    //}
}
