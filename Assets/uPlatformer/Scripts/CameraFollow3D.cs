using UnityEngine;
using System.Collections;

public class CameraFollow3D : MonoBehaviour {

    public PlayerController3D player;
    public Vector3 focusAreaSize = new Vector3(5f,5f,5f);
    public float zAxisOffset = 10f;
    public float verticalOffset = 1f;
    public float lookAheadDistanceX = 1f;
    public float lookSmoothTimeX = 1f;
    public float verticalSmoothTime = 0.2f;

    public FocusArea focusArea;
    private CharacterController playerCC;
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    private bool lookAheadStopped;

    public struct FocusArea {
        public Vector3 center;
        public Vector3 velocity;
        float left, right, top, bottom, near, far;

        public FocusArea(Bounds targetBounds, Vector3 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;
            near = targetBounds.center.z - size.z / 2;
            far = targetBounds.center.z + size.z / 2;
            velocity = Vector3.zero;
            center = new Vector3((left + right) / 2, (top + bottom) / 2, (near + far) / 2);
        }

        public void Update(Bounds targetBounds) {
            float shiftX = 0f;
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0f;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            float shiftZ = 0f;
            if (targetBounds.min.z < near) {
                shiftZ = targetBounds.min.z - near;
            }
            else if (targetBounds.max.z > far) {
                shiftZ = targetBounds.max.z - far;
            }
            near += shiftZ;
            far += shiftZ;
            center = new Vector3((left + right) / 2, (top + bottom) / 2, (near + far) / 2);
            velocity = new Vector3(shiftX, shiftY, shiftZ);
        }
    }
    // Use this for initialization
    void Start() {
        playerCC = player.gameObject.GetComponent<CharacterController>();
        focusArea = new FocusArea(playerCC.bounds, focusAreaSize);
    }

    // Update is called once per frame
    void LateUpdate () {
        focusArea.Update(playerCC.bounds);
        Vector3 focusPosition = focusArea.center + Vector3.up * verticalOffset;

        if (focusArea.velocity.x != 0f) {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(player.currentNormalizedInput.x) == Mathf.Sign(focusArea.velocity.x) && player.currentNormalizedInput.x != 0f) {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
            }
            else {
                if (!lookAheadStopped) {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistanceX - currentLookAheadX) / 4f;
                }
                
            }
        }

        targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

        focusPosition += Vector3.right * currentLookAheadX;

        transform.position = focusPosition + Vector3.forward * -zAxisOffset;
	}
}
