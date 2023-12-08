
using Newtonsoft.Json;

namespace LabWithJson
{
    public partial class NewPage1 : ContentPage
    {
        private MainPage mainPage;
        public NewPage1(MainPage mainPage)
        {
            this.mainPage = mainPage;
            InitializeComponent();
        }

        public void AddToJsone(object sender, EventArgs e)
        {

            Victim newVictim = new Victim
            {
                NameAndSurname = Name.Text,
                YearsOld = int.Parse(Years.Text) ,
                CauseOfDeath = Cause.Text,
                Killer = Killer.Text,
                DateOfDeath = new DateTime(DateT.Date.Year, DateT.Date.Month, DateT.Date.Day)
            };
            mainPage.Victims.Add(newVictim);

            string updatedJson = JsonConvert.SerializeObject(mainPage.Victims, Newtonsoft.Json.Formatting.Indented);

            // Write the updated JSON back to the file
            File.WriteAllText(mainPage.filePath, updatedJson);



            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}