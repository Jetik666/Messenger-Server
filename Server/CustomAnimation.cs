using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Server
{
    internal class AnimationHandler
    {
        public static async Task DoubleAnimation(object sender, DependencyProperty property, double fromValue, double toValue, double timer)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("Sender is null.");
            }

            Debug.WriteLine(sender.GetType().Name + " type.");

            DoubleAnimation animation = new DoubleAnimation
            {
                From = fromValue,
                To = toValue,
                Duration = TimeSpan.FromMilliseconds(timer)
            };

            switch (sender)
            {
                case Grid grid:
                    grid.BeginAnimation(property, animation);
                    break;
                case Button button:
                    button.BeginAnimation(property, animation);
                    break;
                default:
                    Debug.WriteLine("Unknown type.");
                    break;
            }

            await Task.CompletedTask;
        }
    }
}
