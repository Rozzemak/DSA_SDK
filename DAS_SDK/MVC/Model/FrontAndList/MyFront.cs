namespace DAS_SDK.MVC.Model.FrontAndList
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
    /// <typeparam name="TYpe">Type of front items</typeparam>
    class MyFront<TYpe>
    {
        private string customArrayContent;
        public CustomArray<TYpe> CustomArray;

        public MyFront(int capacity = 10)
        {
            this.CustomArray = new CustomArray<TYpe>(capacity);
        }

        public void AddToFront<T>(T item, AddMethodFront addMethod = AddMethodFront.Cyclic)
        {
            switch (addMethod)
            {
                case AddMethodFront.OnlyFill:
                    if (CustomArray.GetFirstEmptyIndexNull() != null)
                    {
                        // Do proper array swap + (indexes++), without last element (i = 1!).
                        var customArray = new CustomArray<TYpe>(CustomArray.GetArrayAsType<TYpe>().Length);
                        for (var i = 1; i < CustomArray.GetArrayAsType<TYpe>().Length; i++)
                        {
                            // Just substract 1 from i to get index of previous element.
                            customArray.GetArrayAsType<TYpe>()[i] = CustomArray.GetArrayAsType<TYpe>()[i - 1];
                        }
                        customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                        // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                        CustomArray = customArray;
                    }
                    break;
                case AddMethodFront.ReplaceFirst:
                    //if (customArray.GetFirstEmptyIndex() < customArray.GetArrayAsType<Type>().Length)
                    //    customArray.GetArrayAsType<T>()[customArray.GetFirstEmptyIndex()] = item;
                    // What the hell was I thinking ?!
                    CustomArray.GetArrayAsType<T>()[0] = item;
                    break;
                case AddMethodFront.Cyclic:
                    // First check if the front was properly filled, 
                    // than make new array with items moved to +1 index 
                    // and first index free.
                    // Do proper array swap + (indexes++), without last element (i = 1!).
                    var customArray2 = new CustomArray<TYpe>(CustomArray.GetArrayAsType<TYpe>().Length);
                    for (var i = 1; i < CustomArray.GetArrayAsType<TYpe>().Length; i++)
                    {
                        // Just substract 1 from i to get index of previous element.
                        customArray2.GetArrayAsType<TYpe>()[i] = CustomArray.GetArrayAsType<TYpe>()[i - 1];
                    }
                    customArray2.GetArrayAsType<T>()[customArray2.GetFirstEmptyIndex()] = item;
                    // Swap references, previous custom array will be destroyed by VM. (Garbage C.)
                    CustomArray = customArray2;
                    break;
            }
        }

        public void RemoveFromFront()
        {
            if (CustomArray.GetFirstEmptyIndex() > 0 || CustomArray.GetArrayAsType<object>().Length > 0)
                if (CustomArray.IsFull())
                    CustomArray.GetArrayAsType<TYpe>()[CustomArray.GetArrayAsType<TYpe>().Length - 1] = default(TYpe);
                else
                    if (CustomArray.GetFirstEmptyIndex() > 0)
                        CustomArray.GetArrayAsType<TYpe>()[CustomArray.GetFirstEmptyIndex() - 1] = default(TYpe);
        }

        public void RemoveFromFront<T>()
        {
            if (CustomArray.GetFirstEmptyIndex() > 0 || CustomArray.GetArrayAsType<T>().Length > 0)
                if (CustomArray.IsFull())
                    CustomArray.GetArrayAsType<T>()[CustomArray.GetArrayAsType<T>().Length - 1] = default(T);
                else
                    if (CustomArray.GetFirstEmptyIndex() > 0)
                        CustomArray.GetArrayAsType<T>()[CustomArray.GetFirstEmptyIndex() - 1] = default(T);
        }

        public string GetFrontContent()
        {
            customArrayContent = "Obj_Hash: [" + CustomArray.GetHashCode().ToString() + "] Front_Length: ([" + CustomArray.GetArrayAsType<TYpe>().Length + "]); ";
            customArrayContent += "Front_Content: " + "\n";
            foreach (var item in CustomArray.GetArrayAsType<TYpe>())
            {
                customArrayContent += "[" + item + "]";
            }
            customArrayContent += ";";
            return customArrayContent;
        }

        public void ClearFront(bool defaultSize = true)
        {
            CustomArray.Clear(defaultSize);
        }

        public void ClearFront(int setSize = 10)
        {
            CustomArray.Clear(setSize);
        }
    }
}
