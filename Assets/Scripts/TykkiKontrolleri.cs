using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TykkiKontrolleri : MonoBehaviour
{
    public GameObject ammusPrefab;
    public Transform piippu;
    public Transform ammusSpawn;
    public Transform positio1;
    public Transform positio2;

    private float piippuMinimiKulma = 10f;
    private float piippuMaksimiKulma = 100f;
    private float runkoNopeus = 10f;
    private float piippuNopeus = 30f;

    private float ammuksenAlkunopeus = 15f;

    private bool valitseeSijaintia = true;

	private void Start()
    {
        piippu.RotateAround(transform.position, transform.right, 20f);
        StartCoroutine(ValitseAloitus());
    }

	void Update()
    {
        if (valitseeSijaintia == false)
        {
            // Liikkuminen
            float vertikaalinen = Input.GetAxisRaw("Vertical") * Time.deltaTime * piippuNopeus;
            float horisontaalinen = Input.GetAxis("Horizontal") * Time.deltaTime * runkoNopeus;
            float kulma = Vector3.Angle(transform.up, piippu.up);
            transform.Rotate(transform.up, horisontaalinen);
            if (kulma + vertikaalinen <= piippuMaksimiKulma
                && kulma + vertikaalinen >= piippuMinimiKulma)
            {
                piippu.RotateAround(transform.position, transform.right, vertikaalinen);
            }

            // Ampuminen
            if (PeliKontrolleri.kontrolleri.vuoro == 0 && Input.GetKeyDown(KeyCode.Mouse0))
			{
                GameObject ammus = Instantiate(ammusPrefab, ammusSpawn.position, Quaternion.identity);
                ammus.GetComponent<Ammus>().Ammu(ammusSpawn.up, ammuksenAlkunopeus);
                PeliKontrolleri.kontrolleri.VaihdaVuoro();
			}
        }
    }

    private IEnumerator ValitseAloitus()
	{
        valitseeSijaintia = true;
        Camera.main.transform.parent = positio1.transform;
        Camera.main.transform.position = positio1.position;
        Camera.main.transform.rotation = positio1.rotation;
        while (true)
        {
            yield return null;
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(
                positio1.position,
                toMouse.direction * 100f,
                Color.blue,
                0.1f);
            RaycastHit hit;
            if (Physics.Raycast(toMouse, out hit, 100, LayerMask.GetMask("Maasto")))
			{
                transform.position = hit.point;
			}
            if (Input.GetKeyDown(KeyCode.Mouse0))
			{
                break;
			}
        }
        Camera.main.transform.parent = positio2.transform;
        Camera.main.transform.position = positio2.position;
        Camera.main.transform.rotation = positio2.rotation;
        valitseeSijaintia = false;
    }
}
