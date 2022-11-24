using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager {
    bool GetButton(int playerId, InputAction action);
    bool GetButtonDown(int playerId, InputAction action);
    bool GetButtonUp(int playerId, InputAction action);
    float GetAxis(int playerId, InputAction action);
}

public abstract class InputManager : MonoBehaviour, IInputManager {
    private static IInputManager _instance;
    public static IInputManager instance { get { return _instance; } }
    
    public static void SetInstance(IInputManager instance) {
        _instance = instance;
    }

    protected abstract void Awake();
    public abstract bool GetButton(int playerId, InputAction action);
    public abstract bool GetButtonDown(int playerId, InputAction action);
    public abstract bool GetButtonUp(int playerId, InputAction action);
    public abstract float GetAxis(int playerId, InputAction action);
}
