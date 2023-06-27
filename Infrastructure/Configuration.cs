using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace Rectangles.Infrastructure
{
    /// <summary>
    /// Считывает из файла App.config, инициализирует и проверяет конфигурационные свойства.
    /// В случае каких-либо неполадок выводит соответствующую информацию. 
    /// </summary>
    public class Configuration : IConfiguration
    {
        public int Seed { get; set; } = 100;
        public int RemovalCycles { get; set; } = 2;
        public int TimerInterval { get; set; } = 2000;

        /// <summary>
        /// Базовый метод считывания переменных из файла App.config
        /// и вызова метода проверки и инициализации соответствующих свойств
        /// </summary>
        /// <returns>В зависимости от результата возвращает значение bool</returns>
        public bool InitializeVariables()
        {
            try
            {
                var keys = ConfigurationManager.AppSettings.Keys;
                if (keys.Count == 0)
                {
                    ShowMessage(
                        $"Ошибка в файле конфигурации *.config. Файл не содержит значение необходимых параметров. Приложение будет закрыто.",
                        "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                foreach (string key in keys)
                {
                    string? valueFromAppConfig = ConfigurationManager.AppSettings.Get(key);
                    if (valueFromAppConfig == "Stop")
                        break;
                    if (valueFromAppConfig is null)
                    {
                        ShowMessage(
                            $"Ошибка в файле конфигурации *.config. Не найден ключ \"{key}\". Приложение будет закрыто.",
                            "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (!CheckVariables(key, valueFromAppConfig))
                        return false;
                }

                return true;
            }
            catch (ConfigurationErrorsException err)
            {
                ShowMessage("Ошибка чтения структуры файла конфигурации *.config. Приложение будет закрыто.",
                    "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        /// <summary>
        /// Непосредственно метод проверки и инициализации свойств
        /// </summary>
        /// <param name="key">Значение key из файла App.config</param>
        /// <param name="value">Значение value из файла App.config</param>
        /// <returns></returns>
        bool CheckVariables(string key, string value)
        {
            int param;
            if (value is not null)
            {
                if (int.TryParse(value, out param) && param > 0)
                {
                    //Создает новый словарь с именем actions, который содержит ключи типа string и значения типа Action<int>
                    //Каждый 'элемент связан с лямбда-выражением, которое принимает аргумент x и присваивает его соответствующему свойству
                    //словарь может использоваться для вызова действий с помощью ключей и передачи им аргументов
                    var actions = new Dictionary<string, Action<int>>
                {
                    { "Seed", x => Seed = x },
                    { "RemovalCycles", x => RemovalCycles = x },
                    { "TimerInterval", x => TimerInterval = x }
                };
                    if (actions.ContainsKey(key))
                    {
                        //Вызов действий с помощью ключа и передачи ему аргумента
                        actions[key](param);
                        return true;
                    }
                }
                else
                {
                    var result = ShowMessage((new StringBuilder($"{ConfigurationManager.AppSettings.Get("Message")}"))
                    .Replace("[name]", key).ToString(),
                        ConfigurationManager.AppSettings.Get("caption"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        ShowMessage("Заданы недопустимые параметры. Приложение будет закрыто.",
                            "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                        return true;
                }
            }
            return false;
        }

        DialogResult ShowMessage(string mess, string? textOfWindow, MessageBoxButtons mesBoxButton, MessageBoxIcon icon)
        {
            return MessageBox.Show(
                  mess,
                  textOfWindow,
                  mesBoxButton,
                  icon);
        }
    }
}
