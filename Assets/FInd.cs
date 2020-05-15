using UnityEngine;
using System;
using System.Collections;

public class FInd : MonoBehaviour
{
    public LayerMask checkLayers;
    public int Health;

    Vector3 Mark1;
    Vector3 Mark2;

    [SerializeField]
    private Transform WallPrefab;

    private GameObject Wall;
    private void Start()
    {
        InstantiateCylinder(WallPrefab, Mark1, Mark2);
    }
    private void Update()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10, checkLayers);
        Array.Sort(colliders, new Close(transform));

        Mark1 = colliders[0].transform.position;
        Mark2 = colliders[1].transform.position;

        foreach (Collider item in colliders)
        {
            Debug.Log(item.name);
        }

        if (Health>0)
        {
          UpdateWallPosition(Wall, Mark1, Mark2);
        }
        if (Health<=0)
        {
            StartCoroutine(Respawn());
        }
        if (Input.GetMouseButtonDown(0))
            Health -= 1;
    }
    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        Wall = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        UpdateWallPosition(Wall, beginPoint, endPoint);
    }

    private void UpdateWallPosition(GameObject walll, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        walll.transform.position = position;
        walll.transform.LookAt(beginPoint);
        Vector3 localScale = walll.transform.localScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        walll.transform.localScale = localScale;
    }
    private IEnumerator Respawn()
    {
            yield return new WaitForSeconds(5);
            Health = 5;
            UpdateWallPosition(Wall, Mark1, Mark2);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
