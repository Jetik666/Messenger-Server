using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Server.Control.Animations
{
    internal class AnimationHandler
    {
        public static void DoubleAnimation(object sender, DependencyProperty property, double fromValue, double toValue, double timer)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender), "Sender is null.");
            }

            DoubleAnimation animation = new()
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
                case TextBox textBox:
                        textBox.BeginAnimation(property, animation);
                    break;
                default:
                    Debug.WriteLine("Unknown type.");
                    break;
            }
        }
    }
}
