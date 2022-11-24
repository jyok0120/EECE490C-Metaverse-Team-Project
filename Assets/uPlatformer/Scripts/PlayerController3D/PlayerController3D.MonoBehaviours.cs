using UnityEngine;
using System.Collections;

public partial class PlayerController3D : MonoBehaviour {
        
    // Use this for initialization
    void Start() {
        input = InputManager.instance;
        myCC = GetComponent<CharacterController>();
        myAnim = GetComponent<Animator>();
        myAnim.stabilizeFeet = true;
        gravity = Physics.gravity.y;
        edgeGrabRayCount = Mathf.Clamp(edgeGrabRayCount, 2, int.MaxValue);
        edgeGrabRaySpacing = (edgeGrabBoundsTop - edgeGrabBoundsBottom) / (edgeGrabRayCount - 1f);        
        standingHeight = Mathf.Round(Vector3.Distance(transform.position, new Vector3(0f, backOfHead.position.y, 0f)) * 100f) / 100f ;
    }

    // Update is called once per frame
    void Update() {
        currentNormalizedInput = new Vector2(input.GetAxis(playerId, InputAction.MoveHorizontal), input.GetAxis(playerId, InputAction.MoveVertical)).normalized;
        currentSmoothedInput = Vector2.SmoothDamp(currentSmoothedInput, currentNormalizedInput, ref smoothedInputVelocity, inputSmoothTime, Mathf.Infinity, Time.deltaTime);
        running = input.GetButton(playerId, InputAction.Run);
        if (debugMode) { allSphereCastData.Clear(); allCapsuleCastData.Clear(); }
        CheckForGround();    
        if (debugMode) { Debug.DrawRay(transform.position + new Vector3(0f, myCC.radius + myCC.skinWidth), forward); }
        
        // Jump Input
        if (input.GetButton(playerId, InputAction.Jump) && !waitingToJump && moveState != MoveState.Jumping) {
            switch (moveState) {
                case MoveState.Standing:
                    waitingToJump = true;
                    myAnim.SetTrigger("Jump");
                    StartCoroutine(Jump(jumpDelay));
                    break;
                case MoveState.Crouching:         
                    break;
                case MoveState.EdgeGrab:
                    break;
                case MoveState.EdgeGrabClimbUp:
                    break;
                case MoveState.EdgeGrabDrop:
                    break;
                case MoveState.ClimbingLadder:
                    // Jump from ladder position
                    myAnim.SetBool("ClimbingLadder", false);
                    StartCoroutine(ResetLadder(0.5f));
                    waitingToJump = true;
                    myAnim.SetTrigger("Jump");
                    StartCoroutine(Jump(0f));
                    break;
                case MoveState.Falling:
                    if (doubleJumpEnabled && jumpCount <= 1) {
                        waitingToJump = true;
                        //myAnim.SetTrigger("Jump");
                        StartCoroutine(Jump(0f));
                    }
                    break;
            }
        }

        switch (moveState) {
            case MoveState.Standing:
                StandingMove(currentSmoothedInput);
                break;
            case MoveState.Jumping:
                JumpingMove(currentSmoothedInput);
                break;
            case MoveState.Falling:
                FallingMove(currentSmoothedInput);
                break;
            case MoveState.EdgeGrab:
                EdgeGrabMove(currentNormalizedInput);
                break;
            case MoveState.EdgeGrabClimbUp:
                EdgeGrabClimbUp();
                break;
            case MoveState.EdgeGrabDrop:
                EdgeGrabDrop(currentSmoothedInput);
                break;
            case MoveState.Crouching:
                CrouchMove(currentSmoothedInput);
                break;
            case MoveState.ClimbingLadder:
                ClimbingLadderMove(currentNormalizedInput);
                break;
        }

        if (lockZAxis == true) {
            transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
        }

        // Animator Control
        float animSpeedPercent = currentSpeed / runSpeed;
        myAnim.SetFloat("Speed", animSpeedPercent, speedSmoothTime, Time.deltaTime);

        // Grounded for animations
        if (grounded == true) {
            if (myAnim.GetBool("Grounded") == false) myAnim.SetBool("Grounded", true);
            if (landingJump == true) landingJump = false;            
            // Crouch Input
            if (input.GetButton(playerId, InputAction.Crouch)) {
                if (moveState == MoveState.ClimbingLadder) {
                    LadderDismount();
                }
                else {
                    moveState = MoveState.Crouching;
                    myAnim.SetBool("Crouching", true);
                }                
            }
            else {
                if (moveState == MoveState.Crouching) {
                    // Spherecast for head clearance
                    Vector3 end = new Vector3(transform.position.x, transform.position.y + crouchingHeadClearance, transform.position.z);
                    RaycastHit crouchHeadhit;                    
                    if (Physics.SphereCast(transform.position, myCC.radius, Vector3.up, out crouchHeadhit, Vector3.Distance(transform.position, end), raycastCollisionMask)) {
                        // Something above player's head, do not stand up
                    }
                    else {
                        if (grounded) {
                            moveState = MoveState.Standing;
                            myAnim.SetBool("Crouching", false);
                        }
                        else {
                            moveState = MoveState.Falling;
                            myAnim.SetBool("Crouching", false);
                        }                        
                    }
                }
            }
        }
        else {
            myAnim.SetBool("Grounded", false);
        }

        // Dynamic Character Controller Bounds
        //bodyY = Mathf.Max(Vector3.Distance(toeLeft.position, backOfHead.position), Vector3.Distance(toeRight.position, backOfHead.position)); // Not using Toe position anymore
        float bodyY = 0f;
        if (staticCCBoundsCrouching && moveState == MoveState.Crouching) {
            bodyY = ccCrouchingHeight;
        } else if (staticCCBoundsClimbing && moveState == MoveState.EdgeGrabClimbUp) {            
            bodyY = ccClimbingHeight;
        } else {
            bodyY = Vector3.Distance(transform.position, backOfHead.position);
        }
        bodyCenter = new Vector3(transform.position.x, transform.position.y + bodyY / 2, transform.position.z);
        myCC.height = bodyY;
        myCC.center = transform.InverseTransformPoint(bodyCenter);
    }

    // Check for edge climing here!    
    void OnControllerColliderHit(ControllerColliderHit hit) {
        // Check if a Ladder was hit
        if (allowClimbingLadder && moveState != MoveState.EdgeGrabClimbUp && !currentLadder && !waitingToJump) {
            if (CheckForLadder()) { return; }
        }

        // Check for and edge to grab on to
        if (grounded == true || moveState == MoveState.EdgeGrab || moveState == MoveState.EdgeGrabClimbUp || moveState == MoveState.EdgeGrabDrop) return;
        EdgeGrabRaycast(hit.transform);
    }

    // Draw Gizmos in Scene View (DebugMode Only)
    private void OnDrawGizmosSelected() {
        if (debugMode) {
            Gizmos.color = Color.red;
            foreach (SphereCastData i in allSphereCastData) {
                Debug.DrawLine(i.start, i.end);
                Gizmos.DrawWireSphere(i.end, i.radius);
            }
            foreach (CapsuleCastData i in allCapsuleCastData) {
                Gizmos.DrawWireSphere(i.point1end, i.radius);
                Gizmos.DrawWireSphere(i.point2end, i.radius);
                Debug.DrawLine(i.point1end, i.point2end, Color.red);
            }
        }        
        //Debug.DrawLine(origin, origin + direction * currentHitDistance);
        //Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
