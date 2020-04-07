using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppTasks.Extensions
{
    public static class TaskExtensions
    {
		// Extensión para un Task para poderlo invocar sin necesidad de la palabra await, 
		// recibe el parámetro task que por default lo recibo,
		// recibe un booleano para decirle si regresa al contexto,
		// recibo una acción si se desea que se ejecute cuando hay una excepción
        public static async void SafeFireAndForget(this Task task, 
			                                       bool returnToCallingContext,
												   Action<Exception> onException = null)
        {
			try
			{
				await task.ConfigureAwait(returnToCallingContext);
			}
			catch (Exception ex) when (onException != null)
			{
				onException(ex);
			}
        }
    }
}
