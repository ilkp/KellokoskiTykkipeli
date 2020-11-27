using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ammus : MonoBehaviour
{
	public Material rajahdysMateriaali;
	private float maxEtaisyys = 3;
	private float elossaoloaika = 0f;
	private float MaxAika = 5f;

	private void Update()
	{
		elossaoloaika += Time.deltaTime;
		if (elossaoloaika > MaxAika)
		{
			Destroy(gameObject);
		}
	}

	public void Ammu(Vector3 suunta, float alkunopeus)
	{
		GetComponent<Rigidbody>().AddForce(suunta * alkunopeus, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		GetComponent<SphereCollider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Renderer>().material = rajahdysMateriaali;
		transform.localScale = new Vector3(maxEtaisyys, maxEtaisyys, maxEtaisyys);
		StartCoroutine(Rajahda());

		GameObject pelaaja = GameObject.FindGameObjectWithTag("Pelaaja");
		GameObject tekoaly = GameObject.FindGameObjectWithTag("Tekoaly");
		float etaisyysPelaajaan = (transform.position - pelaaja.transform.position).magnitude;
		float etaisyysTekoalyyn = (transform.position - tekoaly.transform.position).magnitude;
		if (etaisyysPelaajaan <= maxEtaisyys)
		{
			PeliKontrolleri.kontrolleri.Osuma(0);
		}
		if (etaisyysTekoalyyn <= maxEtaisyys)
		{
			PeliKontrolleri.kontrolleri.Osuma(1);
		}
	}

	private IEnumerator Rajahda()
	{
		float aika = 0.2f;
		float eAika = 0;
		while (eAika < aika)
		{
			eAika += Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		PeliKontrolleri.kontrolleri.VaihdaVuoro();
	}
}
