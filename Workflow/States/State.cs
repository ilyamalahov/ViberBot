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
            return Task.FromException<string>(new Exception("You can't move to next state from current state"));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public virtual Task Start(int botId, Guid agentId) => RouteError();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="altitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public virtual Task SearchGarbageAreas(double altitude, double latitude) => RouteError();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="garbageAreaName"></param>
        /// <returns></returns>
        public virtual Task SelectGarbageArea(string garbageAreaName) => RouteError();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Task WaitMediaFile() => RouteError();
    }
}