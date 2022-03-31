using System;
using Microsoft.AspNetCore.Components;

using Training.Models;
using Training.ViewModels;
using Training.Services;

namespace Training.Pages
{
    public partial class VisualisationProfil
    {
        [Parameter]
        public int ProfilId { get; set; }
        
        public ProfilViewModel ProfilViewModel { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ProfilViewModel = profilService.GetProfilViewModel(ProfilId);
        }

    }
}