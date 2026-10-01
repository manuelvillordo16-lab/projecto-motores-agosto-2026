using UnityEngine;

public class expsystem : MonoBehaviour
{
   private int Exp;
   private int Skillpoints;
   private int ExpLimit = 100;

    void Update()
    {
        if (Exp > ExpLimit) 
        {
            Skillpoints++;
            Exp = 0;
            ExpLimit + 50;
        }
    }
}
