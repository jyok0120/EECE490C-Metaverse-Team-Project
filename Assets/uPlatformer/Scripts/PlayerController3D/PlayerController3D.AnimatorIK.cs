using UnityEngine;
using System.Collections;

public partial class PlayerController3D  {
    // How much IK movements should be smoothed
    public float ikSmoothing = 3f;
    // Weights for various body parts
    private float handWeight, footWeight, kneeWeight;

    private void OnAnimatorIK(int layerIndex) {
        switch (moveState) {            
            case MoveState.EdgeGrab:
                if (handAtEdge) {
                    handWeight += Time.deltaTime * ikSmoothing;
                    if (handWeight >= 1f) { handWeight = 1f; }
                    myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.RightHand, currentEdgePosition + transform.right * distanceBetweenHands / 2f);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftHand, currentEdgePosition - transform.right * distanceBetweenHands / 2f);
                }
                break;
            case MoveState.EdgeGrabClimbUp:
                if (ikLeftFootClimbUp) {
                    footWeight += Time.deltaTime * ikSmoothing;
                    if (footWeight >= 1f) { footWeight = 1f; }
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftFoot, currentEdgePosition);
                    myAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKRotation(AvatarIKGoal.LeftFoot, transform.rotation);

                    if (handAtEdge) {
                        handWeight += Time.deltaTime * ikSmoothing;
                        if (handWeight >= 1f) { handWeight = 1f; }
                        myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                        myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                        myAnim.SetIKPosition(AvatarIKGoal.RightHand, currentEdgePosition + transform.right * distanceBetweenHands / 2f);
                        myAnim.SetIKPosition(AvatarIKGoal.LeftHand, currentEdgePosition - transform.right * distanceBetweenHands / 2f);
                    }
                    else {
                        handWeight -= Time.deltaTime * ikSmoothing;
                        if (handWeight <= 0f) { handWeight = 0f; }
                        myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                        myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                        myAnim.SetIKPosition(AvatarIKGoal.RightHand, currentEdgePosition + transform.right * distanceBetweenHands / 2f);
                        myAnim.SetIKPosition(AvatarIKGoal.LeftHand, currentEdgePosition - transform.right * distanceBetweenHands / 2f);
                    }

                    kneeWeight -= Time.deltaTime * ikSmoothing;
                    if (kneeWeight <= 0f) { kneeWeight = 0f; }
                    //myAnim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, kneeWeight);
                    //myAnim.SetIKHintPosition(AvatarIKHint.RightKnee, currentEdgePosition);
                }
                else {
                    handWeight += Time.deltaTime * ikSmoothing;
                    if (handWeight >= 1f) { handWeight = 1f; }
                    myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.RightHand, currentEdgePosition + transform.right * distanceBetweenHands / 2f);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftHand, currentEdgePosition - transform.right * distanceBetweenHands / 2f);

                    kneeWeight += Time.deltaTime * ikSmoothing;
                    if (kneeWeight >= 1f) { kneeWeight = 1f; }
                    //myAnim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, kneeWeight);
                    //myAnim.SetIKHintPosition(AvatarIKHint.RightKnee, currentEdgePosition);
                
                    
                    footWeight += Time.deltaTime * ikSmoothing;
                    if (footWeight >= 1f) { footWeight = 1f; }
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftFoot, currentEdgePosition);
                    myAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKRotation(AvatarIKGoal.LeftFoot, transform.rotation);
                }
                break;
            case MoveState.EdgeGrabDrop:
                break;
            case MoveState.Standing:
                /*
                if (handWeight > 0f) {
                    handWeight -= Time.deltaTime * ikSmoothing;
                    if (handWeight <= 0f) { handWeight = 0f; }
                    myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.RightHand, currentEdgePosition + transform.right * distanceBetweenHands / 2f);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftHand, currentEdgePosition - transform.right * distanceBetweenHands / 2f);
                }                
                if (footWeight > 0f) {
                    footWeight -= Time.deltaTime * ikSmoothing;
                    if (footWeight <= 0f) { footWeight = 0f; }                                        
                    myAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKPosition(AvatarIKGoal.LeftFoot, currentEdgePosition);                    
                    myAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footWeight);
                    myAnim.SetIKRotation(AvatarIKGoal.LeftFoot, transform.rotation);
                }
                if (kneeWeight > 0f) {
                    kneeWeight -= Time.deltaTime * ikSmoothing;
                    if (kneeWeight <= 0f) { kneeWeight = 0f; }
                    myAnim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, kneeWeight);
                    myAnim.SetIKHintPosition(AvatarIKHint.RightKnee, currentEdgePosition);
                }*/
                break;
            case MoveState.Crouching:                
                break;            
        }
    }

    private void ResetIK() {
        handWeight = 0f;
        footWeight = 0f;
        kneeWeight = 0f;
    }
}
