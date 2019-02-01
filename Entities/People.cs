using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ViberBot.Entities
{
    public class People
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string F { get; set; }
        
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string I { get; set; }
        
        /// <summary>
        /// Отчество клиента
        /// </summary>
        public string O { get; set; }
        
        /// <summary>
        /// Пол клиента
        /// </summary>
        public bool Sex { get; set; }
        
        /// <summary>
        /// День рождения клиента
        /// </summary>
        public DateTime? BDate { get; set; }
        
        /// <summary>
        /// Id обслуживаемой организации
        /// </summary>
        public int OrganizationId { get; set; }
        
        /// <summary>
        /// Дата/время создания записи
        /// </summary>
        public DateTime TimeInput { get; set; }
        
        /// <summary>
        /// Id сотрудника, который завёл запись о клиенте
        /// </summary>
        public Guid AgentId_Input { get; set; }
        
        /// <summary>
        /// Дата/время обновления записи
        /// </summary>
        public DateTime TimeChange { get; set; }
        
        /// <summary>
        /// Id сотрудника, который обновил запись о клиенте
        /// </summary>
        public Guid AgentId_Change { get; set; }
        
        /// <summary>
        /// Id сотрудника, который удалил запись о клиенте
        /// </summary>
        public Guid AgentId_Delete { get; set; }
        
        /// <summary>
        /// Дата/время последнего доступа к записи
        /// </summary>
        public DateTime TimeDeleted { get; set; }
        
        /// <summary>
        /// Xml информация о клиенте
        /// </summary>
        public XElement XmlInfo { get; set; }
        
        /// <summary>
        /// Список контактов пользователя
        /// </summary>
        public IEnumerable<PeopleContact> Contacts { get; set; }
    }
}