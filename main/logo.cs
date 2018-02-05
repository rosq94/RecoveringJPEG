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
    /// Klasa odpowiedzialna za obsługę logowania programu.
    /// </summary>
    public class logo
    {
        /// <summary>
        /// Metoda obsługująca logowanie modułu
        /// </summary>
        /// <param name="tekst">zmienna przechowująca informacje które mają być zapisane do pliku logowania</param>
        public static void Dopisz(string tekst)
        {
            FileStream fs = new FileStream("logJPG.txt", FileMode.OpenOrCreate);
            fs.Position = fs.Length;
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(tekst);
            sw.Close(); fs.Close();
        }
    }
}
