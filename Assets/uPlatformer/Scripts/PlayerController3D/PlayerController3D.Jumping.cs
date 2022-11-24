using UnityEngine;
using System.Collections;

public partial class PlayerController3D {
    // How high the player will go when pressing jump, in world units
    public float jumpHeight = 1f;
    // A small delay from pressing jump button to actually having player leave the ground
    public float jumpDelay = 0.3f;
    // Should the player be able to double jump?
    public bool doubleJumpEnabled = false;
    // How many times has the player jumped without touching ground
    private int jumpCount;
    // Jump button has been pressed, and jump delay is active
    private bool waitingToJump;

    // Movement while jumping
    private void JumpingMove(Vector2 inputDir) {
        if (velocityY > 0f) {
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
        }
        else {
            moveState = MoveState.Falling;
        }
    }

    private IEnumerator Jump(float delay) {
        yield return new WaitForSeconds(delay);
        if (waitingToJump) {
            waitingToJump = false;
            float jumpVelocity = Mathf.Sqrt(-2f * gravity * jumpHeight);
            velocityY = jumpVelocity;
            jumpCount++;
            moveState = MoveState.Jumping;
        }
    }
}
