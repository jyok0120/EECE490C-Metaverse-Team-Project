using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public partial class PlayerController3D {
	// Current state of the player
    public MoveState moveState = MoveState.Standing;
    // Player ID, can be used for multiplayer
    public int playerId;
    // If the player is grounded, in most cases this should be used instead of CharacterController.isGrounded
    public bool grounded;
    // How far player is from the ground
    public float distanceToGround;
    // Ground Status: -1 is downhill, 0 is flat, 1 is uphill
    public float groundStatus;    
    // How much angle in degrees to ignore when calculating if ground is flat or not (avoids setting 'uphill/downhill' when angle is too low)
    public float ignoreSlopeAngle = 5f;
    // Enum for all move states
    public enum MoveState { Standing, Jumping, Falling, Crouching, EdgeGrab, EdgeGrabClimbUp, EdgeGrabDrop, ClimbingLadder };
    // Should rays and other edits hints be shown in the Scene View?
    public bool debugMode = true;
    // The collision mask for all raycasts
    public LayerMask raycastCollisionMask = -1;
    // Player's left hand, right hand, toeLeft, toeRight, and backOfHead Transforms.
    // TODO: populate first 4 automatically via HumanBones
    public Transform handLeft, handRight, toeLeft, toeRight, backOfHead;
    // Input from controller / keyboard    
	public Vector2 currentNormalizedInput;
    // Only used for the inspector, to save which tab is currently open
    public int inspectorTabTop, inspectorTabBottom;
    // Should player be locked in the Z axis
    public bool lockZAxis;
    // The z axis the player will be locked into
    public float zAxis;
    
    private CharacterController myCC;
    private Animator myAnim;
    private float gravity;
	private float velocityY;
    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;       
    private bool running;
    private Vector3 forward;
    private Vector2 currentSmoothedInput;
    private Vector2 smoothedInputVelocity;
           
    // Center of body position, for setting Character Controller's center var
    private Vector3 bodyCenter;
    // Height of player when standing
    private float standingHeight;    
    // Cache the Input Manager on start
    private IInputManager input;

    // SphereCastData and CapsuleCastData are only used for Debug Mode to draw SphereCasts and CapsuleCasts in the Editor Window
    public struct SphereCastData {
        public Vector3 start;
        public Vector3 end;
        public float radius;
    }
    public List<SphereCastData> allSphereCastData = new List<SphereCastData>();

    public struct CapsuleCastData {
        public Vector3 point1;
        public Vector3 point2;
        public Vector3 point1end;
        public Vector3 point2end;        
        public float radius;
    }
    public List<CapsuleCastData> allCapsuleCastData = new List<CapsuleCastData>();

}
