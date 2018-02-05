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
        /// Klasa odpowiedzialna za rekonstrukcję odnalezionych plików JPG.
        /// </summary>
    class ImportDoPliku
    {
        PoszukiwanieJPGv2 jpg;
        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="jpg">Obiekt modułu poszukiwania plików jpg</param>
        public ImportDoPliku(PoszukiwanieJPGv2 jpg)
        {
            this.jpg = jpg;
        }

        /// <summary>
        /// Metoda przywracająca pliki JPG
        /// </summary>
        /// <param name="sciezkaWyjsciowa">ścieżka do katalogu w którym mają zostać zapisane pliki</param>
        /// <returns>Zwraca true jeśli pliki zostały poprawnie przywrócone, false jeśli pliki nie zostały poprawnie przywrócone.</returns>
        public bool ZwracanieOdnalezionychPlikowJPG(string sciezkaWyjsciowa)
        {
            string nazwaPlikuRAW = jpg.NazwaObrazu;
            Tuple<List<long>, List<long>> P_K = jpg.WyszukiwanieJPG();
            
            logo.Dopisz("Rozpoczęto przywracanie plików");
            if (P_K.Item1.Count==0 ||  P_K.Item2.Count==0)
            {
                logo.Dopisz("Nie można przywrócić żadnego pliku JPG z tego obrazu");
                return false;
            }

            FileStream fs = new FileStream(nazwaPlikuRAW, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            
            string ścieżkaPoczątkowa = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(sciezkaWyjsciowa);

            
            int licznik = 0;
            int licznik2 = 0;
            for (int i = 0; i < P_K.Item1.Count; i++)
            {
                licznik2 = 0;
                for (int j = 0; j < P_K.Item2.Count; j++)
                {
                    if(P_K.Item2[j] > P_K.Item1[i] && licznik2 == 0)
                    {
                    FileStream fs2 = new FileStream(sciezkaWyjsciowa + "/JPG_" + licznik + ".jpg", FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs2);
                    
                    fs.Position = P_K.Item1[i];
                    while (fs.Position <= P_K.Item2[j])
                    {
                        bw.Write(br.ReadByte());
                    }
                    if (fs2.Length == 0) licznik--;
                    
                    fs2.Close();
                    bw.Close();
                    licznik++;
                    licznik2++;
                    }
                }               
            }

            fs.Close();
            br.Close();
            logo.Dopisz("Zakończono przywracanie plików");
            return true;
        }

    }
}
