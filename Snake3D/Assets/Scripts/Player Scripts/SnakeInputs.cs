using UnityEngine;

[RequireComponent(typeof(SnakeController))]
public class SnakeInputs : MonoBehaviour
{
    private SnakeController _snakeController;

    private int _horizontal = 0, _vertical = 0;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    private void Awake()
    {
        _snakeController = GetComponent<SnakeController>();
    }

    private void Update()
    {
        _horizontal = 0;
        _vertical = 0;

        GetKeyboardInput();
        SetMovement();
    }

    private void GetKeyboardInput()
    {
        _horizontal = GetAxisRaw(Axis.Horizontal);
        _vertical = GetAxisRaw(Axis.Vertical);

        if(_horizontal != 0)
        {
            _vertical = 0;
        }
    }

    private void SetMovement()
    {
        if (_vertical != 0)
        {
            _snakeController.SetInputDirection((_vertical == 1) ?
                                              PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if (_horizontal != 0)
        {
            _snakeController.SetInputDirection((_horizontal == 1) ?
                                              PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    private int GetAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizontal)
        {
            var left = Input.GetKeyDown(ControllsManager.GoLeftKey());
            var right = Input.GetKeyDown(ControllsManager.GoRightKey());

            return GetPositiveOrNegativeValue(1, right, left);
        }
        else if (axis == Axis.Vertical)
        {
            var up = Input.GetKeyDown(ControllsManager.GoUpKey());
            var down = Input.GetKeyDown(ControllsManager.GoDownKey());

            return GetPositiveOrNegativeValue(1, up, down);
        }
        return 0;
    }

    private int GetPositiveOrNegativeValue(int value, bool shouldGetPositive, bool shouldGetNegative)
    {
        if (shouldGetPositive)
        {
            return value;
        }
        if (shouldGetNegative)
        {
            return -value;
        }
        return 0;
    }
}
