// This file is part of SGGW Odzyskiwanie Danych. SGGW Odzyskiewanie Danych is
// free software: you can redistribute it and/or modify it under the terms of the
// GNU General Public License as published by the Free Software Foundation, version 2.
//
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// this program; if not, write to the Free Software Foundation, Inc., 51
// Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
//
// Copyright SGGW Odzyskiwanie Danych Team Members
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
        /// <summary>
        /// SGGW Odzyskiwanie Danych.<br/>
        /// (c)Zespół projektowy Informatyka sggw 2014/2018
        /// GNU GPL v.2
        /// Autor: Michał Rosa
        /// </summary>
namespace JPG
{
        /// <summary>
        /// Klasa odpowiedzialna za przeszukiwanie dysku w poszukiwaniu plików JPG.
        /// </summary>
    class PoszukiwanieJPGv2
    {
        logo log;
        List<long> adresyPoczatkow;
        List<long> adresyKoncow;
        FileStream fs;
        BinaryReader br;

        string nazwaObrazuRAW;
        /// <summary>
        /// Zwraca ścieżkę i nazwę obrazu .RAW, potrzebne dalszej analizy.
        /// </summary>
        public string NazwaObrazu
        {
            get { return nazwaObrazuRAW; }
        }

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="nazwaObrazuRAW">Parametr pobierający nazwę analizowanego obrazu RAW</param>
        public PoszukiwanieJPGv2(string nazwaObrazuRAW)
        {
            this.nazwaObrazuRAW = nazwaObrazuRAW;
            adresyPoczatkow = new List<long>();
            adresyKoncow = new List<long>();
            fs = new FileStream(nazwaObrazuRAW, FileMode.Open);
            br = new BinaryReader(fs);
            //FF D8 FF E0 lub FF D8 FF E1 - poczatek
            //FF D9 - koniec
            log = new logo();
        }

        /// <summary>
        /// Moduł wyszukujący początki plików JPG
        /// </summary>
        /// <param name="aktualnaPozycja">Parametr określający adres od którego ma zostać rozpoczęte wyszukiwanie początków</param>
        public void WyszukiwaniePoczatku(long aktualnaPozycja)
        {
            logo.Dopisz("Rozpoczęto wyszukiwanie plików JPG");
            fs.Position = aktualnaPozycja;
            int count = 204800;
            while (fs.Position < fs.Length)
            {
                byte[] tabTymczasowa = br.ReadBytes(count);
                for (long i = 0; i < tabTymczasowa.Length - 3; i++)
                {
                    if (tabTymczasowa[i] == 0xFF && tabTymczasowa[i + 1] == 0xD8 && tabTymczasowa[i + 2] == 0xFF && (tabTymczasowa[i + 3] == 0xE0 || tabTymczasowa[i + 3] == 0xE1))
                    {
                        adresyPoczatkow.Add(fs.Position - count + i);
                    }
                }
            }
        }

        /// <summary>
        /// Metoda wyszukująca adresy końców plików JPG
        /// </summary>
        /// <param name="aktualnaPozycja">Parametr określający adres od którego ma zostać rozpoczęte wyszukiwanie początków</param>
        public void WyszukiwanieKonca(long aktualnaPozycja)
        {
            //Wyszukiwanie końców plików JPG            
            int licznikPoczatkow = 0;
            int licznikKoncow = 0;
            fs.Position = aktualnaPozycja;

            while (licznikPoczatkow < adresyPoczatkow.Count)
            {
                fs.Position = adresyPoczatkow[licznikPoczatkow];
                bool CzyZnaleziono = false;
                if (licznikPoczatkow + 1 < adresyPoczatkow.Count)
                {
                    int odlegloscMiedzyAdresamiPoczatkow = (int)(adresyPoczatkow[licznikPoczatkow + 1] - adresyPoczatkow[licznikPoczatkow]);
                    byte[] tabTymczasowa = br.ReadBytes(odlegloscMiedzyAdresamiPoczatkow);
                    fs.Position -= odlegloscMiedzyAdresamiPoczatkow;


                    for (long i = 0; i < tabTymczasowa.Length - 1; i++)
                    {
                        if (tabTymczasowa[i] == 0xFF && tabTymczasowa[i + 1] == 0xD9)
                        {
                            adresyKoncow.Add(fs.Position + i + 1);
                            CzyZnaleziono = true;
                            licznikKoncow++;
                        }
                    }
                }
                else
                {
                    //logo.Dopisz("tutaj kończymy");
                    //logo.Dopisz(""+(fs.Length - fs.Position));
                    byte[] tabTymczasowa2 = br.ReadBytes((int)(fs.Length - fs.Position));
                    //logo.Dopisz("tu dochodze");
                    fs.Position -= tabTymczasowa2.Length;

                    for (long i = 0; i < tabTymczasowa2.Length - 1; i++)
                    {
                        if (tabTymczasowa2[i] == 0xFF && tabTymczasowa2[i + 1] == 0xD9)
                        {
                            adresyKoncow.Add(fs.Position + i + 1);
                            CzyZnaleziono = true;
                            licznikKoncow++;
                        }
                    }
                }
                if (!CzyZnaleziono) { licznikKoncow++; }
                licznikPoczatkow++;
            }

            logo.Dopisz("Zakończono wyszukiwanie plików JPG");
        }

        /// <summary>
        /// Metoda wywołująca i splatająca poszczególne moduły wyszukiwania adresów początkowych i końcowych
        /// </summary>
        /// <param name="AdresRozpoczynajacy">Parametr określający adres od którego ma zostać rozpoczęte wyszukiwanie początków</param>
        /// <returns>Zwraca dwuelementową kolekcję "Tuple" złożoną z dwóch list: lista1 - adresy początków, lista2 - adresy końców</returns>
        public Tuple<List<long>, List<long>> WyszukiwanieJPG(long AdresRozpoczynajacy = 0)
        {
            WyszukiwaniePoczatku(AdresRozpoczynajacy);
            WyszukiwanieKonca(AdresRozpoczynajacy);
            fs.Close();
            br.Close();
            //Zwrócenie początków i końców w jednej kolekcji dla ułatwienia obsługi składania plików
            return new Tuple<List<long>, List<long>>(adresyPoczatkow, adresyKoncow);
        }
    }
}
