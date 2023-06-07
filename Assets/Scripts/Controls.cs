using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    GameObject unit;
    
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new Vector2(mousePosition.x, mousePosition.y);

            if (unit == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Units"));
                if (hit)
                {
                    unit = hit.collider.gameObject;
                }    
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Units"));
                if (hit)
                {
                    if(hit.collider.gameObject == unit)    
                    {
                        unit = null;
                    }
                    else
                    {
                        unit = hit.collider.gameObject;
                    }
                }
                else
                {
                    hit = Physics2D.Raycast(pos, pos, 0, LayerMask.GetMask("Tiles"));
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
    }
}
