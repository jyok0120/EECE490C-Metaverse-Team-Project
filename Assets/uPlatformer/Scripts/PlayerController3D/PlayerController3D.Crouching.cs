using UnityEngine;
using System.Collections;

public partial class PlayerController3D {    
    // Crouching movement speed
    public float crouchSpeed = 2f;
    // If the Character Controller should have static bound size when crouching
    public bool staticCCBoundsCrouching = true;
    // The static height of character controller when crouching
    public float ccCrouchingHeight = 0.90f;
    // The size of the raycast which determines if player can stand up or not while crouching
    public float crouchingHeadClearance = 1.5f;

    // Movement while crouching
    private void CrouchMove(Vector2 inputDir) {
        if (inputDir != Vector2.zero) {
            if (lockZAxis) { inputDir.y = 0f; }
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }        
        float targetSpeed = crouchSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        myCC.Move(velocity * Time.deltaTime);
        if (grounded) { velocityY = 0f; }
        currentSpeed = new Vector2(myCC.velocity.x, myCC.velocity.z).magnitude;
    }
}