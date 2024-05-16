using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float MaxChanceDivision = 100;
    private const float UpForceOffset = 0.7f;
    private const float CoefficientEnclosureRadius = 1.1f;
    private const float CoefficientEnclosureForce = 1.5f;

    [SerializeField] private SpawnerCube _spawnerCube;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private LayerMask _layerMaskExplodebleObject;
    [SerializeField] private MeshRenderer _meshRenderer;

    private int _indexEnclosure = 1;

    private void OnMouseUpAsButton() => StartCoroutine(HandleClick());

    public void Init(int indexEnclosure, Vector3 scale, Color color)
    {
        if (indexEnclosure < 0)
            throw new ArgumentOutOfRangeException(nameof(indexEnclosure));

        _indexEnclosure = indexEnclosure;
        transform.localScale = scale;
        _meshRenderer.material.color = color;
        _explosionRadius *= CoefficientEnclosureRadius;
        _explosionForce *= CoefficientEnclosureForce; 
    }

    private IEnumerator HandleClick()
    {
        if (CanSplit())
        {
            yield return StartCoroutine(_spawnerCube.Create(_indexEnclosure + 1));
        }

        Explode();
        Destroy(gameObject);
    }

    private bool CanSplit()
    {
        float currentChance = MaxChanceDivision / _indexEnclosure;
        return UnityEngine.Random.Range(0, MaxChanceDivision) <= currentChance;
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMaskExplodebleObject, QueryTriggerInteraction.Ignore);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody) == false)
                continue;

            rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, UpForceOffset);
        }
    }
}