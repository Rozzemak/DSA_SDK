using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Interfaces
{
    interface IServise<T>
    {
        void AddToCollection(T type);
        void RemoveFromCollection(T type);
        List<T> GetCollection();
    }
}
