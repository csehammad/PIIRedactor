using System.Collections.Generic;
using System.Windows;
using PIIRedactorApp.Models;

namespace PIIRedactorApp.Views
{
    public partial class SettingsWindow : Window
    {
        public RedactorConfig Config { get; private set; }
        private readonly List<RedactorConfig> templates = TemplateProvider.LoadTemplates();

        public SettingsWindow(RedactorConfig config)
        {
            InitializeComponent();
            Config = config;
            TemplateBox.ItemsSource = templates;
            TemplateBox.DisplayMemberPath = "Name";
            TemplateBox.SelectedIndex = 0;
            UseML.IsChecked = config.UseMLModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (TemplateBox.SelectedItem is RedactorConfig tpl)
            {
                Config.Patterns = tpl.Patterns;
            }
            Config.UseMLModel = UseML.IsChecked == true;
            DialogResult = true;
            Close();
        }
    }
}
