using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Interfaces
{
    interface IDrawable
    {
        bool IsObservable(float range);
        void Draw();
        void Update();
        bool Dispose();
        void AddToDrawCollection();
    }
}
