using UnityEngine;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _unitInfoPrefab;
    [SerializeField] private GameObject _hudCanvas;
    [Header("Field for hero infos")]
    [SerializeField] private Vector3 _heroOrigin;
    [SerializeField] private Vector3 _heroOriginsNeighbour;
    [SerializeField] private Vector3 _heroDiagonalToOrigin;
    [Header("Field for enemy infos")]
    [SerializeField] private Vector3 _enemyOrigin;
    [SerializeField] private Vector3 _enemyOriginsNeighbour;
    [SerializeField] private Vector3 _enemyDiagonalToOrigin;

    private CombatController _controller;
    private Dictionary<Fighter, UnitInfo> _fightersToUnitInfos;

    #endregion

    #region Properties
    public Dictionary<Fighter, UnitInfo> FightersToUnitInfos => _fightersToUnitInfos;
    #endregion

    #region MonoBehaviour methods
    private void Awake()
    {
        _controller = GetComponent<CombatController>();
        _fightersToUnitInfos = new Dictionary<Fighter, UnitInfo>();
    }

    private void SubscribeEvents()
    {
        foreach(var pair in FightersToUnitInfos)
        {
            pair.Key.OnDamageReceived       += (perc, dam)  => pair.Value.SetHP(perc);
            pair.Key.OnManaAmountChanged    += percentage   => pair.Value.SetMP(percentage);
            pair.Key.OnDied += () =>
            {
                pair.Value.SetHP(0f);
            };
            //... for other hero attacks
        }

        foreach (var pair in FightersToUnitInfos)
        {
            pair.Key.OnDamageReceived       += (perc, dam)  => pair.Value.SetHP(perc);
            pair.Key.OnManaAmountChanged    += percentage   => pair.Value.SetMP(percentage);
            pair.Key.OnDied += () =>
            {
                pair.Value.SetHP(0f);
            };
            //... for other enemy attacks
        }
    }

    #endregion

    #region Custom Methods
    public void PlaceAllUnitInfos()
    {
        Vector3[] positions;
        GameObject unitInfoObject;
        UnitInfo script;
        int i;

        positions = CalculateHeroInfosPositions();
        i = 0;

        foreach (var unit in _controller.HerosToFighters)
        {
            unitInfoObject = Instantiate(_unitInfoPrefab, positions[i++], 
                                        Quaternion.identity, _hudCanvas.transform);
            script = unitInfoObject.GetComponent<UnitInfo>();
            script.SetAvatarImage(unit.Value.GetComponent<Unit>().AvatarIcon);
            _fightersToUnitInfos.Add(unit.Value, script);
        }

        positions = CalculateEnemyInfosPositions();
        i = 0;

        foreach (var unit in _controller.EnemiesToFighters)
        {
            unitInfoObject = Instantiate(_unitInfoPrefab, positions[i++],
                                        Quaternion.identity, _hudCanvas.transform);
            script = unitInfoObject.GetComponent<UnitInfo>();
            script.SetAvatarImage(unit.Value.GetComponent<Unit>().AvatarIcon);
            _fightersToUnitInfos.Add(unit.Value, script);
        }

        SubscribeEvents();
    }

    private Vector3[] CalculateHeroInfosPositions()
    {
        const int rowsOfUnitInfoSlots = 1;

        VectorGrid grid = new VectorGrid
            (
            _heroOrigin,
            _heroOriginsNeighbour,
            _heroDiagonalToOrigin,
            _controller.HeroCount,
            rowsOfUnitInfoSlots
            );
        
        return grid.CalculateCellCentres();
    }

    private Vector3[] CalculateEnemyInfosPositions()
    {
        const int rowsOfUnitInfoSlots = 1;

        VectorGrid grid = new VectorGrid
            (
            _enemyOrigin,
            _enemyOriginsNeighbour,
            _enemyDiagonalToOrigin,
            _controller.EnemyCount,
            rowsOfUnitInfoSlots
            );

        return grid.CalculateCellCentres();
    }
    #endregion
}
