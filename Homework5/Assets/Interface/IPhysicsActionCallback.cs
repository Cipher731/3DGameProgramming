using System;
using Util;

namespace Interface
{
    public interface IPhysicsActionCallback
    {
        void ActionEvent(
            PhysicsGameAction source, 
            Type paramType = null,
            object param = null);
    }
}