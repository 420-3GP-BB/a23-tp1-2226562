using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitaires;
using static ThreadLivraison.ConstantesSimulation;

namespace ThreadLivraison
{
    internal class Commande
    {
        public int Numero
        {
            get;
            set;
        }

        public Position Destination
        {
            get;
            set;
        }

        public int TempsPreparation
        {
            get;
            set;
        }

        public int TempsLivraison
        {
            get;
            set;
        }
        public Commande(int numero, Position destination, int tempsPrep, int tempsLiv)
        {
            Numero = numero;
            Destination = destination;
            TempsPreparation = tempsPrep;
            TempsLivraison = tempsLiv;
        }

        public string ToString()
        {
            return $"Commande #{Numero} ==> Temps de préparation : {Utilitaires.Utilitaires.calculerTemps((int)(TempsPreparation * FACTEUR_ACCELERATION))}, Destination : {Destination.ToString()}, Temps Livraison : {Utilitaires.Utilitaires.calculerTemps(TempsLivraison)} ";
        }
    }
        

   }

