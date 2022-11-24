using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputManager : InputManager {

    [SerializeField]
    private static string _playerAxisPrefix = "";
    [SerializeField]
    private static int _maxNumberOfPlayers = 1;
    [SerializeField]
    protected string _horizontalAxis = "Horizontal";
    [SerializeField]
    protected string _verticalAxis = "Vertical";
    [SerializeField]
    protected string _runAxis = "Fire3";
    [SerializeField]
    protected string _crouchAxis = "Fire2";
    [SerializeField]
    protected string _jumpAxis = "Jump";
    [SerializeField]
    protected string _attackAxis = "Fire1";
    [SerializeField]
    protected string _cancelAxis = "Cancel";
    [SerializeField]
    protected string _submitAxis = "Submit";

    private Dictionary<int, string>[] _actions;

    protected override void Awake() {
        if (instance != null) return; // do not override existing input sources
        SetInstance(this); // set this as the singleton instance

        // Set up Actions dictionary for each player
        _actions = new Dictionary<int, string>[_maxNumberOfPlayers];
        for (int i = 0; i < _maxNumberOfPlayers; i++) {
            Dictionary<int, string> playerActions = new Dictionary<int, string>();
            _actions[i] = playerActions;
            string prefix = !string.IsNullOrEmpty(_playerAxisPrefix) ? _playerAxisPrefix + i : string.Empty;
            AddAction(InputAction.MoveHorizontal, prefix + _horizontalAxis, playerActions);
            AddAction(InputAction.MoveVertical, prefix + _verticalAxis, playerActions);
            AddAction(InputAction.Run, prefix + _runAxis, playerActions);
            AddAction(InputAction.Crouch, prefix + _crouchAxis, playerActions);
            AddAction(InputAction.Jump, prefix + _jumpAxis, playerActions);
            AddAction(InputAction.Attack, prefix + _attackAxis, playerActions);
            AddAction(InputAction.Cancel, prefix + _cancelAxis, playerActions);
            AddAction(InputAction.Submit, prefix + _submitAxis, playerActions);
        }
    }

    private static void AddAction(InputAction action, string actionName, Dictionary<int, string> actions) {
        if (string.IsNullOrEmpty(actionName)) return;
        actions.Add((int)action, actionName);
    }

    public override bool GetButton(int playerId, InputAction action) {
        return Input.GetButton(_actions[playerId][(int)action]);
    }

    public override bool GetButtonDown(int playerId, InputAction action) {
        return Input.GetButtonDown(_actions[playerId][(int)action]);
    }

    public override bool GetButtonUp(int playerId, InputAction action) {
        return Input.GetButtonUp(_actions[playerId][(int)action]);
    }

    public override float GetAxis(int playerId, InputAction action) {
        return Input.GetAxis(_actions[playerId][(int)action]);
    }
}