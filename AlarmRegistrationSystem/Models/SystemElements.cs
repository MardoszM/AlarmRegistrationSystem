using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Models
{
    public class SystemElements : ISytemElements
    {
        public IDictionary<string, string> ErrorMessages { get; set; }

        public SystemElements()
        {
            ErrorMessages = new Dictionary<string, string>();
            ErrorMessages.Add("database", "Baza danych jest niedostepna. Skontaktuj sie z administratorem systemu.");
            ErrorMessages.Add("system", "Wystapil blad systemu.Skontaktuj sie z administratorem.");
        }

    }
}
