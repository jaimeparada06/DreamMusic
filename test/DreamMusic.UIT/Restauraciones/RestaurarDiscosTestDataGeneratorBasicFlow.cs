using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamMusic.UIT.Restauraciones
{
    class RestaurarDiscosTestDataGeneratorBasicFlow: IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "123", DateTime.Today.AddYears(2).ToString(), null, null, null},
        new object[] {"Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "elena@uclm.com", "213", "673270"},
    };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
