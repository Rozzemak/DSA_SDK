using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model
{

    enum AddMethodFront
    {
        ReplaceFirst,
        Cyclic,
        OnlyFill
    }

    /// <summary>
    /// So, this is my version of generic array => front.
    /// By using MyArray class, which aslo uses generic type, same as type of this class, 
    /// you will be able to create generic front. 
    /// Common issues:
    /// Type must be nullable!, if not, there is overloaded method, 
    /// where you must specify wanted type for adding.
    /// Ex. =>  myFront.AddToFront<int?>(2);
    /// But this is neccesarry only, if the type is not nullable. As of primitives.
    /// Objects should be fine, so,
    /// Ex for nullable => myFront.AddToFront("nullable");
    /// </summary>
    /// <typeparam name="Type">Type of front items</typeparam>
    class MyFront<Type>
    {
        private string customArrayContent;
        public CustomArray<Type> customArray;

        public MyFront(int capacity = 10)
        {
            this.customArray = new CustomArray<Type>(capacity);
        }

        public void AddToFront<T>(T item, AddMethodFront addMethod = AddMethodFront.Cyclic)
        {
            switch (addMethod)
            {
                case AddMethodFront.OnlyFill:
                    if (customArray.GetFirstEmptyIndexNull() != null)
                    {
                        // Do proper array swap + (indexes++), without last element (i = 1!).
                        CustomArray<Type> _customArray = new CustomArray<Type>(customArray.GetArrayAsType<Type>().Length);
                        for (int i = 1; i < customArray.GetArrayAsType<Type>().Length; i++)
                        {
                            // Just substract 1 from i to get index of previous element.
                            _customArray.GetArrayAsType<Type>()[i] = customArray.GetArrayAsType<Type>()[i - 1];
                        }
                        _customArray.GetArrayAsType<T>()[_customArray.GetFirstEmptyIndex()] = item;
                        // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                        customArray = _customArray;
                    }
                    break;
                case AddMethodFront.ReplaceFirst:
                    //if (customArray.GetFirstEmptyIndex() < customArray.GetArrayAsType<Type>().Length)
                    //    customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    // What the hell was I thinking ?!
                    customArray.GetArrayAsType<T>()[0] = item;
                    break;
                case AddMethodFront.Cyclic:
                    // First check if the front was properly filled, 
                    // than make new array with items moved to +1 index 
                    // and first index free.
                    // Do proper array swap + (indexes++), without last element (i = 1!).
                    CustomArray<Type> _customArray2 = new CustomArray<Type>(customArray.GetArrayAsType<Type>().Length);
                    for (int i = 1; i < customArray.GetArrayAsType<Type>().Length; i++)
                    {
                        // Just substract 1 from i to get index of previous element.
                        _customArray2.GetArrayAsType<Type>()[i] = customArray.GetArrayAsType<Type>()[i - 1];
                    }
                    _customArray2.GetArrayAsType<T>()[_customArray2.GetFirstEmptyIndex()] = item;
                    // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                    customArray = _customArray2;
                    break;
            }
        }

        public void RemoveFromFront()
        {
            if (customArray.GetFirstEmptyIndex() > 0 || customArray.GetArrayAsType<object>().Length > 0)
                if (customArray.IsFull())
                    customArray.GetArrayAsType<Type>()[customArray.GetArrayAsType<Type>().Length - 1] = default(Type);
                else
                    if (customArray.GetFirstEmptyIndex() > 0)
                        customArray.GetArrayAsType<Type>()[customArray.GetFirstEmptyIndex() - 1] = default(Type);
        }

        public void RemoveFromFront<T>()
        {
            if (customArray.GetFirstEmptyIndex() > 0 || customArray.GetArrayAsType<T>().Length > 0)
                if (customArray.IsFull())
                    customArray.GetArrayAsType<T>()[customArray.GetArrayAsType<T>().Length - 1] = default(T);
                else
                    if (customArray.GetFirstEmptyIndex() > 0)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex() - 1] = default(T);
        }

        public string GetFrontContent()
        {
            customArrayContent = "Obj_Hash: [" + customArray.GetHashCode().ToString() + "] Front_Length: ([" + customArray.GetArrayAsType<Type>().Length + "]); ";
            customArrayContent += "Front_Content: " + "\n";
            foreach (var item in customArray.GetArrayAsType<Type>())
            {
                customArrayContent += "[" + item + "]";
            }
            customArrayContent += ";";
            return customArrayContent;
        }

        public void ClearFront(bool defaultSize = true)
        {
            customArray.Clear(defaultSize);
        }

        public void ClearFront(int setSize = 10)
        {
            customArray.Clear(setSize);
        }
    }
}
