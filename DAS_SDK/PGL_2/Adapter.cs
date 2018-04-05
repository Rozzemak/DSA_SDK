using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.PGL_2
{
    class Adapter : Receiver
    {

        public override List<bool> Receive()
        {
            var list = emiter.Emit();
            list.Reverse();
            return list;
        }

    }

    class Emiter
    {
        List<bool> data = new List<bool> { true, false, false, true, true, false};

        public List<bool> Emit()
        {
            return this.data;
        }
        
    }

    class Receiver
    {
        protected Emiter emiter = new Emiter();

        public virtual List<bool> Receive()
        {
            return this.emiter.Emit();
        }
    }
}
