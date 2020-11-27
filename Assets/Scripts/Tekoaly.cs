using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tekoaly : MonoBehaviour
{
    public Maasto maasto;

    void Start()
    {
        ValitseSijainti();
    }

    private void ValitseSijainti()
	{
        float xSijainti = Random.Range(0f, maasto.maastoLeveys);
        float ySijainti = Random.Range(0f, maasto.maastoPituus);
        RaycastHit hit;
        Physics.Raycast(
            new Vector3(xSijainti, 100, ySijainti),
            new Vector3(0, -1, 0),
            out hit,
            1000f,
            LayerMask.GetMask("Maasto"));
        transform.position = hit.point;
    }
}
