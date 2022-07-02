using UnityEngine;

public class FieldMetricSpace : MonoBehaviour
{
    private Spawner _spawner;
    private Vector2[] _heroSlotsToMetricPoints;
    private Vector2[] _enemySlotsToMetricPoints;

    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _heroSlotsToMetricPoints = new Vector2[_spawner.HeroSlots.Length];
        _enemySlotsToMetricPoints= new Vector2[_spawner.EnemySlots.Length];
    }

    private void Start()
    {
        ArrangeVectorField();
    }

    private void ArrangeVectorField()
    {
        switch (_spawner.HeroSlots.Length)
        {
            case 1:
                _heroSlotsToMetricPoints[0] = new Vector2(-0.5f, 0);
                break;
            case 2:
                _heroSlotsToMetricPoints[0] = new Vector2(-0.5f, -0.5f);
                _heroSlotsToMetricPoints[1] = new Vector2(-0.5f, 0.5f);
                break;
            case 3:
                _heroSlotsToMetricPoints[0] = new Vector2(-0.5f, -1);
                _heroSlotsToMetricPoints[1] = new Vector2(-0.5f, 0);
                _heroSlotsToMetricPoints[2] = new Vector2(-0.5f, 1);
                break;
            case 6:
                _heroSlotsToMetricPoints[0] = new Vector2(-0.5f, -1);
                _heroSlotsToMetricPoints[1] = new Vector2(-1.5f, -1);
                _heroSlotsToMetricPoints[2] = new Vector2(-0.5f, 0);
                _heroSlotsToMetricPoints[3] = new Vector2(-1.5f, 0);
                _heroSlotsToMetricPoints[4] = new Vector2(-0.5f, 1);
                _heroSlotsToMetricPoints[5] = new Vector2(-1.5f, 1);
                break;
        }

        for (int i = 0; i < _heroSlotsToMetricPoints.Length; ++i)
        {
            _enemySlotsToMetricPoints[i] = new Vector2
                (-_heroSlotsToMetricPoints[i].x, _heroSlotsToMetricPoints[i].y);
        }  
    }

    public float Metric(int heroSlot, int enemySlot)
    {
        var heroPoint = _heroSlotsToMetricPoints[heroSlot];
        var enemyPoint = _enemySlotsToMetricPoints[enemySlot];

        return (heroPoint - enemyPoint).magnitude;
    }
}