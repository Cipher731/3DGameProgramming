using System;
using UnityEngine;
using Util;

namespace Interface
{
    public interface IActionCallback
    {
        void ActionEvent(
            GameAction source, 
            Type paramType = null,
            object param = null);
    }
}