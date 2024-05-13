using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float MaxChanceDivision = 100;

    [SerializeField] private SpawnerCube _spawnerCube;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private int _indexEnclosure = 1;
    private float _upForceOffset = 0.7f;

    public Rigidbody Rigidbody => _rigidbody;

    private void OnMouseUpAsButton() => StartCoroutine(HandleClick());

    public void SetEnclosure(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        _indexEnclosure = index;
    }

    public void SetColor(Color color)
    {
        _meshRenderer.material.color = color;
    }

    private IEnumerator HandleClick()
    {
        if (CanSplit())
        {
            yield return StartCoroutine(_spawnerCube.Create(_indexEnclosure + 1));
            Explode(_spawnerCube.LastCreatedCubes);
        }

        Destroy(gameObject);
    }

    private bool CanSplit()
    {
        float currentChance = MaxChanceDivision / _indexEnclosure;
        return UnityEngine.Random.Range(0, MaxChanceDivision) <= currentChance;
    }

    private void Explode(IEnumerable<Cube> cubes)
    {
        foreach (Cube cube in cubes)
        {
            cube.Rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _upForceOffset);
        }
    }
}