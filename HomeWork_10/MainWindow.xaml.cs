using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeWork_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramClient telegram_client;
        public MainWindow()
        {
            InitializeComponent();

            // Указать токен бота
            telegram_client = new TelegramClient(this, "токен бота");

            Message_list.ItemsSource = telegram_client.log;
            Log_message.ItemsSource = telegram_client.log;
            list_box_file.ItemsSource = telegram_client.list_file;
        }

        /// <summary>
        /// Нажатие кнопки "Отправить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_message_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telegram_client.SendMessage(Users_id.Text, Message_text.Text);
                if (Users_id.Text == "0")
                    MessageBox.Show("Боту нельзя отправить сообщение!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);

                else Message_text.Text = "Сообщение отправлено";
            }
            catch
            {
                MessageBox.Show("Выберите пользователя!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Нажатие кнопки "Сохранить log"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_log_save_Click(object sender, RoutedEventArgs e)
        {
            TelegramOptions telegram_option = new TelegramOptions();
            try
            {
                bool flag = telegram_option.SerializationLog(telegram_client.log, txt_log_save.Text);

                if (!flag)
                    MessageBox.Show("Ошибка!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);

                else txt_log_save.Text = "Файл сохранён!";
            }
            catch
            {
                MessageBox.Show("Ошибка!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Нажатие кнопки "Загрузить log"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_log_load_Click(object sender, RoutedEventArgs e)
        {
            TelegramOptions telegram_option = new TelegramOptions();
            bool flag = false;

            try
            {
                telegram_client.log = telegram_option.DeSerializationLog(txt_log_load.Text, out flag);

                if (!flag)
                    MessageBox.Show("Ошибка!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);

                else
                {
                    Message_list.ItemsSource = telegram_client.log;
                    Log_message.ItemsSource = telegram_client.log;

                    txt_log_load.Text = "Файл загружен!";
                }
            }
            catch
            {
                MessageBox.Show("Ошибка!", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Нажатие кнопки "Очистить log"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_log_clear_Click(object sender, RoutedEventArgs e)
        {
            telegram_client.log.Clear();
            Message_list.ItemsSource = telegram_client.log;
            Log_message.ItemsSource = telegram_client.log;
        }
    }
}
