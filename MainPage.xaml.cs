using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Json;

using Newtonsoft.Json;


namespace LabWithJson
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient httpClient = new();
        public bool IsRefreshing { get; set; }
        public ObservableCollection<Victim> Victims { get; set; } = new();
        public string filePath = "D:\\universityLabs\\LabWithJson\\LabWithJson\\Json\\victims.json";
        public Command RefreshCommand { get; set; }
        public bool IsWriten = true;
        public Victim SelectedVictim { get; set; }
        public string text;

        public MainPage() 
        {
            RefreshCommand = new Command(async () =>
            {
                if (IsWriten)
                {
                    await LoadVictims();
                    IsWriten = false;
                }

                await Update();
                  
                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            });

            BindingContext = this;
            InitializeComponent();
            var newPage = new NewPage1(this);
        }
        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await Update();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Victims.Clear();
        }

        
        private async Task LoadVictims()
        {
            try
            {
                var crimeVictims = await httpClient.GetFromJsonAsync<Victim[]>("https://gist.githubusercontent.com/g-a-r-p-ia/2bdd3ab0ed2dbaeb19163f14e33ccec2/raw/0463f16ca484ff8ef814b52ec45fcdc350599add/victims.json");
                string startjson = JsonConvert.SerializeObject(crimeVictims, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, startjson);
                Victims.Clear();
                    foreach (Victim victim in crimeVictims)
                    {
    
                        Victims.Add(victim);
                    }
                   
                    
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading victims: {ex.Message}");
            }
        }
        public async Task Update()
        {
            string json = File.ReadAllText(filePath);
            var victims = JsonConvert.DeserializeObject<Victim[]>(json);
            Victims.Clear();
            foreach (Victim victim in victims)
            {
                Victims.Add(victim);
            }
        }
        public void AddVictim(object sender, EventArgs e) 
        {
            App.Current.MainPage = new NavigationPage(new NewPage1(this));
        }
        public async void DeleteVictim(object sender, EventArgs e)
        {
            try
            {
                if (SelectedVictim != null)
                {
                    Victims.Remove(SelectedVictim);

                    // Serialize the updated collection to JSON
                    string updatedJson = JsonConvert.SerializeObject(Victims, Newtonsoft.Json.Formatting.Indented);

                    // Write the updated JSON back to the file
                    File.WriteAllText(filePath, updatedJson);
                    await Update();
                }
                else
                {
                    await DisplayAlert("Alert", "Виберіть рядок для видалення", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting victim: {ex.Message}");
            }


        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            
            text = entry.Text;
        }

        public async void Go(object sender, EventArgs e)
        {
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1 && text != null)
            {

                List<Victim> filteredVictims = new List<Victim>();

                switch (selectedIndex)
                {
                    case 0:
                        filteredVictims = Victims
                            .Where(victim => victim.NameAndSurname.Contains(text))
                            .ToList();
                        break;
                    case 1:
                        filteredVictims = Victims
                            .Where(victim => victim.CauseOfDeath.Equals(text, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                        break;
                    case 2:
                        filteredVictims = Victims
                            .Where(victim => victim.YearsOld== int.Parse(text))
                            .ToList();
                        break;
                }

                // Clear the existing collection and add the filtered results
                Victims.Clear();
                foreach (Victim victim in filteredVictims)
                {
                    Victims.Add(victim);
                }
            }
            else { DisplayAlert("Error", "Напишіть значення для пошуку", "OK"); }

        }


    }
}