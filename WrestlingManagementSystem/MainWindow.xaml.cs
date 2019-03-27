/*
 * Author: Shon Verch
 * File Name: MainWindow.xaml.cs
 * Project Name: WrestlingManagementSystem
 * Creation Date: 03/26/2019
 * Modified Date: 03/26/2019
 * Description: Interaction logic for MainWindow.xaml
 */

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WrestlingManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct TestEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Gender Gender { get; set; }

            public TestEntry(string firstName, string lastName, Gender gender)
            {
                FirstName = firstName;
                LastName = lastName;
                Gender = gender;
            }
        }

        public List<TestEntry> TestData { get; set; }
        public string Label { get; set; } = "Test";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            TestData = new List<TestEntry>
            {
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male),
                new TestEntry("Foo", "Bar", Gender.Male)
            };
        }
    }
}
