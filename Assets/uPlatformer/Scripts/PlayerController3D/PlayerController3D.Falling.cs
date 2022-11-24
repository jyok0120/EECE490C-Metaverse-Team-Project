using UnityEngine;
using System.Collections;

public partial class PlayerController3D {
    // How much control of movement does player have while falling in the air
    [Range(0f, 1f)]
    public float airControlPercent = 0.5f;
    // Distance from player to the ground, to begin the "landing jump" animation
    public float jumpLandRayLength = 0.5f;
    // Is the playering landing a jump
    private bool landingJump;

    // Movement while falling
    private void FallingMove(Vector2 inputDir) {
        if (inputDir != Vector2.zero) {
            if (lockZAxis) { inputDir.y = 0f; }
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = forward * currentSpeed + Vector3.up * velocityY;
        myCC.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(myCC.velocity.x, myCC.velocity.z).magnitude;

        /* Spherecast for landing a jump
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, myCC.radius + myCC.skinWidth), -Vector3.up, out hit, 10000f, raycastCollisionMask)) {
            distanceToGround = hit.distance;
        }
        else if (Physics.SphereCast(transform.position + new Vector3(0f, myCC.radius + myCC.skinWidth), myCC.radius, -Vector3.up, out hit, 10000f, raycastCollisionMask)) {
            distanceToGround = hit.distance;
        }
        else {
            distanceToGround = 10000f;
        }*/
        if (distanceToGround < jumpLandRayLength) {
            if (grounded) {
                landingJump = false;
                moveState = MoveState.Standing;
                jumpCount = 0;
                velocityY = 0f;
                return;
            }
            else if (!landingJump && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Falling")) {
                Debug.Log("landing a jump");
                myAnim.SetTrigger("LandingJump");
                landingJump = true;
            }
        }
        if (debugMode) Debug.DrawRay(transform.position, -Vector3.up * jumpLandRayLength, Color.red);
    }
}
