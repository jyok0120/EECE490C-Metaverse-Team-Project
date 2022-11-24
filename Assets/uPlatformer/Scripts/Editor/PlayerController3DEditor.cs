using UnityEngine;
using UnityEditor;
using System.Collections;
[System.Serializable]
[CustomEditor(typeof(PlayerController3D))]
public class PlayerController3DEditor : Editor {
    PlayerController3D myTarget;
    private SerializedObject SO_Target;   

    private SerializedProperty walkSpeed;
    private SerializedProperty runSpeed;
    private SerializedProperty turnSmoothTime;
    private SerializedProperty speedSmoothTime;
    private SerializedProperty inputSmoothTime;
    private SerializedProperty lockZAxis;
    private SerializedProperty zAxis;

    private SerializedProperty crouchSpeed;
    private SerializedProperty staticCCBoundsCrouching;
    private SerializedProperty ccCrouchingHeight;
    private SerializedProperty crouchingHeadClearance;

    private SerializedProperty jumpHeight;
    private SerializedProperty jumpDelay;
    private SerializedProperty jumpLandRayLength;
    private SerializedProperty doubleJumpEnabled;
    
    private SerializedProperty airControlPercent;

    private SerializedProperty debugMode;
    private SerializedProperty raycastCollisionMask;
    private SerializedProperty playerId;

    private SerializedProperty edgeGrabClimbSpeed;
    private SerializedProperty edgeGrabClimbForward;
    private SerializedProperty edgeGrabClimbUp;
    private SerializedProperty edgeGrabBoundsBottom;
    private SerializedProperty edgeGrabBoundsTop;
    private SerializedProperty edgeGrabBoundsRadius;
    private SerializedProperty edgeGrabMinEdgeSize;        
    private SerializedProperty staticCCBoundsClimbing;
    private SerializedProperty ccClimbingHeight;
    private SerializedProperty edgeGrabRayCount;    

    private SerializedProperty handLeft;
    private SerializedProperty handRight;
    //private SerializedProperty toeLeft;
    //private SerializedProperty toeRight;
    private SerializedProperty backOfHead;

    private SerializedProperty allowClimbingLadder;    
    private SerializedProperty ladderCastLength;
    private SerializedProperty climbLadderSpeed;
	//private SerializedProperty currentLadder;

    private string currentInspectorTab;
        
    private void OnEnable() {
        myTarget = (PlayerController3D)target;
        SO_Target = new SerializedObject(target);

        // Movement
        walkSpeed = SO_Target.FindProperty("walkSpeed");
        runSpeed = SO_Target.FindProperty("runSpeed");
        turnSmoothTime = SO_Target.FindProperty("turnSmoothTime");
        speedSmoothTime = SO_Target.FindProperty("speedSmoothTime");
        inputSmoothTime = SO_Target.FindProperty("inputSmoothTime");
        lockZAxis = SO_Target.FindProperty("lockZAxis");
        zAxis = SO_Target.FindProperty("zAxis");

        // Crouching
        crouchSpeed = SO_Target.FindProperty("crouchSpeed");
        staticCCBoundsCrouching = SO_Target.FindProperty("staticCCBoundsCrouching");
        ccCrouchingHeight = SO_Target.FindProperty("ccCrouchingHeight");
        crouchingHeadClearance = SO_Target.FindProperty("crouchingHeadClearance");

        // Jumping
        jumpHeight = SO_Target.FindProperty("jumpHeight");
        jumpDelay = SO_Target.FindProperty("jumpDelay");
        jumpLandRayLength = SO_Target.FindProperty("jumpLandRayLength");
        doubleJumpEnabled = SO_Target.FindProperty("doubleJumpEnabled");

        // Falling        
        airControlPercent = SO_Target.FindProperty("airControlPercent");

        // Raycasting
        debugMode = SO_Target.FindProperty("debugMode");
        raycastCollisionMask = SO_Target.FindProperty("raycastCollisionMask");
        playerId = SO_Target.FindProperty("playerId");

        // Edge Grab        
        edgeGrabClimbSpeed = SO_Target.FindProperty("edgeGrabClimbSpeed");
        edgeGrabClimbForward = SO_Target.FindProperty("edgeGrabClimbForward");
        edgeGrabClimbUp = SO_Target.FindProperty("edgeGrabClimbUp");
        edgeGrabBoundsBottom = SO_Target.FindProperty("edgeGrabBoundsBottom");
        edgeGrabBoundsTop = SO_Target.FindProperty("edgeGrabBoundsTop");
        edgeGrabBoundsRadius = SO_Target.FindProperty("edgeGrabBoundsRadius");
        edgeGrabMinEdgeSize = SO_Target.FindProperty("edgeGrabMinEdgeSize");                
        edgeGrabRayCount = SO_Target.FindProperty("edgeGrabRayCount");
        staticCCBoundsClimbing = SO_Target.FindProperty("staticCCBoundsClimbing");
        ccClimbingHeight = SO_Target.FindProperty("ccClimbingHeight");        

        // Limbs
        handLeft = SO_Target.FindProperty("handLeft");
        handRight = SO_Target.FindProperty("handRight");
        //toeLeft = SO_Target.FindProperty("toeLeft");
        //toeRight = SO_Target.FindProperty("toeRight");
        backOfHead = SO_Target.FindProperty("backOfHead");

        // Climbing
        allowClimbingLadder = SO_Target.FindProperty("allowClimbingLadder");        
        ladderCastLength = SO_Target.FindProperty("ladderCastLength");
        climbLadderSpeed = SO_Target.FindProperty("climbLadderSpeed");
	    //currentLadder = SO_Target.FindProperty("currentLadder");
    }

    public override void OnInspectorGUI() {
        SO_Target.Update();
        EditorGUI.BeginChangeCheck();
        {
            myTarget.inspectorTabTop = GUILayout.Toolbar(myTarget.inspectorTabTop, new string[] { "Info", "Movement", "Crouching", "Jumping" });
            switch (myTarget.inspectorTabTop) {
                case 0: myTarget.inspectorTabBottom = 4; currentInspectorTab = "Info"; break;
                case 1: myTarget.inspectorTabBottom = 4; currentInspectorTab = "Movement"; break;
                case 2: myTarget.inspectorTabBottom = 4; currentInspectorTab = "Crouching"; break;
                case 3: myTarget.inspectorTabBottom = 4; currentInspectorTab = "Jumping"; break;
            }
            myTarget.inspectorTabBottom = GUILayout.Toolbar(myTarget.inspectorTabBottom, new string[] { "Falling", "EdgeGrab", "LimbPositions", "Climbing" });
            switch (myTarget.inspectorTabBottom) {
                case 0: myTarget.inspectorTabTop = 4; currentInspectorTab = "Falling"; break;           
                case 1: myTarget.inspectorTabTop = 4; currentInspectorTab = "EdgeGrab"; break;
                case 2: myTarget.inspectorTabTop = 4; currentInspectorTab = "LimbPositions"; break;
                case 3: myTarget.inspectorTabTop = 4; currentInspectorTab = "Climbing"; break;
            }
        }
        if (EditorGUI.EndChangeCheck()) {
            SO_Target.ApplyModifiedProperties();
            GUI.FocusControl(null);            
        }
        RenderSeparator();
        EditorGUI.BeginChangeCheck();
        {
            switch (currentInspectorTab) {
                case "Info": PlayerInfo(); break;
                case "Movement": Movement(); break;
                case "Crouching": Crouching(); break;
                case "Jumping": Jumping(); break;
                case "Falling": Falling(); break;
                case "EdgeGrab": EdgeGrab(); break;
                case "LimbPositions": LimbPositions(); break;
                case "Climbing": Climbing(); break;
            }         
        }
        if (EditorGUI.EndChangeCheck()) {
            SO_Target.ApplyModifiedProperties();            
        }
    }

    private void PlayerInfo() {
        BeginSection("Player Info");
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current State:");
            EditorGUI.indentLevel-=4;
            EditorGUILayout.LabelField(myTarget.moveState.ToString());
            EditorGUI.indentLevel+=4;
            EditorGUILayout.EndHorizontal();
            string groundStatus = "";
            if (myTarget.groundStatus == -1f) {
                groundStatus = "Downhill";
            } else if (myTarget.groundStatus == 0f) {
                groundStatus = "Flat Ground";
            } else if(myTarget.groundStatus == 1f) {
                groundStatus = "Uphill";
            }            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ground State:");
            EditorGUI.indentLevel -= 4;
            EditorGUILayout.LabelField(groundStatus);
            EditorGUI.indentLevel += 4;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Distance to Ground:");
            EditorGUI.indentLevel -= 4;
            EditorGUILayout.LabelField(myTarget.distanceToGround.ToString());
            EditorGUI.indentLevel += 4;
            EditorGUILayout.EndHorizontal();            
            EditorGUILayout.Toggle("Grounded:", myTarget.grounded);            
            EditorGUILayout.PropertyField(playerId);                       
            EditorGUILayout.PropertyField(raycastCollisionMask);
            EditorGUILayout.PropertyField(debugMode);
            //EditorGUILayout.PropertyField(currentLadder);
        }
        EndSection();
    }

    private void Movement() {
        BeginSection("Movement Variables");
        {
            EditorGUILayout.PropertyField(walkSpeed);
            EditorGUILayout.PropertyField(runSpeed);
            EditorGUILayout.PropertyField(turnSmoothTime);
            EditorGUILayout.PropertyField(speedSmoothTime);
            EditorGUILayout.PropertyField(inputSmoothTime);
            EditorGUILayout.PropertyField(lockZAxis);
            if (myTarget.lockZAxis == true) {
                EditorGUILayout.PropertyField(zAxis);
            }
        }
        EndSection();
    }

    private void Crouching() {
        BeginSection("Crouching Variables");
        {            
            EditorGUILayout.PropertyField(crouchSpeed);            
            EditorGUILayout.PropertyField(staticCCBoundsCrouching);
            EditorGUILayout.PropertyField(ccCrouchingHeight);
            EditorGUILayout.PropertyField(crouchingHeadClearance);
        }
        EndSection();
    }

    private void Jumping() {
        BeginSection("Jumping Variables");
        {
            EditorGUILayout.PropertyField(jumpHeight);
            EditorGUILayout.PropertyField(jumpDelay);            
            EditorGUILayout.PropertyField(doubleJumpEnabled);
        }
        EndSection();
    }

    private void Falling() {
        BeginSection("Falling Variables");
        {
            EditorGUILayout.PropertyField(airControlPercent);
            EditorGUILayout.PropertyField(jumpLandRayLength);
        }
        EndSection();
    }

    private void EdgeGrab() {
        BeginSection("Edge Grab Variables");
        {
            EditorGUILayout.PropertyField(edgeGrabClimbSpeed);
            EditorGUILayout.PropertyField(edgeGrabClimbForward);
            EditorGUILayout.PropertyField(edgeGrabClimbUp);
            EditorGUILayout.PropertyField(edgeGrabBoundsBottom);
            EditorGUILayout.PropertyField(edgeGrabBoundsTop);
            EditorGUILayout.PropertyField(edgeGrabBoundsRadius);
            EditorGUILayout.PropertyField(edgeGrabMinEdgeSize);            
            EditorGUILayout.PropertyField(edgeGrabRayCount);
            EditorGUILayout.PropertyField(staticCCBoundsClimbing);
            EditorGUILayout.PropertyField(ccClimbingHeight);
        }
        EndSection();
    }

    private void LimbPositions() {
        BeginSection("Limb Position Transforms");
        {
            EditorGUILayout.PropertyField(handLeft);
            EditorGUILayout.PropertyField(handRight);
            //EditorGUILayout.PropertyField(toeLeft);
            //EditorGUILayout.PropertyField(toeRight);
            EditorGUILayout.PropertyField(backOfHead);
        }
        EndSection();
    }

    private void Climbing() {
        BeginSection("Climbing");
        {
            EditorGUILayout.PropertyField(allowClimbingLadder);            
            EditorGUILayout.PropertyField(ladderCastLength);
            EditorGUILayout.PropertyField(climbLadderSpeed);            
        }
        EndSection();
    }

    private void BeginSection(string name) {
        GUIStyle centeredStyle = EditorStyles.boldLabel;
        centeredStyle.alignment = TextAnchor.LowerCenter;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(name, EditorStyles.boldLabel);
        EditorGUILayout.Space();
    }

    private void EndSection() {
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    void RenderSeparator() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    
    void OnSceneGUI() {        
        Handles.color = Color.yellow;
        if (myTarget.debugMode) {
            if (currentInspectorTab == "EdgeGrab") {
                Handles.DrawWireArc(myTarget.transform.position + new Vector3(0f, myTarget.edgeGrabBoundsTop, 0f), Vector3.up, myTarget.transform.forward, 360f, myTarget.edgeGrabBoundsRadius);
                Handles.DrawWireArc(myTarget.transform.position + new Vector3(0f, myTarget.edgeGrabBoundsBottom, 0f), Vector3.up, myTarget.transform.forward, 360f, myTarget.edgeGrabBoundsRadius);
                //myTarget.edgeClimbCurveP0 = Handles.PositionHandle(myTarget.edgeClimbCurveP0, Quaternion.identity);
                //myTarget.edgeClimbCurveP1 = Handles.PositionHandle(myTarget.edgeClimbCurveP1, Quaternion.identity);
                //myTarget.edgeClimbCurveP2 = Handles.PositionHandle(myTarget.edgeClimbCurveP2, Quaternion.identity);
            }
            //Handles.DrawBezier(myTarget.transform.TransformPoint(myTarget.edgeClimbCurveP0), myTarget.transform.TransformPoint(myTarget.edgeClimbCurveP2), myTarget.transform.TransformPoint(myTarget.edgeClimbCurveP1), myTarget.transform.TransformPoint(myTarget.edgeClimbCurveP1), Color.red, null, 5f);            
            Handles.DrawBezier(myTarget.edgeClimbPos0, myTarget.edgeClimbPos2, myTarget.edgeClimbPos1, myTarget.edgeClimbPos1, Color.red, null, 5f);
        }
    }
}