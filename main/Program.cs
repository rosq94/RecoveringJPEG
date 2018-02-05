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

        /// <summary>
        /// SGGW Odzyskiwanie Danych.<br/>
        /// (c)Zespół projektowy Informatyka sggw 2014/2018
        /// GNU GPL v.2
        /// Autor: Michał Rosa
        /// </summary>
        
namespace JPG
{    
        /// <summary>
        /// Klasa podstawowa programu z poziomu której wykonuje się operacje na klasach i modułach programu.
        /// </summary>
    class Program
    {
        /// <summary>
        /// Metoda Main programu
        /// </summary>
        /// <param name="args">parametr przejmujący dane od użytkownika</param>
        static int Main(string[] args)
        {
            try
            {
                logo.Dopisz(Convert.ToString(DateTime.Now));
                logo.Dopisz("---Rozpoczęcie pracy modułu JPG---");
                PoszukiwanieJPGv2 poszukiwanieJPGv2 = new PoszukiwanieJPGv2(args[0]);
                ImportDoPliku idp = new ImportDoPliku(poszukiwanieJPGv2);                
                bool CzyOdzyskano = idp.ZwracanieOdnalezionychPlikowJPG(args[1]);
                if(CzyOdzyskano)
                {
                    logo.Dopisz("---Zakończono pomyślnie---"); logo.Dopisz(" ");
                    return 800;
                }
                return 801;
            }
            catch (Exception e)
            {
               
                logo.Dopisz(e.Message);
                logo.Dopisz("---Zakończono z błędem---"); logo.Dopisz(" ");
                return 802;
            }          
        }
    }
}
