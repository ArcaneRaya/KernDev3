using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Group : MonoBehaviour {
    public List<Cell> Cells = new List<Cell>();

    public Vector2Int CalculateCenter() {
        Vector2Int center = new Vector2Int(0, 0);

        foreach (var cell in Cells) {
            center += cell.Position;
        }

        center /= Cells.Count;

        return center;
    }
}
