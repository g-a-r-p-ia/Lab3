using c = LabWithJson.MainPage;
using Newtonsoft.Json;

namespace LabWithJson
{
    public partial class NewPage1 : ContentPage
    {
        public NewPage1()
        {
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
            c.Victims.Add(newVictim);

            // Serialize the updated data back to JSON
            string updatedJson = JsonConvert.SerializeObject(c.Victims, Formatting.Indented);

            // Write the updated JSON back to the file
            File.WriteAllText(c.filePath, updatedJson);

            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}