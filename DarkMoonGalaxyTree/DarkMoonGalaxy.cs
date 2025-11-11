using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMoonGalaxy
{
    public class DarkMoonGalaxy : StateMachineComponent<DarkMoonGalaxy.StatesInstance>
    {

        public class States : GameStateMachine<States, StatesInstance, DarkMoonGalaxy>
        { 
        
        }


        public class StatesInstance : GameStateMachine<States, StatesInstance, DarkMoonGalaxy, object>.GameInstance
        {
            public StatesInstance(DarkMoonGalaxy master)
                : base(master)
            {


            }
        }



    }
}
