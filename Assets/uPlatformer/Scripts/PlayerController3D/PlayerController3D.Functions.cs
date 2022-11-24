using UnityEngine;
using System.Collections;

public partial class PlayerController3D {

    // Ground Check
    private void CheckForGround() {
        RaycastHit hit;
        // It might not work casting for length of 10,000 because it will always hit and sphere cast will never happen
        if (Physics.Raycast(transform.position + new Vector3(0f, myCC.skinWidth, 0f), Vector3.down, out hit, 10000f, raycastCollisionMask)) {
            distanceToGround = hit.distance;
            if (hit.distance <= 0.05f) {
                grounded = true;
                forward = Vector3.Cross(transform.right, hit.normal);
                SetGroundStatus();
                if (moveState == MoveState.Falling) {
                    moveState = MoveState.Standing;
                    jumpCount = 0;
                    velocityY = 0f;
                }
                return;
            }
        }
        if (Physics.SphereCast(transform.position + new Vector3(0f, myCC.radius + myCC.skinWidth, 0f), myCC.radius, Vector3.down, out hit, 10000f, raycastCollisionMask) && moveState != MoveState.EdgeGrabClimbUp) {
            distanceToGround = hit.distance;
            if (hit.distance <= 0.05f) {
                grounded = true;
                if (Physics.Raycast(transform.position + new Vector3(0f, myCC.skinWidth, 0f), Vector3.down, out hit, 10000f + myCC.stepOffset, raycastCollisionMask)) {
                    forward = Vector3.Cross(transform.right, hit.normal);
                }
                else {
                    forward = transform.forward;
                }
                SetGroundStatus();
                if (moveState == MoveState.Falling) {
                    moveState = MoveState.Standing;
                    jumpCount = 0;
                    velocityY = 0f;
                }
                return;
            }
        }
        else {
            distanceToGround = 10000f;
            SetGroundStatus();
        }
        // Player is not grounded        
        grounded = false;
        forward = transform.forward;
        if (moveState == MoveState.Standing) {
            moveState = MoveState.Falling;
        }
        else if (moveState == MoveState.Crouching) {
            myAnim.SetBool("Crouching", false);
            moveState = MoveState.Falling;
        }
        else if (moveState == MoveState.Falling) {
            if (myAnim.GetBool("Crouching") == true) { myAnim.SetBool("Crouching", false); }
        }        
    }

    private void SetGroundStatus() {
        float angle = Vector3.Angle(transform.forward, forward);
        Vector3 cross = Vector3.Cross(Vector3.forward, forward);
        if (cross.x > 0) angle = -angle;        
        // Uphill
        if (angle > ignoreSlopeAngle) {
            groundStatus = 1f;
            forward = transform.forward;
        }
        // Downhill
        else if (angle < -ignoreSlopeAngle) {
            groundStatus = -1f;
        }
        // Flat ground
        else {
            groundStatus = 0f;
            forward = transform.forward;
        }
    }

    
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private float GetModifiedSmoothTime(float smoothTime) {
        if (grounded) {
            return smoothTime;
        }
        if (airControlPercent == 0) {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    //SphereCast Example
    /*
    float castDistance = ladderCastLength;
    Vector3 direction = forward;
    Vector3 point1 = (transform.position + new Vector3(0f, myCC.radius + myCC.skinWidth, 0f)) + direction * (myCC.radius - ladderCastRadius);
    Vector3 point2 = point1 + new Vector3(0f, myCC.height - myCC.skinWidth - (myCC.radius * 2f), 0f);
    RaycastHit raycastHit;
    if (Physics.CapsuleCast(point1, point2, ladderCastRadius, direction, out raycastHit, castDistance, raycastCollisionMask)) {
        castDistance = raycastHit.distance;
        if (raycastHit.transform.gameObject.CompareTag("Ladder")) {
            currentLadder = raycastHit.transform.gameObject;
            currentLadderNormal = raycastHit.normal;
            Debug.Log(currentLadderNormal.y);
            moveState = MoveState.ClimbingLadder;
            myAnim.SetBool("ClimbingLadder", true);
            ladderInFront = true;
        }
    }
    if (debugMode) {
        CapsuleCastData capsuleCastData = new CapsuleCastData();
        capsuleCastData.point1 = point1;
        capsuleCastData.point2 = point2;
        capsuleCastData.point1end = point1 + direction * castDistance;
        capsuleCastData.point2end = point2 + direction * castDistance;
        capsuleCastData.radius = ladderCastRadius;
        allCapsuleCastData.Add(capsuleCastData);
    }*/

}
