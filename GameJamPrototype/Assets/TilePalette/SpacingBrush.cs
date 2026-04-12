using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Spacing Brush", menuName = "Brushes/Spacing Brush")]
[CustomGridBrush(false, true, false, "Spacing Brush")]
public class SpacingBrush : GridBrush {
    public int spacing = 5;

    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
        Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
        if (tilemap == null) return;

        foreach (Vector3Int location in position.allPositionsWithin) {
            if (location.x % (spacing + 1) == 0) {
                tilemap.SetTile(location, cells.tile);
            }
        }
    }
}