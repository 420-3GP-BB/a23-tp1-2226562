using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public bool EstTraitée
        {
            get;
            set;
        }
        public bool EstLivrée
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

        public Stopwatch Stopwatch
        {
            get;
            set;
        }

        public Commande(int numero, Position destination, int tempsPrep, int tempsLiv, bool estTraitée, bool estLivrée, Stopwatch sw)
        {
            Numero = numero;
            Destination = destination;
            TempsPreparation = tempsPrep;
            TempsLivraison = (int)(sw.ElapsedMilliseconds * FACTEUR_ACCELERATION);
            EstTraitée = estTraitée;
            EstLivrée = estLivrée;
            Stopwatch = sw;
            sw.Start();

        }

        public string ToString()
        {
            return $"Commande #{Numero} ==> Temps de préparation : {Utilitaires.Utilitaires.calculerTemps((int)(TempsPreparation * FACTEUR_ACCELERATION))}, Destination : {Destination.ToString()}";
        }
    }
        

   }

