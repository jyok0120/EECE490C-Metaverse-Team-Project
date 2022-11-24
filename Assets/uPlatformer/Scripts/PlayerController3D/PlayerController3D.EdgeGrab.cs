using UnityEngine;
using System.Collections;

public partial class PlayerController3D {
    // How fast the player will climb up
    public float edgeGrabClimbSpeed = 3f;
    // How far forward past the edge the player should move when climbing up
    public float edgeGrabClimbForward = 0.5f;
    // How far upwards above the edge the player should move when climbing up
    public float edgeGrabClimbUp = 0.5f;
    // How far from player's feet (0,0,0) to "Edge Grab Area" lower bounds
    public float edgeGrabBoundsBottom = 1.25f;
    // How far from player's feet (0,0,0) to "Edge Grab Area" upper bounds
    public float edgeGrabBoundsTop = 1.5f;
    // Radius of edge grab bounds
    public float edgeGrabBoundsRadius = 0.75f;
    // The minimum horizontal size of an edge that the player can grab on to
    public float edgeGrabMinEdgeSize = 0.2f;    
    // How many rays will be fired when searching for an edge
    public int edgeGrabRayCount = 4;
    // Should the character controller bounds be a static height while climbing
    public bool staticCCBoundsClimbing = false;
    // The height the character controller while climbing up
    public float ccClimbingHeight = 0.9f;
    // Raycasting Private Vars
    private float edgeGrabRaySpacing;
    // How far the hands should be placed apart from each other when hanging on an edge
    public float distanceBetweenHands = 0.5f;    

    // Will the player climb up into crouching position
    private bool crouchClimb;
    // Ignore input during this delay, so that the player does not automatically climb up
    public float edgeGrabInputDelay = 0.5f;
    // How long player has been in EdgeGrab position
    private float edgeGrabTimer;
    
    private Vector3 currentEdgePosition;
    private float edgeClimbProgress;
    public Vector3 edgeClimbPos0, edgeClimbPos1, edgeClimbPos2;
    private bool ikLeftFootClimbUp;
    private bool handAtEdge;

    // Edge Grab Move (Called from Update)
    private void EdgeGrabMove(Vector2 inputDir) {
        // Moving Up (player has jumped, and passed by a point they can grab on to)
        if (Mathf.Sign(velocityY) == 1f) {
            velocityY += gravity * Time.deltaTime;
            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            myCC.Move(velocity * Time.deltaTime);
        }
        else if (handAtEdge) {
            edgeGrabTimer += Time.deltaTime;
            // If the player is too far below the edge
            if (transform.position.y + 1f < currentEdgePosition.y) {
                // move the player back up into position
                Vector3 velocity = transform.forward * currentSpeed + Vector3.up;
                myCC.Move(velocity * Time.deltaTime);
            }

            // Player's Hand is at/below ledge, watch for input after custom delay
            if (inputDir != Vector2.zero && edgeGrabTimer >= edgeGrabInputDelay) {                
                // Determine difference between input direction and player direction
                Vector2 forwardDir = new Vector2(transform.forward.x, transform.forward.z);
                float angle = Vector2.Angle(forwardDir, inputDir);
                Vector3 cross = Vector3.Cross(forwardDir, inputDir);
                if (cross.z > 0f) angle = -angle;
                angle = Mathf.Abs(angle);                
                if (angle < 45f) {
                    BeginEdgeGrabClimbUp();
                }
                // If we've pushed "up" while holding an edge
                else if (inputDir.y > 0.9f && myAnim.GetCurrentAnimatorStateInfo(0).IsName("EdgeGrab_Idle")) {
                    BeginEdgeGrabClimbUp();
                }
                // If we've pushed "down" while holding an edge
                else if (inputDir.y < -0.1f && myAnim.GetCurrentAnimatorStateInfo(0).IsName("EdgeGrab_Idle")) {
                    BeginEdgeGrabDrop();
                }
                else {
                    // Make the player look in direction of input using IK on head.
                }
            }
        }
        // Moving Down (falling)
        else {
            //Debug.Log("Lefthand: " + handLeft.position.y + ", Righthand: " + handRight.position.y + ", Curedgepos: " + currentEdgePosition.y);
            // Moving Down, player's hand is above edge- keep falling down
            if (Mathf.Min(handLeft.position.y, handRight.position.y) - 0.2f > currentEdgePosition.y) {
                velocityY += gravity * Time.deltaTime;
                Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
                myCC.Move(velocity * Time.deltaTime);
                //Debug.Log("Hand above ledge, keep falling");
            }
            else {
                handAtEdge = true;
            }
        }
    }

    // Begin an Edge Grab Climb
    private void BeginEdgeGrabClimbUp() {        
        edgeClimbProgress = 0f;
        edgeClimbPos0 = transform.position;
        edgeClimbPos1 = new Vector3(transform.position.x, currentEdgePosition.y + edgeGrabClimbUp, transform.position.z);           
        edgeClimbPos2 = currentEdgePosition + (transform.forward * edgeGrabClimbForward);        
        Vector3 ccPoint1 = CalculateQuadraticBezierPoint(1f, edgeClimbPos0, edgeClimbPos1, edgeClimbPos2);
        Vector3 ccPoint2 = ccPoint1 + Vector3.up * standingHeight;
        RaycastHit climbUpHit;
        if (Physics.SphereCast(ccPoint1, myCC.radius, Vector3.up, out climbUpHit, Vector3.Distance(ccPoint1, ccPoint2), raycastCollisionMask)) {
            // The player cannot fit here, let's try crouching
            float distance = Vector3.Distance(ccPoint1, ccPoint1 + new Vector3(0f, ccCrouchingHeight, 0f));
            //Debug.DrawLine(ccPoint1, ccPoint1 + new Vector3(0f, distance, 0f));            
            if (Physics.SphereCast(ccPoint1, myCC.radius, Vector3.up, out climbUpHit, distance, raycastCollisionMask)) {
                //Debug.Log("Cannot fit crouching, hit distance: " + climbUpHit.distance);                
                return;
            }
            else {
                //Debug.Log("CAN fit crouching");
                //Debug.DrawLine(ccPoint1, ccPoint1 + new Vector3(0f, distance, 0f));
                crouchClimb = true;
                moveState = MoveState.EdgeGrabClimbUp;
                myAnim.SetBool("Crouching", true);
                myAnim.SetTrigger("EdgeGrabClimbUp");
            }
        }
        else {
            // Can fit normally 
            moveState = MoveState.EdgeGrabClimbUp;
            myAnim.SetTrigger("EdgeGrabClimbUp");
            crouchClimb = false;
        }
    }

    // Climb up from an edge Loop (Called from Update)
    private void EdgeGrabClimbUp() {
        // Player is in "EdgeGrab_ClimbUp" State
        AnimatorStateInfo animationState = myAnim.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = myAnim.GetCurrentAnimatorClipInfo(0);
        float animTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
        if (myAnim.IsInTransition(0) && !animationState.IsName("EdgeGrab_ClimbUp")) {
            // Player is Transitioning to "EdgeGrab_ClimbUp" State
            animationState = myAnim.GetNextAnimatorStateInfo(0);
            myAnimatorClip = myAnim.GetNextAnimatorClipInfo(0);
            animTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
        }
        // Player has finished climbing up
        if (grounded || animTime >= 1f) {
            if (!crouchClimb) {                
                moveState = MoveState.Standing;
            }
            else {
                moveState = MoveState.Crouching;
            }
            edgeClimbProgress = 0f;
            ikLeftFootClimbUp = false;
        }
        // Follow a bezier curve to climb up the edge
        else {            
            edgeClimbProgress = animTime;            
            Vector3 target = CalculateQuadraticBezierPoint(edgeClimbProgress, edgeClimbPos0, edgeClimbPos1, edgeClimbPos2);
            Vector3 velocity = target - transform.position;
            velocity = velocity.normalized * (0.01f * edgeGrabClimbSpeed);
            myCC.Move(velocity);
            if (edgeClimbProgress > 0.2f) {
                ikLeftFootClimbUp = true;
            }
            if (edgeClimbProgress > 0.6f) {
                handAtEdge = false;
            }
        }
    }

    // Begin a drop from an Edge
    private void BeginEdgeGrabDrop() {
        moveState = MoveState.EdgeGrabDrop;        
        myAnim.SetTrigger("EdgeGrabDrop");        
    }

    // Edge Grab Drop Loop (Called from Update)
    private void EdgeGrabDrop(Vector2 inputDir) {
        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        myCC.Move(velocity * Time.deltaTime);
        if (grounded) { velocityY = 0f; }
        currentSpeed = new Vector2(myCC.velocity.x, myCC.velocity.z).magnitude;

        if (!myAnim.GetCurrentAnimatorStateInfo(0).IsName("EdgeGrab_Dismount") && !myAnim.GetCurrentAnimatorStateInfo(0).IsName("EdgeGrab_Idle")) {
            moveState = MoveState.Standing;
        }        
    }

    // Edge Grab Raycast (Called from ColliderHit)
    private void EdgeGrabRaycast(Transform objectToCheck) {
        Collider objectCollider = objectToCheck.GetComponent<Collider>();
        float[] rayDistances = new float[edgeGrabRayCount];
        Vector3[] rayPoints = new Vector3[edgeGrabRayCount];

        for (int i = 0; i < edgeGrabRayCount; i++) {
            if (distanceToGround > jumpLandRayLength) {
                if (debugMode) Debug.DrawRay(transform.position + new Vector3(0f, (edgeGrabBoundsBottom) + (edgeGrabRaySpacing * i), 0f), transform.forward * edgeGrabBoundsRadius, Color.red);
                RaycastHit rayHit;
                if (Physics.Raycast(transform.position + new Vector3(0f, (edgeGrabBoundsBottom) + (edgeGrabRaySpacing * i), 0f), transform.forward, out rayHit, edgeGrabBoundsRadius, raycastCollisionMask)) {                    
                    rayDistances[i] = rayHit.distance;
                    rayPoints[i] = rayHit.point;
                }
                else {
                    rayDistances[i] = edgeGrabBoundsRadius;
                }
            }
        }

        for (int i = 1; i < edgeGrabRayCount; i++) {
            if (rayDistances[i] > rayDistances[i - 1]) {                
                if (rayDistances[i] - rayDistances[i - 1] > edgeGrabMinEdgeSize) {
                    currentEdgePosition = rayPoints[i - 1] + new Vector3(0f, edgeGrabRaySpacing, 0f);
                    currentEdgePosition = objectCollider.ClosestPointOnBounds(currentEdgePosition);                       

                    // Spherecast here, if there is a collision do not allow edge grab
                    edgeClimbProgress = 0f;
                    edgeClimbPos0 = transform.position;
                    edgeClimbPos1 = new Vector3(transform.position.x, currentEdgePosition.y + edgeGrabClimbUp, transform.position.z);
                    edgeClimbPos2 = currentEdgePosition + (transform.forward * edgeGrabClimbForward);

                    Vector3 ccPoint1 = CalculateQuadraticBezierPoint(1f, edgeClimbPos0, edgeClimbPos1, edgeClimbPos2);
                    RaycastHit edgeGrabHit;
                    float distance = Vector3.Distance(ccPoint1, ccPoint1 + new Vector3(0f, ccCrouchingHeight, 0f));
                    if (debugMode) Debug.DrawLine(ccPoint1, ccPoint1 + new Vector3(0f, distance, 0f));
                    Vector3 headingAB = edgeClimbPos1 - edgeClimbPos0;
                    float distanceAB = headingAB.magnitude;
                    Vector3 directionAB = headingAB / distanceAB;
                    Vector3 headingBC = edgeClimbPos2 - edgeClimbPos1;
                    float distanceBC = headingBC.magnitude;
                    //Debug.DrawRay(edgeClimbPos0, directionAB, Color.green, 3f);
                    //Debug.DrawRay(edgeClimbPos1, transform.forward, Color.green, 3f);

                    if (Physics.Raycast(edgeClimbPos0, directionAB, out edgeGrabHit, distanceAB, raycastCollisionMask)) {
                        if (edgeGrabHit.collider.gameObject != gameObject) {
                            //Debug.Log("Hit something from point A to B.");
                            //Debug.DrawRay(edgeClimbPos0, directionAB);
                            return;
                        }
                    }
                    else if (Physics.Raycast(edgeClimbPos1, transform.forward, out edgeGrabHit, distanceBC, raycastCollisionMask)) {
                        if (edgeGrabHit.collider.gameObject != gameObject) {
                            //Debug.Log("Hit something from point B to C.");
                            //Debug.DrawRay(edgeClimbPos1, directionBC);
                            return;
                        }
                    }
                    else if (Physics.Raycast(edgeClimbPos2, Vector3.up, distance, raycastCollisionMask)) {
                        //Debug.Log("Hit something with raycast where I should be climbing up");
                        return;
                    }
                    else if (Physics.SphereCast(ccPoint1, myCC.radius, Vector3.up, out edgeGrabHit, distance, raycastCollisionMask)) {
                        //Debug.Log("Hit something with spherecast where I should be climbing up ");
                        return;
                    }
                    else {
                        //Debug.Log("No hits, allow player to climb up");
                    }

                    myAnim.SetTrigger("EdgeGrab");
                    velocityY = 0f;

                    if (debugMode) {
                        Debug.DrawLine(objectToCheck.position, objectCollider.ClosestPointOnBounds(currentEdgePosition), Color.cyan);
                        GameObject helper = GameObject.Find("EdgeGrabPosition");
                        if (helper == null) { helper = new GameObject("EdgeGrabPosition"); }
                        helper.transform.position = currentEdgePosition;
                        IconManager.SetIcon(helper, IconManager.Icon.DiamondYellow);
                    }
                    ResetIK();
                    edgeGrabTimer = 0f;
                    if (moveState == MoveState.ClimbingLadder) {
                        myAnim.SetBool("ClimbingLadder", false);
                        StartCoroutine(ResetLadder(0.5f));
                        BeginEdgeGrabClimbUp();
                    }
                    else {
                        moveState = MoveState.EdgeGrab;
                    }

                    break;
                }
            }
        }
    }
}
