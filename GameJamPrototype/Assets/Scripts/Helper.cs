using UnityEngine;

public static class Helper
{
    public static bool IsInstance(Component component)
    {
        return component.gameObject.scene.IsValid();
    }

    public static Collider2D GetClosestCollider(Vector2 origin, Collider2D[] targets)
    {
        Collider2D closest = null;
        float closestDist = float.MaxValue;

        foreach (Collider2D target in targets)
        {
            float dist = (origin - (Vector2)target.transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = target;
            }
        }

        return closest;
    }
}