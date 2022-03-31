using Training.Models;
using Training.Pages;
using Training.ViewModels;
using System.Linq;

namespace Training.Services
{
    public class AutoMapperConfig : AutoMapper.Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Profil, ProfilViewModel>()
                //.ForMember(e => e.Expedition, e => e.MapFrom(e => e.Expedition))
                ;
            CreateMap<ProfilViewModel, Profil>();
                //.ForMember( e => e.MapFrom(e => new Profil()));

           //CreateMap<Profil, ProfilViewModel>().IncludeBase<Profil, ProfilViewModelBase>();
        }
    }
}
