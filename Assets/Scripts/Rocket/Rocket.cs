using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    bool isFirstCoroutineFinished = false;

    private void OnMouseDown()
    {
      StartCoroutine(SequenceCoroutines());
    }

    IEnumerator SequenceCoroutines()
    {
        int random = (this.gameObject.tag == "R_Right") ? 1 : 0;
        yield return StartCoroutine(LaunchRocket(random));
        yield return StartCoroutine(CheckAfterRocket(random));    
    }

    public IEnumerator LaunchRocket(int random)
    {
        yield return MoveRocket(Convert.ToBoolean(random));
    }

    private IEnumerator MoveRocket(bool isHorizontal)
    {
        // Assume you have a reference to the rocket's transform component
        Transform rocketTransform = this.transform;

        // Assume you have a Vector3 targetPosition that the rocket should move towards
        Vector3 targetPositionFirst = isHorizontal ? new Vector3(0.56f * GridGenerator.Instance.quad.transform.localScale.x, rocketTransform.position.y, rocketTransform.position.z)
                                              : new Vector3(rocketTransform.position.x, 0.55f * GridGenerator.Instance.quad.transform.localScale.y, rocketTransform.position.z);
        Vector3 targetPositionSecond = isHorizontal ? new Vector3(-0.56f * GridGenerator.Instance.quad.transform.localScale.x, rocketTransform.position.y, rocketTransform.position.z)
                                              : new Vector3(rocketTransform.position.x, -0.55f * GridGenerator.Instance.quad.transform.localScale.y, rocketTransform.position.z);
        float startTime = Time.time;

        // Duration of the movement in seconds
        float duration = 0.5f;

        while (Time.time < startTime + duration)
        {
            Transform child0 = this.transform.GetChild(0);
            Transform child1 = this.transform.GetChild(1);
            child0.position = Vector3.Lerp(rocketTransform.position, targetPositionFirst, (Time.time - startTime) / duration);
            child1.position = Vector3.Lerp(rocketTransform.position, targetPositionSecond, (Time.time - startTime) / duration);
            RemoveTileInFrontOfRocket(child0, isHorizontal);
            RemoveTileInFrontOfRocket(child1, isHorizontal);
            yield return null;
        }

        // Ensure the rocket is exactly at the target position in the end
        var Coords = GridController.Instance.GetCoordFromTile(this.gameObject);
        this.transform.GetChild(0).position = targetPositionFirst;
        this.transform.GetChild(1).position = targetPositionSecond;
        Destroy(this.gameObject);
        GridGenerator.Instance.instantiatedPrefabs[Coords.Item1, Coords.Item2] = null;
        isFirstCoroutineFinished = true;

    }
    private void RemoveTileInFrontOfRocket(Transform rocketPartTransform, bool isHorizontal)
    {
        Vector3 direction = isHorizontal ? Vector3.right : Vector3.up;
        float distance = GridGenerator.Instance.QuadSize.x; // Or y if your grid is not square

        int layerMask = 1 << LayerMask.NameToLayer("RocketLayer");
        layerMask = ~layerMask; // invert the mask to exclude the RocketLayer.

        RaycastHit2D hit = Physics2D.Raycast(rocketPartTransform.position, direction, distance, layerMask);
        if (hit.collider != null)
        {
            GameObject tile = hit.collider.gameObject;
            var Coords = GridController.Instance.GetCoordFromTile(tile);
            Destroy(tile);
            GridGenerator.Instance.instantiatedPrefabs[Coords.Item1, Coords.Item2] = null;
        }
    }
    IEnumerator CheckAfterRocket(int random) // yield return new WaitUntil(() => isFirstCoroutineFinished == true); dene 
    {
        switch (random)
        {
            case 0:
                GridController.Instance.CheckAndFill();
                break;
            case 1:
                GridController.Instance.CheckAndHandleFalling();
                GridController.Instance.CheckAndFill();
                break;
        }
        yield return null;
    }
}
