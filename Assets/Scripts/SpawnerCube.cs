using System.Collections;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    private const int ScaleMultiplier = 2;
    private const int MinCount = 2;
    private const int MaxCount = 6;

    [SerializeField] private Cube _prefab;
    [SerializeField] private float _radius;
    [SerializeField] private Color[] _colors;

    public IEnumerator Create(int indexEnclosure)
    {
        int countCubes = Random.Range(MinCount, MaxCount);

        for (int i = 0; i < countCubes; i++)
        {
            Vector3 position = transform.position + Random.insideUnitSphere * _radius;
            position.y = transform.position.y;
            Cube cube = Instantiate(_prefab, position, Quaternion.identity);
            Vector3 scale = cube.transform.localScale / ScaleMultiplier;
            int indexColor = Random.Range(0, _colors.Length);
            cube.Init(indexEnclosure, scale, _colors[indexColor]);
            yield return null;
        }
    }
}