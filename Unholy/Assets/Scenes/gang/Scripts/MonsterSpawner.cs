using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnRadius = 5f;
    public float destroyDelay = 3f;
    public float spawnDelay = 3f;

    private GameObject _spawnedMonster;
    private bool _playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("IN");
            _playerInRange = true;
            if (_spawnedMonster == null)
            {
                StartCoroutine(SpawnMonster());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OUT");
            _playerInRange = false;
            if (_spawnedMonster != null)
            {
                StartCoroutine(DestroyMonster());
            }
        }
    }

    private IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(spawnDelay);
        Debug.Log("Spawn");
        if (_playerInRange && _spawnedMonster == null)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = 1f;
            _spawnedMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private IEnumerator DestroyMonster()
    {
        yield return new WaitForSeconds(destroyDelay);
        if (!_playerInRange && _spawnedMonster != null)
        {
            Destroy(_spawnedMonster);
            Debug.Log("Destroy");
            _spawnedMonster = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
