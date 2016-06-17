//2014,2015 BSD,WinterDev   
//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------

// 2015,2014 ,MIT, WinterDev
//----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Collections;
namespace PixelFarm.Agg
{
    ////--------------------------------------------------------------pod_vector
    //// A simple class template to store Plain Old Data (POD), a vector
    //// of a fixed size. The data is contiguous in memory
    ////------------------------------------------------------------------------
    //public class ArrayList<T>
    //{
    //    int usedSize; 
    //    T[] internalArray;
    //    public ArrayList()
    //    {
    //        //current size=0
    //        internalArray = new T[4];//default 4
    //    }
    //    public ArrayList(int cap)
    //    {
    //        internalArray = new T[cap];//default 4
    //        usedSize = 0;
    //    }
    //    public ArrayList(ArrayList<T> srcCopy, int plusSize)
    //    {
    //        Allocate(srcCopy.AllocatedSize, srcCopy.AllocatedSize + plusSize);
    //        if (srcCopy.usedSize != 0)
    //        {
    //            srcCopy.internalArray.CopyTo(internalArray, 0);
    //        }
    //    }
    //    public void RemoveLast()
    //    {
    //        if (usedSize != 0)
    //        {
    //            usedSize--;
    //        }
    //    }

    //    public int Count
    //    {
    //        get { return usedSize; }
    //    }

    //    public int AllocatedSize
    //    {
    //        get
    //        {
    //            return internalArray.Length;
    //        }
    //    }

    //    public void Clear()
    //    {
    //        usedSize = 0;
    //    }

    //    // Set new capacity. All data is lost, size is set to zero.
    //    public void Clear(int newCapacity)
    //    {
    //        Clear(newCapacity, 0);
    //    }
    //    public void Clear(int newCapacity, int extraTail)
    //    {
    //        usedSize = 0;
    //        if (newCapacity > AllocatedSize)
    //        {
    //            internalArray = null;
    //            int sizeToAllocate = newCapacity + extraTail;
    //            if (sizeToAllocate != 0)
    //            {
    //                internalArray = new T[sizeToAllocate];
    //            }
    //        }
    //    }



    //    // Allocate n elements. All data is lost, 
    //    // but elements can be accessed in range 0...size-1. 
    //    public void Allocate(int size)
    //    {
    //        Allocate(size, 0);
    //    }

    //    void Allocate(int size, int extraTail)
    //    {
    //        Clear(size, extraTail);
    //        usedSize = size;
    //    }

    //    /// <summary>
    //    ///  Resize keeping the content
    //    /// </summary>
    //    /// <param name="newSize"></param>
    //    public void AdjustSize(int newSize)
    //    {
    //        if (newSize > usedSize)
    //        {
    //            if (newSize > AllocatedSize)
    //            {
    //                //create new array and copy data to that 
    //                var newArray = new T[newSize];
    //                if (internalArray != null)
    //                {
    //                    for (int i = internalArray.Length - 1; i >= 0; --i)
    //                    {
    //                        newArray[i] = internalArray[i];
    //                    }
    //                }
    //                internalArray = newArray;
    //            }
    //        }
    //    }


    //    static T zeroed_object = default(T);


    //    public void Zero()
    //    {

    //        for (int i = internalArray.Length - 1; i >= 0; --i)
    //        {
    //            internalArray[i] = zeroed_object;
    //        }
    //    }



    //    public virtual void AddVertex(T v)
    //    {
    //        if (internalArray.Length < (usedSize + 1))
    //        {
    //            if (usedSize < 100000)
    //            {
    //                AdjustSize(usedSize + (usedSize / 2) + 16);
    //            }
    //            else
    //            {
    //                AdjustSize(usedSize + usedSize / 4);
    //            }
    //        }
    //        internalArray[usedSize++] = v;
    //    }


    //    public T this[int i]
    //    {
    //        get
    //        {
    //            return internalArray[i];
    //        }
    //    }

    //    public T[] Array
    //    {
    //        get
    //        {
    //            return internalArray;
    //        }
    //    }



    //    public void SetData(int index, T data)
    //    {
    //        this.internalArray[index] = data;
    //    }

    //    public int Length
    //    {
    //        get
    //        {
    //            return usedSize;
    //        }
    //    }
    //}
    public class ArrayList<T>
    {
        int currentSize;
        T[] internalArray = new T[0];
        public ArrayList()
        {
        }
        public ArrayList(int cap)
        {
            Allocate(cap, 0);
        }
        public ArrayList(ArrayList<T> srcCopy, int plusSize)
        {
            Allocate(srcCopy.AllocatedSize, srcCopy.AllocatedSize + plusSize);
            if (srcCopy.currentSize != 0)
            {
                srcCopy.internalArray.CopyTo(internalArray, 0);
            }
        }
        public void RemoveLast()
        {
            if (currentSize != 0)
            {
                currentSize--;
            }
        }


        public int Count
        {
            get { return currentSize; }
        }

        public int AllocatedSize
        {
            get
            {
                return internalArray.Length;
            }
        }

        public void Clear()
        {
            currentSize = 0;
        }

        // Set new capacity. All data is lost, size is set to zero.
        public void Clear(int newCapacity)
        {
            Clear(newCapacity, 0);
        }
        public void Clear(int newCapacity, int extraTail)
        {
            currentSize = 0;
            if (newCapacity > AllocatedSize)
            {
                internalArray = null;
                int sizeToAllocate = newCapacity + extraTail;
                if (sizeToAllocate != 0)
                {
                    internalArray = new T[sizeToAllocate];
                }
            }
        }



        // Allocate n elements. All data is lost, 
        // but elements can be accessed in range 0...size-1. 
        public void Allocate(int size)
        {
            Allocate(size, 0);
        }

        void Allocate(int size, int extraTail)
        {
            Clear(size, extraTail);
            currentSize = size;
        }

        /// <summary>
        ///  Resize keeping the content
        /// </summary>
        /// <param name="newSize"></param>
        public void AdjustSize(int newSize)
        {
            if (newSize > currentSize)
            {
                if (newSize > AllocatedSize)
                {
                    //create new array and copy data to that 
                    var newArray = new T[newSize];
                    if (internalArray != null)
                    {
                        for (int i = internalArray.Length - 1; i >= 0; --i)
                        {
                            newArray[i] = internalArray[i];
                        }
                    }
                    internalArray = newArray;
                }
            }
        }


        static T zeroed_object = default(T);
        public void Zero()
        {
            for (int i = internalArray.Length - 1; i >= 0; --i)
            {
                internalArray[i] = zeroed_object;
            }
        }



        public virtual void AddVertex(T v)
        {
            if (internalArray.Length < (currentSize + 1))
            {
                if (currentSize < 100000)
                {
                    AdjustSize(currentSize + (currentSize / 2) + 16);
                }
                else
                {
                    AdjustSize(currentSize + currentSize / 4);
                }
            }
            internalArray[currentSize++] = v;
        }


        public T this[int i]
        {
            get
            {
                return internalArray[i];
            }
            set { this.internalArray[i] = value; }
        }

        public T[] Array
        {
            get
            {
                return internalArray;
            }
        }



        public void SetData(int index, T data)
        {
            this.internalArray[index] = data;
        }

        public int Length
        {
            get
            {
                return currentSize;
            }
        }
    }
}
