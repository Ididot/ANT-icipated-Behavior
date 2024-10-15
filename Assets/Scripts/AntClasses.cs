using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntClasses{
    public class AntClass
    { //Default Ant-class
        public int AntHealth=50;
        public int spawnEnergy=100;
        public float movementCost=0.1f;
        public int carryCapacity=1;
        private string type= "Default";


        public AntClass() { 

        }
    }
}
