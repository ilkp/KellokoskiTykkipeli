using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Maasto : MonoBehaviour
{
	public Texture2D korkeuskartta;
	[Min(1)] public float maastoLeveys = 10f;
	[Min(1)] public float maastoPituus = 10f;
	[Range(0f, 10f)] public float maastoKorkeusKerroin = 1f;
	[Range(0.1f, 1f)] public float verteksiTiheys = 0.2f;

	private float vanhaMaastoLeveys;
	private float vanhaMaastoPituus;
	private float vanhaKorkeuskerroin;
	private float vanhaVerteksitiheys;
	private bool muuttujatVaihtuneet = true;

	private Vector3[] verteksit;
	private int[] kolmiot;
	private Vector2[] uvs;

	private Mesh mesh;

	private void Update()
	{
		if (maastoLeveys != vanhaMaastoLeveys
			|| maastoPituus != vanhaMaastoPituus
			|| maastoKorkeusKerroin != vanhaKorkeuskerroin
			|| verteksiTiheys != vanhaVerteksitiheys)
		{
			muuttujatVaihtuneet = true;
		}

		if (muuttujatVaihtuneet)
		{
			GeneroiMaasto();
			vanhaMaastoLeveys = maastoLeveys;
			vanhaMaastoPituus = maastoPituus;
			vanhaKorkeuskerroin = maastoKorkeusKerroin;
			vanhaVerteksitiheys = verteksiTiheys;
			muuttujatVaihtuneet = false;
		}
	}

	private void GeneroiMaasto()
	{
		int nVerteksiaLeveys = (int)(maastoLeveys / verteksiTiheys);
		int nVerteksiaPituus = (int)(maastoPituus / verteksiTiheys);
		float verteksiEtaisyys = maastoLeveys / nVerteksiaLeveys;
		verteksit = new Vector3[nVerteksiaLeveys * nVerteksiaPituus];
		for (int z = 0; z < nVerteksiaPituus; ++z)
		{
			for (int x = 0; x < nVerteksiaLeveys; ++x)
			{
				float korkeus = korkeuskartta.GetPixel(
					(int)(x / (float)nVerteksiaLeveys * korkeuskartta.width),
					(int)(z / (float)nVerteksiaPituus * korkeuskartta.height)).grayscale;
				korkeus = korkeus * korkeus;

				verteksit[z * nVerteksiaLeveys + x] =
					new Vector3(verteksiEtaisyys * x, korkeus * maastoKorkeusKerroin, verteksiEtaisyys * z);
			}	
		}

		int verteksiIndeksi = 0;
		kolmiot = new int[(nVerteksiaLeveys - 1) * (nVerteksiaPituus - 1) * 6];
		for (int z = 0; z < nVerteksiaPituus - 1; ++z)
		{
			for (int x = 0; x < nVerteksiaLeveys - 1; ++x)
			{
				kolmiot[verteksiIndeksi++] = z * nVerteksiaLeveys + x;
				kolmiot[verteksiIndeksi++] = (z+1) * nVerteksiaLeveys + x;
				kolmiot[verteksiIndeksi++] = (z+1) * nVerteksiaLeveys + (x+1);
				kolmiot[verteksiIndeksi++] = z * nVerteksiaLeveys + x;
				kolmiot[verteksiIndeksi++] = (z + 1) * nVerteksiaLeveys + (x + 1);
				kolmiot[verteksiIndeksi++] = z * nVerteksiaLeveys + (x+1);
			}
		}

		uvs = new Vector2[nVerteksiaLeveys * nVerteksiaPituus];
		for (int i = 0; i < uvs.Length; ++i)
		{
			uvs[i] = new Vector2(verteksit[i].x, verteksit[i].z);
		}

		mesh = new Mesh();
		mesh.vertices = verteksit;
		mesh.triangles = kolmiot;
		mesh.uv = uvs;
		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
