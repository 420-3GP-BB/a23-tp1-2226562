using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitaires;

namespace ThreadLivraison
{
    internal class CommandeLivraison
    {
        private List<Commande> _commandes = new List<Commande>();
        private object _lock = new object();

        public void Ajouter(Commande cmd)
        {
            lock (_lock)
            {
                _commandes.Add(cmd);
            }
        }

        public bool estVide()
        {
            lock (this)
            {
                return (_commandes.Count == 0);
            }
        }

        public List<Commande> GetCommandes()
        {
            return _commandes;
        }

        public void SetCommandes(List<Commande> commandes)
        {
            _commandes = commandes;
        }

        public int length()
        {
            return _commandes.Count;
        }
        public List<Commande> obtenirCommande(Commande uneCommande)
        {
            List<Commande> livraison = new List<Commande>();
            lock (_lock)
            {
                livraison.Add(uneCommande);
                _commandes.Remove(uneCommande);
                

                for (int i = 0; i < _commandes.Count; i++)
                {
                    int depart;
                    int arrive;
                    if (i == 0)
                    {
                        depart = 0;
                        arrive = Utilitaires.Utilitaires.calculerPosition(uneCommande.Destination.X, uneCommande.Destination.Y);
                    }
                    else
                    {
                        depart = Utilitaires.Utilitaires.calculerPosition(uneCommande.Destination.X, uneCommande.Destination.Y);
                        arrive = Utilitaires.Utilitaires.calculerPosition(_commandes[i].Destination.X, _commandes[i].Destination.X);
                        if ((Utilitaires.Utilitaires.calculerDistance(depart, arrive) < 10) && livraison.Count < 5)
                        {
                            livraison.Add(_commandes[i]);
                            _commandes.Remove(_commandes[i]);
                            //lesCommandesPrepareListe.ElementAt(i).EstLivrée = true;
                        }


                    }

                }
                return livraison;
            }
        }
    }
}
