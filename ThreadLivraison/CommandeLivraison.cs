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
            lock (_lock)
            {
                return (_commandes.Count == 0);
            }
        }

        public List<Commande> GetCommandes()
        {
            lock (_lock)
            {
                return _commandes;

            }
        }

        public void SetCommandes(List<Commande> commandes)
        {
            lock ( _lock)
            {
                _commandes = commandes;
            }
           
        }

        public int length()
        {
            lock(_lock)
            {
                return _commandes.Count;
            }
            
        }
        public List<Commande> obtenirCommande()
        {
            List<Commande> livraison = new List<Commande>();
            
            lock (_lock)
            {
                if(_commandes.Count != 0)
                {
                    Position depart = _commandes[0].Destination;
                    for (int i = 0; i < _commandes.Count; i++)
                    {
                        if (i == 0)
                        {
                            livraison.Add(_commandes[i]);
                        }
                        else
                        {
                            int distance = Math.Abs(depart.X - _commandes[i].Destination.X) + Math.Abs(depart.Y - _commandes[i].Destination.Y);
                            if ((distance < 10) && (livraison.Count < 5))
                            {
                                livraison.Add(_commandes[i]);
                            }
                        }
                    }

                    for (int j = 0; j < livraison.Count; j++)
                    {
                        _commandes.Remove(livraison[j]);
                    }
                    
                }
                return livraison;


            }
        }
    }
}
