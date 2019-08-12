using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data;
using Mono;
using SQLite;
using Java.IO;

namespace Angame
{
    class Upgrades
    {
        
        public static bool SaveGame(Upgrades upgrades)
        {
            string content = (upgrades.Motyka + ";" + upgrades.Nawoz + ";" + upgrades.Rolnik + ";" + upgrades.Opryski + ";" + upgrades.Traktor + ";" + upgrades.MotykaStaraCena + ";"
                    + upgrades.NawozStaraCena + ";" + upgrades.RolnikStaraCena + ";" + upgrades.OpryskiStaraCena + ";" + upgrades.TraktorStaraCena + ";" + upgrades.Pieniadze);
            byte[] text = Encoding.ASCII.GetBytes(content);
            File path;
            File file;
            
            OutputStream ofile;
            string filename = "Angame.sav";
            try
            {
                path = Android.OS.Environment.ExternalStorageDirectory;
                file = new File(path.AbsolutePath + "/" + "Angame", filename);
                file.Mkdir();
                file.Delete();
                file.CreateNewFile();
                ofile = new FileOutputStream(file);
                ofile.Write(text);
                ofile.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static Upgrades LoadGame()
        {
            File path;
            File file;
            string filename = "Angame.sav";
            InputStream ifile;
            string content = "";
            try
            {
                path = Android.OS.Environment.ExternalStorageDirectory;
                file = new File(path.AbsolutePath + "/" + "Angame", filename);
                if(file.Exists())
                {
                    ifile = new FileInputStream(file);
                    int con = ifile.Read();
                    while(con != -1)
                    {
                        content += (char)con;
                        con = ifile.Read();
                    }
                    List<string> _content = content.Split(';').ToList();
                    if (_content.Count == 11)
                    {
                        int motyka = Int32.Parse(_content[0]);
                        int nawoz = Int32.Parse(_content[1]);
                        int rolnik = Int32.Parse(_content[2]);
                        int opryski = Int32.Parse(_content[3]);
                        int traktor = Int32.Parse(_content[4]);
                        double motykaStaraCena = double.Parse(_content[5]);
                        double nawozStaraCena = double.Parse(_content[6]);
                        double rolnikStaraCena = double.Parse(_content[7]);
                        double opryskiStaraCena = double.Parse(_content[8]);
                        double traktorStaraCena = double.Parse(_content[9]);
                        double pieniadze = double.Parse(_content[10]);
                        return new Upgrades(motyka, nawoz, rolnik, opryski, traktor, motykaStaraCena, nawozStaraCena, rolnikStaraCena, opryskiStaraCena, traktorStaraCena, pieniadze);
                    }
                    
                    
                }
            }
            catch(Exception)
            {
                
            }

            return new Upgrades();
        }

        public Upgrades()
        {
            this.Motyka = 0;
            this.Nawoz = 0;
            this.Rolnik = 0;
            this.Opryski = 0;
            this.Traktor = 0;
            this.MotykaStaraCena = 10;
            this.NawozStaraCena = 100;
            this.RolnikStaraCena = 500;
            this.OpryskiStaraCena = 2500;
            this.TraktorStaraCena = 5000;
            this.Pieniadze = 0;
            this.Pieniadzedod = 0;
            this.Pieniadzeidle = 0;
        }

        public Upgrades(int motyka,int nawoz, int rolnik, int opryski, int traktor, 
            double motykaStaraCena, double nawozStaraCena, double rolnikStaraCena, double opryskiStaraCena, double traktorStaraCena,
            double pieniadze)
        {
            this.Motyka = motyka;
            this.Nawoz = nawoz;
            this.Rolnik = rolnik;
            this.Opryski = opryski;
            this.Traktor = traktor;
            this.MotykaStaraCena = motykaStaraCena;
            this.NawozStaraCena = nawozStaraCena;
            this.RolnikStaraCena = rolnikStaraCena;
            this.OpryskiStaraCena = opryskiStaraCena;
            this.TraktorStaraCena = traktorStaraCena;
            this.Pieniadze = pieniadze;

            KosztMotyka(motyka);
            KosztNawoz(nawoz);
            KosztOpryski(opryski);
            KosztRolnik(rolnik);
            KosztTraktor(traktor);
        }
        

        //pieniądze
        public double Pieniadze { get; private set; }
        public  double Pieniadzedod { get; private set; }
        public double Pieniadzeidle { get; private set; }

        public void Click()
        {
            Pieniadzedod = Motyka * 0.3;
            Pieniadze = Math.Round(Pieniadze + 0.2 + Pieniadzedod,1);
        }

        public void Idle()
        {
            Pieniadzeidle = Nawoz * 0.3;
            Pieniadzeidle += Rolnik * 1;
            Pieniadzeidle += Opryski * 3;
            Pieniadzeidle += Traktor * 50;
            Pieniadze = Math.Round(Pieniadze + Pieniadzeidle, 1);
        }
        


        // Motyka
        public int Motyka { get; private set; }
        public double MotykaCena { get; private set; } = 10;
        public double MotykaStaraCena { get; private set; }
        public void KupMotyka()
        {
            Pieniadze = Math.Round(Pieniadze - KosztMotyka(Motyka),1);
            MotykaStaraCena = KosztMotyka(Motyka);
            Motyka++;
        }
        public double KosztMotyka(int poziom)
        {
            double cena;
            if(poziom == 0)
            {
                cena = 10;
            }
            else
            {
                cena = MotykaStaraCena + 2 + MotykaStaraCena*0.05; 
            }
            cena = Math.Round(cena, 1);
            MotykaCena = cena;
            return cena;
        }

        //nawoz
        public int Nawoz { get; private set; }
        public double NawozCena { get; private set; } = 100;
        public double NawozStaraCena { get; private set; }
        public void KupNawoz()
        {
            Pieniadze = Math.Round(Pieniadze - KosztNawoz(Nawoz), 1);
            NawozStaraCena = KosztNawoz(Nawoz);
            Nawoz++;
        }
        public double KosztNawoz(int poziom)
        {
            double cena;
            if (poziom == 0)
            {
                cena = 100;
            }
            else
            {
                cena = NawozStaraCena + 2 + NawozStaraCena * 0.1;
            }
            cena = Math.Round(cena, 1);
            NawozCena = cena;
            return cena;
        }

        //Rolnik
        public int Rolnik { get; private set; }
        public double RolnikCena { get; private set; } = 500;
        public double RolnikStaraCena { get; private set; }
        public void KupRolnik()
        {
            Pieniadze = Math.Round(Pieniadze - KosztRolnik(Rolnik), 1);
            RolnikStaraCena = KosztRolnik(Rolnik);
            Rolnik++;
        }
        public double KosztRolnik(int poziom)
        {
            double cena;
            if (poziom == 0)
            {
                cena = 500;
            }
            else
            {
                cena = RolnikStaraCena + 2 + RolnikStaraCena * 0.1;
            }
            cena = Math.Round(cena, 1);
            RolnikCena = cena;
            return cena;
        }

        //Opryski
        public int Opryski { get; private set; }
        public double OpryskiCena { get; private set; } = 2500;
        public double OpryskiStaraCena { get; private set; }
        public void KupOpryski()
        {
            Pieniadze = Math.Round(Pieniadze - KosztOpryski(Opryski), 1);
            OpryskiStaraCena = KosztOpryski(Opryski);
            Opryski++;
        }
        public double KosztOpryski(int poziom)
        {
            double cena;
            if (poziom == 0)
            {
                cena = 2500;
            }
            else
            {
                cena = OpryskiStaraCena + 2 + OpryskiStaraCena * 0.1;
            }
            cena = Math.Round(cena, 1);
            OpryskiCena = cena;
            return cena;
        }

        //Traktor
        public int Traktor { get; private set; }
        public double TraktorCena { get; private set; } = 5000;
        public double TraktorStaraCena { get; private set; }
        public void KupTraktor()
        {
            Pieniadze = Math.Round(Pieniadze - KosztTraktor(Traktor), 1);
            TraktorStaraCena = KosztTraktor(Traktor);
            Traktor++;
        }
        public double KosztTraktor(int poziom)
        {
            double cena;
            if (poziom == 0)
            {
                cena = 5000;
            }
            else
            {
                cena = TraktorStaraCena + 2 + TraktorStaraCena * 0.1;
            }
            cena = Math.Round(cena, 1);
            TraktorCena = cena;
            return cena;
            
        }
        
        public Upgrades Load()
        {

            return new Upgrades();
        }

        public bool Save()
        {

            return false;
        }
      
    }

    /*static class Zapis
    {
        
        public async static Task<bool> WriteTextAllAsync(this string filename, Angame.Upgrades upgrades, IFolder rootFolder = null)
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            string content = upgrades.Motyka + ";" + upgrades.Nawoz + ";" + upgrades.Rolnik + ";" + upgrades.Opryski + ";" + upgrades.Traktor + ";" + upgrades.MotykaStaraCena + ";"
                + upgrades.NawozStaraCena + ";" + upgrades.RolnikStaraCena + ";" + upgrades.OpryskiStaraCena + ";" + upgrades.TraktorStaraCena + ";" + upgrades.Pieniadze;
            await file.WriteAllTextAsync(content);

            return true;
        }

        public async static Task<Upgrades> ReadAllTextAsync(this string fileName, IFolder rootFolder = null)
        {
                string content;
                IFolder folder = FileSystem.Current.LocalStorage;
                IFile file = await folder.GetFileAsync(fileName);
                content = await file.ReadAllTextAsync();

                List<string> _content = content.Split(';').ToList();
                if(_content.Count == 11)
                {
                    int motyka = Int32.Parse(_content[0]);
                    int nawoz = Int32.Parse(_content[1]);
                    int rolnik = Int32.Parse(_content[2]);
                    int opryski = Int32.Parse(_content[3]);
                    int traktor = Int32.Parse(_content[4]);
                    double motykaStaraCena = double.Parse(_content[5]);
                    double nawozStaraCena = double.Parse(_content[6]);
                    double rolnikStaraCena = double.Parse(_content[7]);
                    double opryskiStaraCena = double.Parse(_content[8]);
                    double traktorStaraCena = double.Parse(_content[9]);
                    double pieniadze = double.Parse(_content[10]);
                    return new Upgrades(motyka, nawoz, rolnik, opryski, traktor, motykaStaraCena, nawozStaraCena, rolnikStaraCena, opryskiStaraCena, traktorStaraCena, pieniadze); 
                }
                else
                {
                    return new Upgrades();
                }
        }

    }
    */
}