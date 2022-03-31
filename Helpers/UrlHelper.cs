using Training.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training.Helpers
{
    public static class UrlHelper
    {
        public const string PROFIL_UTILISATEUR = "/ListeProfils";
        public const string TYPE_ACTIVITE = "/TypeActivites";
        public const string TYPE_EXERCICE = "/TypeExercices";
        public const string PHASE_ACTIVITE = "/PhaseActivites";

        //public const string NOUVEAU_DECHET = "/nouveau_dechet";
        public const string EDITER_PROFIL = "/editer_profil";
        //public const string LISTE_DECHETS = "/liste_dechets";
        //public const string LISTE_EFFLUENTS = "/liste_effluents";
        //public const string STOCKS_DECHETS = "/etat_des_stocks_dechets";
        //public const string STOCKS_EFFLUENTS = "/etat_des_stocks_effluents";
        public const string VISUALISATION_PROFIL = "/visu_profil";
        //public const string VISUALISATION_EFFLUENT = "/visualisation_effluent";
        //public const string OPERATION_RAPPORT = "/operation_rapport";
        //public const string HINT_OPERATION_TYPE_DECHET = "dechet";
        //public const string HINT_OPERATION_TYPE_EFFLUENT = "effluent";
        //public const string EDITER_EFFLUENT = "/editer_effluent";
        //public const string RELEVE_EFFLUENT = "/releve_effluent";
        //public const string INFO_EFFLUENT = "/info_effluent";
        //public const string DEPOTAGE_EFFLUENT = "/depotage_effluent";
        //public const string EXPEDITION = "/expedition";
        public const string SUPPRIMER_PROFIL = "/supprimer_profil";
        //public const string JUSTIFICATION_DECHET = "/justification_dechet";
        //public const string JUSTIFICATION_EFFLUENT = "/justification_effluent";
        //public const string EXPORTATION_OPERATIONS_DECHETS = "/export_operations_dechets";
        //public const string EXPORTATION_OPERATIONS_EFFLUENT = "/export_operations_effluent";
        //public const string SYNCHRONISATION = "/sync";

        //private const string ADMIN_ROOT = "/admin/";
        //public const string LISTE_ETATS = ADMIN_ROOT + "liste_etats";
        //public const string LISTE_LOCAUX_ENTREPOSAGE = ADMIN_ROOT + "liste_locaux_entreposage";
        //public const string LISTE_LOTS_TFA = ADMIN_ROOT + "liste_lots_TFA";
        //public const string LISTE_SPECTRES = ADMIN_ROOT + "liste_spectres";
        //public const string LISTE_TYPES_COLIS = ADMIN_ROOT + "liste_types_colis";
        //public const string LISTE_VERROUS = ADMIN_ROOT + "liste_verrous";

        //public static string EditerDechet(RevisionDechet revisionDechet) => EditerDechet(revisionDechet.Id);

        //public static string EditerDechet(RevisionDechet revisionDechet) => EditerDechet(revisionDechet.Id);
        public static string EditerProfil(int profilId) => $"{EDITER_PROFIL}/{profilId}";
        public static string VisualisationProfil(int profilId) => $"{VISUALISATION_PROFIL}/{profilId}";
       
        //public static string VisualisationEffluent(RevisionEffluent revisionEffluent) => $"{VISUALISATION_EFFLUENT}/{revisionEffluent.Id}";
        //public static string InfoOperation(Operation operation) => $"{OPERATION_RAPPORT}/{operation.Id}";
        //public static string EditerEffluent(RevisionEffluent revisionEffluent) => EditerEffluent(revisionEffluent.Id);
        //public static string EditerEffluent(Guid revisionEffluentId) => $"{EDITER_EFFLUENT}/{revisionEffluentId}";
        //public static string ReleveEffluent(RevisionEffluent revisionEffluent) => $"{RELEVE_EFFLUENT}/{revisionEffluent.Id}";
        //public static string DepotageEffluent(RevisionEffluent revisionEffluent) => $"{DEPOTAGE_EFFLUENT}/{revisionEffluent.Id}";
        //public static string SupprimerProfil(Profil profil) => SupprimerProfil(profil.Id);
        public static string SupprimerProfil(int profilId) => $"{SUPPRIMER_PROFIL}/{profilId}";

        public static string GetApiRoute<Controller>(string methodName, object routeParameters = null) =>
            GetApiRoute(typeof(Controller), methodName, routeParameters);
        public static string GetApiRoute(Type Controller, string methodName, object routeParameters = null)
        {
            methodName = methodName.ToLower().RemoveSuffix("async");
            var methods = Controller.GetMethods().Where(e => e.Name.ToLower() == methodName || e.Name.ToLower() == methodName + "async");
            var parameters = routeParameters == null ? new Dictionary<string, object>() :
                routeParameters.GetType().GetProperties().Select(e => new { Key = e.Name.ToLower(), Value = e.GetValue(routeParameters) })
                .Where(e => e.Value != null).ToDictionary(e => e.Key, e => e.Value);

            var methodFound = false;
            foreach (var method in methods)
            {
                var methodParameters = method.GetParameters().ToDictionary(e => e.Name.ToLower());
                if (parameters.All(e => methodParameters.TryGetValue(e.Key, out var p) &&
                    (p.ParameterType == e.Value.GetType() || Nullable.GetUnderlyingType(p.ParameterType) == e.Value.GetType())))
                {
                    methodFound = true;
                    break;
                }
            }

            if (!methodFound)
                throw new Exception("No method found with mathing name and parameters");


            string controllerName = Controller.Name.ToLower().RemoveSuffix("controller");

            string parametersString = parameters.Any() ?
                parametersString = "?" + string.Join("&", parameters.Select(e => e.Key + "=" + ObjectToUrlEncoded(e.Value))) : "";


            return $"/api/{controllerName}/{methodName}{parametersString}";
        }

        private static string ObjectToUrlEncoded(object param)
        {
            string toString;
            if (param is DateTime date)
            {
                toString = date.ToString("s");
            }
            else if (param.GetType().IsValueType)
            {
                toString = param.ToString();
            }
            else
            {
                throw new Exception("parameter must be a valueType");
            }

            return HttpUtility.UrlEncode(toString);
        }
        private static string RemoveSuffix(this string str, string suffix)
        {
            if (str.EndsWith(suffix))
                return str.Remove(str.Length - suffix.Length);
            return str;
        }
    }
}
