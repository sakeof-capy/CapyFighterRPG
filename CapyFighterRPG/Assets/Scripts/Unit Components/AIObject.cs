using UnityEngine;
using System;

public class AIObject : MonoBehaviour
{
    public Fighter Fighter  { get; private set; }
    public Task Task        { get; private set; }

    void Start()
    {
        Fighter = GetComponent<Fighter>();    
    }

    public void Assign(PossibleAssignment assignment)
    {
        if (Task != null) return;
        Task = assignment.TaskToDo;
    }

    public bool IsTaskSuitable(Task task)
    {
        switch (task.Type)
        {
            case Task.TaskType.Attack:
                return Fighter.CanAttack();
            case Task.TaskType.SuperAttack:
                return Fighter.CanSuperAttack();
            case Task.TaskType.EquipShield:
                return Fighter.CanEquipShield();
            case Task.TaskType.Move:
                return false;
            case Task.TaskType.SkipTurn:
                return true;
            default:
                throw new InvalidOperationException("No such task type.");
        }
    }
}
