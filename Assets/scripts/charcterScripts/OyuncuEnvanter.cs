using UnityEngine;
using System.Collections.Generic; // List kullanmak icin gerekli

public class OyuncuEnvanter : MonoBehaviour
{
    // Toplanan parca ID'lerini (1, 2, 3, 4) tutar
    public List<int> toplananParcalar = new List<int>();

    [Header("Bulmaca Ayarlari")]
    public int toplamParcaSayisi = 4;

    /// <summary>
    /// Envantere yeni parca ekler.
    /// </summary>
   
    /// <returns>Ekleme basariliysa true.</returns>
    public bool ParcaEkle(int id)
    {
        if (!toplananParcalar.Contains(id))
        {
            toplananParcalar.Add(id);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tum parcalarin toplanip toplanmadigini kontrol eder.
    /// </summary>
    public bool TumParcalarToplandiMi()
    {
        return toplananParcalar.Count == toplamParcaSayisi;
    }

    /// <summary>
    /// Parcalari envanterden temizler (Bulmaca bittiginde kullanilacak).
    /// </summary>
    public void ParcalariTemizle()
    {
        toplananParcalar.Clear();
    }
}