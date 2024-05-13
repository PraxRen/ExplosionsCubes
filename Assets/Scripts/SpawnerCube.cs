using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    private const int ScaleMultiplier = 2;
    private const int MinCountCudes = 2;
    private const int MaxCountCudes = 6;

    [SerializeField] private Cube _prefabCube;
    [SerializeField] private float _radiusSpawn;
    [SerializeField] private Color[] _colors;

    private List<Cube> _lastCreatedCubes = new List<Cube>();

    public IEnumerable<Cube> LastCreatedCubes => _lastCreatedCubes;

    public IEnumerator Create(int indexEnclosure)
    {
        _lastCreatedCubes.Clear();
        int countCubes = UnityEngine.Random.Range(MinCountCudes, MaxCountCudes);

        for (int i = 0; i < countCubes; i++)
        {
            Vector3 position = transform.position + Random.insideUnitSphere * _radiusSpawn;
            position.y = transform.position.y;
            Cube cube = Instantiate(_prefabCube, position, Quaternion.identity);
            cube.transform.localScale = cube.transform.localScale / ScaleMultiplier;
            cube.SetEnclosure(indexEnclosure);
            int indexColor = UnityEngine.Random.Range(0, _colors.Length);
            cube.SetColor(_colors[indexColor]);
            _lastCreatedCubes.Add(cube);
            yield return null;
        }
    }
}