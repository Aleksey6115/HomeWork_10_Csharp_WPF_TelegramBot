using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork_10
{
    class UsersMessage
    {
        /// <summary>
        /// Дата получения сообщения
        /// </summary>
        public string Date { get; private set; }

        /// <summary>
        /// Id пользователя отправившего сообщение
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Имя пользователя отправившего сообщение
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Тип отправленного сообщения
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Отправленное сообщение
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Конструктор класса UsersMessage
        /// </summary>
        /// <param name="date">Дата получения сообщения</param>
        /// <param name="name">Имя пользователя</param>
        /// <param name="type">Тип сообщения</param>
        /// <param name="message">Текст сообщения</param>
        public UsersMessage(string date, string name, long id, string type, string message)
        {
            Date = date;
            Name = name;
            Id = id;
            Type = type;
            Message = message;
        }
    }
}
