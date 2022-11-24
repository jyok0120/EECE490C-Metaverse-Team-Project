using UnityEngine;
using System.Collections;

public partial class PlayerController3D {	
    // Allow ladder climbing
    public bool allowClimbingLadder;    
    // How far player will cast in front of them for a ladder
    public float ladderCastLength = 0.05f;
    // How fast player will climb ladders
    public float climbLadderSpeed = 1.5f;
    // The gameobject of current ladder being climbed
    public GameObject currentLadder;

    private Vector3 currentLadderNormal;
    private bool hasMovedDownOnLadder;

    // Movement while standing (called from Update)
    private void ClimbingLadderMove(Vector2 inputDir) {
        if (hasMovedDownOnLadder && grounded) { LadderDismount(); return; }
        // Make the player rotate to face the ladder
        float step = Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, -currentLadderNormal, step, 0.0F);
        Quaternion targetQuaternion = Quaternion.LookRotation(newDir);
        transform.eulerAngles = new Vector3(0f, targetQuaternion.eulerAngles.y, 0f);
        // Move player towards center of ladder
        if (Mathf.RoundToInt(currentLadderNormal.x) != 0f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, currentLadder.GetComponent<Collider>().bounds.center.z), Time.deltaTime);
        }
        else if (Mathf.RoundToInt(currentLadderNormal.z) != 0f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(currentLadder.GetComponent<Collider>().bounds.center.x, transform.position.y, transform.position.z), Time.deltaTime);
        }

        float targetSpeed = 0f;
        if (inputDir != Vector2.zero) {
            // Set distance to ground
            RaycastHit groundHit;
            if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 10000f, raycastCollisionMask)) {
                distanceToGround = groundHit.distance;
            } else { distanceToGround = 10000f; }

            // Determine difference between input direction and ladder direction
            Vector2 forwardDir = new Vector2(transform.forward.x, transform.forward.z);
            float angle = Vector2.Angle(forwardDir, inputDir);                       
            Vector3 cross = Vector3.zero;
            if (forwardDir.x >= 0f) {
                cross = Vector3.Cross(inputDir, forwardDir);                
            } else {
                cross = Vector3.Cross(forwardDir, inputDir);
            }
            if (cross.z > 0f) angle = -angle;            

            // Climb Up
            if (angle > -45f && angle < 95f) {                
                inputDir.x = Mathf.Abs(inputDir.x);                
                targetSpeed = Mathf.Max(inputDir.x, inputDir.y) * climbLadderSpeed;
                EdgeGrabRaycast(currentLadder.transform);
            
            // Climb Down    
            } else if (angle < 0f && angle > -110f) {
                hasMovedDownOnLadder = true;
                targetSpeed = inputDir.y * climbLadderSpeed;
                if (grounded) { LadderDismount(); }  
            }            
            angle = Mathf.Abs(angle);
            
            // Turn around and fall off
            if (angle > 110f) {                
                //Debug.Log("Turn around & fall off");
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
                LadderDismount();
            }
        }        
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        Vector3 velocity = Vector3.up * currentSpeed;
        myCC.Move(velocity * Time.deltaTime);
    }

    // Check for a Ladder in front of player (called from ColliderHit)
    private bool CheckForLadder() {        
        bool ladderInFront = false;
        for (int i = 0; i < edgeGrabRayCount; i++) {            
            if (debugMode) Debug.DrawRay(transform.position + new Vector3(0f, (edgeGrabBoundsBottom - edgeGrabRaySpacing) + (edgeGrabRaySpacing * i), 0f), transform.forward * (myCC.radius + ladderCastLength), Color.red);
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position + new Vector3(0f, (edgeGrabBoundsBottom - edgeGrabRaySpacing) + (edgeGrabRaySpacing * i), 0f), transform.forward, out raycastHit, myCC.radius + ladderCastLength, raycastCollisionMask)) {                
                if (raycastHit.transform.gameObject.CompareTag("Ladder")) {
                    currentLadder = raycastHit.transform.gameObject;
                    currentLadderNormal = raycastHit.normal;                    
                    moveState = MoveState.ClimbingLadder;
                    myAnim.SetBool("ClimbingLadder", true);
                    ladderInFront = true;
                    break;
                }
            }            
        }
        return ladderInFront;
    }

    // Call when dismounting ladder
    private void LadderDismount() {
        myAnim.SetBool("ClimbingLadder", false);
        StartCoroutine(ResetLadder(0.5f));
        if (grounded) {
            moveState = MoveState.Standing;
        }
        else {
            moveState = MoveState.Falling;
        }
    }

    // There must be a small delay when dismounting the ladder or player will immediately grab back onto the ladder when a dismount occurs
    private IEnumerator ResetLadder(float delay) {
        yield return new WaitForSeconds(delay);
        hasMovedDownOnLadder = false;
        currentLadder = null;
        currentLadderNormal = Vector3.zero;
    }
}
