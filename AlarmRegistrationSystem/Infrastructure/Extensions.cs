using AlarmRegistrationSystem.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Infrastructure
{
    public static class Extensions
    {

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        /// <summary>
        /// Checks whether the element contains text regardless of letter size and spaces
        /// </summary>
        public static bool IsStringContains(this string element, string text)
        {
            if(element.ToLower().Contains(text.ToLower()) || element.ToLower().Replace(" ", "").Contains(text.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SaveToExcel(this AppUser user,string password , string filename = @"UsersPass.xlsx", string path = null)
        {
            if(path == null)
            {
                path = @"C:\Users\Mateusz\Desktop";
            }
            
            FileInfo file = new FileInfo(Path.Combine(path, filename));

            using (var package = new ExcelPackage(file))
            {
                string tab = "Dane_uzytkownikow";
                var worksheet = package.Workbook.Worksheets.FirstOrDefault(m => m.Name == tab);
                if (worksheet == null)
                {
                    worksheet = package.Workbook.Worksheets.Add(tab);
                    worksheet.Cells[1, 1].Value = "Imie";
                    worksheet.Cells[1, 2].Value = "Nazwisko";
                    worksheet.Cells[1, 3].Value = "Nazwa Uzytkownika";
                    worksheet.Cells[1, 4].Value = "e-mail";
                    worksheet.Cells[1, 5].Value = "Haslo";
                }
                const int maxRow = 1000000; //One Millon Rows
                for (int i = worksheet.Dimension.End.Row; i < maxRow; i++)
                {
                    if (worksheet.Cells["A" + i].Value == null)
                    {
                        worksheet.Cells["A" + i].Value = user.FirstName;
                        worksheet.Cells["B" + i].Value = user.SecondName;
                        worksheet.Cells["C" + i].Value = user.UserName;
                        worksheet.Cells["D" + i].Value = user.Email;
                        worksheet.Cells["E" + i].Value = password;
                        break;
                    }
                }
                package.Save();
            }
        }
    }
}





