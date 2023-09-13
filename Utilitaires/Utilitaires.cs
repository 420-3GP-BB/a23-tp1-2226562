namespace Utilitaires
{
    public class Utilitaires
    {
       public static string calculerTemps(int nbMillisecondes)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(nbMillisecondes);
            return ts.ToString(@"hh\:mm\:ss");
        }



    }
}