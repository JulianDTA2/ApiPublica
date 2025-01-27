using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SPJApiPublica.Models;
using SPJApiPublica.Services;
using System.ComponentModel;

namespace SPJApiPublica.ViewModel
{
    public class CarViewModel : INotifyPropertyChanged
    {
        private readonly CarService _carService;
        private readonly CarRepository _carRepository;
        public ObservableCollection<Car> Cars { get; private set; }

        public ICommand LoadCarsCommand { get; }
        public ICommand AddCarCommand { get; }
        public ICommand UpdateCarCommand { get; }
        public ICommand DeleteCarCommand { get; }

        public CarViewModel()
        {
            _carService = new CarService();
            _carRepository = new CarRepository();
            Cars = new ObservableCollection<Car>();

            LoadCarsCommand = new Command(async () => await LoadCarsAsync());
            AddCarCommand = new Command(async () => await AddCarAsync());
            UpdateCarCommand = new Command(async () => await UpdateCarAsync());
            DeleteCarCommand = new Command(async () => await DeleteCarAsync());
        }

        private async Task LoadCarsAsync()
        {
            var cars = await _carService.GetCarsAsync();
            Cars.Clear();
            foreach (var car in cars)
            {
                await _carRepository.AddCarAsync(car);
                Cars.Add(car);
            }
        }

        private async Task AddCarAsync()
        {
            var newCar = new Car();

            string make = await Application.Current.MainPage.DisplayPromptAsync("Agregar Coche", "Marca:");
            string model = await Application.Current.MainPage.DisplayPromptAsync("Agregar Coche", "Modelo:");
            string yearStr = await Application.Current.MainPage.DisplayPromptAsync("Agregar Coche", "Año:");
            string color = await Application.Current.MainPage.DisplayPromptAsync("Agregar Coche", "Color:");

            if (!string.IsNullOrWhiteSpace(make) &&
                !string.IsNullOrWhiteSpace(model) &&
                int.TryParse(yearStr, out int year) &&
                !string.IsNullOrWhiteSpace(color))
            {
                newCar.Make = make;
                newCar.Model = model;
                newCar.Year = year;
                newCar.Color = color;

                await _carRepository.AddCarAsync(newCar);
                Cars.Add(newCar);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese datos válidos.", "OK");
            }
        }

        private async Task UpdateCarAsync()
        {
            if (Cars.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay coches para actualizar.", "OK");
                return;
            }

            var selectedCar = await Application.Current.MainPage.DisplayActionSheet("Seleccionar Coche", "Cancelar", null, Cars.Select(car => car.Make + " " + car.Model).ToArray());

            var carToUpdate = Cars.FirstOrDefault(c => (c.Make + " " + c.Model) == selectedCar);
            if (carToUpdate != null)
            {
                string newMake = await Application.Current.MainPage.DisplayPromptAsync("Actualizar Coche", "Nueva Marca:", initialValue: carToUpdate.Make);
                string newModel = await Application.Current.MainPage.DisplayPromptAsync("Actualizar Coche", "Nuevo Modelo:", initialValue: carToUpdate.Model);
                string newYearStr = await Application.Current.MainPage.DisplayPromptAsync("Actualizar Coche", "Nuevo Año:", initialValue: carToUpdate.Year.ToString());
                string newColor = await Application.Current.MainPage.DisplayPromptAsync("Actualizar Coche", "Nuevo Color:", initialValue: carToUpdate.Color);

                if (!string.IsNullOrWhiteSpace(newMake) &&
                    !string.IsNullOrWhiteSpace(newModel) &&
                    int.TryParse(newYearStr, out int newYear) &&
                    !string.IsNullOrWhiteSpace(newColor))
                {
                    carToUpdate.Make = newMake;
                    carToUpdate.Model = newModel;
                    carToUpdate.Year = newYear;
                    carToUpdate.Color = newColor;

                    await _carRepository.UpdateCarAsync(carToUpdate);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese datos válidos.", "OK");
                }
            }
        }

        private async Task DeleteCarAsync()
        {
            if (Cars.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No hay coches para eliminar.", "OK");
                return;
            }

            var selectedCar = await Application.Current.MainPage.DisplayActionSheet("Seleccionar Coche a Eliminar", "Cancelar", null, Cars.Select(car => car.Make + " " + car.Model).ToArray());

            var carToDelete = Cars.FirstOrDefault(c => (c.Make + " " + c.Model) == selectedCar);
            if (carToDelete != null)
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar este coche?", "Sí", "No");
                if (confirm)
                {
                    await _carRepository.DeleteCarAsync(carToDelete.Id);
                    Cars.Remove(carToDelete);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
