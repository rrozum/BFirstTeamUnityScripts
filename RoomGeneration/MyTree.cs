using UnityEngine;
using System.Collections.Generic;

public class MyTree
{
    // leaf / lchild / rchild - пока не очень понятно что тут должно быть

    private object leaf;
    private object lchild = null;
    private object rchild = null;
    public MyTree(object leaf)
    {
        this.leaf = leaf;
    }

    public List<object> getLeafs()
    {
        if (lchild == null && rchild == null)
        {
            var temp = new List<object>();
            temp.Add(leaf);
            return temp;
        }
        else
        {
            lchild = 

            return ;
        }
    }
}