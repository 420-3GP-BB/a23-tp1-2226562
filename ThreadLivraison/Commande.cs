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
        private bool _estLivree;
        private Stopwatch _stopwatch;
        public static int tempsLivraisonGlobal = 0;

        public void setEstLivree(bool estLivree)
        {
            _estLivree = estLivree;
            _stopwatch.Stop();
            
        }

        public int Numero
        {
            get;
            set;
        }

        public bool EstTraitee
        {
            get;
            set;
        }
        public bool EstLivree
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
            _stopwatch = sw;
            TempsLivraison++;
            //TempsLivraison = (int)(sw.ElapsedMilliseconds * FACTEUR_ACCELERATION);
            EstTraitee = estTraitée;
            EstLivree = estLivrée;
            sw.Start();

            tempsLivraisonGlobal += TempsLivraison;
            

        }

        public string ToString()
        {
            return $"Commande #{Numero} ==> Temps de préparation : {Utilitaires.Utilitaires.calculerTemps((int)(TempsPreparation * FACTEUR_ACCELERATION))}, Destination : {Destination.ToString()}";
        }
    }
        

   }

