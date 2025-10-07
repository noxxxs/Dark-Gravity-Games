using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabList;

    [SerializeField, Min(0.2f)] private float _timeToSpawnObject;
    [SerializeField, Min(0f)] private float _spawnRadius;

    private Vector3 _spawnPoint;
    void Start()
    {
        StartCoroutine(ActivateSpawner());
    }

    private IEnumerator ActivateSpawner()
    {
        while (true)
        {
            GameObject nextObjToSpawn = _prefabList[Random.Range(0, _prefabList.Count)];
            FindNextSpawnPoint(nextObjToSpawn);
            // Spawn object 
            // Could be improved using objectPool
            Instantiate(nextObjToSpawn, _spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(_timeToSpawnObject);
        }
    }

    void FindNextSpawnPoint(GameObject nextObjToSpawn)
    {
        //Reset spawnPoint
        _spawnPoint = transform.position;

        if (_spawnRadius == 0f)
            return;
        else
        {
            Vector2 offset = Random.insideUnitCircle.normalized * _spawnRadius;
            _spawnPoint += new Vector3(offset.x, 0f, offset.y);
        }
        _spawnPoint += Vector3.up * (GetColliderHeight(nextObjToSpawn) / 2f);
    }
     
    // Func to calculate object Height
    private float GetColliderHeight(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            return 0f;
        }

        // BoxCollider
        if (col is BoxCollider box)
        {
            return box.size.y * obj.transform.localScale.y;
        }

        // CapsuleCollider
        if (col is CapsuleCollider capsule)
        {
            return capsule.height * obj.transform.localScale.y;
        }

        // SphereCollider
        if (col is SphereCollider sphere)
        {
            return sphere.radius * 2f * obj.transform.localScale.y;
        }

        // MeshCollider
        if (col is MeshCollider mesh)
        {
            return mesh.sharedMesh.bounds.size.y * obj.transform.localScale.y;
        }

        return col.bounds.size.y;
    }

    private void OnDrawGizmos()
    {
        if (_spawnRadius <= 0f) return;
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _spawnRadius);
        Handles.color = Color.cyan;
        Handles.DrawWireCube(transform.position, Vector3.one * 0.15f);
    }
}
