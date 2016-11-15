using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChoixRestau.Models
{
    public class Dal:IDal
    {
        private BddContext bdd;

        public Dal()
        {
            bdd = new BddContext();
        }

        public List<Resto> ObtientTousLesRestaurants()
        {
            return bdd.Restos.ToList();
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        public void CreerRestaurant(string nom, string telephone)
        {
            bdd.Restos.Add(new Resto { Nom = nom, Telephone = telephone });
            bdd.SaveChanges();
        }


        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            Resto restoTrouve = bdd.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoTrouve != null)
            {
                restoTrouve.Nom = nom;
                restoTrouve.Telephone = telephone;
                bdd.SaveChanges();
            }
        }


        public bool RestaurantExiste(string nom)
        {
            Resto restoTrouve = bdd.Restos.FirstOrDefault(resto => resto.Nom == nom);
            if (restoTrouve != null)
                return true;

            return false;

        }

        public Utilisateur ObtenirUtilisateur(int id)
        {
            Utilisateur utilisateurTrouve = bdd.Utilisateurs.FirstOrDefault(user => user.Id == id);

            if (utilisateurTrouve != null)
                return utilisateurTrouve;

            return null;
        }


        public Utilisateur ObtenirUtilisateur(string id)
        {
            int valeur;

            if (int.TryParse(id, out valeur))
                return ObtenirUtilisateur(valeur);

            return null;
        }

        public int AjouterUtilisateur(string nom, string motDePasse)
        {
            Utilisateur userCreer = new Utilisateur
            {
                Prenom = nom
              ,
                MotDePasse = EncodeMD5(motDePasse)
            };

            bdd.Utilisateurs.Add(userCreer);
            bdd.SaveChanges();

            return userCreer.Id;
        }

        public Utilisateur Authentifier(string prenom, string motDePasse)
        {
            string motDePasseEncode = EncodeMD5(motDePasse);

            Utilisateur user = bdd.Utilisateurs.FirstOrDefault(u => u.Prenom == prenom && u.MotDePasse == motDePasseEncode);
            return user;
        }

        public bool ADejaVote(int idSondage, string idStr)
        {
            int idUser;
            if (int.TryParse(idStr, out idUser))
            {
                Sondage sondageTrouve = bdd.Sondages.First(s => s.Id == idSondage);
                if (sondageTrouve.Votes == null)
                    return false;

                return sondageTrouve.Votes.Any(v => v.Utilisateur != null && v.Utilisateur.Id == idUser);
            }

            return false;


        }

        public int CreerUnSondage()
        {
            Sondage sondageCreer = new Sondage { Date = DateTime.Now };
            bdd.Sondages.Add(sondageCreer);

            bdd.SaveChanges();
            return sondageCreer.Id;
        }

        public void AjouterVote(int idSondage, int idResto, int idUtilisateur)
        {

            Vote vote = new Vote
            {
                Resto = bdd.Restos.First(r => r.Id == idResto)
               ,
                Utilisateur = bdd.Utilisateurs.First(u => u.Id == idUtilisateur)
            };
            Sondage sondage = bdd.Sondages.First(s => s.Id == idSondage);
            if (sondage.Votes == null)
            {
                sondage.Votes = new List<Vote>();
            }
            sondage.Votes.Add(vote);
            bdd.SaveChanges();
        }

        public List<Resultats> ObtenirLesResultats(int idSondage)
        {
            List<Resto> lesResto = ObtenirTousLesRestaurants();
            List<Resultats> lesResultats = new List<Resultats>();
            Sondage sondage = bdd.Sondages.First(s => s.Id == idSondage);

            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = bdd.Restos.First(r => r.Id == idRestaurant);
                int nombreDeVotes = grouping.Count();
                lesResultats.Add(new Resultats
                {
                    Nom = resto.Nom
                ,
                    Telephone = resto.Telephone
                ,
                    NombreDeVotes = nombreDeVotes
                });
            }
            return lesResultats;
        }

        private List<Resto> ObtenirTousLesRestaurants()
        {
            return bdd.Restos.ToList();
        }

        private string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }
    }
}