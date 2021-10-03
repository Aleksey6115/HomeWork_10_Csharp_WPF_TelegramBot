using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HomeWork_10
{
    class TelegramClient
    {
        TelegramOptions telegramoptions = new TelegramOptions(); // Функционал бота
        TelegramBotClient bot; // Работа с ботом
        public ObservableCollection<string> list_file; // Список ранее загруженных файлов
        public ObservableCollection<UsersMessage> log; // Сообщения пользователя
        MainWindow window; // Рабочие окно

        /// <summary>
        /// Обработчик события OnMessage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Message(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document) // Если отправили документ
            #region
            {
                //Сохранить файл на диск
                try
                {
                    telegramoptions.Upload(e.Message.Document.FileId, e.Message.Document.FileName, bot);

                    window.Dispatcher.Invoke(() =>
                    {
                        list_file.Add(e.Message.Document.FileName);
                        log.Add(new UsersMessage(DateTime.Now.ToString(), e.Message.Chat.FirstName, e.Message.Chat.Id,
                            e.Message.Type.ToString(), e.Message.Document.FileName));
                    });

                    telegramoptions.Serialization(list_file, "base.json");
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Загрузка прошла успешно!");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Загрузка прошла успешно!"));
                    });
                }
                catch
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Ошибка загрузки...");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Ошибка загрузки..."));
                    });
                }
            }
            #endregion

            else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo) // Если отправили фото
            #region
            {
                //Сохранить фото на диск
                try
                {
                    telegramoptions.Upload(e.Message.Photo[3].FileId, $"photo{list_file.Count + 1}", bot);

                    window.Dispatcher.Invoke(() =>
                    {
                        list_file.Add($"photo{list_file.Count + 1}");
                        log.Add(new UsersMessage(DateTime.Now.ToString(), e.Message.Chat.FirstName, e.Message.Chat.Id,
                            e.Message.Type.ToString(), $"photo{list_file.Count + 1}"));
                    });

                    telegramoptions.Serialization(list_file, "base.json");
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Загрузка прошла успешно!");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Загрузка прошла успешно!"));
                    });
                }
                catch
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Ошибка загрузки...");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Ошибка загрузки..."));
                    });
                }
            }
            #endregion

            else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Audio) // Если отправили аудио
            #region
            {
                // Сохранить аудио на диск
                try
                {
                    telegramoptions.Upload(e.Message.Audio.FileId, e.Message.Audio.FileName, bot);

                    window.Dispatcher.Invoke(() =>
                    {
                        list_file.Add(e.Message.Audio.FileName);
                        log.Add(new UsersMessage(DateTime.Now.ToString(), e.Message.Chat.FirstName, e.Message.Chat.Id,
                            e.Message.Type.ToString(), e.Message.Audio.FileName));
                    });

                    telegramoptions.Serialization(list_file, "base.json");
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Загрузка прошла успешно!");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Загрузка прошла успешно!"));
                    });
                }
                catch
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Ошибка загрузки...");

                    window.Dispatcher.Invoke(() =>
                    {
                        log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Ошибка загрузки..."));
                    });
                }
            }
            #endregion

            else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text) // Если отправили текст
            #region
            {
                window.Dispatcher.Invoke(() =>
                {
                    log.Add(new UsersMessage(DateTime.Now.ToString(), e.Message.Chat.FirstName, e.Message.Chat.Id,
                        e.Message.Type.ToString(), e.Message.Text));
                });

                if (e.Message.Text.Contains("/file")) // Отправить файл пользователю
                #region
                {
                    if (telegramoptions.GettFileNumber(e.Message.Text) == -1)
                    {
                        bot.SendTextMessageAsync(e.Message.Chat.Id, "К сожалению такого файла не найдено!");

                        window.Dispatcher.Invoke(() =>
                        {
                            log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "К сожалению такого файла не найдено!"));
                        });

                        telegramoptions.SerializationLog(log, "log.json");
                    }

                    else
                    {
                        try
                        {
                            if (list_file[telegramoptions.GettFileNumber(e.Message.Text)].Contains("."))
                            {
                                telegramoptions.Download(e.Message.Chat.Id.ToString(),
                                                    list_file[telegramoptions.GettFileNumber(e.Message.Text)], bot);

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "bot", 0,
                                       "File", list_file[telegramoptions.GettFileNumber(e.Message.Text)]));
                                });

                                telegramoptions.SerializationLog(log, "log.json");
                            }


                            // Когда сохраняется фото, то в его имени нет символа "."
                            else
                            {
                                telegramoptions.DownloadPhoto(e.Message.Chat.Id.ToString(),
                                    list_file[telegramoptions.GettFileNumber(e.Message.Text)], bot);

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "bot", 0,
                                       "Photo", list_file[telegramoptions.GettFileNumber(e.Message.Text)]));
                                });

                                telegramoptions.SerializationLog(log, "log.json");
                            }
                        }
                        catch
                        {
                            bot.SendTextMessageAsync(e.Message.Chat.Id, "Ошибка скачивания...");

                            window.Dispatcher.Invoke(() =>
                            {
                                log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Ошибка скачивания..."));
                            });
                        }
                    }
                }
                #endregion

                else
                {
                    switch (e.Message.Text)
                    {
                        case "/start":
                            #region
                            bot.SendTextMessageAsync(e.Message.Chat.Id, telegramoptions.Start($"{e.Message.Chat.LastName} " +
                                $"{e.Message.Chat.FirstName}"));

                            window.Dispatcher.Invoke(() =>
                            {
                                log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text",
                                        telegramoptions.Start($"{e.Message.Chat.LastName} {e.Message.Chat.FirstName}")));

                            });
                            #endregion
                            break;

                        case "/download_files":
                            #region
                            if (list_file.Count == 0)
                            {
                                bot.SendTextMessageAsync(e.Message.Chat.Id, "Список загруженных файлов пуст");

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Список загруженных файлов пуст"));
                                });
                            }

                            else
                            {
                                bot.SendTextMessageAsync(e.Message.Chat.Id, telegramoptions.ShowListFilesForDownload(list_file));

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "File-List", "Список загруженных файлов"));
                                });
                            }
                            #endregion
                            break;

                        case "/list_files":
                            #region
                            if (list_file.Count == 0)
                            {
                                bot.SendTextMessageAsync(e.Message.Chat.Id, "Список загруженных файлов пуст");

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Список загруженных файлов пуст"));
                                });
                            }

                            else
                            {
                                bot.SendTextMessageAsync(e.Message.Chat.Id, telegramoptions.ShowListFiles(list_file));

                                window.Dispatcher.Invoke(() =>
                                {
                                    log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text", "Список загруженных файлов пуст"));
                                });
                            }
                            #endregion
                            break;
                    }
                }
            }
            #endregion

            else
            #region
            {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "К сожалению этот тип файла мне не знаком...");

                window.Dispatcher.Invoke(() =>
                {
                    log.Add(new UsersMessage(DateTime.Now.ToString(), "Bot", 0, "Text",
                                             "К сожалению этот тип файла мне не знаком..."));
                });
            }
            #endregion
        }

        /// <summary>
        /// Конструктор TelegramClient
        /// </summary>
        /// <param name="w">Текущие окно</param>
        public TelegramClient (MainWindow w, string token)
        {
            this.list_file = telegramoptions.DeSerialization("base.json");
            this.log = new ObservableCollection<UsersMessage>();
            this.window = w;

            bot = new TelegramBotClient(token);
            bot.OnMessage += Message;
            bot.StartReceiving();
        }

        /// <summary>
        /// Метод отправляет сообщение пользователю
        /// </summary>
        /// <param name="id_string"></param>
        /// <param name="Text"></param>
        public void SendMessage(string id_string, string Text) 
        {
            long id = Convert.ToInt64(id_string);
            bot.SendTextMessageAsync(id, Text);

            if (id_string != "0")
            window.Dispatcher.Invoke(() =>
            {
                log.Add(new UsersMessage(DateTime.Now.ToLongTimeString(), "Bot", 0, "Text", Text));
            });

        }
    }
}
