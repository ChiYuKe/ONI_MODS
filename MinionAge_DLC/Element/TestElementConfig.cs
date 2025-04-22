using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MinionAge_DLC
{
    internal class TestElementConfig : IOreConfig
    {
        
        public SimHashes ElementID
        {
            get
            {
                return NewSimHashes.TestElement;
            }
        }


        public GameObject CreatePrefab()
        {



            GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
       
            gameObject.SetActive(false);

          

            return gameObject;
        }
    }
}
