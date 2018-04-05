using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model
{
    class CustomArray<T>
    {
        private T[] array;

        public CustomArray(int capacity = 10)
        {
            array = new T[capacity];
        }

        public Type[] GetArrayAsType<Type>()
        {
            return array as Type[];
        }


        /// <summary>
        /// This was necessary creation from similarly named method. 
        /// This will return null, if there is no empty index, but will return 
        /// int as empty index. .. if it exists.
        /// </summary>
        /// <returns> First empty index of array.  Null if there isn´t.</returns>
        public int? GetFirstEmptyIndexNull()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    return i;
                }
                if (i == array.Length && array[i] != null) return (array.Length - 1);
            }
            return null;
        }

        public bool IsFull()
        {
            foreach (var item in array)
            {
                if (item == null) return false;
            }
            return true;
        }

        /// <summary>
        /// This method is little bit tricky,
        /// because it will return 0 when the array is full. 
        /// Usefull for front.
        /// </summary>
        /// <returns>First empty index of array.  0 if there isn´t.</returns>
        public int GetFirstEmptyIndex()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                {
                    return i;
                }
            }
            return 0;
        }

        public void Clear(bool defaultSize = true)
        {
            if (defaultSize)
                array = new T[10];
            else
                array = new T[array.Length];
        }

        public void Clear(int setSize = 10)
        {
            array = new T[setSize];
        }
    }
}
