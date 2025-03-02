using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gauniv.Client.Pages;
using Gauniv.Client.Services;
using Gauniv.Network;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauniv.Client.ViewModel
{
    public partial class IndexViewModel : ObservableObject
    {
        private readonly v1Client _apiClient;
        public ObservableCollection<Category3> Categories { get; set; } = new();
        [ObservableProperty]
        private string testMessage = "🎉 Ceci est un test MAUI MVVM !";

        public IndexViewModel(v1Client apiClient) // Injection du client API
        {
            _apiClient = apiClient;
            LoadGames();
        }

        public async void LoadGames()
        {
            try
            {
                var categories = await _apiClient.CategoriesAsync(); // 🔥 Appel API
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de la récupération des catégories : {ex.Message}");
            }
        }
    }
}
