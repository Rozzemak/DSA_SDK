namespace DAS_SDK.MVC.Model.FrontAndList
{
    enum AddMethodList
    {
        ReplaceFirst,
        AddToStackReplaceLast,
        OnlyFill,
        AddToStackCyclic,
        AddToStackDynamic
    }

    class MyList<TYpe>
    {
        private string customArrayContent;
        public CustomArray<TYpe> CustomArray;

        public MyList(int capacity = 10)
        {
            this.CustomArray = new CustomArray<TYpe>(capacity);
        }

        public void AddToList<T>(T item, AddMethodList addMethod = AddMethodList.AddToStackDynamic)
        {
            switch (addMethod)
            {
                case AddMethodList.OnlyFill:
                    if (CustomArray.GetFirstEmptyIndexNull() != null)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex()] = item;
                    break;
                case AddMethodList.ReplaceFirst:
                    CustomArray.GetArrayAsType<T>()[0] = item;
                    break;
                case AddMethodList.AddToStackReplaceLast:
                    // First check if the front was properly filled, 
                    if (CustomArray.GetFirstEmptyIndexNull() != null)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetArrayAsType<TYpe>().Length-1] = item;
                    }
                    break;
                case AddMethodList.AddToStackCyclic:
                    // First check if the front was properly filled, 
                    if (CustomArray.GetFirstEmptyIndexNull() != null)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetArrayAsType<TYpe>().Length - 1] = item;
                    }
                    break;
                case AddMethodList.AddToStackDynamic:
                    // First check if the front was properly filled, 
                    if (CustomArray.GetFirstEmptyIndexNull() != null)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex()] = item;
                    else
                    {
                        var size = CustomArray.GetArrayAsType<TYpe>().Length;
                        // Replace last element or move every index to -1 position, or create new array (bigger?)
                        if (CustomArray.IsFull())
                        {
                            size = CustomArray.GetArrayAsType<TYpe>().Length+1;
                        }
                        var customArray2 = new CustomArray<TYpe>(size);
                        for (var i = 1; i < CustomArray.GetArrayAsType<T>().Length-1; i++)
                        {
                            // Just substract 1 from i to get index of previous element.
                            if(CustomArray.GetArrayAsType<TYpe>()[i] != null)
                            customArray2.GetArrayAsType<TYpe>()[i-1] = CustomArray.GetArrayAsType<TYpe>()[i];
                        }
                        customArray2.GetArrayAsType<T>()[customArray2.GetFirstEmptyIndex()] = item;
                        // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                        CustomArray = customArray2;                 
                    }
                    break;
            }
        }

        public void RemoveFromList()
        {
            if (CustomArray.GetFirstEmptyIndex() > 0 || CustomArray.GetArrayAsType<object>().Length > 0)
                if (CustomArray.IsFull())
                    CustomArray.GetArrayAsType<TYpe>()[CustomArray.GetArrayAsType<TYpe>().Length-1] = default(TYpe);
                else
                    if (CustomArray.GetFirstEmptyIndex() > 0)
                        CustomArray.GetArrayAsType<TYpe>()[CustomArray.GetFirstEmptyIndex() - 1] = default(TYpe);
        }

        public void RemoveFromList<T>()
        {
            if (CustomArray.GetFirstEmptyIndex() > 0 || CustomArray.GetArrayAsType<T>().Length > 0)
                if (CustomArray.IsFull())
                    CustomArray.GetArrayAsType<T>()[CustomArray.GetArrayAsType<T>().Length-1] = default(T);
                else
                    if (CustomArray.GetFirstEmptyIndex() > 0)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex() - 1] = default(T);
        }

        public string GetListContent()
        {
            customArrayContent = "Obj_Hash: [" + CustomArray.GetHashCode().ToString() + "] List_Length: ([" + CustomArray.GetArrayAsType<TYpe>().Length + "]); ";
            customArrayContent += "List_Content: " + "\n";
            foreach (var item in CustomArray.GetArrayAsType<TYpe>())
            {
                customArrayContent += "[" + item + "]";
            }
            customArrayContent += ";";
            return customArrayContent;
        }

        public void ClearList(bool defaultSize = true)
        {
            CustomArray.Clear(defaultSize);
        }

        public void ClearList(int setSize = 10)
        {
            CustomArray.Clear(setSize);
        }

        

    }
}
