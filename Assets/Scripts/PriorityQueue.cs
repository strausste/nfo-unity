using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    // ====================================================================================
    // Class attributes
    // ====================================================================================
    
    private List<T> heap;
    private Comparison<T> comparison; // priority criterion (lambda)
    
    public PriorityQueue(Comparison<T> comparison)
    {
        this.heap = new List<T>();
        this.comparison = comparison;
    }
    
    // ====================================================================================
    
    
    // ====================================================================================
    // Class methods
    // ====================================================================================
    
    /** Returns the number of items in the heap */
    public int Count()
    {
        return this.heap.Count;
    }
    
    public int GetHeapLastIndex()
    {
        return this.heap.Count - 1;
    }

    /** This method adds a T item to the heap maintaining the heap property  */
    public void Enqueue(T item)
    {
        heap.Add(item); // item is added at the end of the heap
        int i = GetHeapLastIndex();

        while (i > 0)
        {
            int j = (i - 1) / 2; // index of the parent node
            
            // Check if heap property is satisfied
            if (comparison(heap[i], heap[j]) >= 0)
                break; // if it satisfied, exit from while
            
            // Rearrange heap: swap i and j
            (heap[i], heap[j]) = (heap[j], heap[i]);
            
            // i takes the parent index
            i = j; 
        }
    }

    public T Dequeue()
    {
        T headItem = heap[0]; // item to dequeue (the smallest one in the heap and root)
        
        int count = GetHeapLastIndex();
        heap[0] = heap[count];  // the last heap's item becomes the new temporary head item 
        heap.RemoveAt(count);   // remove duplicated last item 
        
        // We are removing 1 item from heap
        count--; 
        
        // Starting from root
        int i = 0;
        while (true) // actually, it continues until heap property is satisfied
        {
            // Index of the left child
            int j = (i * 2) + 1; 
            
            // If there is not a left child
            if (j > count) 
                break;

            // Index of right child
            int k = j + 1;
            
            // Check if right child is smaller than left child
            if (k <= count && comparison(heap[k], heap[i]) < 0) 
                j = k; // if so, update j 
            
            // Heap property (current item smaller than or equal to its smallest child)
            if (comparison(heap[j], heap[i]) >= 0)
                break;
            
            // Rearrange heap: swap i and j
            (heap[i], heap[j]) = (heap[j], heap[i]);
            
            // i takes the smallest child's index
            i = j; 
        }

        return headItem;
    }
    
    // ====================================================================================
}
