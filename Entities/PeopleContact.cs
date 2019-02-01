using System;
using ViberBot.Models;

namespace ViberBot.Entities
{
    public partial class PeopleContact
    {
        /// <summary>
        /// Id записи 
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Тип контакта
        /// </summary>
        public int ContactTypeId { get; set; }
        
        /// <summary>
        /// Id обслуживаемой организации
        /// </summary>
        public int OrganizationId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public long InfoBigint { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string InfoTextId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string InfoText { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeActual { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int MessageRecommendId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int VoiceRecommendId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ContactServiceStateId { get; set; }
    }
}