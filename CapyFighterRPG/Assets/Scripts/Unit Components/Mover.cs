using UnityEngine;
using System;
using System.Collections;

public class Mover : MonoBehaviour
{
    #region Fields
    private Transform _transform;
    [SerializeField] private float _timeOfMovementSeconds;
    [SerializeField] private float _distanceEpsilon;
    #endregion
    #region Properties
    public bool IsMoving { get; private set; }
    #endregion

    #region Events
    public event Action<int> OnMoving;
    public event Func<int, Vector2> GetPositionOfSlot;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
    #endregion

    #region Custom Methods
    public void MoveToSlot(int slot)
    {
        OnMoving?.Invoke(slot);
        StartCoroutine(MoveToSlotCoroutine(slot));
    }

    public IEnumerator MoveToSlotCoroutine(int slot)
    {
        IsMoving = true;
        var targetPosition = GetPositionOfSlot(slot);
        var speed = Vector3.Distance(_transform.position, targetPosition) / _timeOfMovementSeconds;
        while (Vector3.Distance(_transform.position, targetPosition) > _distanceEpsilon)
        {
            _transform.position = Vector3.Lerp(_transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        IsMoving = false;
    }
    #endregion
}