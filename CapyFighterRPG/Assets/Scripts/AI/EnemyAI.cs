using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private CombatController _controller;
    private List<PossibleAssignment> _possibleAssignmentList;
    private List<AIObject> _heroList;
    private List<AIObject> _enemyList;
    private List<Task> _taskList;

    [Header("Weights for enemy factors")]
    [SerializeField] private float _possibleDamageWeight;
    [SerializeField] private float _possibleHPSaveWeight;
    [SerializeField] private float _possibleMPCostWeight;

    [Header("Weights for hero factors")]
    [SerializeField] private float _heroHPWeight;
    [SerializeField] private float _heroMPWeight;

    public float PossibleDamageWeight => _possibleDamageWeight;
    public float PossibleHPSaveWeight => _possibleHPSaveWeight;
    public float PossibleMPCostWeight => _possibleMPCostWeight;
    public float HeroHPWeight => _heroHPWeight;
    public float HeroMPWeight => _heroMPWeight;
    public List<AIObject> Heros => _heroList;
    public List<AIObject> Enemies => _enemyList;



    private void Awake()
    {
        _controller = GetComponent<CombatController>();
        _possibleAssignmentList = new List<PossibleAssignment>();
        _heroList = new List<AIObject>();
        _enemyList = new List<AIObject>();
        _taskList = new List<Task>();
    }

    public Task NextTurnTask()
    {
        ClearAllLists();
        FillListsOfObjects();
        GatherTasks();
        GeneratePossibleAssignments();
        _possibleAssignmentList.Sort();
        _possibleAssignmentList[0].Assign();
        return _possibleAssignmentList[0].TaskToDo;
    }

    private void ClearAllLists()
    {
        _heroList.Clear();
        _enemyList.Clear();
        _possibleAssignmentList.Clear();
        _taskList.Clear();
    }

    private void FillListsOfObjects()
    {
        foreach(var heroObj in _controller.HerosToFighters.Keys)
        {
            _heroList.Add(heroObj.GetComponent<AIObject>());
        }

        foreach (var enemyObj in _controller.EnemiesToFighters.Values)
        {
            _enemyList.Add(enemyObj.GetComponent<AIObject>());
        }
    }

    private void GatherTasks()
    {
        _taskList.Add(new Task(this, Task.TaskType.SkipTurn));

        foreach(var hero in _heroList)
        {
            _taskList.Add(new Task(this, Task.TaskType.Attack, hero));
            _taskList.Add(new Task(this, Task.TaskType.SuperAttack, hero));
        }

        foreach (var enemy in _enemyList)
        {
            _taskList.Add(new Task(this, Task.TaskType.EquipShield));
        }
    }

    private void GeneratePossibleAssignments()
    {
        foreach(var task in _taskList)
        {
            foreach(var enemy in _enemyList)
            {
                if(enemy.IsTaskSuitable(task))
                {
                    _possibleAssignmentList.Add(new PossibleAssignment(task, enemy, this));
                }
            }
        }
    }
}