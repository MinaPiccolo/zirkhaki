/*
 * Description: Mange state of game.
 * Author: Ideen Molavi, ideenmolavi@gmail.com
 * Creation Date: April 19, 2018
 */

using System.Collections.Generic;

namespace Revy.Framework
{
    public abstract class GenericStateSystem<TContext> : IGenericState<TContext>
    {
        #region Fields
        private Dictionary<TContext, System.Object> _statesDictionary = new Dictionary<TContext, object>();
        #endregion Fields

        #region Class Interface
        public TContextState GetState<TContextState>(TContext context)
        {
            TContextState stateResult = default(TContextState);
            if (_statesDictionary.ContainsKey(context))
            {
                stateResult = (TContextState)_statesDictionary[context];
            }
            return stateResult;
        }

        public void SetState<TContextState>(TContext context, TContextState contextState)
        {
            if (_statesDictionary.ContainsKey(context))
            {
                _statesDictionary[context] = contextState;
            }
            else
            {
                _statesDictionary.Add(context, contextState);
            }
        }
        #endregion
    }
}