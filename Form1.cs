using System.Configuration;
using Rectangles.Infrastructure;
using Configuration = Rectangles.Infrastructure.Configuration;

namespace Rectangles
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Конфигурационные свойства для задания параметров работы ПО
        /// Seed - параметр для генерации случайных чисел
        /// RemovalCycles - количество циклов жизни прямоугольников
        /// TimerInterval - интервал появления прямоугольников, миллисекунда
        /// </summary>
        private int Seed { get; }
        private int RemovalCycles { get; }
        private int TimerInterval { get; }

        private Random random { get; }
        private List<RectangleFigure> rectangles { get; }
        private readonly IConfiguration _config;


        public Form1()
        {
            _config = new Configuration();
            if (_config.InitializeVariables())
            {
                Seed = _config.Seed;
                RemovalCycles = _config.RemovalCycles;
                TimerInterval = _config.TimerInterval;
            }
            else
                //Завершить работу ПО, если инициализация свойств не удалась
                Load += (s, e) => Close();
            InitializeComponent();
            random = new Random();
            rectangles = new List<RectangleFigure>();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CreateRandomRectangle();
            RemovePartRectangles();
            Invalidate();
        }

        /// <summary>
        /// Создавать случайный прямоугольник
        /// Цикл do-while используется для того, чтобы создаваемый
        /// прямоугольник не выходил за пределы формы
        /// </summary>
        private void CreateRandomRectangle()
        {
            int x_tmp = 0, y_tmp = 0;
            do
            {
                x_tmp = ClientSize.Width - random.Next(Seed);
                y_tmp = ClientSize.Height - random.Next(Seed);
            } while (x_tmp < 0 || y_tmp < 0);

            var rectangle = new RectangleFigure(random.Next(x_tmp), random.Next(y_tmp),
                Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)), Seed);
            rectangles.Add(rectangle);
            CheckIntersections(rectangle);
        }

        /// <summary>
        /// Проверка пересечения прямоугольников
        /// </summary>
        /// <param name="rec">Прямоугольник, с которым нужно выполнить действие</param>
        private void CheckIntersections(RectangleFigure rec)
        {
            foreach (var rectangle in rectangles)
            {
                if (rectangle != rec && rectangle.CheckIntersections(rec))
                    rectangle.RemovalCycles = RemovalCycles;
            }
        }

        /// <summary>
        /// Удалить прямоугольники у которых жизненный цикл подошел к 0
        /// </summary>
        private void RemovePartRectangles()
        {
            rectangles.RemoveAll(r => r.RemovalCycles == 0);
            foreach (var rectangle in rectangles)
            {
                if (rectangle.RemovalCycles > 0)
                    rectangle.RemovalCycles--;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var rectangle in rectangles)
            {
                e.Graphics.FillRectangle(new SolidBrush(rectangle.RecColor), rectangle.Rectangle);
            }
        }
    }
}