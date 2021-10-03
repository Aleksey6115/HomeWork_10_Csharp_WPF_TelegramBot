﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Telegram.Bot;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HomeWork_10
{
    class TelegramOptions
    {
        /// <summary>
        /// Метод загружает файл на ПК
        /// </summary>
        /// <param name="fileId">id файла</param>
        /// <param name="path">Путь для созранения</param>
        /// <param name="bot">бот</param>
        public async void Upload(string fileId, string path, TelegramBotClient bot)
        {
            var file = await bot.GetFileAsync(fileId);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                await bot.DownloadFileAsync(file.FilePath, fs);
            }
        }

        /// <summary>
        /// Метод отправляет файл пользователю
        /// </summary>
        /// <param name="chatid">id чата</param>
        /// <param name="path">Путь к файлу</param>
        /// <param name="bot">бот</param>
        public async void Download(string chatid, string path, TelegramBotClient bot)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    await bot.SendDocumentAsync(chatid, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fs, path));
                }
            }
            catch
            {
                await bot.SendTextMessageAsync(long.Parse(chatid), "Не так быстро...");
            }
        }

        /// <summary>
        /// Метод отправляет фото пользователю
        /// </summary>
        /// <param name="chatid">id чата</param>
        /// <param name="path">Путь к файлу</param>
        /// <param name="bot">бот</param>
        public async void DownloadPhoto(string chatid, string path, TelegramBotClient bot)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    await bot.SendPhotoAsync(chatid, new Telegram.Bot.Types.InputFiles.InputOnlineFile(fs, path));
                }
            }
            catch
            {
                bot.SendTextMessageAsync(long.Parse(chatid), "Не так быстро...");
            }
        }

        /// <summary>
        /// Метод показывает перечень команд
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public string Start(string user_name)
        {
            string result = $"Привет, {user_name}!\n\n" +
                $"- Если Вы хотите загрузить файл, то просто отправьте его" +
                $"\n- Если Вы хотите скачать файл выберите /download_files" +
                $"\n- Для просмотра списка загруженных файлов выберите /list_files";
            return result;
        }

        /// <summary>
        /// Дессериализация файла со списком ранее загруженных файлов
        /// </summary>
        /// <param name="file_base"></param>
        /// <returns></returns>
        public ObservableCollection<string> DeSerialization(string path)
        {
            ObservableCollection<string> file_base = new ObservableCollection<string>();

            try // Если бот открывается первый раз и такого файла нет
            {
                string json = File.ReadAllText(path); // Считать список загруженных ранее файлов
                file_base = JsonConvert.DeserializeObject<ObservableCollection<string>>(json);
                return file_base;
            }
            catch
            {
                return file_base;
            }
        }

        /// <summary>
        /// Сериализация файла со списком загруженных файлов
        /// </summary>
        /// <param name="list_files">Список загруженных файлов</param>
        public void Serialization(ObservableCollection<string> collection, string path)
        {
            string json = JsonConvert.SerializeObject(collection);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Метод показывает список ранее загруженных файлов
        /// </summary>
        /// <param name="list_files"></param>
        /// <returns></returns>
        public string ShowListFiles(ObservableCollection<string> list_files)
        {
            string result = $"Список ранее загруженных файлов:\n";

            for (int i = 0; i < list_files.Count; i++)
                result += $"\n{i + 1}. {list_files[i]}";

            return result;
        }

        /// <summary>
        /// Метод показывает список файлов для скачивания
        /// </summary>
        /// <param name="list_files">Список ранее загруженных файлов</param>
        /// <returns></returns>
        public string ShowListFilesForDownload(ObservableCollection<string> list_files)
        {
            string result = $"Для скачивание нажмите на файл:\n";

            for (int i = 0; i < list_files.Count; i++)
                result += $"\n/file{i + 1}-{list_files[i]}";

            return result;
        }

        /// <summary>
        /// Метод находит номер файла из строки, в противном случае возвращает -1
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int GettFileNumber(string message)
        {
            bool input; // Проверка ввода
            int file_number; // Номер файла

            input = int.TryParse(message.Remove(0, 5), out file_number);

            if (input) return file_number - 1;
            else return -1;
        }

        /// <summary>
        /// Сериализация файла со списком сообщений
        /// </summary>
        /// <param name="list_files">Список загруженных файлов</param>
        public bool SerializationLog(ObservableCollection<UsersMessage> collection, string path)
        {
            bool flag = false;
            try
            {
                string json = JsonConvert.SerializeObject(collection);
                File.WriteAllText(path, json);
                flag = true;
                return flag;
            }
            catch
            {
                return flag;
            }
        }

        /// <summary>
        /// Дессериализация файла со списком сообщений
        /// </summary>
        /// <param name="file_base"></param>
        /// <returns></returns>
        public ObservableCollection<UsersMessage> DeSerializationLog(string path, out bool flag)
        {
            flag = false;
            ObservableCollection<UsersMessage> log = new ObservableCollection<UsersMessage>();

            try // Если бот открывается первый раз и такого файла нет
            {
                string json = File.ReadAllText(path); // Считать список сообщений
                log = JsonConvert.DeserializeObject<ObservableCollection<UsersMessage>>(json);
                flag = true;
                return log;
            }
            catch
            {
                return log;
            }
        }
    }
}
