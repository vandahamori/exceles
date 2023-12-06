using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
namespace exceles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateExcel();
        }
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;
        Models.HajosContext hajosContext = new Models.HajosContext();

        
        public void CreateExcel()
        {
            try

            {

                // Excel elindítása és az applikáció objektum betöltése 

                xlApp = new Excel.Application();



                // Új munkafüzet 

                xlWB = xlApp.Workbooks.Add(Missing.Value);



                // Új munkalap 

                xlSheet = xlWB.ActiveSheet;



                // Tábla létrehozása 

                CreateTable(); // Ennek megírása a következõ feladatrészben következik 



                // Control átadása a felhasználónak 

                xlApp.Visible = true;

                xlApp.UserControl = true;

            }

            catch (Exception ex) // Hibakezelés a beépített hibaüzenettel 

            {

                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);

                MessageBox.Show(errMsg, "Error");



                // Hiba esetén az Excel applikáció bezárása automatikusan 

                xlWB.Close(false, Type.Missing, Type.Missing);

                xlApp.Quit();

                xlWB = null;

                xlApp = null;

            }
        }
        public void CreateTable()
        { 
            string[] fejlécek = new string[] {

                "Kérdés",

                "1. válasz",

                "2. válaszl",

                "3. válasz",

                "Helyes válasz",

                "kép"};
            for (int i = 0; i < fejlécek.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = fejlécek[i];
            }
            var mindenKérdés = hajosContext.Questions.ToList();
            object[,] adatTömb = new object[mindenKérdés.Count(), fejlécek.Count()];

            for (int i = 0; i < mindenKérdés.Count(); i++)
            {
                adatTömb[i, 0] = mindenKérdés[i].Question1;
                adatTömb[i, 1] = mindenKérdés[i].Answer1;
                adatTömb[i, 2] = mindenKérdés[i].Answer2;
                adatTömb[i, 3] = mindenKérdés[i].Answer3;
                adatTömb[i, 4] = mindenKérdés[i].CorrectAnswer;
                adatTömb[i, 5] = mindenKérdés[i].Image;
            }
            int sorokSzáma = adatTömb.GetLength(0);

            int oszlopokSzáma = adatTömb.GetLength(1);

            Excel.Range adatRange = xlSheet.get_Range("A2", Type.Missing).get_Resize(sorokSzáma, oszlopokSzáma);

            adatRange.Value2 = adatTömb;

            adatRange.Columns.AutoFit();

            Excel.Range fejllécRange = xlSheet.get_Range("A1", Type.Missing).get_Resize(1, 6);

            fejllécRange.Font.Bold = true;

            fejllécRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            fejllécRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            fejllécRange.EntireColumn.AutoFit();

            fejllécRange.RowHeight = 40;

            fejllécRange.Interior.Color = Color.Fuchsia;

            fejllécRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            //Excel.Range teljestabla = xlSheet.get_Range("A1", Type.Missing).get_Resize(860,6);
            //teljestabla.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
            //Excel.Range elsooszlop = xlSheet.get_Range("A1", Type.Missing).get_Resize(860, 1);
            //elsooszlop.Font.Bold = true;
            //fejllécRange.Interior.Color = Color.LightYellow;
            //Excel.Range utolsooszlop = xlSheet.get_Range("A1", Type.Missing).get_Resize(860, 6);
            //utolsooszlop.Interior.Color=Color.LightGreen;
        }
        
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}