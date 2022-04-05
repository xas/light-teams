using System.Drawing;
using Xas.LightTeams.Device;

namespace Xas.LightTeams.App
{
    internal class LightManager : IDisposable
    {
        private readonly Blinkt _blinkt;
        private Color _currentColor = Color.White;

        public LightManager()
        {
            _blinkt = new Blinkt();
        }

        public void SetStatus(Color color)
        {
            if (color == _currentColor)
            {
                return;
            }
            _currentColor = color;
            ShowRefresh();
            ShowColor();
        }

        public void Stop()
        {
            _blinkt.Clear();
            _blinkt.Show();
        }

        private void ShowRefresh()
        {
            for (int i = 0; i < 8; i++)
            {
                _blinkt.Clear();
                _blinkt.SetPixel(i > 0 ? i - 1 : i, _currentColor, 0.05M);
                _blinkt.SetPixel(i < 7 ? i + 1 : i, _currentColor, 0.05M);
                _blinkt.SetPixel(i, _currentColor);
                _blinkt.Show();
                Thread.Sleep(50);
            }
            for (int i = 7; i > -1; i--)
            {
                _blinkt.Clear();
                _blinkt.SetPixel(i > 0 ? i - 1 : i, _currentColor, 0.05M);
                _blinkt.SetPixel(i < 7 ? i + 1 : i, _currentColor, 0.05M);
                _blinkt.SetPixel(i, _currentColor);
                _blinkt.Show();
                Thread.Sleep(50);
            }
        }

        private void ShowColor()
        {
            _blinkt.Clear();
            for (int i = 0; i < 8; i++)
            {
                _blinkt.SetPixel(i, _currentColor);
            }
            _blinkt.Show();
        }

        public void Dispose()
        {
            _blinkt?.Dispose();
        }
    }
}
