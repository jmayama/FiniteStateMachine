using System;
using System.Collections.Generic;
using UnityEngine;


//The interface for collider hit listeners.  Interfaces are very similar to abstract classes
//where all of the functions are abstract.  They are great when you want to have a bunch
//of unrelated objects that need to be used by a system interchangeably.  I tend to use abstract classes to define
//the "main type" the objects will be and interfaces when I am designing systems that I want to work 
//with a wide range of objects
//
//For more information on abstract classes vs interfaces check out this link:
//http://msdn.microsoft.com/en-CA/library/scsyfw1d(v=vs.71).aspx
interface ColliderHitListener
{
    void OnControllerColliderHit(ControllerColliderHit hitInfo);
}
