using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Sort
{
    class test
    {
        public void two()
        {
            one();
            Console.WriteLine("方法1");
        }
        public virtual void one()
        {
            //return "测试1完成！！！";
            Console.WriteLine("测试1完成！！！");
        }
    }
    class sssss : test
    {
        int x = 1;
        int y;
        public void B()
        {
            y = 2;
        }
        public override void one()
        {
            Console.WriteLine("x=" + x + " y=" + y);
        }
    }
}
