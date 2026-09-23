using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AutopesulaApp.Models;
using AutopesulaApp.Services;

namespace AutopesulaApp
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<CarWashOrder> _orders = new();

        public MainWindow()
        {
            InitializeComponent();

            // Tekstid Resources.resx failist
            Title = Properties.Resources.WindowTitle;

            RegistrationLabelText.Text =
                Properties.Resources.RegistrationLabel;

            VehicleLabelText.Text =
                Properties.Resources.VehicleLabel;

            ProgramLabelText.Text =
                Properties.Resources.ProgramLabel;

            AddButton.Content =
                Properties.Resources.AddButton;

            EditButton.Content =
                Properties.Resources.EditButton;

            DeleteButton.Content =
                Properties.Resources.DeleteButton;

            OrdersDataGrid.Columns[0].Header =
                Properties.Resources.RegistrationColumn;

            OrdersDataGrid.Columns[1].Header =
                Properties.Resources.VehicleColumn;

            OrdersDataGrid.Columns[2].Header =
                Properties.Resources.ProgramColumn;

            OrdersDataGrid.Columns[3].Header =
                Properties.Resources.PriceColumn;

            OrdersDataGrid.Columns[4].Header =
                Properties.Resources.DurationColumn;

            // Enum väärtused ComboBoxidesse
            VehicleComboBox.ItemsSource =
                Enum.GetValues(typeof(VehicleType));

            ProgramComboBox.ItemsSource =
                Enum.GetValues(typeof(WashProgram));

            // DataGrid andmed
            OrdersDataGrid.ItemsSource = _orders;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetInput(
                out string registrationNumber,
                out VehicleType vehicleType,
                out WashProgram washProgram))
            {
                return;
            }

            CarWashOrder order = CreateOrder(
                registrationNumber,
                vehicleType,
                washProgram);

            _orders.Add(order);

            ClearInputs();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not CarWashOrder selectedOrder)
            {
                ShowError(Properties.Resources.SelectionError);
                return;
            }

            if (!TryGetInput(
                out string registrationNumber,
                out VehicleType vehicleType,
                out WashProgram washProgram))
            {
                return;
            }

            int index = _orders.IndexOf(selectedOrder);

            _orders[index] = CreateOrder(
                registrationNumber,
                vehicleType,
                washProgram);

            ClearInputs();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not CarWashOrder selectedOrder)
            {
                ShowError(Properties.Resources.SelectionError);
                return;
            }

            _orders.Remove(selectedOrder);

            ClearInputs();
        }

        private CarWashOrder CreateOrder(
            string registrationNumber,
            VehicleType vehicleType,
            WashProgram washProgram)
        {
            return new CarWashOrder
            {
                RegistrationNumber = registrationNumber,
                VehicleType = vehicleType,
                WashProgram = washProgram,

                Price = OrderCalculator.CalculatePrice(
                    vehicleType,
                    washProgram),

                DurationMinutes = OrderCalculator.CalculateDuration(
                    vehicleType,
                    washProgram)
            };
        }

        private bool TryGetInput(
            out string registrationNumber,
            out VehicleType vehicleType,
            out WashProgram washProgram)
        {
            registrationNumber = RegistrationTextBox.Text.Trim();

            vehicleType = default;
            washProgram = default;

            if (string.IsNullOrWhiteSpace(registrationNumber))
            {
                ShowError(Properties.Resources.RegistrationError);
                return false;
            }

            if (VehicleComboBox.SelectedItem is not VehicleType selectedVehicle)
            {
                ShowError(Properties.Resources.VehicleError);
                return false;
            }

            if (ProgramComboBox.SelectedItem is not WashProgram selectedProgram)
            {
                ShowError(Properties.Resources.ProgramError);
                return false;
            }

            vehicleType = selectedVehicle;
            washProgram = selectedProgram;

            return true;
        }

        private void OrdersDataGrid_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is not CarWashOrder order)
            {
                return;
            }

            RegistrationTextBox.Text = order.RegistrationNumber;
            VehicleComboBox.SelectedItem = order.VehicleType;
            ProgramComboBox.SelectedItem = order.WashProgram;
        }

        private void ClearInputs()
        {
            RegistrationTextBox.Clear();
            VehicleComboBox.SelectedIndex = -1;
            ProgramComboBox.SelectedIndex = -1;
            OrdersDataGrid.SelectedItem = null;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                Properties.Resources.ErrorTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}