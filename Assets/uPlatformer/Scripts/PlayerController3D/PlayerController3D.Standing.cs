using UnityEngine;
using System.Collections;

public partial class PlayerController3D {
    // Player's walking speed
    public float walkSpeed = 3f;
    // Player's running speed
    public float runSpeed = 6f;
    // How fast turning movements will be smoothed
    public float turnSmoothTime = 0.2f;
    // How fast position movements will be smoothed
    public float speedSmoothTime = 0.1f;
    // How much input will be smoothed
    public float inputSmoothTime = 0.1f;

    // Movement while standing
    private void StandingMove(Vector2 inputDir) {
        if (inputDir != Vector2.zero) {
            if (lockZAxis == true) { inputDir.y = 0f; }
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        Vector3 velocity = forward * currentSpeed;
        myCC.Move(velocity * Time.deltaTime);

        if (groundStatus >= 0f) {
            currentSpeed = new Vector2(myCC.velocity.x, myCC.velocity.z).magnitude;
        }
        else {
            currentSpeed = new Vector3(myCC.velocity.x, myCC.velocity.y, myCC.velocity.z).magnitude;
        }
    }
}
