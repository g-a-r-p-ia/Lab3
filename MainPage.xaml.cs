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
        public static ObservableCollection<Victim> Victims { get; set; } = new();
        public static string filePath = "D:\\universityLabs\\LabWithJson\\LabWithJson\\Json\\victims.json";
        public Command RefreshCommand { get; set; }
        public Victim SelectedVictim { get; set; }

        public MainPage() 
        {
            RefreshCommand = new Command(async () =>
            {
                await LoadVictims();    
                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
            });

            BindingContext = this;
            InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await LoadVictims();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Victims.Clear();
        }

        //private async Task LoadVictims()
        //{
        //    var crimeVictims = await httpClient.GetFromJsonAsync<Victim[]>("https://gist.githubusercontent.com/g-a-r-p-ia/2bdd3ab0ed2dbaeb19163f14e33ccec2/raw/0463f16ca484ff8ef814b52ec45fcdc350599add/victims.json");
        //    Victims.Clear();
        //    foreach (Victim victim in crimeVictims)
        //    {
        //        Victims.Add(victim);
        //    }
        //}
        private async Task LoadVictims()
        {
            try
            {
                using (var stream = await httpClient.GetStreamAsync("https://gist.githubusercontent.com/g-a-r-p-ia/2bdd3ab0ed2dbaeb19163f14e33ccec2/raw/0463f16ca484ff8ef814b52ec45fcdc350599add/victims.json"))
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();

                   
                    var crimeVictims = JsonConvert.DeserializeObject<List<Victim>>(json);
                    Victims.Clear();
                    foreach (Victim victim in crimeVictims)
                    {
                        Exception ew = new Exception(victim.ToString());
                        throw ew;
                        Victims.Add(victim);
                    }
                   
                    File.WriteAllText(filePath, json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading victims: {ex.Message}");
            }
        }
        public void AddVictim(object sender, EventArgs e) 
        {
            App.Current.MainPage = new NavigationPage(new NewPage1());
        }
        public void DeleteVictim(object sender, EventArgs e)
        {

        }
        public void Search(object sender, EventArgs e)
        {

        }

    }
}