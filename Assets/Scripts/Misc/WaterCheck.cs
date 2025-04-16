using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WaterCheck : MonoBehaviour
{
    private Vector3 _waterHeight;
    private bool _underWater;

    private WaterSurface water;
    private WaterSearchParameters _search;
    private WaterSearchResult _searchResult;

    private void Start()
    {
        water = FindFirstObjectByType<WaterSurface>();
    }

    private void FixedUpdate()
    {
        _search.startPositionWS = transform.position;
        water.ProjectPointOnWaterSurface(_search, out _searchResult);

        _waterHeight = _searchResult.projectedPositionWS;

        _underWater = (transform.position.y < _waterHeight.y);
    }

    public bool UnderWater() => _underWater;

    public Vector3 WaterHeight() => _waterHeight;
}
