using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameState gameState;
    GameObject unit;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && gameState.activeArmy != null && gameState.gameMode == GameState.Mode.freeMode)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new Vector2(mousePosition.x, mousePosition.y);

            unit = gameState.activeArmy.gameObject;
            RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));
            if (hit)
            {
                unit.GetComponent<UnitControl>().path = new List<Vector2>();
                unit.GetComponent<UnitControl>().currNode = 0;
                unit.GetComponent<UnitControl>().delay = 0.5f;
                unit.GetComponent<UnitControl>().path = GetComponent<PathFinding>().GetPath(unit.transform.position, hit.collider.gameObject.transform.position, 9);
            }
        }
    }
}