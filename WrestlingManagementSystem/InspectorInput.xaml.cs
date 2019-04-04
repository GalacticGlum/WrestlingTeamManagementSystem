/*
 * Author: Shon Verch
 * File Name: InspectorInput.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 04/03/2019
 * Modified Date: 04/03/2019
 * Description: Inspector input control.
 */

using System.Windows;
using System.Windows.Controls;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// Interaction logic for InspectorInput.xaml
    /// </summary>
    public partial class InspectorInput : UserControl
    {
        /// <summary>
        /// The name of this <see cref="InspectorInput"/>.
        /// </summary>
        public static DependencyProperty InputNameProperty;

        /// <summary>
        /// The name of this <see cref="InspectorInput"/>.
        /// </summary>
        public string InputName
        {
            get => (string) GetValue(InputNameProperty);
            set => SetValue(InputNameProperty, value);
        }

        /// <summary>
        /// Initialize static members of <see cref="InspectorInput"/>.
        /// </summary>
        static InspectorInput()
        {
            InputNameProperty = DependencyProperty.Register("InputName", typeof(string), typeof(InspectorInput));
        }

        /// <summary>
        /// Inspector input control.
        /// </summary>
        public InspectorInput()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
