using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    private int potionType, potionValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Returns the type of potion (1 = Health, 2 = Mana)
    public int getPotionType()
    {
        return potionType;
    }

    // Returns the value of the potion (SH = 5, LH = 20, SM = 1, LM = 5)
    public int getPotionValue()
    {
        return potionValue;
    }
}
