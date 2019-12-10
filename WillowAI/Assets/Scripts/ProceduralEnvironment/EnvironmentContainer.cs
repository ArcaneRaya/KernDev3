using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentContainer : MonoBehaviour {
    public Vector2Int Dimensions { get { return dimensions; } }
    public Cell[,] Cells;
    [SerializeField] private Vector2Int dimensions = Vector2Int.zero;

    public Material FloorMat;
    public Material DownpourMat;
    public LayerMask FloorLayer;
    public LayerMask ObstacleLayer;

    [ContextMenu("Clearup Children")]
    public void CleanupChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            GameObject child = transform.GetChild(i).gameObject;
            if (Application.isPlaying) {
                Destroy(child);
            } else {
                DestroyImmediate(child);
            }
        }
    }
}

public static class ShiftingHelper {
    public static int ShiftBack(int value) {
        int count = 0;
        while (value != 1) {
            value /= 2;
            count++;
        }
        return count;
    }
}