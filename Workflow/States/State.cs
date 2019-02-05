using System;
using System.Threading.Tasks;

namespace ViberBot.Workflow.States
{
    public abstract class State
    {
        protected IContext context;

        public void SetContext(IContext context)
        {
            this.context = context;
        }

        protected Task RouteError()
        {
            return Task.CompletedTask;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public virtual Task Start(int botId, Guid agentId) { return Task.CompletedTask; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="altitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public virtual Task SearchGarbageAreas(double altitude, double latitude) { return Task.CompletedTask; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="garbageAreaName"></param>
        /// <returns></returns>
        public virtual Task SelectGarbageArea(string garbageAreaName) { return Task.CompletedTask; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task WaitMediaFile() { return Task.CompletedTask; }
    }
}