using UnityEngine;

public class WallScript : MonoBehaviour
{
    public Collider wallCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallCollider = GetComponent<Collider>();
        wallCollider.enabled = true;
    }

    private void OnEnable()
    {
        FearMovement.OnFearEnemyDeath += DisableWall;
    }

    private void OnDisable()
    {
        FearMovement.OnFearEnemyDeath -= DisableWall;
    }

    private void DisableWall()
    {
        if (wallCollider != null)
        {
            wallCollider.enabled = false;
            Debug.Log("Wall disabled because a Fear enemy died.");
        }
    }
}
