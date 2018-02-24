using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Sort
{
    class Sis
    {
        /// <summary>
        /// 一、插入排序-直接插入排序（Straight Insertion Sort)直接插入排序(Straight Insertion Sort)的基本思想是：把n个待排序的元素看成为一个有序表和一个无序表。开始时有序表中只包含1个元素，无序表中包含有n-1个元素，排序过程中每次从无序表中取出第一个元素，将它插入到有序表中的适当位置，使之成为新的有序表，重复n-1次可完成排序过程。
        /// </summary>
        public static void tableOne()
        {
            int[] a = { -3, -1, -5, -7, -2, -4, -9, -6 };
            InsertSort(a);
            foreach (var item in a)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// 插入排序是把无序列的数一个一个插入到有序的数
        /// 先默认下标为零的这个数为有序
        /// </summary>
        /// <param name="a"></param>
        public static void InsertSort(int[] a)
        {
            //int n = 8;
            for (int i = 0; i < a.Length; i++)
            {
                int ins = a[i];//准备做对比的数
                int inst = i - 1;//找出它前一个数的下标（等下 准备插入的数 要跟这个数做比较）

                while (inst >= 0 && ins < a[inst])//这里小于是升序，大于是降序
                {
                    a[inst + 1] = a[inst];//同时把比插入数要大的数往后移
                    inst--;//指针往后移，等下插入的数也要跟这个指针指向的数做比较
                }
                //插入(这时候给ins找到适当位置)
                a[inst + 1] = ins;
            }
        }
    }
}
