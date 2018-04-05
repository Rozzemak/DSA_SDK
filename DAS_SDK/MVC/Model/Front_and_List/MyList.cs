using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model
{
    enum AddMethodList
    {
        ReplaceFirst,
        AddToStack_ReplaceLast,
        OnlyFill,
        AddToStack_Cyclic,
        AddToStack_Dynamic
    }

    class MyList<Type>
    {
        private string customArrayContent;
        public CustomArray<Type> customArray;

        public MyList(int capacity = 10)
        {
            this.customArray = new CustomArray<Type>(capacity);
        }

        public void AddToList<T>(T item, AddMethodList addMethod = AddMethodList.AddToStack_Dynamic)
        {
            switch (addMethod)
            {
                case AddMethodList.OnlyFill:
                    if (customArray.GetFirstEmptyIndexNull() != null)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    break;
                case AddMethodList.ReplaceFirst:
                    customArray.GetArrayAsType<T>()[0] = item;
                    break;
                case AddMethodList.AddToStack_ReplaceLast:
                    // First check if the front was properly filled, 
                    if (customArray.GetFirstEmptyIndexNull() != null)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        customArray.GetArrayAsType<T>()[customArray.GetArrayAsType<Type>().Length-1] = item;
                    }
                    break;
                case AddMethodList.AddToStack_Cyclic:
                    // First check if the front was properly filled, 
                    if (customArray.GetFirstEmptyIndexNull() != null)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        customArray.GetArrayAsType<T>()[customArray.GetArrayAsType<Type>().Length - 1] = item;
                    }
                    break;
                case AddMethodList.AddToStack_Dynamic:
                    // First check if the front was properly filled, 
                    if (customArray.GetFirstEmptyIndexNull() != null)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        int _size = customArray.GetArrayAsType<Type>().Length;
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        if (customArray.IsFull())
                        {
                            _size = customArray.GetArrayAsType<Type>().Length+1;
                        }
                        CustomArray<Type> _customArray2 = new CustomArray<Type>(_size);
                        for (int i = 1; i < customArray.GetArrayAsType<T>().Length-1; i++)
                        {
                            // Just substract 1 from i to get index of previous element.
                            if(customArray.GetArrayAsType<Type>()[i] != null)
                            _customArray2.GetArrayAsType<Type>()[i-1] = customArray.GetArrayAsType<Type>()[i];
                        }
                        _customArray2.GetArrayAsType<T>()[_customArray2.GetFirstEmptyIndex()] = item;
                        // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                        customArray = _customArray2;                 
                    }
                    break;
            }
        }

        public void RemoveFromList()
        {
            if (customArray.GetFirstEmptyIndex() > 0 || customArray.GetArrayAsType<object>().Length > 0)
                if (customArray.IsFull())
                    customArray.GetArrayAsType<Type>()[customArray.GetArrayAsType<Type>().Length-1] = default(Type);
                else
                    if (customArray.GetFirstEmptyIndex() > 0)
                        customArray.GetArrayAsType<Type>()[customArray.GetFirstEmptyIndex() - 1] = default(Type);
        }

        public void RemoveFromList<T>()
        {
            if (customArray.GetFirstEmptyIndex() > 0 || customArray.GetArrayAsType<T>().Length > 0)
                if (customArray.IsFull())
                    customArray.GetArrayAsType<T>()[customArray.GetArrayAsType<T>().Length-1] = default(T);
                else
                    if (customArray.GetFirstEmptyIndex() > 0)
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex() - 1] = default(T);
        }

        public string GetListContent()
        {
            customArrayContent = "Obj_Hash: [" + customArray.GetHashCode().ToString() + "] List_Length: ([" + customArray.GetArrayAsType<Type>().Length + "]); ";
            customArrayContent += "List_Content: " + "\n";
            foreach (var item in customArray.GetArrayAsType<Type>())
            {
                customArrayContent += "[" + item + "]";
            }
            customArrayContent += ";";
            return customArrayContent;
        }

        public void ClearList(bool defaultSize = true)
        {
            customArray.Clear(defaultSize);
        }

        public void ClearList(int setSize = 10)
        {
            customArray.Clear(setSize);
        }

        

    }
}
