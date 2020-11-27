using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeliKontrolleri : MonoBehaviour
{
	public static PeliKontrolleri kontrolleri;

	public int vuoro = 0;
	private int edellinenVuoro = 0;
	private int[] pisteet = new int[2];

	private void Start()
	{
		if (kontrolleri != null)
			Destroy(this);
		kontrolleri = this;
	}

	public void Osuma(int osuttu)
	{
		pisteet[osuttu] += 1;
	}

	public void VaihdaVuoro()
	{
		if (vuoro == 0)
			vuoro = 2;
		else if (vuoro == 1)
			vuoro = 2;
		else
		{
			if (edellinenVuoro == 0)
			{
				vuoro = 1;
				edellinenVuoro = 1;
			}
			else
			{
				vuoro = 0;
				edellinenVuoro = 0;
			}
		}
	}
}
