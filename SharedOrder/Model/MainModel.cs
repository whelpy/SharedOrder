using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SharedOrder.Properties;

namespace SharedOrder.Model
{
    public static class MainModel
    {
        private static string DatabaseFile = "SharedOrder.db3";

        public static SQLiteConnection Connection { get; private set; }

        public static bool InitDb()
        {
            if (!File.Exists(DatabaseFile))
            {
                try
                {
                    SQLiteConnection.CreateFile(DatabaseFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удаётся создать базу данных, ошибка: {ex.Message}.",
                        Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // проверим, может он и не создался на самом деле
            if (File.Exists(DatabaseFile))
            {
                try
                {
                    var connection = new SQLiteConnection($"Data Source={DatabaseFile};Version=3;");
                    connection.Open();
                    Connection = connection;
                }
                catch
                {
                    MessageBox.Show($"База данных повреждена, удалите файл {DatabaseFile} и перезапустите программу.",
                        Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (!UpdateStructure())
                return false;

            return true;
        }

        private static bool UpdateStructure()
        {
            try
            {
                var currentBinVersion = Assembly.GetExecutingAssembly().GetName().Version;
                var firstRun = false;

                try
                {
                    var dbVersion = "select `DataBaseVersion` from `Settings` LIMIT 1;";
                    var dbVerCommand = new SQLiteCommand(dbVersion, Connection);
                    var foundVersion = dbVerCommand.ExecuteScalar();

                    if (foundVersion != null)
                    {
                        var version = new Version(foundVersion.ToString());
                        if (version >= currentBinVersion) // не надо обновлять БД
                            return true;
                    }
                    else
                        firstRun = true;
                }
                catch (Exception ex)
                {
                    // если такой таблицы нет, просто выполняем скрипт
                    if (!ex.Message.Contains("no such table: Settings"))
                        throw;
                }

                var sql = SqlScript.Structure;
                var command = new SQLiteCommand(sql, Connection);
            
                command.ExecuteNonQuery();

                if (currentBinVersion != null)
                {
                    if (firstRun)
                        sql = $"INSERT INTO `Settings` (`DataBaseVersion`) VALUES ('{currentBinVersion}')";
                    else
                        sql = $"UPDATE `Settings` SET `DataBaseVersion` = '{currentBinVersion}'";

                    command = new SQLiteCommand(sql, Connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выполнения SQL команды: {ex.Message}",
                        Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
