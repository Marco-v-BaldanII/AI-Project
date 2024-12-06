using Pada1.BBCore.Framework;
using Pada1.BBCore;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BBCore.Conditions
{
    /// <summary>
    /// It is a basic condition to check if Booleans have the same value.
    /// </summary>
    [Condition("Basic/CheckBool")]
    [Help("Checks whether two booleans have the same value")]
    public class CheckBool : ConditionBase
    {
        [InParam("executor")]
        [Help(" I hate this pluggin ")]
        public BehaviorExecutor executor;


        ///<value>Input First Boolean Parameter.</value>
        [InParam("valueA_index")]
        [Help("First value to be compared")]
        public int valueA_index;

        ///<value>Input Second Boolean Parameter.</value>
        [InParam("valueB")]
        [Help("Second value to be compared")]
        public bool valueB;

        /// <summary>
        /// Checks whether two booleans have the same value.
        /// </summary>
        /// <returns>the value of compare first boolean with the second boolean.</returns>
		public override bool Check()
		{

            var booleans = executor.blackboard.boolParams;

            //UnityEngine.Debug.Log(booleans[valueA_index] == valueB  );
			return booleans[valueA_index] == valueB;
		}
    }
}