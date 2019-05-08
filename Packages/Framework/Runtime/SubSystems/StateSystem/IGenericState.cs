/*
 * Description: Interface for FStateManagement.
 * Author: Ideen Molavi, ideenmolavi@gmail.com
 * Creation Date: April 19, 2018
 */

namespace Revy.Framework
{
	public interface IGenericState<TContext>
	{
        void SetState<TContextState>(TContext context, TContextState contextState);

        TContextState GetState<TContextState>(TContext context);
	}
}